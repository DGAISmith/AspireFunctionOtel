using Aspire.Hosting.Azure;
using Azure.Messaging.ServiceBus;

namespace AspireFunctionOtel.AppHost;

public static class Commands
{
    public static IResourceBuilder<AzureServiceBusResource> WithMessageCommand(
            this IResourceBuilder<AzureServiceBusResource> builder)
    {
        builder.WithCommand(
            name: "send-message",
            displayName: "Send Message",
            executeCommand: context => OnSendMessageCommandAsync(builder),
            commandOptions: new CommandOptions
            {
                ConfirmationMessage = "Are you sure you want to send a message?",
                Description = "Send a message to the service bus queue.",
                IconName = "MailAdd",
                IconVariant = IconVariant.Filled,
            });

        return builder;
    }

    private static async Task<ExecuteCommandResult> OnSendMessageCommandAsync(IResourceBuilder<AzureServiceBusResource> builder)
    {
        var connectionString = await builder.Resource.ConnectionStringExpression.GetValueAsync(new CancellationToken());

        var serviceBusClient = new ServiceBusClient(connectionString);
        var sender = serviceBusClient.CreateSender("queue1");
        var message = new ServiceBusMessage("Hello, World!");
        await sender.SendMessageAsync(message);

        return CommandResults.Success();
    }
}
