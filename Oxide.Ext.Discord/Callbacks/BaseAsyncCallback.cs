using System;
using System.Threading.Tasks;
using Oxide.Ext.Discord.Logging;
using Oxide.Ext.Discord.Pooling;

namespace Oxide.Ext.Discord.Callbacks
{
    /// <summary>
    /// Represents a base callback to be used when needing a lambda callback so no delegate or class is generated
    /// This class is pooled to prevent allocations
    /// </summary>
    public abstract class BaseAsyncCallback : BasePoolable
    {
        /// <summary>
        /// The callback to be called by the delegate
        /// </summary>
        private readonly Action _callback;
        
        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseAsyncCallback()
        {
            _callback = CallbackInternal;
        }

        /// <summary>
        /// Overridden in the child class to handle the callback
        /// </summary>
        protected abstract Task HandleCallback();

        protected abstract string ExceptionData();
        
        /// <summary>
        /// Runs the callback using async
        /// </summary>
        protected void Run()
        {
            Task.Run(_callback);
        }

        private async void CallbackInternal()
        {
            try
            {
                await HandleCallback().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                DiscordExtension.GlobalLogger.Exception("{0}.CallbackInternal had exception. Callback Data: {1}", GetType().Name, ExceptionData(), ex);
            }
            finally
            {
                Dispose();
            }
        }
    }
}