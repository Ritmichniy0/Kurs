using System;

interface ICalculator
{
	double Calculate(double x, double y);
}

class Addition : ICalculator
{
	public double Calculate(double x, double y) => x + y;
}

class Subtraction : ICalculator
{
	public double Calculate(double x, double y) => x - y;
}

class Multiplication : ICalculator
{
	public double Calculate(double x, double y) => x * y;
}

class Division : ICalculator
{
	public double Calculate(double x, double y)
	{
		if (y == 0)
		{
			Console.WriteLine("Error: Division by zero");
			return 0;
		}
		return x / y;
	}
}

interface IOperationFactory
{
	ICalculator CreateOperation();
}

class AdditionFactory : IOperationFactory
{
	public ICalculator CreateOperation() => new Addition();
}

class SubtractionFactory : IOperationFactory
{
	public ICalculator CreateOperation() => new Subtraction();
}

class MultiplicationFactory : IOperationFactory
{
	public ICalculator CreateOperation() => new Multiplication();
}

class DivisionFactory : IOperationFactory
{
	public ICalculator CreateOperation() => new Division();
}

class Calculator
{
	private ICalculator _operation;

	public Calculator(ICalculator operation)
	{
		_operation = operation ?? throw new ArgumentNullException(nameof(operation));
	}

	public void SetOperation(ICalculator operation)
	{
		_operation = operation ?? throw new ArgumentNullException(nameof(operation));
	}

	public double PerformCalculation(double x, double y)
	{
		return _operation.Calculate(x, y);
	}
}

class Program
{
	static void Main()
	{
		Console.WriteLine("Введите первое значение:");
		if (!double.TryParse(Console.ReadLine(), out double x))
		{
			Console.WriteLine("Недопустимый ввод для второго числа");
			return;
		}

		Console.WriteLine("Введите второе значение:");
		if (!double.TryParse(Console.ReadLine(), out double y))
		{
			Console.WriteLine("Недопустимый ввод для второго числа");
			return;
		}

		Console.WriteLine("Выберите оператор (+, -, *, /):");
		char operationSymbol = Convert.ToChar(Console.ReadLine());

		IOperationFactory factory;
		switch (operationSymbol)
		{
			case '+':
				factory = new AdditionFactory();
				break;
			case '-':
				factory = new SubtractionFactory();
				break;
			case '*':
				factory = new MultiplicationFactory();
				break;
			case '/':
				factory = new DivisionFactory();
				break;
			default:
				Console.WriteLine("Недопустимый ввод");
				return;
		}

		var calculator = new Calculator(factory.CreateOperation());

		double result = calculator.PerformCalculation(x, y);
		Console.WriteLine($"Результат: {result}");
	}
}