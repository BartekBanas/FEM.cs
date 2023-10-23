using System.Globalization;

namespace FEM_cs;

internal static class Example
{
    private static void Main()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        
        var simulationModel = new SimulationModel();
        
        const string data = "../../../data/dane.txt";       //Path to data
        
        simulationModel.Initialize(data);
        
        simulationModel.PrintConditions();
        simulationModel.PrintNodes();
        simulationModel.PrintElements();

        UniversalElement.Initialize(4);
        
        var simulation = new Simulation(simulationModel);
        simulation.RunSimulation();
    }
}