using System.Globalization;
using FEM.cs;
using Xunit;

namespace Tests;

public class IntegrativeTests
{
    [Fact]
    public void Test4X4Matrix()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        const string pathToDataFile = "../../../../data/example_4x4_1.txt";
        
        UniversalElement.Initialize(4);

        var simulationModel = new SimulationModel();
        simulationModel.Initialize(pathToDataFile);
        var simulation = new Simulation(simulationModel);
        
        // Act
        simulation.RunSimulation();
        
        // Assert
        for (int i = 0; i <= 10; i++)
        {
            var expected = File.ReadAllText("../../../../results/" + $"Data_{i:D3}.vtk");
            var received = File.ReadAllText("../../../testOutput/" + $"Data_{i:D3}.test");

            Assert.Equal(received,expected);
        }
    }
}