using System.Globalization;

namespace MES_Csharp;

public class SystemOfEquations
{
    public double[,] system;
    public double[] globalPvector;
    private List<Element> elements;
    private int amountOfNodes;

    public SystemOfEquations(List<Element> elements)
    {
        this.elements = elements;

        amountOfNodes = elements[^1].nodes[2].ID;

        system = new double[amountOfNodes, amountOfNodes];
        globalPvector = new double [amountOfNodes];

        Aggregation();
    }


    private void Aggregation()
    {
        foreach (var element in elements)
        {
            double[,] hmatrix = element.Hmatrix();
            double[,] hbcmatrix = element.HBCmatrix();
            double[] pVector = element.Pvector();
            double[,] cMatrix = element.Cmatrix();
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    system[element.nodes[i].ID - 1, element.nodes[j].ID - 1] += hmatrix[i, j];
                    system[element.nodes[i].ID - 1, element.nodes[j].ID - 1] += hbcmatrix[i, j];
                }

                globalPvector[element.nodes[i].ID - 1] += pVector[i];
            }
        }
    }

    public void PrintSystem()
    {
        Console.WriteLine("System of Equations:");
        for (int i = 0; i < amountOfNodes; i++)
        {
            for (int j = 0; j < amountOfNodes; j++)
            {
                Console.Write(system[i, j] < 0 ? "-" : " ");

                Console.Write(Math.Abs(system[i, j]).ToString("F2", CultureInfo.InvariantCulture));
                Console.Write("\t");
            }

            Console.Write("*  ");
            Console.Write(globalPvector[i] < 0 ? "-" : " ");
            Console.Write(Math.Abs(globalPvector[i]).ToString("F2", CultureInfo.InvariantCulture));
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public double[] CalculateSystem()
    {
        double epsilon = 1;
        double peak;
        int peakId = 0;

        double[,] coefficients = new double [amountOfNodes, amountOfNodes + 1];

        for (int i = 0; i < amountOfNodes; i++)
        {
            for (int j = 0; j < amountOfNodes; j++)
                coefficients[i, j] = system[i, j];

            coefficients[i, amountOfNodes] = globalPvector[i];
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
        for (int i = 0; i < amountOfNodes; i++)
        {
            peakId = i;
            peak = Math.Abs(coefficients[i, i]);
            for (int j = i; j < amountOfNodes; j++)
            {
                if (peak + epsilon < Math.Abs(coefficients[j, i]) && coefficients[j, i] != 0)
                {
                    peak = Math.Abs(coefficients[j, i]);
                    peakId = j;
                }
            }

            if (peakId != i)
            {
                for (int j = 0; j < amountOfNodes + 1; j++)
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


        double[,] multiplier = new double[amountOfNodes, 2];
        for (int i = 1; i < amountOfNodes; i++)
        {
            multiplier[i, 0] = coefficients[i, 0] / coefficients[0, 0];
            multiplier[i, 1] = coefficients[i, 1] / coefficients[1, 1];
        }

        Console.WriteLine();

        for (int k = 0; k < amountOfNodes - 1; k++)
        {
            for (int i = k + 1; i < amountOfNodes; i++) //Operations
            {
                for (int j = k; j <= amountOfNodes; j++)
                {
                    coefficients[i, j] -= (coefficients[k, j] * multiplier[i, 0]);
                }
            }

            for (int i = k + 2; i < amountOfNodes; i++) //multipliers
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

        double[] xi = new double[amountOfNodes];
        double[] sum = new double[amountOfNodes];
        xi[amountOfNodes - 1] = coefficients[amountOfNodes - 1, amountOfNodes] /
                                coefficients[amountOfNodes - 1, amountOfNodes - 1];
        Console.WriteLine("Solution of the system of equations:");

        for (int i = amountOfNodes - 2; i >= 0; i--)
        {
            sum[i] = 0;
            for (int k = i + 1; k < amountOfNodes; k++)
            {
                sum[i] += coefficients[i, k] * xi[k];
            }

            xi[i] = (coefficients[i, amountOfNodes] - sum[i]) / coefficients[i, i];
        }

        for (int i = 0; i < amountOfNodes; i++)
        {
            Console.WriteLine($"x{i + 1} = {xi[i]}");
        }

        //	Eliminacja Gaussa-Crouta maybe someday

        return xi;
    }
}