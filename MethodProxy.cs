using System;
using System.Diagnostics;
using System.Reflection;

namespace Decorating
{
    //https://stackoverflow.com/questions/38467753/realproxy-in-dotnet-core
    public class MethodProxy<T> : DispatchProxy where T : class
    {
        private T _decorated;
        private Stopwatch _stopwatch;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                Before(targetMethod, args);

                var result = targetMethod.Invoke(_decorated, args);

                After(targetMethod, args, result);
                return result;
            }
            catch (Exception ex)
            {
                LogException(ex.InnerException ?? ex, targetMethod);
            }

            return null;
        }

        public static T Create(T decorated)
        {
            object proxy = Create<T, MethodProxy<T>>();
            ((MethodProxy<T>)proxy).SetParameters(decorated);

            return (T)proxy;
        }

        private void SetParameters(T decorated) =>
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));

        private void LogException(Exception exception, MethodInfo methodInfo = null) =>
            Console.WriteLine($"Class {_decorated.GetType().FullName}, Method {methodInfo.Name} threw exception:\n{exception}\n");

        private void Before(MethodInfo methodInfo, object[] args)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            Console.WriteLine($"Class {_decorated.GetType().FullName}, Method {methodInfo.Name} is executing with args: {string.Join(", ", args)}\n");
        }

        private void After(MethodInfo methodInfo, object[] args, object result)
        {
            _stopwatch.Stop();
            var ellapsed = _stopwatch.Elapsed.TotalMilliseconds;

            Console.WriteLine($"Class {_decorated.GetType().FullName}, Method {methodInfo.Name} executed in {ellapsed}ms, Output: {result}\n");
        }
    }
}
