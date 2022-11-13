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

    Node(int _ID, int _x, int _y)
    {
        if (_ID < 0)
            Console.Write("Warning: Given Identicator less than 0!");

        ID = _ID;
        x = _x;
        y = _y;
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