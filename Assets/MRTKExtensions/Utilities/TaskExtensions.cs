using System;
using System.Threading.Tasks;

namespace MRKTExtensions.Utilities
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Nicked and adapted from https://stackoverflow.com/questions/35645899/awaiting-task-with-timeout
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static async Task AwaitWithTimeout<T>(this Task<T> task, int timeout, Action<T> success, Action error)
        {
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                success?.Invoke(task.Result);
            }
            else
            {
                error?.Invoke();
            }
        }
    }
}
