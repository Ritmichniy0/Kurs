using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MessageText;

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

	public static async Task Main(string[] args)
	{
		UdpClientApp client = new UdpClientApp("127.0.0.1", 5000);

		Console.WriteLine("Введите 'Exit' для завершения работы.");
		while (true)
		{
			string input = Console.ReadLine();
			if (input.Equals("Exit", StringComparison.OrdinalIgnoreCase))
			{
				break;
			}

			Message message = new Message
			{
				text = input,
				dateTime = DateTime.Now,
				nikenameFrom = "Клиент",
				nikenameTo = "Server"
			};

			await client.SendMessage(message);
		}
	}
}