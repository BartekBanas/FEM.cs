namespace MES_Csharp;

public static class DiscreteElement
{
    public static double[,] KsiTable;
    public static double[,] EtaTable;
    public static double[] Wages = new double[] { };
    
    public static double[] Points;
    
    private static double[] _etas = new double[] { };
    private static double[] _ksis = new double[] { };
    public static int IntegralPoints;
    
    
    public static void Update(int size)
    {
        IntegralPoints = size;
        FillCoordinates();
        MakeWages();

        KsiTable = MakeKsiTable();
        EtaTable = MakeEtaTable();
    }


    private static void FillCoordinates()
    {
        if (IntegralPoints == 2)
        {
            Points = new[] { -1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
            _etas = new double[] {-1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
            _ksis = new double[] {-1 / Math.Sqrt(3), 1 / Math.Sqrt(3), -1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
        }
        else if(IntegralPoints == 3)
        {
            Points = new[] { -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0) };
            
            _etas = new double[9] {-Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, 0, 0, 
                Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0), Math.Sqrt(3.0 / 5.0)};
            
            _ksis = new double[9] {-Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0), 
                -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0), -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0)};
        }
        if (IntegralPoints == 4)
        {
            Points = new[]
            {
                -Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                -Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                Math.Sqrt(3.0 / 7.0 - 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0)),
                Math.Sqrt(3.0 / 7.0 + 2.0 / 7.0 * Math.Sqrt(6.0 / 5.0))
            };
            
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

    private static double[,] MakeKsiTable()
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
    
    private static double[,] MakeEtaTable()
    {
        int lenght = IntegralPoints * IntegralPoints;
        double[,] resultTable = new double [4, lenght];

        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            resultTable[0, i] = Functions.N1dη(_ksis[i]);
            resultTable[1, i] = Functions.N2dη(_ksis[i]);
            resultTable[2, i] = Functions.N3dη(_ksis[i]);
            resultTable[3, i] = Functions.N4dη(_ksis[i]);
        }

        return resultTable;
    }

    public static void PrintKsiTable()
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
    
    public static void PrintEtaTable()
    {
        Console.WriteLine("\nEta table:");
        for (int i = 0; i < IntegralPoints * IntegralPoints; i++)
        {
            Console.WriteLine($"{_ksis[i]:F8}\t" +
                              $"{EtaTable[0, i]:F8}\t" +
                              $"{EtaTable[1, i]:F8}\t" +
                              $"{EtaTable[2, i]:F8}\t" +
                              $"{EtaTable[3, i]:F8}");
        }   Console.WriteLine("\n");
    }

    private static void MakeWages()
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