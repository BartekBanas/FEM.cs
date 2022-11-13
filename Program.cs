using System.Globalization;

namespace MES_Csharp;

internal static class Example
{
    static void Main(string[] args)
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        
        const string data = "dane.txt";         //Path to data

        Conditions.ReadConditions(data);
        Conditions.PrintConditions();

        var fileText = File.ReadAllText(data);          //Preparation for file absorption
        var lines = fileText.Split(Environment.NewLine);

        string[][]
            readNodes = new string[Conditions.NodesNumber][]; //Getting a proper bunch of text that'll become nodes
        for (int i = 0; i < Conditions.NodesNumber; i++)
            readNodes[i] = lines[i + 11].Trim().Split(',');

        string[][]
            readElements =
                new string[Conditions.ElementsNumber][];    //Getting a proper bunch of text that'll become Elements
        for (int i = 0; i < Conditions.ElementsNumber; i++)
            readElements[i] = lines[i + 12 + Conditions.NodesNumber].Trim().Split(',');

        List<int> everyBC = new List<int>();                //Getting a proper bunch of words that'll become BCs
        foreach (var word in lines[13 + Conditions.NodesNumber + Conditions.ElementsNumber].Trim().Split(','))
            everyBC.Add(Convert.ToInt16(word.Trim()));


        List<Node> everyNode = new List<Node> { new Node() };
        List<Element> everyElement = new List<Element> { new Element() };

        for (int i = 0; i < Conditions.NodesNumber; i++)    //Filling list of nodes
        {
            everyNode.Add(new Node());
            everyNode[i + 1].ID = Convert.ToInt16(readNodes[i][0].Trim());
            everyNode[i + 1].x = Convert.ToDouble(readNodes[i][1].Trim());
            everyNode[i + 1].y = Convert.ToDouble(readNodes[i][2].Trim());
        }

        for (int i = 0; i < Conditions.ElementsNumber; i++) //Filling list of Elements
        {
            everyElement.Add(new Element());

            everyElement[i + 1].ID = Convert.ToInt16(readElements[i][0].Trim());

            everyElement[i + 1].AddNode(everyNode[Convert.ToInt16(readElements[i][1].Trim())]);
            everyElement[i + 1].AddNode(everyNode[Convert.ToInt16(readElements[i][2].Trim())]);
            everyElement[i + 1].AddNode(everyNode[Convert.ToInt16(readElements[i][3].Trim())]);
            everyElement[i + 1].AddNode(everyNode[Convert.ToInt16(readElements[i][4].Trim())]);
        }

        foreach (var id in everyBC.Skip(1)) //Setting BC for eligible nodes
        {
            everyNode[id].BC = true;
        }

        PrintNodeArray(everyNode);

        Console.WriteLine();

        PrintElementArray(everyElement);

        Console.WriteLine();
        
        
        Console.WriteLine($"Integral: {Integrate1d2(Functions.function11)}");
        Console.WriteLine($"Integral: {Integrate1d3(Functions.function11)}");
        
        Console.WriteLine($"Integral 2: {Integrate2d2(Functions.function21)}");
        Console.WriteLine($"Integral 2: {Integrate2d3(Functions.function21)}\n");

        DiscreteElement discreteElement = new DiscreteElement(3);
        discreteElement.PrintKsiTable();
        discreteElement.PrintEtaTable();

        for (int i = 1; i < everyElement.Count; i++)
        {
            Jacobian(everyElement[i], discreteElement);
        }
    }

    static void PrintNodeArray(List<Node> array)
    {
        for (int i = 1; i < Conditions.NodesNumber + 1; i++)
        {
            array[i].PrintNode();
        }
    }

    static void PrintElementArray(List<Element> array)
    {
        for (int i = 1; i < Conditions.ElementsNumber + 1; i++)
        {
            array[i].PrintElement();
        }
    }

    static double Integrate1d2(Func<double, double> function)
    {
        double[] nodes = new double[3] { 0, -1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
        
        double result = 0;
        for (int i = 1; i < 3; i++)
        {
            result += function(nodes[i]);
        }
        
        return result;
    }

    static double Integrate1d3(Func<double, double> function)
    {
        double[] nodes = new double[4] { 0,-Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0) };
        double[] coefficients = new double[4] { 0, 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };
        double result = 0;

        for (int i = 1; i <= 3; i++)
        {
            result += function(nodes[i]) * coefficients[i];
        }
        return result;
    }

    static double Integrate2d2(Func<double, double, double> function)
    {
        double[] nodes = new double[3] { 0, -1 / Math.Sqrt(3), 1 / Math.Sqrt(3) };
        double[] coefficients = new double[3] { 0, 1, 1 };
        double result = 0;

        for (int i = 1; i <= 2; i++)
        {
            for (int j = 1; j <= 2; j++)
            {
                result += function(nodes[j], nodes[i]) * coefficients[i] * coefficients[j];
            }
        }

        return result;
    }

    static double Integrate2d3(Func<double, double, double> function)
    {
        double[] nodes = new double[4] { 0, -Math.Sqrt(3.0 / 5.0), 0, Math.Sqrt(3.0 / 5.0) };
        double[] coefficients = new double[4] { 0, 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };
        double result = 0;

        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                result += function(nodes[i], nodes[j]) * coefficients[i] * coefficients[j];
            }
        }

        return result;
    }
    
    static double[] Jacobian (Element element, DiscreteElement discreteElement)
    {
        double dxdξ = 0, dxdη = 0, dydξ = 0, dydη = 0;

        for (int i = 0; i < 4; i++)
        {
            dxdξ += discreteElement.KsiTable[i, 0] * element.points[i].x;
            dxdη += discreteElement.EtaTable[i, 1] * element.points[i].x;
            dydξ += discreteElement.KsiTable[i, 2] * element.points[i].y;
            dydη += discreteElement.EtaTable[i, 3] * element.points[i].y;
        }

        return new[] { dxdξ, dxdη, dydξ, dydη };
    }
}