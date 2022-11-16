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
    
    double[,] Jacobian (DiscreteElement discreteElement, int number)
    {
        double dxdξ = 0, dxdη = 0, dydξ = 0, dydη = 0;

        for (int i = 0; i < 4; i++)
        {   
            dxdξ += discreteElement.KsiTable[number, i] * this.points[i].x;
            dxdη += discreteElement.EtaTable[number, i] * this.points[i].x;
            dydξ += discreteElement.KsiTable[number, i] * this.points[i].y;
            dydη += discreteElement.EtaTable[number, i] * this.points[i].y;
        }

        return new[,]
        {
            { dxdξ, dxdη },
            { dydξ, dydη }
        };
    }

    double[,] Hmatrix(DiscreteElement discreteElement, int pointIndex)
    {
        double[,] inversedJacobian = Functions.MatrixInversion(Jacobian(discreteElement, pointIndex));
        int ip = discreteElement.integralPoints * discreteElement.integralPoints;

        double[] dNdx = new double [ip];
        double[] dNdy = new double [ip];
        for (int i = 0; i < ip; i++)
        {
            dNdx[i] = inversedJacobian[0, 0] * discreteElement.KsiTable[pointIndex, i] +
                      inversedJacobian[0, 1] * discreteElement.EtaTable[pointIndex, i];
            
            dNdy[i] = inversedJacobian[1, 0] * discreteElement.KsiTable[pointIndex, i] +
                      inversedJacobian[1, 1] * discreteElement.EtaTable[pointIndex, i];
        }
        
        
    }
};