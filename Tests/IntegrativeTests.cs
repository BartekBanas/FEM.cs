using System.Globalization;
using FEM.cs;
using NUnit.Framework;

namespace Tests;

public class IntegrativeTests
{
    [SetUp]
    public void SetUp()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        UniversalElement.Initialize(4);
    }

    [Test]
    public void Test4X4Matrix()
    {
        // Arrange
        const string pathToDataFile = "../../../test_input/example_4x4_1.txt";

        var simulationModel = new SimulationModel();
        simulationModel.Initialize(pathToDataFile);
        var simulation = new Simulation(simulationModel);

        // Act
        simulation.RunSimulation();

        // Assert
        for (int i = 0; i <= 10; i++)
        {
            var expected = File.ReadAllText("../../../../results/" + $"Data_{i:D3}.vtk");
            var actual = File.ReadAllText("../../../test_output/4x4_1/" + $"Data_{i:D3}.test");

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    [Test]
    public void Test4X4Matrix2()
    {
        // Arrange
        const string pathToDataFile = "../../../test_input/example_4x4_2.txt";

        var simulationModel = new SimulationModel();
        simulationModel.Initialize(pathToDataFile);
        var simulation = new Simulation(simulationModel);

        // Act
        simulation.RunSimulation();

        // Assert
        for (int i = 0; i <= 10; i++)
        {
            var expected = File.ReadAllText("../../../../results/" + $"Data_{i:D3}.vtk");
            var actual = File.ReadAllText("../../../test_output/4x4_2/" + $"Data_{i:D3}.test");

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    [Test]
    public void Test31X31Matrix()
    {
        // Arrange
        const string pathToDataFile = "../../../test_input/example_31x31_1.txt";

        var simulationModel = new SimulationModel();
        simulationModel.Initialize(pathToDataFile);
        var simulation = new Simulation(simulationModel);

        // Act
        simulation.RunSimulation();

        // Assert
        for (int i = 0; i <= 20; i++)
        {
            var expected = File.ReadAllText("../../../../results/" + $"Data_{i:D3}.vtk");
            var actual = File.ReadAllText("../../../test_output/31x31_1/" + $"Data_{i:D3}.test");

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}