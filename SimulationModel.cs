namespace FEM_cs;

public class SimulationModel
{
    public List<Node> Nodes { get; } = new();
    public List<Element> Elements { get; } = new();

    public void Initialize(string pathToDataFile)
    {
        var fileText = File.ReadAllText(pathToDataFile);          //Preparation for file absorption
        var lines = fileText.Split(Environment.NewLine);

        string[][]
            readNodes = new string[Conditions.NodesNumber][]; //Getting a proper bunch of text that'll become nodes
        for (int i = 0; i < Conditions.NodesNumber; i++)
            readNodes[i] = lines[i + 11].Trim().Split(',');

        
        string[][] readElements =
            new string[Conditions.ElementsNumber][];    //Getting a proper bunch of text that'll become Elements
        for (int i = 0; i < Conditions.ElementsNumber; i++)
            readElements[i] = lines[i + 12 + Conditions.NodesNumber].Trim().Split(',');

        
        List<int> everyBc = new List<int>();                //Getting a proper bunch of words that'll become BCs
        foreach (var word in lines[13 + Conditions.NodesNumber + Conditions.ElementsNumber].Trim().Split(','))
            everyBc.Add(Convert.ToInt16(word.Trim()));
        
        for (int i = 0; i < Conditions.NodesNumber; i++)    //Filling list of nodes
        {
            Nodes.Add(new Node());
            Nodes[i].ID = Convert.ToInt16(readNodes[i][0].Trim());
            Nodes[i].X = Convert.ToDouble(readNodes[i][1].Trim());
            Nodes[i].Y = Convert.ToDouble(readNodes[i][2].Trim());
        }

        for (int i = 0; i < Conditions.ElementsNumber; i++) //Filling list of Elements
        {
            Elements.Add(new Element());

            Elements[i].Id = Convert.ToInt16(readElements[i][0].Trim());

            Elements[i].AddNode(Nodes[Convert.ToInt16(readElements[i][1].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(readElements[i][2].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(readElements[i][3].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(readElements[i][4].Trim()) - 1]);
        }

        foreach (var id in everyBc) //Setting BC for eligible nodes
        {
            Nodes[id - 1].Bc = true;
        }
    }
    
    public void PrintNodeArray()
    {
        for (int i = 0; i < Conditions.NodesNumber; i++)
        {
            Nodes[i].PrintNode();
        }   Console.WriteLine();
    }

    public void PrintElementArray()
    {
        for (int i = 0; i < Conditions.ElementsNumber; i++)
        {
            Elements[i].PrintElement();
        }   Console.WriteLine();
    }
}