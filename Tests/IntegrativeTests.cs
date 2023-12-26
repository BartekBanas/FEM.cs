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
            var received = File.ReadAllText("../../../testOutput_4x4_1/" + $"Data_{i:D3}.test");

            Assert.Equal(received,expected);
        }
    }
    
    [Fact]
    public void Test4X4Matrix2()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        const string pathToDataFile = "../../../testInput/example_4x4_2.txt";
        
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
            var received = File.ReadAllText("../../../testOutput_4x4_2/" + $"Data_{i:D3}.test");

            Assert.Equal(received,expected);
        }
    }
    
    [Fact]
    public void Test31X31Matrix()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        const string pathToDataFile = "../../../../data/example_31x31_1.txt";
        
        UniversalElement.Initialize(4);

        var simulationModel = new SimulationModel();
        simulationModel.Initialize(pathToDataFile);
        var simulation = new Simulation(simulationModel);
        
        // Act
        simulation.RunSimulation();
        
        // Assert
        for (int i = 0; i <= 20; i++)
        {
            var expected = File.ReadAllText("../../../../results/" + $"Data_{i:D3}.vtk");
            var received = File.ReadAllText("../../../testOutput_31x31_1/" + $"Data_{i:D3}.test");

            Assert.Equal(received,expected);
        }
    }
}