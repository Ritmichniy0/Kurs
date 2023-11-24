using PdfProgtam;
using System;
using System.Globalization;
using System.IO;

class Market : Interfaces.QueueBehaviour, Interfaces.MarketBehaviour
{
	private Queue<string> queue = new Queue<string>();
	private Queue<string> orders = new Queue<string>();

	public void Enqueue(string person)
	{
		queue.Enqueue(person);
	}

	public string DequeuePerson()
	{
		if(queue.Count > 0) 
			return queue.Dequeue();
		return null;
	}

	public int Count
	{
		get { return queue.Count; }
	}

	public void AcceptOrder(string order)
	{
		orders.Enqueue(order);
	}

	public string GetNextOrder()
	{
		if(orders.Count > 0)
			return orders.Dequeue();
		return null;
	}

	public int OrdersCount
	{
		get { return orders.Count; }
	}
}

class Programm
{
	public static void Main(string[] args)
	{
		Market market = new Market();

		// Работаем с интерфейсом QueueBehaviour
		market.Enqueue("Ваня");
		market.Enqueue("Петя");
		market.Enqueue("Аня");
		market.Enqueue("Гена");
		market.AcceptOrder("Заказ 1 - Ваня");
		market.AcceptOrder("Заказ 2 - Петя");
		market.AcceptOrder("Заказ 3 - Аня");
		market.AcceptOrder("Заказ 4 - Гена");


		while (market.Count > 0)
		{
			Console.WriteLine("В очереди: " + market.DequeuePerson());
			Console.WriteLine("Оскольк осталось: " + market.Count);

			

			Console.WriteLine("Заказ: " + market.GetNextOrder());
			Console.WriteLine("Количество заказов: " + market.OrdersCount);
			Console.WriteLine("---------------------------------------------------------------------------------------------------------");
		}
	}
}