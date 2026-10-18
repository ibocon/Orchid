using Microsoft.AspNetCore.SignalR.Client;
using Orchid.Contract.Payload;

namespace Orchid.Core.Sync.Relay;

/// <summary>
/// <see cref="HubConnection"/> 기반 릴레이 클라이언트 구현체입니다.
/// </summary>
public sealed class SignalRClipboardRelayClient : IClipboardRelayClient
{
    private readonly HubConnection _connection;

    /// <summary>
    /// <see cref="SignalRClipboardRelayClient"/> 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="options">허브 접속 옵션</param>
    public SignalRClipboardRelayClient(RelayOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _connection = new HubConnectionBuilder()
            .WithUrl(options.HubUri)
            .WithAutomaticReconnect()
            .Build();
        _connection.On<EncryptedPayload>("OnClipboardReceived", OnReceivedAsync);
    }

    /// <inheritdoc />
    public event Func<EncryptedPayload, Task>? PayloadReceived;

    /// <inheritdoc />
    public async Task ConnectAsync(string pairingId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pairingId);

        _connection.Reconnected += _ => _connection.InvokeAsync("JoinPairing", pairingId);
        await
            _connection.StartAsync(cancellationToken)
            .ConfigureAwait(false);
        await
            _connection.InvokeAsync("JoinPairing", pairingId, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task SendAsync(string pairingId, EncryptedPayload payload, CancellationToken cancellationToken = default)
    {
        return _connection.InvokeAsync("SendClipboard", pairingId, payload, cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        return _connection.DisposeAsync();
    }

    private Task OnReceivedAsync(EncryptedPayload payload)
    {
        Func<EncryptedPayload, Task>? handler = PayloadReceived;
        return handler is null ? Task.CompletedTask : handler(payload);
    }
}
