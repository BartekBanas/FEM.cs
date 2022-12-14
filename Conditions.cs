namespace MES_Csharp;

public static class Conditions
{
    public static double SimulationTime;
    public static double SimulationStepTime;
    public static double Conductivity;
    public static double α;
    public static double TemperatureSurr;
    public static double TemperatureInitial;
    public static double Density;
    public static double SpecificHeat;
    public static int NodesNumber;
    public static int ElementsNumber;
    public static int dimension = 2;

    public static void PrintConditions()
    {
        Console.WriteLine("Conditions of the Simulation:\t_____");
        
        Console.WriteLine($"Simulation Time:\t\t{Conditions.SimulationTime}");
        Console.WriteLine($"Simulation Step Time: \t\t{Conditions.SimulationStepTime}");
        Console.WriteLine($"Conductivity: \t\t\t{Conditions.Conductivity}");
        Console.WriteLine($"Heat transfer coefficient alfa: {Conditions.α}");
        Console.WriteLine($"Ambient Temperature: \t\t{Conditions.TemperatureSurr}");
        Console.WriteLine($"Initial Temperature: \t\t{Conditions.TemperatureInitial}");
        Console.WriteLine($"Density of the material: \t{Conditions.Density}");
        Console.WriteLine($"Specific Heat: \t\t\t{Conditions.SpecificHeat}");
        Console.WriteLine($"Amount of Nodes: \t\t{Conditions.NodesNumber}");
        Console.WriteLine($"Amount of Elements: \t\t{Conditions.ElementsNumber}\n");
    }
    
    public static void ReadConditions(string dataPath)
    {
        string data = dataPath;
    
        var fileText = File.ReadAllText(data);
        var lines = fileText.Split(Environment.NewLine);

        string[][] words = new string[10][];
    
        for (int i = 0; i < 10; i++)
        {
            words[i] = lines[i].Trim().Split(' ');
        }

        Conditions.SimulationTime =     Convert.ToInt16(words[0][1]);
        Conditions.SimulationStepTime = Convert.ToInt16(words[1][1]);
        Conditions.Conductivity =       Convert.ToInt16(words[2][1]);
        Conditions.α =                  Convert.ToInt16(words[3][1]);
        Conditions.TemperatureSurr =    Convert.ToInt16(words[4][1]);
        Conditions.TemperatureInitial = Convert.ToInt16(words[5][1]);
        Conditions.Density =            Convert.ToInt16(words[6][1]);
        Conditions.SpecificHeat =       Convert.ToInt16(words[7][1]);
        Conditions.NodesNumber =        Convert.ToInt16(words[8][2]);
        Conditions.ElementsNumber =     Convert.ToInt16(words[9][2]);
    }
}