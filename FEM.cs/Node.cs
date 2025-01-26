using System.Globalization;

namespace FEM.cs;

public class Node
{
    public int Id;
    public double X, Y, Temperature;
    public bool Bc = false;

    public Node()
    {
        Id = -1;
        X = 0;
        Y = 0;
    }
    
    public Node(double x, double y)
    {
        Id = -1;
        X = x;
        Y = y;
    }

    public Node(uint id, double x, double y)
    {
        Id = (int)id;
        X = x;
        Y = y;
    }

    public void PrintNode()
    {
        Console.Write($"Node; ID: {Id}\tx: ");
        Console.Write(X.ToString("F8", CultureInfo.InvariantCulture));
        Console.Write($"\ty: ");
        Console.Write(Y.ToString("F8", CultureInfo.InvariantCulture));
        Console.Write($"\tt: ");
        Console.Write(Temperature.ToString("F8", CultureInfo.InvariantCulture));
        Console.Write($"\tBC: {Bc}\n");
    }
    
    public static double GetDistance(Node node1, Node node2)
    {
        return Math.Sqrt(Math.Pow(node2.X - node1.X, 2) + Math.Pow(node2.Y - node1.Y, 2));
    }
}