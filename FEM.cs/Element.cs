using Utilities;

namespace FEM.cs;

public class Element
{
    public int Id = -1;
    public readonly Node[] Nodes = new Node[4];

    private readonly Conditions _conditions;
    private readonly int _dimension;
    private int _capacity;

    public Element(Conditions conditions)
    {
        _conditions = conditions;
        _dimension = _conditions.Dimension;
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
            throw new Exception($"Element {Id} has been overloaded");
        }
    }

    public void PrintElement()
    {
        Console.Write($"Element; ID:{Id}\tIncludes Nodes: ");
        for (int j = 0; j < 4; j++)
        {
            Console.Write($"{Nodes[j].Id}");
            if (j < 3)
                Console.Write(", ");
        }

        Console.Write("\n");
    }

    public void PrintNodes()
    {
        for (int j = 0; j < 4; j++)
        {
            Nodes[j].PrintNode();
        }
    }

    private double[,] Jacobian(int pointIndex)
    {
        double dxdξ = 0, dxdη = 0, dydξ = 0, dydη = 0;

        for (int i = 0; i < 4; i++)
        {
            dxdξ += UniversalElement.KsiDerivativeTable[i, pointIndex] * Nodes[i].X;
            dxdη += UniversalElement.EtaDerivativeTable[i, pointIndex] * Nodes[i].X;
            dydξ += UniversalElement.KsiDerivativeTable[i, pointIndex] * Nodes[i].Y;
            dydη += UniversalElement.EtaDerivativeTable[i, pointIndex] * Nodes[i].Y;
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
        double determinant = jacobian.MatrixDeterminant();
        double[,] inversedJacobian = jacobian.MatrixInversion();

        double[] dNdx = new double [_dimension * _dimension];
        double[] dNdy = new double [_dimension * _dimension];

        for (int i = 0; i < _dimension * _dimension; i++)
        {
            dNdx[i] = inversedJacobian[0, 0] * UniversalElement.KsiDerivativeTable[i, pointIndex] +
                      inversedJacobian[0, 1] * UniversalElement.EtaDerivativeTable[i, pointIndex];

            dNdy[i] = inversedJacobian[1, 0] * UniversalElement.KsiDerivativeTable[i, pointIndex] +
                      inversedJacobian[1, 1] * UniversalElement.EtaDerivativeTable[i, pointIndex];
        }

        double[,] hmatrixPartial = AlgebraicUtils.MatrixSummation(
            AlgebraicUtils.VectorsMultiplication(dNdx, dNdx),
            AlgebraicUtils.VectorsMultiplication(dNdy, dNdy));

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                hmatrixPartial[i, j] *= _conditions.Conductivity * determinant;
            }
        }

        return hmatrixPartial;
    }

    public double[,] Hmatrix()
    {
        double[,] hMatrix = new double[_dimension * _dimension, _dimension * _dimension];

        int pointIndex = 0;
        for (int i = 0; i < UniversalElement.IntegralPoints; i++)
        {
            for (int j = 0; j < UniversalElement.IntegralPoints; j++)
            {
                double[,] partialHmatrix = HmatrixPartial(pointIndex);

                for (int k = 0; k < _dimension * _dimension; k++)
                {
                    for (int l = 0; l < _dimension * _dimension; l++)
                    {
                        partialHmatrix[k, l] *= UniversalElement.Wages[i] * UniversalElement.Wages[j];
                    }
                }

                hMatrix.AddMatrix(partialHmatrix);
                pointIndex++;
            }
        }

        return hMatrix;
    }

    public double[,] HbcMatrix()
    {
        double[,] hbcMatrix = new double[4, 4];

        if (Nodes[3].Bc && Nodes[0].Bc)
        {
            hbcMatrix.AddMatrix(new BcEdge(Nodes[3], Nodes[0], 4, _conditions).HbcMatrix());
        }

        for (int i = 0; i < Nodes.Length - 1; i++)
        {
            if (Nodes[i].Bc && Nodes[i + 1].Bc)
            {
                hbcMatrix.AddMatrix(new BcEdge(Nodes[i], Nodes[i + 1], i + 1, _conditions).HbcMatrix());
            }
        }

        return hbcMatrix;
    }

    public double[] PVector()
    {
        double[] pVector = new double[4];

        if (Nodes[3].Bc && Nodes[0].Bc)
        {
            pVector.AddVector(new BcEdge(Nodes[3], Nodes[0], 4, _conditions).PVector());
        }

        for (int i = 0; i < Nodes.Length - 1; i++)
        {
            if (Nodes[i].Bc && Nodes[i + 1].Bc)
            {
                pVector.AddVector(new BcEdge(Nodes[i], Nodes[i + 1], i + 1, _conditions).PVector());
            }
        }

        return pVector;
    }

    public double[,] CMatrix()
    {
        var pointIndex = 0;

        var cMatrix = new double[4, 4];
        var partialMatrix = new double[4, 4];

        for (int i = 0; i < UniversalElement.IntegralPoints; i++)
        {
            for (int j = 0; j < UniversalElement.IntegralPoints; j++, pointIndex++)
            {
                var jacobian = Jacobian(pointIndex);
                var determinant = jacobian.MatrixDeterminant();

                partialMatrix.CopyMatrix(UniversalElement.ShapeFunctionMatrix[i, j]);
                partialMatrix.MultiplyMatrix(UniversalElement.Wages[i] * UniversalElement.Wages[j] *
                                             determinant * _conditions.SpecificHeat * _conditions.Density);

                cMatrix.AddMatrix(partialMatrix);
            }
        }

        return cMatrix;
    }
}