using System.Globalization;

namespace FEM_cs;

internal static class Example
{
    private static void Main()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        
        var simulationModel = new SimulationModel();
        
        const string data = "../../../data/dane.txt";       //Path to data
        
        Conditions.ReadConditions(data);
        Conditions.PrintConditions();
        
        simulationModel.Initialize(data);
        
        simulationModel.PrintNodes();
        simulationModel.PrintElements();

        UniversalElement.Initiate(4);
        
        Simulation simulation = new Simulation(simulationModel);
        simulation.RunSimulation();
    }
}