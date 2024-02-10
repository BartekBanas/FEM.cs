namespace Utilities;

public static class ShapeFunctions
{
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
}