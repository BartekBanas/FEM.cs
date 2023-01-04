using System.Globalization;

namespace MES_Csharp;

public static class DiscreteElement
{
    public static int IntegralPoints;

    public static double[,] KsiDerivativeTable = new double[,] { };
    public static double[,] EtaDerivativeTable = new double[,] { };
    public static double[] Wages = new double[] { };

    public static double[] Points = new double[] { };
    public static double[,][,] ShapeFunctionMatrix = new double[,][,] { };

    private static double[] _etas = new double[] { };
    private static double[] _ksis = new double[] { };


    public static void Update(int size)
    {
        IntegralPoints = size;
        FillCoordinates();
        MakeWages();

        KsiDerivativeTable = MakeKsiDerivativeTable();
        EtaDerivativeTable = MakeEtaDerivativeTable();
        MakeShapeFunctionMatrix();
    }

    private static void FillCoordinates()
    {
        if (IntegralPoints == 2)
        {
            Points = new[] { -1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
            _etas = new double[] { -1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
            _ksis = new double[] { -1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
        }
        else if (IntegralPoints == 3)
        {
            Points = new[] { -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0) };

            _etas = new double[9]
            {
                -Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, 0, 0,
                Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0)
            };

            _ksis = new double[9]
            {
                -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0),
                -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0)
            };
        }

        if (IntegralPoints == 4)
        {
            Points = new[]
            {
                -Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                -Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0))
            };

            _etas = new double[16];
            for (int i = 0; i < 4; i++)
            {
                _etas[i * 4 + 0] = -Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _etas[i * 4 + 1] = -Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _etas[i * 4 + 2] = Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _etas[i * 4 + 3] = Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
            }

            _ksis = new double[16];
            for (int i = 0; i < 4; i++)
            {
                _ksis[i + 0] = -Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _ksis[i + 4] = -Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _ksis[i + 8] = Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _ksis[i + 12] = Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
            }
        }
    }

    public static void PrintKsiDerivativeTable()
    {
        Console.WriteLine("\nKsi table:");
        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            Console.WriteLine($"{_etas[i]:F8}\t" +
                              $"{KsiDerivativeTable[0, i]:F8}\t" +
                              $"{KsiDerivativeTable[1, i]:F8}\t" +
                              $"{KsiDerivativeTable[2, i]:F8}\t" +
                              $"{KsiDerivativeTable[3, i]:F8}");
        }

        Console.WriteLine();
    }

    public static void PrintEtaDerivativeTable()
    {
        Console.WriteLine("\nEta table:");
        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            Console.WriteLine($"{_ksis[i]:F8}\t" +
                              $"{EtaDerivativeTable[0, i]:F8}\t" +
                              $"{EtaDerivativeTable[1, i]:F8}\t" +
                              $"{EtaDerivativeTable[2, i]:F8}\t" +
                              $"{EtaDerivativeTable[3, i]:F8}");
        }

        Console.WriteLine("\n");
    }

    private static double[,] MakeKsiDerivativeTable()
    {
        int lenght = IntegralPoints * IntegralPoints;
        double[,] resultTable = new double [4, lenght];

        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            resultTable[0, i] = Functions.N1dξ(null,_etas[i]);
            resultTable[1, i] = Functions.N2dξ(null,_etas[i]);
            resultTable[2, i] = Functions.N3dξ(null,_etas[i]);
            resultTable[3, i] = Functions.N4dξ(null,_etas[i]);
        }

        return resultTable;
    }

    private static double[,] MakeEtaDerivativeTable()
    {
        int lenght = IntegralPoints * IntegralPoints;
        double[,] resultTable = new double [4, lenght];

        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            resultTable[0, i] = Functions.N1dη(_ksis[i], null);
            resultTable[1, i] = Functions.N2dη(_ksis[i], null);
            resultTable[2, i] = Functions.N3dη(_ksis[i], null);
            resultTable[3, i] = Functions.N4dη(_ksis[i], null);
        }

        return resultTable;
    }

    private static void MakeWages()
    {
        if (IntegralPoints == 2)
        {
            Wages = new[] { 1.0, 1.0 };
        }

        if (IntegralPoints == 3)
        {
            Wages = new[] { 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };
        }

        if (IntegralPoints == 4)
        {
            Wages = new[]
            {
                (18.0 - Math.Sqrt(30.0)) / 36.0, (18.0 + Math.Sqrt(30.0)) / 36.0,
                (18.0 + Math.Sqrt(30.0)) / 36.0, (18.0 - Math.Sqrt(30.0)) / 36.0
            };
        }
    }

    private static void MakeShapeFunctionMatrix()
    {
        ShapeFunctionMatrix = new double[IntegralPoints, IntegralPoints][,];

        for (int i = 0; i < IntegralPoints; i++)
        {
            for (int j = 0; j < IntegralPoints; j++)
            {
                double[] vector =
                {
                    Functions.N1(Points[i], Points[j]),
                    Functions.N2(Points[i], Points[j]),
                    Functions.N3(Points[i], Points[j]),
                    Functions.N4(Points[i], Points[j])
                };

                ShapeFunctionMatrix[i, j] = Functions.VectorsMultiplication(vector, vector);
            }
        }
    }
    
    public static void PrintShapeFunctionMatrix()
    {
        for (int i = 0; i < IntegralPoints; i++)
        {
            for (int j = 0; j < IntegralPoints; j++)
            {
                Console.WriteLine($"Shape function for point {i * IntegralPoints + j + 1}:");
                for (int l = 0; l < 4; l++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //ShapeFunctionMatrix[i, o][j, k] *= Conditions.SpecificHeat * Conditions.Density;

                        Console.Write(ShapeFunctionMatrix[i, j][l, k].ToString("F4", CultureInfo.InvariantCulture));
                        Console.Write("\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }
    }
}