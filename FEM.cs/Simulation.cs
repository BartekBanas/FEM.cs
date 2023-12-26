using System.Globalization;

namespace FEM.cs;

public class Simulation
{
    private readonly Conditions _conditions;
    private readonly List<Element> _elements;
    private readonly List<Node> _nodes;
    private readonly int _amountOfNodes;
    
    private readonly double[,] _system;
    private readonly double[] _globalPVector;
    private readonly double[,] _globalHmatrix;
    private readonly double[,] _globalHbcMatrix;
    private readonly double[,] _globalCMatrix;
    private readonly double[] _temperatureVector;
    
    public Simulation(SimulationModel simulationModel)
    {
        _conditions = simulationModel.Conditions;
        
        _elements = simulationModel.Elements;
        _nodes = simulationModel.Nodes;

        _amountOfNodes = simulationModel.Elements[^1].Nodes[2].ID;

        _system = new double[_amountOfNodes, _amountOfNodes];
        _globalHmatrix = new double[_amountOfNodes, _amountOfNodes];
        _globalCMatrix = new double[_amountOfNodes, _amountOfNodes];
        _globalHbcMatrix = new double[_amountOfNodes, _amountOfNodes];
        _globalPVector = new double [_amountOfNodes];
        _temperatureVector = new double[_amountOfNodes];
        foreach (var node in _nodes)
        {
            node.Temperature = _conditions.TemperatureInitial;
        }
    }
    
    public void PrintSystem()
    {
        Console.WriteLine("System of Equations:");
        for (int i = 0; i < _amountOfNodes; i++)
        {
            for (int j = 0; j < _amountOfNodes; j++)
            {
                Console.Write(_system[i, j] < 0 ? "-" : " ");

                Console.Write(Math.Abs(_system[i, j]).ToString("F1", CultureInfo.InvariantCulture));
                Console.Write("\t");
            }

            Console.Write("*  ");
            Console.Write(_globalPVector[i] < 0 ? "-" : " ");
            Console.Write(Math.Abs(_globalPVector[i]).ToString("F2", CultureInfo.InvariantCulture));
            
            Console.Write(",\tt = ");
            Console.Write(Math.Abs(_temperatureVector[i]).ToString("F2", CultureInfo.InvariantCulture));
            
            Console.WriteLine();
        }

        Console.WriteLine();
    }
    
    private void Aggregation()
    {
        ClearGlobalStructures();
        
        for (int i = 0; i < _amountOfNodes; i++)
        {
            _temperatureVector[i] = _nodes[i].Temperature;
        }
        
        foreach (var element in _elements)
        {
            double[,] hmatrix = element.Hmatrix();
            double[,] hbcMatrix = element.HbcMatrix();
            double[,] cMatrix = element.CMatrix();
            double[] pVector = element.PVector();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _globalHmatrix[element.Nodes[i].ID - 1, element.Nodes[j].ID - 1] += hmatrix[i, j];
                    _globalCMatrix[element.Nodes[i].ID - 1, element.Nodes[j].ID - 1] += cMatrix[i, j];
                    _globalHbcMatrix[element.Nodes[i].ID - 1, element.Nodes[j].ID - 1] += hbcMatrix[i, j];

                }   _globalPVector[element.Nodes[i].ID - 1] += pVector[i];
            }
        }

        double[,] cPerΔτ = Functions.MatrixMultiplication(_globalCMatrix, 1 / _conditions.SimulationStepTime);

        double[,] dashMatrix = Functions.MatrixSummation(_globalHmatrix, cPerΔτ, _globalHbcMatrix);

        _globalPVector.AddVector(Functions.MultiplyMatrixByVector(cPerΔτ, _temperatureVector));

        for (int i = 0; i < _amountOfNodes; i++)
        {
            for (int j = 0; j < _amountOfNodes; j++)
            {
                _system[i, j] += dashMatrix[i, j];
            }
        }
    }
    
    private double[] CalculateSystem()
    {
        double epsilon = 1;
        double peak;
        int peakId = 0;

        double[,] coefficients = new double [_amountOfNodes, _amountOfNodes + 1];

        for (int i = 0; i < _amountOfNodes; i++)
        {
            for (int j = 0; j < _amountOfNodes; j++)
                coefficients[i, j] = _system[i, j];

            coefficients[i, _amountOfNodes] = _globalPVector[i];
        }

        // Console.WriteLine("\nExtended matrix before Partial Pivoting:");
        // for (int i = 0; i < amountOfNodes; i++)
        // {
        //     for (int j = 0; j < amountOfNodes + 1; j++)
        //     {
        //         Console.Write(coefficents[i, j].ToString("F2", CultureInfo.InvariantCulture));
        //         Console.Write("\t");
        //     }
        //
        //     Console.WriteLine();
        // }

        // Partial Pivoting
        for (int i = 0; i < _amountOfNodes; i++)
        {
            peakId = i;
            peak = Math.Abs(coefficients[i, i]);
            for (int j = i; j < _amountOfNodes; j++)
            {
                if (peak + epsilon < Math.Abs(coefficients[j, i]) && coefficients[j, i] != 0)
                {
                    peak = Math.Abs(coefficients[j, i]);
                    peakId = j;
                }
            }

            if (peakId != i)
            {
                for (int j = 0; j < _amountOfNodes + 1; j++)
                {
                    (coefficients[i, j], coefficients[peakId, j]) = (coefficients[peakId, j], coefficients[i, j]);
                }
            }
        }

        // Console.WriteLine("\nExtended matrix after Partial Pivoting:");
        // for (int i = 0; i < amountOfNodes; i++)
        // {
        //     for (int j = 0; j < amountOfNodes + 1; j++) //Printing
        //     {
        //         Console.Write(coefficents[i, j].ToString("F2", CultureInfo.InvariantCulture));
        //         Console.Write("\t");
        //     }
        //
        //     Console.WriteLine();
        // }
        //
        // Console.WriteLine();


        double[,] multiplier = new double[_amountOfNodes, 2];
        for (int i = 1; i < _amountOfNodes; i++)
        {
            multiplier[i, 0] = coefficients[i, 0] / coefficients[0, 0];
            multiplier[i, 1] = coefficients[i, 1] / coefficients[1, 1];
        }

        for (int k = 0; k < _amountOfNodes - 1; k++)
        {
            for (int i = k + 1; i < _amountOfNodes; i++) //Operations
            {
                for (int j = k; j <= _amountOfNodes; j++)
                {
                    coefficients[i, j] -= (coefficients[k, j] * multiplier[i, 0]);
                }
            }

            for (int i = k + 2; i < _amountOfNodes; i++) //multipliers
            {
                multiplier[i, 0] = coefficients[i, k + 1] / coefficients[k + 1, k + 1];
            }
        }

        // Console.WriteLine("Extended matrix after the first stage of calculations:");
        // for (int i = 0; i < amountOfNodes; i++) //Printing
        // {
        //     for (int j = 0; j < amountOfNodes + 1; j++)
        //     {
        //         Console.Write(coefficents[i, j].ToString("F2", CultureInfo.InvariantCulture));
        //         Console.Write("\t");
        //     }
        //
        //     Console.WriteLine();
        // }
        //
        // Console.WriteLine();

        double[] xi = new double[_amountOfNodes];
        double[] sum = new double[_amountOfNodes];
        xi[_amountOfNodes - 1] = coefficients[_amountOfNodes - 1, _amountOfNodes] /
                                coefficients[_amountOfNodes - 1, _amountOfNodes - 1];
        
        
        for (int i = _amountOfNodes - 2; i >= 0; i--)
        {
            sum[i] = 0;
            for (int k = i + 1; k < _amountOfNodes; k++)
            {
                sum[i] += coefficients[i, k] * xi[k];
            }
        
            xi[i] = (coefficients[i, _amountOfNodes] - sum[i]) / coefficients[i, i];
        }

        //	Elimination Gauss-Crout maybe someday

        return xi;
    }

    public void RunSimulation()
    {
        Console.WriteLine("Time[s]\tMinTemp\tMaxTemp");
        
        int i;
        for (i = 0; i < _conditions.SimulationTime / _conditions.SimulationStepTime; i++)
        {
            Aggregation();

            var calculatedTemperature = CalculateSystem();

            for (int j = 0; j < _conditions.NodesNumber; j++)
            {
                _nodes[j].Temperature = calculatedTemperature[j];
            }

            //Console.WriteLine($"Simulation; Iteration nr: {i + 1}");
            // Console.WriteLine(
            //     $"Time = {(i+1) * Conditions.SimulationStepTime} min_T = {calculatedTemperature.Min()},\tmax_T = {calculatedTemperature.Max()}");
            
            Console.WriteLine(
                ((i+1) * _conditions.SimulationStepTime).ToString(CultureInfo.InvariantCulture) + "\t" +
                calculatedTemperature.Min().ToString("F2", CultureInfo.InvariantCulture) + "\t" +
                calculatedTemperature.Max().ToString("F2", CultureInfo.InvariantCulture));

            //PrintSystem();
            WriteResults(i);
        }
        
        Aggregation();
        WriteResults(i);
    }

    private void WriteResults(int iteration)
    {
        int index = 0;
        string[] lines = new string[_nodes.Count * 2 + _elements.Count * 2 + 15];


        lines[index] = "# vtk DataFile Version 2.0";
        index++;
        lines[index] = "Unstructured Grid Example";
        index++;
        lines[index] = "ASCII";
        index++;
        lines[index] = "DATASET UNSTRUCTURED_GRID";
        index++;
        index++;
        
        lines[index] = $"POINTS {_nodes.Count} float";
        index++;
        
        for (int i = 0; i < _nodes.Count; i++)
        {
            lines[index + i] = _nodes[i].X + " " + _nodes[i].Y + " " + 0;
        }

        index += _nodes.Count;
        index++;
        
        lines[index] = $"CELLS {_elements.Count} {_elements.Count * 5}";
        index++;

        for (int i = 0; i < _elements.Count; i++, index++)
        {
            lines[index] = _elements[i].Nodes.Length + " " + (_elements[i].Nodes[0].ID - 1) + " " +
                           (_elements[i].Nodes[1].ID - 1) + " " + (_elements[i].Nodes[2].ID - 1) + " " + 
                           (_elements[i].Nodes[3].ID - 1);
        }
        
        index++;
        lines[index] = $"CELL_TYPES {_elements.Count}";
        index++;

        for (int i = 0; i < _elements.Count; i++, index++)
        {
            lines[index] = "9";
        }

        index++;
        lines[index] = $"POINT_DATA {_nodes.Count}";
        index++;
        lines[index] = "SCALARS Temp float 1";
        index++;
        lines[index] = "LOOKUP_TABLE default";
        index++;
        
        for (int i = 0; i < _nodes.Count; i++, index++)
        {
            lines[index] = _temperatureVector[i].ToString(CultureInfo.InvariantCulture);
        }
        
        const string directoryPath = "../../../../results/";
        
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        var filePath = Path.Combine(directoryPath, $"Data_{iteration:D3}.vtk");
        File.WriteAllLines(filePath, lines);
    }

    private void ClearGlobalStructures()
    {
        _system.Clear();
        _globalHmatrix.Clear();
        _globalCMatrix.Clear();
        _globalHbcMatrix.Clear();
        _globalPVector.Clear();
        _temperatureVector.Clear();
    }
}