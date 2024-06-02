using MessageText;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class UdpServer
{
	private UdpClient udpServer;
	private IPEndPoint remoteEndPoint;
	private List<Message> messages = new List<Message>();

	public UdpServer(int port)
	{
		udpServer = new UdpClient(port);
		remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
		Console.WriteLine("UDP-сервер запущен на порту " + port);
	}

	public async Task Start(CancellationToken cancellationToken)
	{
		try
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				var receivedResult = await udpServer.ReceiveAsync();
				byte[] receivedBytes = receivedResult.Buffer;
				string receivedMessageJson = Encoding.UTF8.GetString(receivedBytes);
				Message? receivedMessage = Message.DeserializeFromJson(receivedMessageJson);

				if (receivedMessage != null)
				{
					if (receivedMessage.text == "List")
					{
						foreach (var msg in messages)
						{
							if (msg.nikenameTo == receivedMessage.nikenameFrom)
							{
								string messageJson = msg.SerializeMessageTojson();
								byte[] messageBytes = Encoding.UTF8.GetBytes(messageJson);
								await udpServer.SendAsync(messageBytes, messageBytes.Length, receivedResult.RemoteEndPoint);
							}
						}
					}
					else
					{
						Console.WriteLine("Получено сообщение от клиента:");
						receivedMessage.Print();
						messages.Add(receivedMessage);
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
					byte[] confirmationBytes = Encoding.UTF8.GetBytes(confirmationJson);
					await udpServer.SendAsync(confirmationBytes, confirmationBytes.Length, receivedResult.RemoteEndPoint);
				}
			}
		}
		catch (OperationCanceledException)
		{
			Console.WriteLine("Сервер остановлен.");
		}
	}

	public static async Task Main(string[] args)
	{
		UdpServer server = new UdpServer(5000);
		using (CancellationTokenSource cts = new CancellationTokenSource())
		{
			Task serverTask = server.Start(cts.Token);
			Console.WriteLine("Нажмите ESC для завершения работы сервера.");
			while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
			cts.Cancel();
			await serverTask;
		}
	}
}
