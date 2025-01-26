using Utilities;

namespace FEM.cs;

public class BcEdge
{
    private readonly Conditions _conditions;
    
    private readonly int _side;
    private readonly double _jacobianDeterminant;
    private readonly double[,] _integrationPoints = new double[UniversalElement.IntegralPoints, 2];

    public BcEdge(Node node1, Node node2, int side, Conditions conditions)
    {
        _conditions = conditions;
        _side = side;
        
        _jacobianDeterminant = Node.GetDistance(node1, node2) / 2;
        SetIntegrationPoints();
    }

    private void SetIntegrationPoints()
    {
        switch (_side)
        {
            case 1:
            {
                for (int i = 0; i < UniversalElement.IntegralPoints; i++)
                {
                    _integrationPoints[i, 0] = UniversalElement.Points[i];
                    _integrationPoints[i, 1] = -1;
                }
                break;
            }
            case 2:
            {
                for (int i = 0; i < UniversalElement.IntegralPoints; i++)
                {
                    _integrationPoints[i, 0] = 1;
                    _integrationPoints[i, 1] = UniversalElement.Points[i];
                }
                break;
            }
            case 3:
            {
                for (int i = 0; i < UniversalElement.IntegralPoints; i++)
                {
                    _integrationPoints[i, 0] = -UniversalElement.Points[i];
                    _integrationPoints[i, 1] = 1;
                }
                break;
            }
            case 4:
            {
                for (int i = 0; i < UniversalElement.IntegralPoints; i++)
                {
                    _integrationPoints[i, 0] = -1;
                    _integrationPoints[i, 1] = -UniversalElement.Points[i];
                }
                break;
            }
        }
    }
    
    private double[,] PartialHbcMatrix(double ξ, double η, int index)
    {
        double[] row = [ShapeFunctions.N1(ξ, η), ShapeFunctions.N2(ξ, η), ShapeFunctions.N3(ξ, η), ShapeFunctions.N4(ξ, η)];

        double[,] matrix = AlgebraicUtils.VectorsMultiplication(row, row);
        
        matrix.MultiplyMatrix(UniversalElement.Wages[index]);

        return matrix;
    }

    public double[,] HbcMatrix()
    {
        double[,] hbcMatrix = new double[4,4];
        
        for (int i = 0; i < UniversalElement.IntegralPoints; i++)
        {
            hbcMatrix = AlgebraicUtils.MatrixSummation(hbcMatrix,
                PartialHbcMatrix(_integrationPoints[i, 0], _integrationPoints[i, 1], i));
        }
        
        hbcMatrix.MultiplyMatrix(_conditions.α * _jacobianDeterminant);

        return hbcMatrix;
    }
    
    private double[] PartialPVector(double ξ, double η, int index)
    {
        double[] vector = [ShapeFunctions.N1(ξ, η), ShapeFunctions.N2(ξ, η), ShapeFunctions.N3(ξ, η), ShapeFunctions.N4(ξ, η)];
        
        for (int i = 0; i < 4; i++)
        {
            vector[i] *= UniversalElement.Wages[index];
        }

        return vector;
    }
    
    public double[] PVector()
    {
        double[] pVector = new double[4];

        for (int i = 0; i < UniversalElement.IntegralPoints; i++)
        {
            pVector.AddVector(PartialPVector(_integrationPoints[i, 0], _integrationPoints[i, 1], i));
        }
        
        for (int i = 0; i < 4; i++)
        {
            pVector[i] *= _conditions.α * _jacobianDeterminant * _conditions.TemperatureSurr;
        }

        return pVector;
    }
}