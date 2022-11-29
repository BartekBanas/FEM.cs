using System.Globalization;

namespace MES_Csharp;

public class SystemOfEquations
{
    public double[,] system;
    private List<Element> elements;
    private DiscreteElement discreteElement;
    private int amountOfNodes;
    
    public SystemOfEquations(List<Element> elements, DiscreteElement discreteElement)
    {   
        this.elements = elements;
        this.discreteElement = discreteElement;

        amountOfNodes = elements[^1].points[2].ID;
        
        system = new double[amountOfNodes, amountOfNodes];
        
        Aggregation();
    }


    private void Aggregation()
    {
        foreach (var element in elements)
        {
            double[,] hmatrix = element.Hmatrix(discreteElement);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double value = hmatrix[i, j];
                    system[element.points[i].ID - 1, element.points[j].ID - 1] += value;
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