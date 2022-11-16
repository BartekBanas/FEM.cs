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
    
    public double[,] Jacobian (DiscreteElement discreteElement, int number)
    {
        //int iterations = discreteElement.integralPoints * discreteElement.integralPoints;
        double dxdξ = 0, dxdη = 0, dydξ = 0, dydη = 0;

        for (int i = 0; i < 4; i++)
        {   
            dxdξ += discreteElement.KsiTable[i, number] * this.points[i].x;
            dxdη += discreteElement.EtaTable[i, number] * this.points[i].x;
            dydξ += discreteElement.KsiTable[i, number] * this.points[i].y;
            dydη += discreteElement.EtaTable[i, number] * this.points[i].y;
        }

        return new[,]
        {
            { dxdξ, dxdη },
            { dydξ, dydη }
        };
    }

    public double[,] HmatrixPartial(DiscreteElement discreteElement, int pointIndex)
    {
        double[,] jacobian = Jacobian(discreteElement, pointIndex);
        // Console.WriteLine("Jacobian: ");
        // Functions.PrintMatrix(jacobian, 2);
        double determinant = Functions.MatrixDeterminant(jacobian);
        //Console.WriteLine($"Jacobian determinant: {determinant}");
        double[,] inversedJacobian = Functions.MatrixInversion(jacobian);
        //Console.WriteLine("Inversed Jacobian: ");
        //Functions.PrintMatrix(inversedJacobian, 2);
        int ip = discreteElement.IntegralPoints * discreteElement.IntegralPoints;

        double[] dNdx = new double [4];
        double[] dNdy = new double [4];
        for (int i = 0; i < 4; i++) 
        {
            dNdx[i] = inversedJacobian[0, 0] * discreteElement.KsiTable[i, pointIndex] +
                      inversedJacobian[0, 1] * discreteElement.EtaTable[i, pointIndex];

            dNdy[i] = inversedJacobian[1, 0] * discreteElement.KsiTable[i, pointIndex] +
                      inversedJacobian[1, 1] * discreteElement.EtaTable[i, pointIndex];
        }

        // for (int i = 0; i < 4; i++)
        // {
        //     Console.WriteLine($"dN{i}/dx: {dNdx[i]}");
        // }   Console.WriteLine();
        // for (int i = 0; i < 4; i++)
        // {
        //     Console.WriteLine($"dN{i}/dy: {dNdy[i]}");
        // }   Console.WriteLine();
        
        
        double[,] hmatrixPartial = Functions.MatrixSummation(Functions.MultiplingSimpleMatrices(dNdx, dNdx, 4),
            Functions.MultiplingSimpleMatrices(dNdy, dNdy, 4), 4);
        //Functions.PrintMatrix(Hmatrix, ip);
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                hmatrixPartial[i, j] *= Conditions.Conductivity * determinant;
            }
        }

        Console.WriteLine($"Hmatrix of point {pointIndex+1}");
        Functions.PrintMatrix(hmatrixPartial, 4);
        return hmatrixPartial;
    }

    public double[,] Hmatrix(DiscreteElement discreteElement)
    {
        double[,] hmatrix = new double[4, 4];

        int pointIndex = 0;
        for (int i = 0; i < discreteElement.IntegralPoints; i++)
        {
            for (int j = 0; j < discreteElement.IntegralPoints; j++)
            {
                double[,] partialHmatrix = HmatrixPartial(discreteElement, pointIndex);

                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        partialHmatrix[k, l] *= discreteElement.Wages[i] * discreteElement.Wages[j];
                    }
                }
                
                hmatrix = Functions.MatrixSummation(hmatrix, partialHmatrix, 4);
                pointIndex++;
            }
        }

        return hmatrix;
    }
};