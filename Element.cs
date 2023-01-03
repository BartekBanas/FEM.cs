namespace MES_Csharp;

public class Element
{
    private static readonly int Dimension = Conditions.Dimension;
    private int _capacity = 0;
    
    public int ID;
    public Node[] Nodes = new Node[4];
    
    public Element()
    {
        ID = -1;
    }

    public void AddNode(Node newNode)
    {
        if (_capacity < 4)
        {
            Nodes[_capacity] = newNode;
            _capacity++;
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
            Console.Write($"{Nodes[j].ID}");
            if (j < 3)
                Console.Write(", ");
        }   Console.Write("\n");
    }

    public void PrintNodes()
    {
        for (int j = 0; j < 4; j++)
        {
            Nodes[j].PrintNode();
        }
    }

    private double[,] Jacobian (int pointIndex)
    {
        double dxdξ = 0, dxdη = 0, dydξ = 0, dydη = 0;

        for (int i = 0; i < 4; i++)
        {   
            dxdξ += DiscreteElement.KsiDerivativeTable[i, pointIndex] * this.Nodes[i].X;
            dxdη += DiscreteElement.EtaDerivativeTable[i, pointIndex] * this.Nodes[i].X;
            dydξ += DiscreteElement.KsiDerivativeTable[i, pointIndex] * this.Nodes[i].Y;
            dydη += DiscreteElement.EtaDerivativeTable[i, pointIndex] * this.Nodes[i].Y;
        }

        double[,] tableToReturn =
        {
            { dxdξ, dxdη },
            { dydξ, dydη }
        };

        return tableToReturn;
    }

    private double[,] HmatrixPartial(int pointIndex)
    {
        double[,] jacobian = Jacobian(pointIndex);
        double determinant = Functions.MatrixDeterminant(jacobian);
        double[,] inversedJacobian = Functions.MatrixInversion(jacobian);

        double[] dNdx = new double [Dimension * Dimension];
        double[] dNdy = new double [Dimension * Dimension];
        for (int i = 0; i < Dimension * Dimension; i++) 
        {
            dNdx[i] = inversedJacobian[0, 0] * DiscreteElement.KsiDerivativeTable[i, pointIndex] +
                      inversedJacobian[0, 1] * DiscreteElement.EtaDerivativeTable[i, pointIndex];

            dNdy[i] = inversedJacobian[1, 0] * DiscreteElement.KsiDerivativeTable[i, pointIndex] +
                      inversedJacobian[1, 1] * DiscreteElement.EtaDerivativeTable[i, pointIndex];
        }

        double[,] hmatrixPartial = Functions.MatrixSummation(
            Functions.VectorsMultiplication(dNdx, dNdx),
            Functions.VectorsMultiplication(dNdy, dNdy));

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                hmatrixPartial[i, j] *= Conditions.Conductivity * determinant;
            }
        }
        
        return hmatrixPartial;
    }

    public double[,] Hmatrix()
    {
        double[,] hmatrix = new double[Dimension * Dimension, Dimension * Dimension];

        int pointIndex = 0;
        for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
        {
            for (int j = 0; j < DiscreteElement.IntegralPoints; j++)
            {
                double[,] partialHmatrix = HmatrixPartial(pointIndex);

                for (int k = 0; k < Dimension * Dimension; k++)
                {
                    for (int l = 0; l < Dimension * Dimension; l++)
                    {
                        partialHmatrix[k, l] *= DiscreteElement.Wages[i] * DiscreteElement.Wages[j];
                    }
                }
                
                hmatrix = Functions.MatrixSummation(hmatrix, partialHmatrix);
                pointIndex++;
            }
        }

        return hmatrix;
    }

    public double[,] HBCmatrix()
    {
        double[,] hBCmatrix = new double[4, 4];

        if (Nodes[3].Bc && Nodes[0].Bc)
        {
            hBCmatrix = Functions.MatrixSummation(hBCmatrix, new BCedge(Nodes[3], Nodes[0], 4).HBCmatrix());
        }

        for (int i = 0; i < Nodes.Length - 1; i++)
        {
            if (Nodes[i].Bc && Nodes[i + 1].Bc)
            {
                hBCmatrix = Functions.MatrixSummation(hBCmatrix, new BCedge(Nodes[i], Nodes[i + 1], i + 1).HBCmatrix());
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

        if (Nodes[3].Bc && Nodes[0].Bc)
        {
            pVector = Functions.VectorSummation(pVector, new BCedge(Nodes[3], Nodes[0], 4).PVector());
        }
        
        for (int i = 0; i < Nodes.Length - 1; i++)
        {
            if (Nodes[i].Bc && Nodes[i + 1].Bc)
            {
                pVector = Functions.VectorSummation(pVector, new BCedge(Nodes[i], Nodes[i + 1], i + 1).PVector());
            }
        }

        return pVector;
    }

    public double[,] Cmatrix()
    {
        int pointIndex = 0;

        double[,] cMatrix = new double[4 ,4];
        double[,] temporary;

        for (int i = 0; i < DiscreteElement.IntegralPoints; i++)
        {
            for (int j = 0; j < DiscreteElement.IntegralPoints; j++, pointIndex++)
            {
                double[,] jacobian = Jacobian(pointIndex);
                double determinant = Functions.MatrixDeterminant(jacobian);
                
                temporary = Functions.CopyMatrix(DiscreteElement.ShapeFunctionMatrix[i, j]);
                temporary = Functions.MultiplyMatrix(temporary,
                    DiscreteElement.Wages[i] * DiscreteElement.Wages[j] * determinant *
                    Conditions.SpecificHeat * Conditions.Density);
                
                cMatrix = Functions.MatrixSummation(cMatrix, temporary);
            }
        }

        return cMatrix;
    }
}