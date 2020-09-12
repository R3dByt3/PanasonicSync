using System;
using System.Threading.Tasks;

namespace PanasonicSync.GUI.Extensions
{
    public static class TaskExtensions
    {
        public static Task IgnoreExceptions(this Task task)
        {
            task.ContinueWith(c => 
            { 
                var ignored = c.Exception; 
            },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        public static Task FailFastOnException(this Task task)
        {
            task.ContinueWith(c => Environment.FailFast("Task faulted", c.Exception),
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }
        public static Task<T> IgnoreExceptions<T>(this Task<T> task)
        {
            task.ContinueWith(c => 
            { 
                var ignored = c.Exception; 
            },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        public static Task<T> FailFastOnException<T>(this Task<T> task)
        {
            task.ContinueWith(c => Environment.FailFast("Task faulted", c.Exception),
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }
    }
}
