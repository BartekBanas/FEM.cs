using System.Globalization;
namespace MES_Csharp;

public class Node
{
    public int ID;
    public double x, y, temperature;
    public bool BC = false;

    public Node()
    {
        ID = 0;
        x = 0;
        y = 0;
    }

    public Node(int id, double x, double y)
    {
        if (id < 0)
            Console.Write("Warning: Given Identicator less than 0!");

        this.ID = id;
        this.x = x;
        this.y = y;
    }

    public void PrintNode()
    {
        Console.Write($"Node; ID: {ID}\tx: ");
        Console.Write(x.ToString("F8", CultureInfo.InvariantCulture));
        Console.Write($"\ty: ");
        Console.Write(y.ToString("F8", CultureInfo.InvariantCulture));
        Console.Write($"\tt: ");
        Console.Write(temperature.ToString("F8", CultureInfo.InvariantCulture));
        Console.Write($"\tBC: {BC}\n");
    }
};