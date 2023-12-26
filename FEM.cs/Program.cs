using System.Globalization;
using FEM.cs;

CultureInfo.CurrentCulture = new CultureInfo("en-US");

var simulationModel = new SimulationModel();

const string pathToDataFile = "../../../../data/example_4x4_1.txt";

simulationModel.Initialize(pathToDataFile);

simulationModel.PrintConditions();
simulationModel.PrintNodes();
simulationModel.PrintElements();

UniversalElement.Initialize(4);

var simulation = new Simulation(simulationModel);
simulation.RunSimulation();