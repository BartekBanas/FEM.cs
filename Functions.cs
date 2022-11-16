using Math = System.Math;

namespace MES_Csharp;

public static class Functions
{
    public static double function11(double x)
    {
        return 2 * (Math.Pow(x, 2)) + 3 * x - 8;
    }
    
    public static double function21(double x, double y)
    {
        return (-2 * Math.Pow(x, 2) * y) + 2 * x * y + 4;
    }

    public static double N1dξ(double η)
    {
        return - (1 - η) / 4;
    }
    public static double N2dξ(double η)
    {
        return  (1 - η) / 4;
    }
    public static double N3dξ(double η)
    {
        return (1 + η) / 4;
    }
    public static double N4dξ(double η)
    {
        return - (1 + η) / 4;
    }
    
    //
    
    public static double N1eta(double ξ)
    {
        return (1 - ξ) * - 1 / 4;
    }
    public static double N2eta(double ξ)
    {
        return (1 + ξ) * - 1 / 4;
    }
    public static double N3eta(double ξ)
    {
        return (1 + ξ) / 4;
    }
    public static double N4eta(double ξ)
    {
        return (1 - ξ) / 4;
    }

    public static double[,] MatrixInversion(double[,] matrix)
    {
        double inverseDeterminant = 1 / MatrixDeterminant(matrix);

        return new double[,]
        {
            { matrix[1, 1] * inverseDeterminant, -matrix[0, 1] * inverseDeterminant },
            { -matrix[1, 0] * inverseDeterminant, matrix[0, 0] * inverseDeterminant }
        };
    }

    public static double MatrixDeterminant(double[,] matrix)
    {
        return matrix[0, 0] + matrix[1, 1] - matrix[0, 1] - matrix[1, 0];
    }

    public static double[,] MultiplingSimpleMatrices(double[] matrixA, double[] matrixB, int size)
    {
        double[,] matrixToReturn = new double[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                matrixToReturn[i, j] = matrixA[i] * matrixB[j];
            }
        }

        return matrixToReturn;
    }

    public static double[,] MatrixSummation(double[,] matrixA, double[,] matrixB, int size)
    {
        double[,] matrixToReturn = new double[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                matrixToReturn[i, j] = matrixA[i, j] + matrixB[i, j];
            }
        }

        return matrixToReturn;
    }
}