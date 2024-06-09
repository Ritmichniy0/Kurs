using interfce;
using MessageText;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class NetMQClient : IMessageSourceClient
{
	private readonly string _address;

	public static async Task Main(string[] args)
	{
		NetMQClient client = new NetMQClient("tcp://127.0.0.1:5000");

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

			await client.SendMessageAsync(message);
		}
	}


	public NetMQClient(string address)
	{
		_address = address;
	}

	public async Task SendMessageAsync(Message message)
	{
		using (var requestSocket = new RequestSocket())
		{
			requestSocket.Connect(_address);

			string messageJson = message.SerializeMessageTojson();
			requestSocket.SendFrame(messageJson);

			string responseJson = requestSocket.ReceiveFrameString();
			Message? confirmationMessage = Message.DeserializeFromJson(responseJson);

			if (confirmationMessage != null)
			{
				Console.WriteLine("Получено подтверждение от сервера:");
				confirmationMessage.Print();
			}
		}
	}
}

