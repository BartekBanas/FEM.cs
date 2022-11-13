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
}