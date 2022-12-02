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
                    integrationPoints[i, 0] = DiscreteElement.points[i];
                    integrationPoints[i, 1] = -1;
                }
                break;
            }
            case 2:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    integrationPoints[i, 0] = 1;
                    integrationPoints[i, 1] = DiscreteElement.points[i];
                }
                break;
            }
            case 3:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    integrationPoints[i, 0] = -DiscreteElement.points[i];
                    integrationPoints[i, 1] = 1;
                }
                break;
            }
            case 4:
            {
                for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
                {
                    integrationPoints[i, 0] = -1;
                    integrationPoints[i, 1] = -DiscreteElement.points[i];
                }
                break;
            }
        }
    }
}