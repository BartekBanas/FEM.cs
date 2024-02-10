using System.Globalization;
using FEM.cs;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;

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
        var fileSystem = new MockFileSystem();

        var pathToInputFile = Path.Combine(Utils.GetTestInputDirectory(), "example_4x4_1.txt");
        var pathToOutputDirectory = Path.Combine(Utils.GetTestOutputDirectory(), "4x4_1");

        fileSystem.AddFile(pathToInputFile, File.ReadAllText(pathToInputFile));
        var fileInfo = fileSystem.FileInfo.New(pathToInputFile);
        var outputDirectory = fileSystem.DirectoryInfo.New("results");

        var simulationModel = new SimulationModel(fileInfo, outputDirectory);
        var simulation = new Simulation(simulationModel);

        // Act
        simulation.RunSimulation();

        // Assert
        for (int i = 0; i <= 10; i++)
        {
            var actual = fileSystem.File.ReadAllText("results" + $"/Data_{i:D3}.vtk");
            var expected = File.ReadAllText(pathToOutputDirectory + $"/Data_{i:D3}.test");

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    [Test]
    public void Test4X4Matrix2()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        var pathToInputFile = Path.Combine(Utils.GetTestInputDirectory(), "example_4x4_2.txt");
        var pathToOutputDirectory = Path.Combine(Utils.GetTestOutputDirectory(), "4x4_2");

        fileSystem.AddFile(pathToInputFile, File.ReadAllText(pathToInputFile));
        var fileInfo = fileSystem.FileInfo.New(pathToInputFile);
        var outputDirectory = fileSystem.DirectoryInfo.New("results");

        var simulationModel = new SimulationModel(fileInfo, outputDirectory);
        var simulation = new Simulation(simulationModel);

        // Act
        simulation.RunSimulation();

        // Assert
        for (int i = 0; i <= 10; i++)
        {
            var actual = fileSystem.File.ReadAllText("results" + $"/Data_{i:D3}.vtk");
            var expected = File.ReadAllText(pathToOutputDirectory + $"/Data_{i:D3}.test");

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    [Test]
    public void Test31X31Matrix()
    {
        // Arrange
        var fileSystem = new MockFileSystem();

        var pathToInputFile = Path.Combine(Utils.GetTestInputDirectory(), "example_31x31_1.txt");
        var pathToOutputDirectory = Path.Combine(Utils.GetTestOutputDirectory(), "31x31_1");

        fileSystem.AddFile(pathToInputFile, File.ReadAllText(pathToInputFile));
        var fileInfo = fileSystem.FileInfo.New(pathToInputFile);
        var outputDirectory = fileSystem.DirectoryInfo.New("results");

        var simulationModel = new SimulationModel(fileInfo, outputDirectory);
        var simulation = new Simulation(simulationModel);

        // Act
        simulation.RunSimulation();

        // Assert
        for (int i = 0; i <= 20; i++)
        {
            var actual = fileSystem.File.ReadAllText("results" + $"/Data_{i:D3}.vtk");
            var expected = File.ReadAllText(pathToOutputDirectory + $"/Data_{i:D3}.test");

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}