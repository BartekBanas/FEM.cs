namespace MES_Csharp;

public class DiscreteElement
{
    public double[,] KsiTable;
    public double[,] EtaTable;
    public double[] Wages = new double[] { };
    
    double[] _etas = new double[] { };
    double[] _ksis = new double[] { };
    public int IntegralPoints;
    
    
    public DiscreteElement(int size)
    {
        IntegralPoints = size;
        FillCoordinates();
        MakeWages();

        KsiTable = MakeKsiTable();
        EtaTable = MakeEtaTable();
    }


    private void FillCoordinates()
    {
        if (IntegralPoints == 2)
        {
            _etas = new double[] {-1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
            _ksis = new double[] {-1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
        }
        else if(IntegralPoints == 3)
        {
            _etas = new double[9] {-Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, 0, 0, 
                Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0)};
            
            _ksis = new double[9] {-Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0), 
                -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0)};
        }
        if (IntegralPoints == 4)
        {
            _etas = new double[16];
            for (int i = 0; i < 4; i++)
            {
                _etas[i * 4 + 0] = -Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _etas[i * 4 + 1] = -Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _etas[i * 4 + 2] =  Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _etas[i * 4 + 3] =  Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
            }
            
            _ksis = new double[16];
            for (int i = 0; i < 4; i++)
            {
                _ksis[i + 0] = -Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _ksis[i + 4] = -Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _ksis[i + 8] =  Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
                _ksis[i + 12] = Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0));
            }
        }
    }

    double[,] MakeKsiTable()
    {
        int lenght = IntegralPoints * IntegralPoints;
        double[,] resultTable = new double [4, lenght];

        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
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
        int lenght = IntegralPoints * IntegralPoints;
        double[,] resultTable = new double [4, lenght];

        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
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
        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            Console.WriteLine($"{_etas[i]:F8}\t" +
                              $"{KsiTable[0, i]:F8}\t" +
                              $"{KsiTable[1, i]:F8}\t" +
                              $"{KsiTable[2, i]:F8}\t" +
                              $"{KsiTable[3, i]:F8}");
        }   Console.WriteLine();
    }
    
    public void PrintEtaTable()
    {
        Console.WriteLine("\nEta table:");
        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            Console.WriteLine($"{_ksis[i]:F8}\t" +
                              $"{EtaTable[0, i]:F8}\t" +
                              $"{EtaTable[1, i]:F8}\t" +
                              $"{EtaTable[2, i]:F8}\t" +
                              $"{EtaTable[3, i]:F8}");
        }   Console.WriteLine();
    }

    void MakeWages()
    {
        if (IntegralPoints == 2)
        {
            Wages = new[] { 1.0, 1.0 };
        }
        if (IntegralPoints == 3)
        {
            Wages = new[] { 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };
        }
        
        if (IntegralPoints == 4)
        {
            Wages = new[]
            {
                (18.0 - Math.Sqrt(30.0)) / 36.0, (18.0 + Math.Sqrt(30.0)) / 36.0,
                (18.0 + Math.Sqrt(30.0)) / 36.0, (18.0 - Math.Sqrt(30.0)) / 36.0
            };
        }
    }
}