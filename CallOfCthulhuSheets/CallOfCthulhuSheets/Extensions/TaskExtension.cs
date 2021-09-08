using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CallOfCthulhuSheets.Extensions
{
    public static class TaskExtension
    {
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }
            catch (Exception e) when (onException != null)
            {
                onException(e);
            }
        }

    }
}
