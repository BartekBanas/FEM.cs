using System.Globalization;
using FEM.cs;

CultureInfo.CurrentCulture = new CultureInfo("en-US");

const string pathToDataFile = "../../../../Tests/test_input/example_4x4_1.txt";

var simulationModel = new SimulationModel(pathToDataFile);

simulationModel.PrintConditions();
simulationModel.PrintNodes();
simulationModel.PrintElements();

UniversalElement.Initialize(4);

var simulation = new Simulation(simulationModel);
simulation.RunSimulation();