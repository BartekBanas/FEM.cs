﻿using System.Globalization;

namespace MES_Csharp;

public class Simulation
{
    public double[,] System;
    public double[] GlobalPVector;
    private double[,] _globalHmatrix;
    private double[,] _globalCmatrix;
    private double[] _temperatureVector;
    
    private readonly List<Element> _elements;
    private readonly List<Node> _nodes;
    private readonly int _amountOfNodes;

    public Simulation(List<Element> elements, List<Node> nodes)
    {
        _elements = elements;
        _nodes = nodes;

        _amountOfNodes = elements[^1].Nodes[2].ID;

        System = new double[_amountOfNodes, _amountOfNodes];
        _globalHmatrix = new double[_amountOfNodes, _amountOfNodes];
        _globalCmatrix = new double[_amountOfNodes, _amountOfNodes];
        GlobalPVector = new double [_amountOfNodes];
        _temperatureVector = new double[_amountOfNodes];
        foreach (var node in _nodes)
        {
            node.Temperature = Conditions.TemperatureInitial;
        }

        Aggregation();
        
        //RunSimulation();
    }


    private void Aggregation()
    {
        for (int i = 0; i < _amountOfNodes; i++)
        {
            _temperatureVector[i] = _nodes[i].Temperature;
        }
        
        foreach (var element in _elements)
        {
            //hmatrix = Functions.MatrixSummation(hmatrix, element.HBCmatrix());
            double[,] hmatrix = element.Hmatrix();
            double[,] cMatrix = element.Cmatrix();
            double[] pVector = element.Pvector();
            
            // Console.WriteLine("C Matrix:");
            // Functions.PrintMatrix(cMatrix);
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //System[element.Nodes[i].ID - 1, element.Nodes[j].ID - 1] += hmatrix[i, j] + cPerΔτ[i, j];
                    _globalHmatrix[element.Nodes[i].ID - 1, element.Nodes[j].ID - 1] += hmatrix[i, j];
                    _globalCmatrix[element.Nodes[i].ID - 1, element.Nodes[j].ID - 1] += cMatrix[i, j];
                }   GlobalPVector[element.Nodes[i].ID - 1] += pVector[i];
            }
        }
        
        Functions.PrintMatrix(_globalHmatrix);
        Console.WriteLine("C Matrix:");
        Functions.PrintMatrix(_globalCmatrix);

        double[,] cPerΔτ = Functions.MultiplyMatrix(_globalCmatrix, 1 / Conditions.SimulationStepTime);

        double[,] dashMatrix = Functions.MatrixSummation(_globalHmatrix, cPerΔτ);

        GlobalPVector = Functions.VectorSummation
            (GlobalPVector, Functions.MultiplyMatrixByVector(cPerΔτ, _temperatureVector));

        for (int i = 0; i < _amountOfNodes; i++)
        {
            for (int j = 0; j < _amountOfNodes; j++)
            {
                System[i, j] = dashMatrix[i, j];
            }
        }
    }

    public void PrintSystem()
    {
        Console.WriteLine("System of Equations:");
        for (int i = 0; i < _amountOfNodes; i++)
        {
            for (int j = 0; j < _amountOfNodes; j++)
            {
                Console.Write(System[i, j] < 0 ? "-" : " ");

                Console.Write(Math.Abs(System[i, j]).ToString("F2", CultureInfo.InvariantCulture));
                Console.Write("\t");
            }

            Console.Write("*  ");
            Console.Write(GlobalPVector[i] < 0 ? "-" : " ");
            Console.Write(Math.Abs(GlobalPVector[i]).ToString("F2", CultureInfo.InvariantCulture));
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public double[] CalculateSystem()
    {
        double epsilon = 1;
        double peak;
        int peakId = 0;

        double[,] coefficients = new double [_amountOfNodes, _amountOfNodes + 1];

        for (int i = 0; i < _amountOfNodes; i++)
        {
            for (int j = 0; j < _amountOfNodes; j++)
                coefficients[i, j] = System[i, j];

            coefficients[i, _amountOfNodes] = GlobalPVector[i];
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

        Console.WriteLine();

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
        Console.WriteLine("Solution of the system of equations:");

        for (int i = _amountOfNodes - 2; i >= 0; i--)
        {
            sum[i] = 0;
            for (int k = i + 1; k < _amountOfNodes; k++)
            {
                sum[i] += coefficients[i, k] * xi[k];
            }

            xi[i] = (coefficients[i, _amountOfNodes] - sum[i]) / coefficients[i, i];
        }

        for (int i = 0; i < _amountOfNodes; i++)
        {
            Console.WriteLine($"x{i + 1} = {xi[i]}");
        }

        //	Elimination Gauss-Crout maybe someday

        return xi;
    }

    public void RunSimulation()
    {
        for (int i = 0; i < Conditions.SimulationTime / Conditions.SimulationStepTime; i++)
        {
            Aggregation();

            var calculatedTemperature = CalculateSystem();

            for (int j = 0; j < Conditions.NodesNumber; j++)
            {
                _nodes[j].Temperature = calculatedTemperature[j];
            }
            
            Console.WriteLine($"Simulation; Iteration nr: {i + 1}");
        }
    }
}