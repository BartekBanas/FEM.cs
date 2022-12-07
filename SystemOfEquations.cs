using System.Globalization;

namespace MES_Csharp;

public class SystemOfEquations
{
    public double[,] system;
    public double[] globalPvector;
    private List<Element> elements;
    private int amountOfNodes;
    
    public SystemOfEquations(List<Element> elements)
    {   
        this.elements = elements;

        amountOfNodes = elements[^1].nodes[2].ID;
        
        system = new double[amountOfNodes, amountOfNodes];
        globalPvector = new double [amountOfNodes];
        
        Aggregation();
    }


    private void Aggregation()
    {
        Console.WriteLine("System of Equations:");
        foreach (var element in elements)
        {
            double[,] hmatrix = element.Hmatrix();
            double[,] hbcmatrix = element.HBCmatrix();
            double[] pVector = element.Pvector();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    system[element.nodes[i].ID - 1, element.nodes[j].ID - 1] += hmatrix[i, j];
                    system[element.nodes[i].ID - 1, element.nodes[j].ID - 1] += hbcmatrix[i, j];
                    
                }
                globalPvector[element.nodes[i].ID - 1] += pVector[i];
            }
        }
    }

    public void PrintSystem()
    {
        for (int i = 0; i < amountOfNodes; i++)
        {
            for (int j = 0; j < amountOfNodes; j++)
            {
                Console.Write(system[i, j] < 0 ? "-" : " ");

                Console.Write(Math.Abs(system[i, j]).ToString("F2", CultureInfo.InvariantCulture));
                Console.Write("\t");

            }

            Console.Write("*  ");
            Console.Write(Math.Abs(globalPvector[i]).ToString("F2", CultureInfo.InvariantCulture));
            Console.WriteLine();
            
        }   Console.WriteLine();  
    }

    public double[] calculateSystem()
    {
        double[,] coefficents = new double [amountOfNodes, amountOfNodes + 1];
		for (int i = 0; i < amountOfNodes; i++) {
			coefficents[i] = new double[degree + 1];
	}
	
	cout << "The degree of the system of equations is: " << degree << endl << endl;
	cout << "Extended matrix before calculations:" << endl;
	for (unsigned int i = 0; i < degree; i++) {
		for (unsigned int j = 0; j < degree + 1; j++)
		{
			dane >> coefficents[i][j];
			cout << coefficents[i][j] << ",\t";
		}

		cout << endl;
	}

	//	Partial Pivoting
	//for (unsigned int k = 0; k < degree; k++)
	{
		for (unsigned int i = 0; i < degree; i++)
		{
			peak = abs(coefficents[i][i]);
			for (unsigned int j = i; j < degree; j++)
			{
				if (peak < abs(coefficents[j][i]) && (coefficents[j][i]) != 0) {
					peak = abs(coefficents[j][i]);
					peakID = j;
				}

			}
			if (peakID != i) {
				for (unsigned int j = 0; j < degree + 1; j++) {
					swap(coefficents[i][j], coefficents[peakID][j]);
				}
			}
			//cout << i + 1 << " iteration of the matrix" << endl;
			//for (unsigned int k = 0; k < degree; k++) {                //Wypisywanie
			//	for (unsigned int j = 0; j < degree + 1; j++) {
			//		cout << coefficents[k][j] << ",\t";
			//	}
			//	cout << endl;
			//}   cout << endl;
		}
	}	cout << endl << "Extended matrix after Partial Pivoting:" << endl;
	for (unsigned int i = 0; i < degree; i++) {				//Wypisywanie
		for (unsigned int j = 0; j < degree + 1; j++) {
			cout << coefficents[i][j] << ",\t";
		}
		cout << endl;
	}	cout << endl;



	double** multiplier = new double* [degree];
	for (unsigned int i = 1; i < degree; i++) {
		multiplier[i] = new double[2];
		multiplier[i][0] = coefficents[i][0] / coefficents[0][0];
		multiplier[i][1] = coefficents[i][1] / coefficents[1][1];
	}	cout << endl;

	for (unsigned int k = 0; k < degree - 1; k++)
	{
		for (unsigned int i = k + 1; i < degree; i++) {			//Operacje
			for (unsigned int j = k; j <= degree; j++) {
				coefficents[i][j] -= (coefficents[k][j] * multiplier[i][0]);
			}
		}

		for (unsigned int i = k + 2; i < degree; i++) {			//Mnożniki
			multiplier[i][0] = coefficents[i][k + 1] / coefficents[k + 1][k + 1];
		}
	}

	cout << "Extended matrix after the first stage of calculations:" << endl;
	for (unsigned int i = 0; i < degree; i++) {				//Wypisywanie
		for (unsigned int j = 0; j < degree + 1; j++) {
			cout << coefficents[i][j] << ",\t";
		}
		cout << endl;
	}	cout << endl;

	double* xi = new double[degree];
	double* sum = new double[degree];
	xi[degree - 1] = coefficents[degree - 1][degree] / coefficents[degree - 1][degree - 1];
	cout << "Solution of the system of equations:" << endl;

	for (int i = degree - 2; i >= 0; i--) {
		sum[i] = 0;
		for (unsigned int k = i + 1; k < degree; k++) {
			sum[i] += (coefficents[i][k] * xi[k]);
		}

		xi[i] = (coefficents[i][degree] - sum[i]) / coefficents[i][i];
	}

	for (unsigned int i = 0; i < degree; i++)
	{
		cout << "x" << i + 1 << " = " << xi[i] << endl;
	}

	//	Eliminacja Gaussa-Crouta


	dane2 >> degree2;
	unsigned int* Xs = new unsigned int[degree2];
	for (unsigned int i = 0; i < degree2; i++) {
		Xs[i] = i;
	}

	double** secondArray = new double* [degree2];
	for (unsigned int i = 0; i < degree2; i++) {
		secondArray[i] = new double[degree2 + 1];
	}


	cout << endl << endl << "The degree of the second system of equations is: " << degree2 << endl << endl;
	cout << "Extended second matrix before calculations:" << endl;
	for (unsigned int i = 0; i < degree2; i++) {
		for (unsigned int j = 0; j < degree2 + 1; j++)
		{
			dane2 >> secondArray[i][j];
			cout << secondArray[i][j] << ",\t";
		}
		cout << endl;
	}
	
	//for (unsigned int k = 0; k < degree2; k++)
	{
		for (unsigned int i = 0; i < degree2; i++)
		{
			peak = abs(secondArray[i][i]), peakID = i;
			for (unsigned int j = i; j < degree2; j++)
			{
				if (peak < abs(secondArray[i][j]) && (secondArray[j][j]) != 0) {
					peak = abs(secondArray[i][j]);
					peakID = j;
				}
			}
			if (peakID != i) {
				swap(Xs[peakID], Xs[i]);
				for (unsigned int j = 0; j < degree2; j++) {
					swap(secondArray[j][i], secondArray[j][peakID]);
				}	/*
				cout << "Columns " << peakID + 1 << " and " << i + 1 << " have been swapped" << endl;
				for (unsigned int k = 0; k < degree2; k++) {                //Wypisywanie
					for (unsigned int j = 0; j < degree2 + 1; j++) {
						cout << secondArray[k][j] << ",\t";
					}
					cout << endl;
				}   cout << endl;	*/
			}
		}
	}	cout << endl << "Extended second matrix after eliminating zeros from the diagonal:" << endl;
	for (unsigned int i = 0; i < degree2; i++) {				//Wypisywanie
		for (unsigned int j = 0; j < degree2 + 1; j++) {
			cout << secondArray[i][j] << ",\t";
		}
		cout << endl;
	}	cout << endl;

	double** multiplier2 = new double* [degree2];
	for (unsigned int i = 1; i < degree2; i++) {
		multiplier2[i] = new double[2];
		multiplier2[i][0] = secondArray[i][0] / secondArray[0][0];
		multiplier2[i][1] = secondArray[i][1] / secondArray[1][1];
	}	cout << endl;

	for (unsigned int k = 0; k < degree2 - 1; k++)
	{
		for (unsigned int i = k + 1; i < degree2; i++) {			//Operacje
			for (unsigned int j = k; j <= degree2; j++) {
				secondArray[i][j] -= (secondArray[k][j] * multiplier2[i][0]);
			}
		}

		for (unsigned int i = k + 2; i < degree2; i++) {			//Mnożniki
			multiplier2[i][0] = secondArray[i][k + 1] / secondArray[k + 1][k + 1];
		}
	}

	cout << "Extended matrix after the first stage of calculations:" << endl;
	for (unsigned int i = 0; i < degree2; i++) {				//Wypisywanie
		for (unsigned int j = 0; j < degree2 + 1; j++) {
			cout << secondArray[i][j] << ",\t";
		}
		cout << endl;
	}	cout << endl;

	double* xi2 = new double[degree2];
	double* sum2 = new double[degree2];
	xi2[degree2 - 1] = secondArray[degree2 - 1][degree2] / secondArray[degree2 - 1][degree2 - 1];
	

	for (int i = degree2 - 2; i >= 0; i--) {
		sum2[i] = 0;
		for (unsigned int k = i + 1; k < degree2; k++) {
			sum2[i] += (secondArray[i][k] * xi2[k]);
		}

		xi2[i] = (secondArray[i][degree2] - sum2[i]) / secondArray[i][i];
	}

	for (unsigned int i = 0; i < degree2; i++)
	{
		cout << Xs[i] + 1 << "  ";
	}	cout << endl;

	for (unsigned int i = 0; i < degree2; i++) {		//Przypisywanie poprawnych wartości zmiennych
		sum2[Xs[i]] = xi2[i];
	}

	cout << endl << "Solution of the system of equations:" << endl;
	for (unsigned int i = 0; i < degree2; i++)
	{
		cout << "x" << i + 1 << " = " << sum2[i] << endl;
	}
    }
}