using System.Globalization;

namespace MES_Csharp;

public class SystemOfEquations
{
    public double[,] system;
    private List<Element> elements;
    private int amountOfNodes;
    
    public SystemOfEquations(List<Element> elements)
    {   
        this.elements = elements;

        amountOfNodes = elements[^1].nodes[2].ID;
        
        system = new double[amountOfNodes, amountOfNodes];
        
        Aggregation();
    }


    private void Aggregation()
    {
        foreach (var element in elements)
        {
            double[,] hmatrix = element.Hmatrix();
            double[,] hbcmatrix = element.HBCmatrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //double value = hmatrix[i, j];
                    system[element.nodes[i].ID - 1, element.nodes[j].ID - 1] += hmatrix[i, j];
                    system[element.nodes[i].ID - 1, element.nodes[j].ID - 1] += hbcmatrix[i, j];
                }
            }
        }
    }

    public void PrintSystem()
    {
        for (int i = 0; i < amountOfNodes; i++)
        {
            for (int j = 0; j < amountOfNodes; j++)
            {
                Console.Write(system[i, j] < 0 ? "-" : " ");

                Console.Write(Math.Abs(system[i, j]).ToString("F2", CultureInfo.InvariantCulture));
                Console.Write("\t");

            }   Console.WriteLine();
        }   
    }
}