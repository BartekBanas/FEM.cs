﻿using System.Globalization;
using Utilities;

namespace FEM.cs;

public static class UniversalElement
{
    public static int IntegralPoints { get; private set; }

    public static double[,] KsiDerivativeTable { get; private set; } = { };
    public static double[,] EtaDerivativeTable { get; private set; } = { };
    public static double[] Wages { get; private set; } = [];

    public static double[] Points { get; private set; } = [];
    public static double[,][,] ShapeFunctionMatrix { get; private set; } = { };

    private static double[] _etas = [];
    private static double[] _ksis = [];

    public static void Initialize(int integralPoints)
    {
        IntegralPoints = integralPoints;
        InitializeCoordinates();
        InitializeWages();

        KsiDerivativeTable = InitializeKsiDerivativeTable();
        EtaDerivativeTable = InitializeEtaDerivativeTable();
        InitializeShapeFunctionMatrix();
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
                        Console.Write(ShapeFunctionMatrix[i, j][l, k].ToString("F4", CultureInfo.InvariantCulture));
                        Console.Write("\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }
    }
    
    private static void InitializeCoordinates()
    {
        switch (IntegralPoints)
        {
            case 2:
            {
                Points = [-1 / Math.Sqrt(3), 1 / Math.Sqrt(3)];
                _etas = [-1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3)];
                _ksis = [-1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3)];
                break;
            }
            
            case 3:
            {
                Points = [-Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0)];

                _etas =
                [
                    -Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, 0, 0,
                    Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0)
                ];

                _ksis =
                [
                    -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0),
                    -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0)
                ];
                break;
            }
            
            case 4:
            {
                Points =
                [
                    -Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                    -Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                    Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                    Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0))
                ];

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

                break;
            }
        }
    }
    
    private static double[,] InitializeKsiDerivativeTable()
    {
        var lenght = IntegralPoints * IntegralPoints;
        var resultTable = new double [4, lenght];

        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            resultTable[0, i] = ShapeFunctions.N1dξ(null, _etas[i]);
            resultTable[1, i] = ShapeFunctions.N2dξ(null, _etas[i]);
            resultTable[2, i] = ShapeFunctions.N3dξ(null, _etas[i]);
            resultTable[3, i] = ShapeFunctions.N4dξ(null, _etas[i]);
        }

        return resultTable;
    }

    private static double[,] InitializeEtaDerivativeTable()
    {
        var lenght = IntegralPoints * IntegralPoints;
        var resultTable = new double [4, lenght];

        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            resultTable[0, i] = ShapeFunctions.N1dη(_ksis[i], null);
            resultTable[1, i] = ShapeFunctions.N2dη(_ksis[i], null);
            resultTable[2, i] = ShapeFunctions.N3dη(_ksis[i], null);
            resultTable[3, i] = ShapeFunctions.N4dη(_ksis[i], null);
        }

        return resultTable;
    }

    private static void InitializeWages()
    {
        Wages = IntegralPoints switch
        {
            2 => [1.0, 1.0],
            3 => [5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0],
            4 =>
            [
                (18.0 - Math.Sqrt(30.0)) / 36.0, (18.0 + Math.Sqrt(30.0)) / 36.0,
                (18.0 + Math.Sqrt(30.0)) / 36.0, (18.0 - Math.Sqrt(30.0)) / 36.0
            ],
            _ => Wages
        };
    }

    private static void InitializeShapeFunctionMatrix()
    {
        ShapeFunctionMatrix = new double[IntegralPoints, IntegralPoints][,];

        for (int i = 0; i < IntegralPoints; i++)
        {
            for (int j = 0; j < IntegralPoints; j++)
            {
                double[] vector =
                [
                    ShapeFunctions.N1(Points[i], Points[j]),
                    ShapeFunctions.N2(Points[i], Points[j]),
                    ShapeFunctions.N3(Points[i], Points[j]),
                    ShapeFunctions.N4(Points[i], Points[j])
                ];

                ShapeFunctionMatrix[i, j] = AlgebraicUtils.VectorsMultiplication(vector, vector);
            }
        }
    }
}