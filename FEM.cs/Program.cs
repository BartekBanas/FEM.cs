using System.Globalization;
using FEM_cs;

CultureInfo.CurrentCulture = new CultureInfo("en-US");

var simulationModel = new SimulationModel();

const string pathToDataFile = "../../../../data/dane.txt";

simulationModel.Initialize(pathToDataFile);

simulationModel.PrintConditions();
simulationModel.PrintNodes();
simulationModel.PrintElements();

UniversalElement.Initialize(4);

var simulation = new Simulation(simulationModel);
simulation.RunSimulation();