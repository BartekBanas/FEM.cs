namespace MES_Csharp;

public class DiscreteElement
{
    public double[,] KsiTable;
    public double[,] EtaTable;
    
    double[] _etas = new double[] { };
    double[] _ksis = new double[] { };
    public int integralPoints;
    
     
    

    public DiscreteElement(int size)
    {
        integralPoints = size;
        FillCoordinates();

        KsiTable = MakeKsiTable();
        EtaTable = MakeEtaTable();
    }


    private void FillCoordinates()
    {
        if (integralPoints == 2)
        {
            _etas = new double[4] {-1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
            _ksis = new double[4] {-1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
        }
        else if(integralPoints == 3)
        {
            _etas = new double[9] {-Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, 0, 0, 
                Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0)};
            
            _ksis = new double[9] {-Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0), 
                -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0)};
        }
    }

    double[,] MakeKsiTable()
    {
        int lenght = integralPoints * integralPoints;
        double[,] resultTable = new double [4, lenght];

        for (int i = 0; i < integralPoints * integralPoints; i++)
        {
            resultTable[0, i] = Functions.N1dξ(_etas[i]);
            resultTable[1, i] = Functions.N2dξ(_etas[i]);
            resultTable[2, i] = Functions.N3dξ(_etas[i]);
            resultTable[3, i] = Functions.N4dξ(_etas[i]);
        }

        return resultTable;
    }
    
    double[,] MakeEtaTable()
    {
        int lenght = integralPoints * integralPoints;
        double[,] resultTable = new double [4, lenght];

        for (int i = 0; i < integralPoints * integralPoints; i++)
        {
            resultTable[0, i] = Functions.N1eta(_ksis[i]);
            resultTable[1, i] = Functions.N2eta(_ksis[i]);
            resultTable[2, i] = Functions.N3eta(_ksis[i]);
            resultTable[3, i] = Functions.N4eta(_ksis[i]);
        }

        return resultTable;
    }

    public void PrintKsiTable()
    {
        Console.WriteLine("\nKsi table:");
        for (int i = 0; i < integralPoints * integralPoints; i++)
        {
            Console.WriteLine($"{_etas[i]:F8}\t" +
                              $"{KsiTable[0, i]:F8}\t" +
                              $"{KsiTable[1, i]:F8}\t" +
                              $"{KsiTable[2, i]:F8}\t" +
                              $"{KsiTable[3, i]:F8}");
        }
    }
    
    public void PrintEtaTable()
    {
        Console.WriteLine("\nEta table:");
        for (int i = 0; i < integralPoints * integralPoints; i++)
        {
            Console.WriteLine($"{_ksis[i]:F8}\t" +
                              $"{EtaTable[0, i]:F8}\t" +
                              $"{EtaTable[1, i]:F8}\t" +
                              $"{EtaTable[2, i]:F8}\t" +
                              $"{EtaTable[3, i]:F8}");
        }
    }
}