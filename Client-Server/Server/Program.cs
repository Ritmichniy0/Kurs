using interfce;
using MessageText;
using NetMQ;
using NetMQ.Sockets;

public class NetMQServer : IMessageSource
{
	private readonly string _address;
	private readonly List<Message> _messages = new List<Message>();

	public static async Task Main(string[] args)
	{
		NetMQServer server = new NetMQServer("tcp://*:5000");
		using (CancellationTokenSource cts = new CancellationTokenSource())
		{
			Task serverTask = server.Start(cts.Token);
			Console.WriteLine("Нажмите ESC для завершения работы сервера.");
			while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
			cts.Cancel();
			await serverTask;
		}
	}

	public NetMQServer(string address)
	{
		_address = address;
	}

	public async Task Start(CancellationToken cancellationToken)
	{
		using (var responseSocket = new ResponseSocket())
		{
			responseSocket.Bind(_address);
			Console.WriteLine("NetMQ-сервер запущен на " + _address);

			try
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					if (responseSocket.TryReceiveFrameString(TimeSpan.FromMilliseconds(100), out string messageJson))
					{
						Message? receivedMessage = Message.DeserializeFromJson(messageJson);

						if (receivedMessage != null)
						{
							if (receivedMessage.text == "List")
							{
								foreach (var msg in _messages)
								{
									if (msg.nikenameTo == receivedMessage.nikenameFrom)
									{
										string messageJsonResponse = msg.SerializeMessageTojson();
										responseSocket.SendFrame(messageJsonResponse);
									}
								}
							}
							else
							{
								Console.WriteLine("Получено сообщение от клиента:");
								receivedMessage.Print();
								_messages.Add(receivedMessage);
							}

							// Отправка подтверждения
							Message confirmationMessage = new Message
							{
								text = "Сообщение получено",
								dateTime = DateTime.Now,
								nikenameFrom = "Server",
								nikenameTo = receivedMessage.nikenameFrom
							};
							string confirmationJson = confirmationMessage.SerializeMessageTojson();
							responseSocket.SendFrame(confirmationJson);
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Сервер остановлен.");
			}
		}
	}
}
