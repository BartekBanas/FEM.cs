using System.Globalization;

namespace FEM_cs;

internal static class Example
{
    static void Main()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        
        const string data = "../../../data/Test3_31_31_kwadrat.txt";       //Path to data

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
        

        DiscreteElement.Initiate(4);
        
        // DiscreteElement.PrintKsiDerivativeTable();
        // DiscreteElement.PrintEtaDerivativeTable();
        // Console.WriteLine($"\nIntegral points:");
        // for (int i = 0; i < DiscreteElement.Points.Length; i++)
        // {
        //     Console.Write($"{DiscreteElement.Points[i]}\t");
        // }   Console.WriteLine("\n\n");
        //
        // DiscreteElement.PrintShapeFunctionMatrix();
        

        Simulation simulation = new Simulation(everyElement, everyNode);
        simulation.RunSimulation();
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