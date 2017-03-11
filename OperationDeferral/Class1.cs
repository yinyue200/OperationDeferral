using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yinyue200.OperationDeferral
{
    /// <summary>
    /// 表示延迟操作
    /// </summary>
    public class OperationDeferral : IDisposable
    {
        System.Threading.ManualResetEvent are = new System.Threading.ManualResetEvent(false);
        public void Complete()
        {
            CompleteWithoutDispose();
            are.Dispose();
        }
        public void CompleteWithoutDispose()
        {
            are.Set();
        }
        public void Start()
        {
            are.Reset();
        }
        public Task WaitOneAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    are.WaitOne();
                }
                catch { }
            });
        }
        public void WaitOne()
        {
            are.WaitOne();
        }

        public void Dispose()
        {
            are.Dispose();
        }
    }
    public class ValuePackage<T>
    {
        public T Value { get; set; }
    }
    /// <summary>
    /// 表示带返回值的延迟操作
    /// </summary>
    public class OperationDeferral<TResult> : IDisposable
    {
        System.Threading.ManualResetEvent are = new System.Threading.ManualResetEvent(false);
        TResult Result;
        public void Complete(TResult result)
        {
            if (are == null) return;
            Result = result;
            are.Set();
        }
        public void Start()
        {
            are.Reset();
        }
        public async Task<TResult> WaitOneAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    are.WaitOne();
                }
                catch { }
            });
            return Result;
        }
        public TResult WaitOne()
        {
            are.WaitOne();
            return Result;
        }

        public void Dispose()
        {
            are.Dispose();
        }
    }
}
