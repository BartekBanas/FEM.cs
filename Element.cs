namespace MES_Csharp;

public class Element
{
    private static int dimension = Conditions.dimension;
    public int ID;
    public int capacity = 0;
    public Node[] nodes = new Node[4];
    
    public Element()
    {
        ID = -1;
    }

    public void AddNode(Node newNode)
    {
        if (capacity < 4)
        {
            nodes[capacity] = newNode;
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
            Console.Write($"{nodes[j].ID}");
            if (j < 3)
                Console.Write(", ");
        }   Console.Write("\n");
    }

    public void PrintNodes()
    {
        for (int j = 0; j < 4; j++)
        {
            nodes[j].PrintNode();
        }
    }

    private double[,] Jacobian (int number)
    {
        //int iterations = discreteElement.integralPoints * discreteElement.integralPoints;
        double dxdξ = 0, dxdη = 0, dydξ = 0, dydη = 0;

        for (int i = 0; i < 4; i++)
        {   
            dxdξ += DiscreteElement.KsiTable[i, number] * this.nodes[i].x;
            dxdη += DiscreteElement.EtaTable[i, number] * this.nodes[i].x;
            dydξ += DiscreteElement.KsiTable[i, number] * this.nodes[i].y;
            dydη += DiscreteElement.EtaTable[i, number] * this.nodes[i].y;
        }

        return new[,]
        {
            { dxdξ, dxdη },
            { dydξ, dydη }
        };
    }

    private double[,] HmatrixPartial(int pointIndex)
    {
        double[,] jacobian = Jacobian(pointIndex);
        // Console.WriteLine("Jacobian: ");
        // Functions.PrintMatrix(jacobian, 2);
        double determinant = Functions.MatrixDeterminant(jacobian);
        //Console.WriteLine($"Jacobian determinant: {determinant}");
        double[,] inversedJacobian = Functions.MatrixInversion(jacobian);
        //Console.WriteLine("Inversed Jacobian: ");
        //Functions.PrintMatrix(inversedJacobian, 2);

        double[] dNdx = new double [dimension * dimension];
        double[] dNdy = new double [dimension * dimension];
        for (int i = 0; i < dimension * dimension; i++) 
        {
            dNdx[i] = inversedJacobian[0, 0] * DiscreteElement.KsiTable[i, pointIndex] +
                      inversedJacobian[0, 1] * DiscreteElement.EtaTable[i, pointIndex];

            dNdy[i] = inversedJacobian[1, 0] * DiscreteElement.KsiTable[i, pointIndex] +
                      inversedJacobian[1, 1] * DiscreteElement.EtaTable[i, pointIndex];
        }

        // for (int i = 0; i < 4; i++)
        // {
        //     Console.WriteLine($"dN{i}/dx: {dNdx[i]}");
        // }   Console.WriteLine();
        // for (int i = 0; i < 4; i++)
        // {
        //     Console.WriteLine($"dN{i}/dy: {dNdy[i]}");
        // }   Console.WriteLine();
        
        
        double[,] hmatrixPartial = Functions.MatrixSummation(
            Functions.MultiplyingVectors(dNdx, dNdx, dimension * dimension),
            Functions.MultiplyingVectors(dNdy, dNdy, dimension * dimension), dimension * dimension);
        //Functions.PrintMatrix(Hmatrix, ip);
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                hmatrixPartial[i, j] *= Conditions.Conductivity * determinant;
            }
        }

        //Console.WriteLine($"Hmatrix of point {pointIndex + 1}");
        //Functions.PrintMatrix(hmatrixPartial, dimension * dimension);
        return hmatrixPartial;
    }

    public double[,] Hmatrix()
    {
        double[,] hmatrix = new double[dimension * dimension, dimension * dimension];

        int pointIndex = 0;
        for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
        {
            for (int j = 0; j < DiscreteElement.IntegralPoints; j++)
            {
                double[,] partialHmatrix = HmatrixPartial(pointIndex);

                for (int k = 0; k < dimension * dimension; k++)
                {
                    for (int l = 0; l < dimension * dimension; l++)
                    {
                        partialHmatrix[k, l] *= DiscreteElement.Wages[i] * DiscreteElement.Wages[j];
                    }
                }
                
                hmatrix = Functions.MatrixSummation(hmatrix, partialHmatrix, dimension * dimension);
                pointIndex++;
            }
        }

        return hmatrix;
    }

    public double[,] HBCmatrix()
    {
        double[,] hBCmatrix = new double[4, 4];

        if (nodes[3].BC && nodes[0].BC)
        {
            hBCmatrix = Functions.MatrixSummation(hBCmatrix, new BCedge(nodes[3], nodes[0], 4).HBCmatrix(), 4);
        }

        for (int i = 0; i < nodes.Length - 1; i++)
        {
            if (nodes[i].BC && nodes[i + 1].BC)
            {
                hBCmatrix = Functions.MatrixSummation(hBCmatrix, new BCedge(nodes[i], nodes[i + 1], i + 1).HBCmatrix(), 4);
            }
        }
        
        //  Printing for debugging
        // Console.WriteLine($"HBCmatrix nr {ID}");
        // Functions.PrintMatrix(hBCmatrix, 4);

        return hBCmatrix;
    }

    public double[] Pvector()
    {
        double[] pVector = new double[4];
        double[] subVector;
        
        if (nodes[3].BC && nodes[0].BC)
        {
            subVector = new BCedge(nodes[3], nodes[0], 4).Pvector();
            for (int i = 0; i < 4; i++)
            {
                pVector[i] += subVector[i];
            }
        }
        
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            if (nodes[i].BC && nodes[i + 1].BC)
            {
                subVector = new BCedge(nodes[i], nodes[i + 1], i + 1).Pvector();
                for (int j = 0; j < 4; j++)
                {
                    pVector[j] += subVector[j];
                }
            }
        }

        return pVector;
    }
};