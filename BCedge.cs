namespace MES_Csharp;

public class BCedge
{
    private readonly int _side;

    private readonly double _jacobianDeterminant;
    private readonly double[,] _integrationPoints = new double[DiscreteElement.IntegralPoints, 2];

    public BCedge(Node node1, Node node2, int side)
    {
        this._side = side;
        
        _jacobianDeterminant = Functions.GetDistance(node1, node2) / 2;
        SetIntegrationPoints();
    }

    private void SetIntegrationPoints()
    {
        switch (_side)
        {
            case 1:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    _integrationPoints[i, 0] = DiscreteElement.Points[i];
                    _integrationPoints[i, 1] = -1;
                }
                break;
            }
            case 2:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    _integrationPoints[i, 0] = 1;
                    _integrationPoints[i, 1] = DiscreteElement.Points[i];
                }
                break;
            }
            case 3:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    _integrationPoints[i, 0] = -DiscreteElement.Points[i];
                    _integrationPoints[i, 1] = 1;
                }
                break;
            }
            case 4:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    _integrationPoints[i, 0] = -1;
                    _integrationPoints[i, 1] = -DiscreteElement.Points[i];
                }
                break;
            }
        }
    }
    
    private double[,] PartialMatrix(double ξ, double η, int index)
    {
        double[] row = { Functions.N1(ξ, η), Functions.N2(ξ, η), Functions.N3(ξ, η), Functions.N4(ξ, η) };

        double[,] matrix = Functions.VectorsMultiplication(row, row);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matrix[i, j] *= DiscreteElement.Wages[index];
            }
        }

        return matrix;
    }

    public double[,] HBCmatrix()
    {
        double[,] hbcMatrix = new double[4,4];
        
        for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
        {
            hbcMatrix = Functions.MatrixSummation(hbcMatrix,
                PartialMatrix(_integrationPoints[i, 0], _integrationPoints[i, 1], i));
        }
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                hbcMatrix[i, j] *= Conditions.α * _jacobianDeterminant;
            }
        }

        return hbcMatrix;
    }
    
    private double[] PartialPVector(double ξ, double η, int index)
    {
        double[] vector = { Functions.N1(ξ, η), Functions.N2(ξ, η), Functions.N3(ξ, η), Functions.N4(ξ, η) };
        
        for (int i = 0; i < 4; i++)
        {
            vector[i] *= DiscreteElement.Wages[index];
        }

        return vector;
    }
    
    public double[] PVector()
    {
        double[] pVector = new double[4];

        for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
        {
            pVector = Functions.VectorSummation(pVector,
                PartialPVector(_integrationPoints[i, 0], _integrationPoints[i, 1], i));
        }
        
        for (int i = 0; i < 4; i++)
        {
            pVector[i] *= Conditions.α * _jacobianDeterminant * Conditions.TemperatureSurr;
        }

        return pVector;
    }
}