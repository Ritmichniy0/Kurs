using MessageText;
using System;
namespace interfce
{
	public interface IMessageSource
	{
		Task Start(CancellationToken cancellationToken);
	}

	public interface IMessageSourceClient
	{
		Task SendMessageAsync(Message message);
	}
}