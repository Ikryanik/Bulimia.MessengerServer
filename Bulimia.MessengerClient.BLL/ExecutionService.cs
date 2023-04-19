using System;
using System.Threading.Tasks;

namespace Bulimia.MessengerClient.BLL
{
    public static class ExecutionService
    {
        public static T Execute<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default;
            }
        }
        public static async Task<T> Execute<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch
            {
                return default;
            }
        }
    }
}