namespace FEM_cs;

public class SimulationModel
{
    public Conditions Conditions { get; } = new();
    public List<Node> Nodes { get; } = new();
    public List<Element> Elements { get; } = new();

    public void Initialize(string pathToDataFile)
    {
        var fileText = File.ReadAllText(pathToDataFile);
        var lines = fileText.Split(Environment.NewLine);
        
        Conditions.ReadConditions(pathToDataFile);

        ReadNodes(lines);
        ReadElements(lines);
        ReadBcs(lines);
    }
    
    public void PrintNodes()
    {
        for (int i = 0; i < Conditions.NodesNumber; i++)
        {
            Nodes[i].PrintNode();
        }   Console.WriteLine();
    }

    public void PrintElements()
    {
        for (int i = 0; i < Conditions.ElementsNumber; i++)
        {
            Elements[i].PrintElement();
        }   Console.WriteLine();
    }

    public void PrintConditions()
    {
        Conditions.PrintConditions();
    }

    private void ReadNodes(IReadOnlyList<string> dataFile)
    {
        var readNodes = new string[Conditions.NodesNumber][];       //Getting a proper bunch of text that'll become nodes
        for (int i = 0; i < Conditions.NodesNumber; i++)
        {
            readNodes[i] = dataFile[i + 11].Trim().Split(',');
        }
        
        for (int i = 0; i < Conditions.NodesNumber; i++)            //Filling list of nodes
        {
            Nodes.Add(new Node());
            Nodes[i].ID = Convert.ToInt16(readNodes[i][0].Trim());
            Nodes[i].X = Convert.ToDouble(readNodes[i][1].Trim());
            Nodes[i].Y = Convert.ToDouble(readNodes[i][2].Trim());
        }
    }

    private void ReadElements(IReadOnlyList<string> dataFile)
    {
        var readElements = new string[Conditions.ElementsNumber][];    //Getting a proper bunch of text that'll become Elements
        for (int i = 0; i < Conditions.ElementsNumber; i++)
        {
            readElements[i] = dataFile[i + 12 + Conditions.NodesNumber].Trim().Split(',');
        }

        for (int i = 0; i < Conditions.ElementsNumber; i++)         //Filling list of Elements
        {
            Elements.Add(new Element(Conditions));

            Elements[i].Id = Convert.ToInt16(readElements[i][0].Trim());

            Elements[i].AddNode(Nodes[Convert.ToInt16(readElements[i][1].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(readElements[i][2].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(readElements[i][3].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(readElements[i][4].Trim()) - 1]);
        }
    }

    private void ReadBcs(IReadOnlyList<string> dataFile)
    {
        var everyBc = dataFile[13 + Conditions.NodesNumber + Conditions.ElementsNumber]
            .Trim()
            .Split(',')
            .Select(expression => Convert.ToInt16(expression.Trim()))
            .Select(point => (int)point).ToList();            //Getting a proper bunch of words that'll become BCs

        foreach (var id in everyBc)         //Setting BC for eligible nodes
        {
            Nodes[id - 1].Bc = true;
        }
    }
}