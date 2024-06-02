using System.Text.Json;

namespace MessageText
{
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
}
