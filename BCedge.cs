namespace MES_Csharp;

public class BCedge
{
    private DiscreteElement _discreteElement;
    private Node node1;
    private Node node2;

    private double JacobianDeterminant;

    public BCedge(DiscreteElement discreteElement, Node node1, Node node2)
    {
        _discreteElement = discreteElement;
        this.node1 = node1;
        this.node2 = node2;

        JacobianDeterminant = Functions.GetDistance(node1, node2);
    }
}