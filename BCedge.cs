namespace MES_Csharp;

public class BCedge
{
    private Node _node1;
    private Node _node2;

    private double _jacobianDeterminant;

    public BCedge(Node node1, Node node2)
    {
        this._node1 = node1;
        this._node2 = node2;
        

        _jacobianDeterminant = Functions.GetDistance(node1, node2) / 2;
    }
}