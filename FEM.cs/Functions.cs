﻿using System.Globalization;

namespace FEM.cs;

public static class Functions
{
    //  Shape functions
    
    public static double N1(double ξ, double η)
    {
        return (1 - ξ) * (1 - η) / 4;
    }
    public static double N2(double ξ, double η)
    {
        return  (1 + ξ) * (1 - η) / 4;
    }
    public static double N3(double ξ, double η)
    {
        return (1 + ξ) * (1 + η) / 4;
    }
    public static double N4(double ξ, double η)
    {
        return (1 - ξ) * (1 + η) / 4;
    }
    
    //  Derivatives of Shape Functions 
    
    public static double N1dξ(double? ξ, double η)
    {
        return - (1 - η) / 4;
    }
    public static double N2dξ(double? ξ, double η)
    {
        return  (1 - η) / 4;
    }
    public static double N3dξ(double? ξ, double η)
    {
        return (1 + η) / 4;
    }
    public static double N4dξ(double? ξ, double η)
    {
        return - (1 + η) / 4;
    }
    
    
    public static double N1dη(double ξ, double? η = 0)
    {
        return (1 - ξ) * - 1 / 4;
    }
    public static double N2dη(double ξ, double? η = 0)
    {
        return (1 + ξ) * - 1 / 4;
    }
    public static double N3dη(double ξ, double? η = 0)
    {
        return (1 + ξ) / 4;
    }
    public static double N4dη(double ξ, double? η = 0)
    {
        return (1 - ξ) / 4;
    }

    public static double FindFromIntegralPoint(double value1, double value2, double ksi)
    {
        var wage1 = Math.Abs(ksi - 1) / 2;
        var wage2 = Math.Abs(ksi + 1) / 2;
        
        return value1 * wage1 + value2 * wage2;
    }
    
    //  Mathematical operations

    public static double[,] MatrixInversion(this double[,] matrix)
    {
        var inverseDeterminant = 1 / MatrixDeterminant(matrix);

        return new[,]
        {
            { matrix[1, 1] * inverseDeterminant, -matrix[0, 1] * inverseDeterminant },
            { -matrix[1, 0] * inverseDeterminant, matrix[0, 0] * inverseDeterminant }
        };
    }

    public static double MatrixDeterminant(this double[,] matrix)
    {
        return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
    }

    public static double[,] VectorsMultiplication(double[] column, double[] row)
    {
        var matrixToReturn = new double[column.Length, column.Length];

        for (int i = 0; i < column.Length; i++)
        {
            for (int j = 0; j < column.Length; j++)
            {
                matrixToReturn[i, j] = column[i] * row[j];
            }
        }

        return matrixToReturn;
    }
    
    public static void AddMatrix(this double[,] matrixA, double[,] matrixB)
    {
        var lenght = Convert.ToInt32(Math.Sqrt(matrixA.Length));

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrixA[i, j] += matrixB[i, j];
            }
        }
    }
    
    public static double[,] MatrixSummation(double[,] matrixA, double[,] matrixB)
    {
        var lenght = Convert.ToInt32(Math.Sqrt(matrixA.Length));
        var matrixToReturn = new double[lenght, lenght];

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrixToReturn[i, j] = matrixA[i, j] + matrixB[i, j];
            }
        }

        return matrixToReturn;
    }
    
    public static double[,] MatrixSummation(double[,] matrixA, double[,] matrixB, double[,] matrixC)
    {
        var lenght = Convert.ToInt32(Math.Sqrt(matrixA.Length));
        var matrixToReturn = new double[lenght, lenght];

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrixToReturn[i, j] = matrixA[i, j] + matrixB[i, j] + matrixC[i, j] ;
            }
        }

        return matrixToReturn;
    }
    
    public static double[,] MultiplyMatrix(this double[,] matrix, double multiplier)
    {
        var lenght = Convert.ToInt32(Math.Sqrt(matrix.Length));
        var matrixToReturn = new double[lenght, lenght];

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrixToReturn[i, j] = matrix[i, j] * multiplier;
            }
        }

        return matrixToReturn;
    }
    
    public static void PrintMatrix(this double[,] matrix)
    {
        var lenght = Convert.ToInt32(Math.Sqrt(matrix.Length));
        
        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                Console.Write($"{matrix[i, j].ToString("F2", CultureInfo.InvariantCulture)}\t");
            }   Console.WriteLine();
        }   Console.WriteLine();
    }

    public static void AddVector(this double[] vector, double[] vectorToAdd)
    {
        for (int i = 0; i < vector.Length; i++)
        {
            vector[i] += vectorToAdd[i];
        }
    }

    public static double[] VectorSummation(double[] vectorA, double[] vectorB)
    {
        var vectorToReturn = new double [vectorA.Length];
        for (int i = 0; i < vectorA.Length; i++)
        {
            vectorToReturn[i] = vectorA[i] + vectorB[i];
        }

        return vectorToReturn;
    }

    public static double[] MultiplyVector(this double[] vector, double multiplier)
    {
        for (int i = 0; i < vector.Length; i++)
        {
            vector[i] *= multiplier;
        }

        return vector;
    }
    
    public static double GetDistance(Node node1, Node node2)
    {
        return Math.Sqrt(Math.Pow(node2.X - node1.X, 2) + Math.Pow(node2.Y - node1.Y, 2));
    }

    public static double[,] CopyMatrix(this double[,] matrix)
    {
        var lenght = Convert.ToInt32(Math.Sqrt(matrix.Length));
        var matrixToReturn = new double[lenght, lenght];

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrixToReturn[i, j] = matrix[i, j];
            }
        }

        return matrixToReturn;
    }
    
    public static void CopyMatrix(this double[,] matrix, double[,] copiedMatrix)
    {
        var lenght = Convert.ToInt32(Math.Sqrt(copiedMatrix.Length));

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrix[i, j] = copiedMatrix[i, j];
            }
        }
    }

    public static void Clear(this double[,] matrix)
    {
        var lenght = Convert.ToInt32(Math.Sqrt(matrix.Length));

        for (int i = 0; i < lenght; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrix[i, j] = 0;
            }
        }
    }
    
    public static void Clear(this double[] vector)
    {
        var lenght = vector.Length;

        for (int i = 0; i < lenght; i++)
        {
            vector[i] = 0;
        }
    }

    public static void Print(this IEnumerable<double> vector)
    {
        foreach (var number in vector)
        {
            Console.WriteLine($"{number}");
        }
    }
    
    public static double[] MultiplyMatrixByVector(double[,] matrix, double[] vector)
    {
        var length = vector.Length;
        var vectorToReturn = new double[length];

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                vectorToReturn[i] += matrix[i, j] * vector[j];
            }
        }

        return vectorToReturn;
    }
}