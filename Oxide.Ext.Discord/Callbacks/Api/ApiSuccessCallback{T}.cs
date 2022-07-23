using System;
using System.Threading.Tasks;
using Oxide.Ext.Discord.Entities.Api;
using Oxide.Ext.Discord.Json.Pooling;
using Oxide.Ext.Discord.Logging;
using Oxide.Ext.Discord.Pooling;
using Oxide.Ext.Discord.Rest.Requests;

namespace Oxide.Ext.Discord.Callbacks.Api
{
    internal class ApiSuccessCallback<T> : BaseApiCallback
    {
        private Action<T> _onSuccess;
        private T _data;

        public async Task Init(Request<T> request, RequestResponse response)
        {
            base.Init(request);
            _onSuccess = request.OnSuccess;

            JsonReaderPoolable reader = DiscordPool.Get<JsonReaderPoolable>();
            await reader.CopyAsync(response.Content).ConfigureAwait(false);
            DiscordExtension.GlobalLogger.Debug($"{nameof(ApiSuccessCallback<T>)}.{nameof(Init)} Body: {await reader.ReadAsStringAsync()}");
            _data = await reader.Deserialize<T>(Client.Bot);
            reader.Dispose();
        }

        protected override void HandleApiCallback()
        {
            _onSuccess.Invoke(_data);
        }

        ///<inheritdoc/>
        protected override void DisposeInternal()
        {
            DiscordPool.Free(this);
        }
        
        ///<inheritdoc/>
        protected override void EnterPool()
        {
            base.EnterPool();
            _data = default(T);
            _onSuccess = null;
        }
    }
}