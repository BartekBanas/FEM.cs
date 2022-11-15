namespace MES_Csharp;

public class Element
{
    public int ID;
    public int capacity = 0;
    public Node[] points = new Node[4];
    
    public Element()
    {
        ID = -1;
    }

    public void AddNode(Node newNode)
    {
        if (capacity < 4)
        {
            points[capacity] = newNode;
            capacity++;
        }
        else
        {
            throw new Exception($"Element {ID} has been overloaded");
        }
    }

    public void PrintElement()
    {
        Console.Write($"Element; ID:{ID}\tIncludes Nodes: ");
        for (int j = 0; j < 4; j++)
        {
            Console.Write($"{points[j].ID}");
            if (j < 3)
                Console.Write(", ");
        }   Console.Write("\n");
    }

    public void PrintNodes()
    {
        for (int j = 0; j < 4; j++)
        {
            points[j].PrintNode();
        }
    }
    
    double[] Jacobian (DiscreteElement discreteElement)
    {
        double dxdξ = 0, dxdη = 0, dydξ = 0, dydη = 0;

        for (int i = 0; i < 4; i++)
        {
            dxdξ += discreteElement.KsiTable[i, 0] * this.points[i].x;
            dxdη += discreteElement.EtaTable[i, 1] * this.points[i].x;
            dydξ += discreteElement.KsiTable[i, 2] * this.points[i].y;
            dydη += discreteElement.EtaTable[i, 3] * this.points[i].y;
        }

        return new[] { dxdξ, dxdη, dydξ, dydη };
    }
};