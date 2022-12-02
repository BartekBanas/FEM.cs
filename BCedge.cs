namespace MES_Csharp;

public class BCedge
{
    private Node _node1;
    private Node _node2;

    private int side;

    private double _jacobianDeterminant;
    private double[,] integrationPoints = new double[DiscreteElement.IntegralPoints, 2];

    public BCedge(Node node1, Node node2, int side)
    {
        this._node1 = node1;
        this._node2 = node2;
        this.side = side;
        
        _jacobianDeterminant = Functions.GetDistance(node1, node2) / 2;
        setIntegrationPoints();
    }

    private void setIntegrationPoints()
    {
        switch (side)
        {
            case 1:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    integrationPoints[i, 0] = DiscreteElement.Points[i];
                    integrationPoints[i, 1] = -1;
                }
                break;
            }
            case 2:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    integrationPoints[i, 0] = 1;
                    integrationPoints[i, 1] = DiscreteElement.Points[i];
                }
                break;
            }
            case 3:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    integrationPoints[i, 0] = -DiscreteElement.Points[i];
                    integrationPoints[i, 1] = 1;
                }
                break;
            }
            case 4:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    integrationPoints[i, 0] = -1;
                    integrationPoints[i, 1] = -DiscreteElement.Points[i];
                }
                break;
            }
        }
    }
    
    private double[,] partialMatrix(double ξ, double η, int index)
    {
        double[] row = { Functions.N1(ξ, η), Functions.N2(ξ, η), Functions.N3(ξ, η), Functions.N4(ξ, η) };

        double[,] matrix = Functions.MultiplingSimpleMatrices(row, row, 4);
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
        double[,] HBCmatrix = new double[4,4];
        
        for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
        {
            HBCmatrix = Functions.MatrixSummation(HBCmatrix,
                partialMatrix(integrationPoints[i, 0], integrationPoints[i, 1], i), 4);
        }
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                HBCmatrix[i, j] *= Conditions.Conductivity * _jacobianDeterminant;
            }
        }

        return HBCmatrix;
    }
}