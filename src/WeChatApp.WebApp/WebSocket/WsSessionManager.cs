using System.Collections.Concurrent;
using System.Linq;

namespace WeChatApp.WebApp.WebSocket
{
    public interface IWsSessionManager
    {
        Task TryAddOrUpdateAsync(Guid id, WsSession session);

        Task<WsSession?> TryRemoveAsync(Guid id);

        void TryRemoveAll();

        WsSession? TryGetValue(Guid id);

        ConcurrentDictionary<Guid, WsSession> GetAllSessions();

        bool IsActive(Guid id, out WsSession? session);
    }

    public class WsSessionManager : IWsSessionManager
    {
        private ConcurrentDictionary<Guid, WsSession> _sessions = new ConcurrentDictionary<Guid, WsSession>();

        public WsSessionManager()
        {
        }

        public ConcurrentDictionary<Guid, WsSession> GetAllSessions()
        {
            return _sessions;
        }

        public bool IsActive(Guid id, out WsSession? session)
        {
            session = TryGetValue(id);

            if (session is null)
            {
                return false;
            }

            if (session.State == SuperSocket.SessionState.Connected)
            {
                return true;
            }

            return false;
        }

        public async Task TryAddOrUpdateAsync(Guid id, WsSession session)
        {
            await Task.Run(() =>
            {
                if (_sessions.TryGetValue(id, out var result))
                {
                    _sessions.TryUpdate(id, session, result);
                }

                _sessions.TryAdd(id, session);
            });
        }

        public WsSession? TryGetValue(Guid id)
        {
            _sessions.TryGetValue(id, out var result);
            return result;
        }

        public async Task<WsSession?> TryRemoveAsync(Guid id)
        {
            return await Task.Run(() =>
            {
                _sessions.TryRemove(id, out var result);

                return result;
            });
        }

        public void TryRemoveAll()
        {
            _sessions.Clear();
        }
    }
}