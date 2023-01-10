namespace MES_Csharp;

public struct Extension
{
    //Console.WriteLine($"\n" + $"{Functions.N3dη(0.2, 0.6)}");      //Calculate derivative
        
        // simulation.CalculateSystem();

        //DiscreteElement.PrintShapeFunctionMatrix();

        // Element testElement = new Element();
        // testElement.AddNode(new Node(0, 0, 0));
        // testElement.AddNode(new Node(0, 0.025, 0));
        // testElement.AddNode(new Node(0, 0.025, 0.025));
        // testElement.AddNode(new Node(0, 0, 0.025));
        
        
         // Element testElement = new Element();
         // testElement.AddNode(new Node(0, 0));
         // testElement.AddNode(new Node(5, 0));
         // testElement.AddNode(new Node(5, 3));
         // testElement.AddNode(new Node(0, 3));
         //
         // testElement.PrintElement();
         // foreach (var VARIABLE in testElement.Nodes)
         // {
         //     VARIABLE.PrintNode();
         // }
         //
         // Console.WriteLine($"input = {DiscreteElement.Points[1]}");
         //
         // double x = 0, y = 0;
         // double ksi = - 1 / Math.Sqrt(3);
         // double eta = -1 / Math.Sqrt(3);
         // double N1 = 0.4, N2 = 0.2, N3 = 0.15, N4 = 0.25;
        
         // x += Functions.N1(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[0].X;
         // x += Functions.N2(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[1].X;
         // x += Functions.N3(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[2].X;
         // x += Functions.N4(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[3].X;
         // y += Functions.N1(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[0].Y;
         // y += Functions.N2(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[1].Y;
         // y += Functions.N3(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[2].Y;
         // y += Functions.N4(DiscreteElement.Points[1], DiscreteElement.Points[1]) * testElement.Nodes[3].Y;
         
         // x += Functions.N1(ksi, eta) * testElement.Nodes[0].X;
         // x += Functions.N2(ksi, eta) * testElement.Nodes[1].X;
         // x += Functions.N3(ksi, eta) * testElement.Nodes[2].X;
         // x += Functions.N4(ksi, eta) * testElement.Nodes[3].X;
         // y += Functions.N1(ksi, eta) * testElement.Nodes[0].Y;
         // y += Functions.N2(ksi, eta) * testElement.Nodes[1].Y;
         // y += Functions.N3(ksi, eta) * testElement.Nodes[2].Y;
         // y += Functions.N4(ksi, eta) * testElement.Nodes[3].Y;
         
        //  x += N1 * testElement.Nodes[0].X;
        //  x += N2 * testElement.Nodes[1].X;
        //  x += N3 * testElement.Nodes[2].X;
        //  x += N4 * testElement.Nodes[3].X;
        //  y += N1 * testElement.Nodes[0].Y;
        //  y += N2 * testElement.Nodes[1].Y;
        //  y += N3 * testElement.Nodes[2].Y;
        //  y += N4 * testElement.Nodes[3].Y;
        //
        // Console.WriteLine($"x = {x}, y = {y}");
        //
        // Console.WriteLine($"{Functions.FindFromIntegralPoint(45, 122, -0.577)}");

        // BcEdge testEdge = new BcEdge(new Node(0, 0.025, 0.025), new Node(0, 0, 0.025), 4);
        // Functions.PrintMatrix(testEdge.HBCMatrix(), 4);

        // Console.WriteLine($"\n\n");
        // for (int i = 1; i < everyElement.Count; i++)
        // {
        //     Console.WriteLine($"Matrix nr: {i}");
        //     Functions.PrintMatrix(everyElement[i].Hmatrix(discreteElement), 4);
        // }

        //      LICZENIE MACIERZY C
        // double[] randomRow = { 0.1037, 0.0663, 0.3227 ,0.5063 };
        //
        // double[,] m = Functions.VectorsMultiplication(randomRow, randomRow);
        // m.PrintMatrix();
}