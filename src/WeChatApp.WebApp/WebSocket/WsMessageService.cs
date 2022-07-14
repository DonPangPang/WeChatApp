using Microsoft.Extensions.Options;
using SuperSocket;
using SuperSocket.WebSocket.Server;
using WeChatApp.Shared.Options;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.WebSocket
{
    public class WsMessageService : IHostedService
    {
        private readonly IWsSessionManager _sessionManager;
        private readonly IMessageToastService _messageToastService;
        private readonly WsServerOptions _options;

        public WsMessageService(
            IWsSessionManager sessionManager,
            IOptions<WsServerOptions> options,
            IMessageToastService messageToastService)
        {
            _sessionManager = sessionManager;
            _messageToastService = messageToastService;
            _options = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var host = WebSocketHostBuilder.Create()
                   .ConfigureSuperSocket(options =>
                   {
                       options.AddListener(new ListenOptions()
                       {
                           Ip = _options.IP,
                           Port = _options.Port
                       });
                   })
                   .UseSession<WsSession>()
                   .UseClearIdleSession()
                   .UseSessionHandler(async (s) =>
                   {
                       var session = ((WsSession)s).FormatParameter();
                       await _sessionManager.TryAddOrUpdateAsync(session.UserId, session);

                       await _messageToastService.ReSendMessageAsync(session.UserId);
                   }, async (s, e) =>
                   {
                       await _sessionManager.TryRemoveAsync(((WsSession)s).UserId);
                   })
                   .UseWebSocketMessageHandler(async (s, v) =>
                   {
                   })
                   .UseInProcSessionContainer()
                   .BuildAsServer();

            await host.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _sessionManager.TryRemoveAll();
                Console.WriteLine($"WebSocket Service Stop");
            }
            catch (Exception e)
            {
                Console.WriteLine($"WebSocket Service Stop error", e.Message);
            }

            return Task.CompletedTask;
        }
    }
}