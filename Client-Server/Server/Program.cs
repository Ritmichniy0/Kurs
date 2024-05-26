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

public class UdpServer
{
	private UdpClient udpServer;
	private IPEndPoint remoteEndPoint;

	public UdpServer(int port)
	{
		udpServer = new UdpClient(port);
		remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
		Console.WriteLine("UDP-сервер запущен по порту " + port);
	}

	public async Task Start()
	{
		while (true)
		{
			var receivedResult = await udpServer.ReceiveAsync();
			byte[] receivedBytes = receivedResult.Buffer;
			string receivedMessageJson = Encoding.UTF8.GetString(receivedBytes);
			Message? receivedMessage = Message.DeserializeFromJson(receivedMessageJson);

			if (receivedMessage != null)
			{
				Console.WriteLine("Получено сообщение от клиента:");
				receivedMessage.Print();

				// Send confirmation to client
				Message confirmationMessage = new Message
				{
					text = "Сообщение",
					dateTime = DateTime.Now,
					nikenameFrom = "Server",
					nikenameTo = receivedMessage.nikenameFrom
				};
				string confirmationJson = confirmationMessage.SerializeMessageTojson();
				byte[] confirmationBytes = Encoding.UTF8.GetBytes(confirmationJson);
				await udpServer.SendAsync(confirmationBytes, confirmationBytes.Length, receivedResult.RemoteEndPoint);
			}
		}
	}

	public static void Main(string[] args)
	{
		UdpServer server = new UdpServer(5000);
		Task.Run(() => server.Start()).Wait();
	}
}
