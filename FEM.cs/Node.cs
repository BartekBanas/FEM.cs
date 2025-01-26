using System.Globalization;

namespace FEM.cs;

public class Node
{
    public int ID;
    public double X, Y, Temperature;
    public bool Bc = false;

    public Node()
    {
        ID = -1;
        X = 0;
        Y = 0;
    }
    
    public Node(double x, double y)
    {
        ID = -1;
        X = x;
        Y = y;
    }

    public Node(int id, double x, double y)
    {
        if (id < 0)
            Console.Write("Warning: Given identifier less than 0!");

        this.ID = id;
        this.X = x;
        this.Y = y;
    }

    public void PrintNode()
    {
        Console.Write($"Node; ID: {ID}\tx: ");
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
};