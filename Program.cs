using System.Globalization;

namespace MES_Csharp;

internal static class Example
{
    static void Main()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        
        const string data = "../../../data/dane.txt";       //Path to data

        Conditions.ReadConditions(data);
        Conditions.PrintConditions();

        var fileText = File.ReadAllText(data);          //Preparation for file absorption
        var lines = fileText.Split(Environment.NewLine);

        string[][]
            readNodes = new string[Conditions.NodesNumber][]; //Getting a proper bunch of text that'll become nodes
        for (int i = 0; i < Conditions.NodesNumber; i++)
            readNodes[i] = lines[i + 11].Trim().Split(',');

        
        string[][] readElements =
                new string[Conditions.ElementsNumber][];    //Getting a proper bunch of text that'll become Elements
        for (int i = 0; i < Conditions.ElementsNumber; i++)
            readElements[i] = lines[i + 12 + Conditions.NodesNumber].Trim().Split(',');

        
        List<int> everyBC = new List<int>();                //Getting a proper bunch of words that'll become BCs
        foreach (var word in lines[13 + Conditions.NodesNumber + Conditions.ElementsNumber].Trim().Split(','))
            everyBC.Add(Convert.ToInt16(word.Trim()));


        List<Node> everyNode = new List<Node> {};
        List<Element> everyElement = new List<Element> {};

        for (int i = 0; i < Conditions.NodesNumber; i++)    //Filling list of nodes
        {
            everyNode.Add(new Node());
            everyNode[i].ID = Convert.ToInt16(readNodes[i][0].Trim());
            everyNode[i].X = Convert.ToDouble(readNodes[i][1].Trim());
            everyNode[i].Y = Convert.ToDouble(readNodes[i][2].Trim());
        }

        for (int i = 0; i < Conditions.ElementsNumber; i++) //Filling list of Elements
        {
            everyElement.Add(new Element());

            everyElement[i].ID = Convert.ToInt16(readElements[i][0].Trim());

            everyElement[i].AddNode(everyNode[Convert.ToInt16(readElements[i][1].Trim()) - 1]);
            everyElement[i].AddNode(everyNode[Convert.ToInt16(readElements[i][2].Trim()) - 1]);
            everyElement[i].AddNode(everyNode[Convert.ToInt16(readElements[i][3].Trim()) - 1]);
            everyElement[i].AddNode(everyNode[Convert.ToInt16(readElements[i][4].Trim()) - 1]);
        }

        foreach (var id in everyBC) //Setting BC for eligible nodes
        {
            everyNode[id - 1].Bc = true;
        }

        PrintNodeArray(everyNode);
        PrintElementArray(everyElement);
        

        DiscreteElement.Update(3);
        DiscreteElement.PrintKsiDerivativeTable();
        DiscreteElement.PrintEtaDerivativeTable();
        Console.WriteLine($"\n\nIntegral points:");
        for (int i = 0; i < DiscreteElement.Points.Length; i++)
        {
            Console.Write($"{DiscreteElement.Points[i]}\t");
        }   Console.WriteLine("\n\n");
        
        DiscreteElement.PrintShapeFunctionMatrix();
        

        Simulation simulation = new Simulation(everyElement, everyNode);
        simulation.PrintSystem();
        
        // simulation.CalculateSystem();

        //DiscreteElement.PrintShapeFunctionMatrix();

        // Element testElement = new Element();
        // testElement.AddNode(new Node(0, 0, 0));
        // testElement.AddNode(new Node(0, 0.025, 0));
        // testElement.AddNode(new Node(0, 0.025, 0.025));
        // testElement.AddNode(new Node(0, 0, 0.025));
        
        
        // Element testElement = new Element();
        // testElement.AddNode(new Node(0, 0));
        // testElement.AddNode(new Node(2, 0));
        // testElement.AddNode(new Node(3, 3));
        // testElement.AddNode(new Node(0, 4));
        //
        //
        //
        // testElement.PrintElement();
        // foreach (var VARIABLE in testElement.Nodes)
        // {
        //     VARIABLE.PrintNode();
        // }
        //
        // Console.WriteLine($"input = {DiscreteElement.Points[1]}");
        //
        // double x = 0, y = 0;
        //
        // x += Functions.N1(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[0].X;
        // x += Functions.N2(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[1].X;
        // x += Functions.N3(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[2].X;
        // x += Functions.N4(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[3].X;
        // y += Functions.N1(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[0].Y;
        // y += Functions.N2(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[1].Y;
        // y += Functions.N3(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[2].Y;
        // y += Functions.N4(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[3].Y;
        //
        // Console.WriteLine($"x = {x}, y = {y}");
        //
        // double t3 = Functions.FindFromIntegralPoint(45, 122, -0.771);
        // Console.WriteLine($"Result is: {t3}");


        // BCedge testEdge = new BCedge(new Node(0, 0.025, 0.025), new Node(0, 0, 0.025), 4);
        // Functions.PrintMatrix(testEdge.HBCmatrix(), 4);

        // Console.WriteLine($"\n\n");
        // for (int i = 1; i < everyElement.Count; i++)
        // {
        //     Console.WriteLine($"Matrix nr: {i}");
        //     Functions.PrintMatrix(everyElement[i].Hmatrix(discreteElement), 4);
        // }
    }
    

    static void PrintNodeArray(List<Node> array)
    {
        for (int i = 0; i < Conditions.NodesNumber; i++)
        {
            array[i].PrintNode();
        }   Console.WriteLine();
    }

    static void PrintElementArray(List<Element> array)
    {
        for (int i = 0; i < Conditions.ElementsNumber; i++)
        {
            array[i].PrintElement();
        }   Console.WriteLine();
    }
}