using System.IO.Abstractions;

namespace FEM.cs;

public class SimulationModel
{
    public IFileSystem FileSystem { get; private set; }
    public IFileInfo InputFile { get; set; }
    public IDirectoryInfo OutputDirectory { get; set; }
    public Conditions Conditions { get; } = new();
    public List<Node> Nodes { get; } = [];
    public List<Element> Elements { get; } = [];

    public SimulationModel(string pathToDataFile, IFileSystem? fileSystem = null)
    {
        FileSystem = fileSystem ?? new FileSystem();
        InputFile = FileSystem.FileInfo.New(pathToDataFile);
        OutputDirectory = FileSystem.DirectoryInfo.New("../../../../results/");
        
        var fileText = FileSystem.File.ReadAllText(pathToDataFile);
        var lines = fileText.Split(Environment.NewLine);
        
        Conditions.ReadConditions(lines);

        ReadNodes(lines);
        ReadElements(lines);
        ReadBcs(lines);
    }
    
    public SimulationModel(IFileInfo inputFile, IDirectoryInfo outputDirectory)
    {
        FileSystem = inputFile.FileSystem;
        InputFile = inputFile;
        OutputDirectory = outputDirectory;
        
        var fileText = FileSystem.File.ReadAllText(inputFile.FullName);
        var lines = fileText.Split(Environment.NewLine);
        
        Conditions.ReadConditions(lines);

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
        Conditions.Print();
    }

    private void ReadNodes(IReadOnlyList<string> dataFile)
    {
        var nodes = new string[Conditions.NodesNumber][];
        
        for (int i = 0; i < Conditions.NodesNumber; i++)
        {
            nodes[i] = dataFile[i + 11].Trim().Split(',');
        }
        
        for (int i = 0; i < Conditions.NodesNumber; i++)
        {
            Nodes.Add(new Node());
            Nodes[i].Id = Convert.ToInt16(nodes[i][0].Trim());
            Nodes[i].X = Convert.ToDouble(nodes[i][1].Trim());
            Nodes[i].Y = Convert.ToDouble(nodes[i][2].Trim());
        }
    }

    private void ReadElements(IReadOnlyList<string> dataFile)
    {
        var elements = new string[Conditions.ElementsNumber][];
        
        for (int i = 0; i < Conditions.ElementsNumber; i++)
        {
            elements[i] = dataFile[i + 12 + Conditions.NodesNumber].Trim().Split(',');
        }

        for (int i = 0; i < Conditions.ElementsNumber; i++)
        {
            Elements.Add(new Element(Conditions));

            Elements[i].Id = Convert.ToInt16(elements[i][0].Trim());

            Elements[i].AddNode(Nodes[Convert.ToInt16(elements[i][1].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(elements[i][2].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(elements[i][3].Trim()) - 1]);
            Elements[i].AddNode(Nodes[Convert.ToInt16(elements[i][4].Trim()) - 1]);
        }
    }

    private void ReadBcs(IReadOnlyList<string> dataFile)
    {
        var bcs = dataFile[13 + Conditions.NodesNumber + Conditions.ElementsNumber]
            .Trim()
            .Split(',')
            .Select(expression => Convert.ToInt16(expression.Trim()))
            .Select(point => (int)point).ToList();

        foreach (var id in bcs)
        {
            Nodes[id - 1].Bc = true;
        }
    }
}