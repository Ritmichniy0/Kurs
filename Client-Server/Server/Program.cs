using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

public class Message
{
	public string? text { get; set; }
	public DateTime? dateTime { get; set; }
	public string? nikenameFrom { get; set; }
	public string? nikenameTo { get; set; }

	public string SerializeMessageTojson() => JsonSerializer.Serialize(this);

	public static Message? DeserializeFromJson(string message) => JsonSerializer.Deserialize<Message>(message);

	public void Print()
	{
		Console.WriteLine(ToString());
	}

	public override string ToString() => $"{this.dateTime} получено сообщение {this.text} от {this.nikenameFrom}";
}

public class UdpServer
{
	private UdpClient udpServer;
	private IPEndPoint remoteEndPoint;
	private bool isRunning;

	public UdpServer(int port)
	{
		udpServer = new UdpClient(port);
		remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
		Console.WriteLine("UDP-сервер запущен по порту " + port);
	}

	public async Task Start()
	{
		isRunning = true;

		while (isRunning)
		{
			if (udpServer.Available > 0)
			{
				var receivedResult = await udpServer.ReceiveAsync();
				byte[] receivedBytes = receivedResult.Buffer;
				string receivedMessageJson = Encoding.UTF8.GetString(receivedBytes);
				Message? receivedMessage = Message.DeserializeFromJson(receivedMessageJson);

				if (receivedMessage != null)
				{
					Console.WriteLine("Получено сообщение от клиента:");
					receivedMessage.Print();

					Message confirmationMessage = new Message
					{
						text = "Сообщение получено",
						dateTime = DateTime.Now,
						nikenameFrom = "Server",
						nikenameTo = receivedMessage.nikenameFrom
					};
					string confirmationJson = confirmationMessage.SerializeMessageTojson();
					byte[] confirmationBytes = Encoding.UTF8.GetBytes(confirmationJson);
					await udpServer.SendAsync(confirmationBytes, confirmationBytes.Length, receivedResult.RemoteEndPoint);
				}
			}
			await Task.Delay(100);
		}
	}

	public void Stop()
	{
		isRunning = false;
		udpServer.Close();
		Console.WriteLine("UDP-сервер остановлен.");
	}

	public static void Main(string[] args)
	{
		UdpServer server = new UdpServer(5000);
		Task serverTask = Task.Run(() => server.Start());

		Console.WriteLine("Нажмите любую клавишу для завершения работы сервера...");
		Console.ReadKey();

		server.Stop();
		serverTask.Wait();
	}
}