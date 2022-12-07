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

    public double[] calculateSystem()
    {
        double peak;
        int peakID = 0;

        double[,] coefficents = new double [amountOfNodes, amountOfNodes + 1];

        for (int i = 0; i < amountOfNodes; i++)
        {
            for (int j = 0; j < amountOfNodes; j++)
            {
                coefficents[i, j] = system[i, j];
            }

            coefficents[i, amountOfNodes] = globalPvector[i];
        }

        //	Partial Pivoting
        for (int i = 0; i < amountOfNodes; i++)
        {
            peak = Math.Abs(coefficents[i, i]);
            for (int j = i; j < amountOfNodes; j++)
            {
                if (peak < Math.Abs(coefficents[j, i]) && coefficents[j, i] != 0)
                {
                    peak = Math.Abs(coefficents[j, i]);
                    peakID = j;
                }
            }

            if (peakID != i)
            {
                for (int j = 0; j < amountOfNodes + 1; j++)
                {
                    (coefficents[i, j], coefficents[peakID, j]) = (coefficents[peakID, j], coefficents[i, j]);
                }
            }
            //cout << i + 1 << " iteration of the matrix" << endl;
            //for (unsigned int k = 0; k < degree; k++) {                //Wypisywanie
            //	for (unsigned int j = 0; j < degree + 1; j++) {
            //		cout << coefficents[k][j] << ",\t";
            //	}
            //	cout << endl;
            //}   cout << endl;
        }

        Console.WriteLine("\nExtended matrix after Partial Pivoting:\n");
        for (int i = 0; i < amountOfNodes; i++)
        {
            //Wypisywanie
            for (int j = 0; j < amountOfNodes + 1; j++)
            {
                Console.Write($"{coefficents[i, j]},\t");
            }

            Console.WriteLine();
        }

        Console.WriteLine();


        double[,] multiplier = new double[amountOfNodes, 2];
        for (int i = 1; i < amountOfNodes; i++)
        {
            multiplier[i, 0] = coefficents[i, 0] / coefficents[0, 0];
            multiplier[i, 1] = coefficents[i, 1] / coefficents[1, 1];
        }

        Console.WriteLine();

        for (int k = 0; k < amountOfNodes - 1; k++)
        {
            for (int i = k + 1; i < amountOfNodes; i++)
            {
                //Operacje
                for (int j = k; j <= amountOfNodes; j++)
                {
                    coefficents[i, j] -= (coefficents[k, j] * multiplier[i, 0]);
                }
            }

            for (int i = k + 2; i < amountOfNodes; i++)
            {
                //Mnożniki
                multiplier[i, 0] = coefficents[i, k + 1] / coefficents[k + 1, k + 1];
            }
        }

        Console.WriteLine("Extended matrix after the first stage of calculations:");
        for (int i = 0; i < amountOfNodes; i++)
        {
            //Wypisywanie
            for (int j = 0; j < amountOfNodes + 1; j++)
            {
                Console.Write($"{coefficents[i, j]},\t");
            }

            Console.WriteLine();
        }

        Console.WriteLine();

        double[] xi = new double[amountOfNodes];
        double[] sum = new double[amountOfNodes];
        xi[amountOfNodes - 1] = coefficents[amountOfNodes - 1, amountOfNodes] /
                                coefficents[amountOfNodes - 1, amountOfNodes - 1];
        Console.WriteLine("Solution of the system of equations:");

        for (int i = amountOfNodes - 2; i >= 0; i--)
        {
            sum[i] = 0;
            for (int k = i + 1; k < amountOfNodes; k++)
            {
                sum[i] += coefficents[i, k] * xi[k];
            }

            xi[i] = (coefficents[i, amountOfNodes] - sum[i]) / coefficents[i, i];
        }

        for (int i = 0; i < amountOfNodes; i++)
        {
            Console.WriteLine($"x{i + 1} = {xi[i]}");
        }

        //	Eliminacja Gaussa-Crouta maybe someday

        return xi;
    }
}