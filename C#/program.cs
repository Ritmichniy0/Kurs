int writeConsole = int.Parse(Console.ReadLine());
ConsoleWrite(writeConsole);

int ConsoleWrite(int number)
{
	Console.WriteLine(number);
	number -= 1;
	if (number != 0)
	{


		return ConsoleWrite(number);
	}
	return 0;
}


int writeConsoleM = int.Parse(Console.ReadLine());
int writeConsoleN = int.Parse(Console.ReadLine());

Console.WriteLine(SumNumbers(writeConsoleM, writeConsoleN));

int SumNumbers(int numberFirst, int numberSecond)
{
	if (numberFirst - 1 != numberSecond)
		return numberSecond + SumNumbers(numberFirst, --numberSecond);
	return 0;

}


int writeConsoleRecursM = int.Parse(Console.ReadLine());
int writeConsoleRecursN = int.Parse(Console.ReadLine());

Console.WriteLine(A(writeConsoleRecursM, writeConsoleRecursN));

static int A(int n, int m)
{
    if(n < 0|| m < 0){
        Console.WriteLine("Отрицательные числа!!!!");
        return 0;
    }
	else if (n == 0)
		return m + 1;
	else if (m == 0)
		return A(n - 1, 1);

	else
		return A(n - 1, A(n, m - 1));
}