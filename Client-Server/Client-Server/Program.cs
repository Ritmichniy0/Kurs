using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

public class UdpClientApp
{
	private UdpClient udpClient;
	private IPEndPoint serverEndPoint;

	public UdpClientApp(string serverIp, int serverPort)
	{
		udpClient = new UdpClient();
		serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
	}

	public async Task SendMessage(Message message)
	{
		string messageJson = message.SerializeMessageTojson();
		byte[] messageBytes = Encoding.UTF8.GetBytes(messageJson);
		await udpClient.SendAsync(messageBytes, messageBytes.Length, serverEndPoint);

		// Receive confirmation
		UdpReceiveResult receivedResult = await udpClient.ReceiveAsync();
		byte[] responseBytes = receivedResult.Buffer;
		string responseJson = Encoding.UTF8.GetString(responseBytes);
		Message? confirmationMessage = Message.DeserializeFromJson(responseJson);

		if (confirmationMessage != null)
		{
			Console.WriteLine("Получено подтверждение от сервера:");
			confirmationMessage.Print();
		}
	}

	public static void Main(string[] args)
	{
		UdpClientApp client = new UdpClientApp("127.0.0.1", 5000);

		Message message = new Message
		{
			text = "Привет!",
			dateTime = DateTime.Now,
			nikenameFrom = "Клиент",
			nikenameTo = "Server"
		};

		Task.Run(async () => await client.SendMessage(message)).Wait();
		Console.ReadKey();
	}
}
