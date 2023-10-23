﻿namespace FEM_cs;

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
    public static int Dimension = 2;

    public static void PrintConditions()
    {
        Console.WriteLine("Conditions of the Simulation:\t_____");
        
        Console.WriteLine($"Simulation Time:\t\t{SimulationTime}");
        Console.WriteLine($"Simulation Step Time: \t\t{SimulationStepTime}");
        Console.WriteLine($"Conductivity: \t\t\t{Conductivity}");
        Console.WriteLine($"Heat transfer coefficient alfa: {α}");
        Console.WriteLine($"Ambient Temperature: \t\t{TemperatureSurr}");
        Console.WriteLine($"Initial Temperature: \t\t{TemperatureInitial}");
        Console.WriteLine($"Density of the material: \t{Density}");
        Console.WriteLine($"Specific Heat: \t\t\t{SpecificHeat}");
        Console.WriteLine($"Amount of Nodes: \t\t{NodesNumber}");
        Console.WriteLine($"Amount of Elements: \t\t{ElementsNumber}\n");
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

        SimulationTime =     Convert.ToInt16(words[0][1]);
        SimulationStepTime = Convert.ToInt16(words[1][1]);
        Conductivity =       Convert.ToInt16(words[2][1]);
        α =                  Convert.ToInt16(words[3][1]);
        TemperatureSurr =    Convert.ToInt16(words[4][1]);
        TemperatureInitial = Convert.ToInt16(words[5][1]);
        Density =            Convert.ToInt16(words[6][1]);
        SpecificHeat =       Convert.ToInt16(words[7][1]);
        NodesNumber =        Convert.ToInt16(words[8][2]);
        ElementsNumber =     Convert.ToInt16(words[9][2]);
    }
}