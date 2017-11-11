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
        Microsoft.VisualStudio.Threading.AsyncManualResetEvent are = new Microsoft.VisualStudio.Threading.AsyncManualResetEvent(false);
        public void Complete()
        {
            CompleteWithoutDispose();
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
            return are.WaitAsync();
        }
        public void WaitOne()
        {
            are.WaitAsync().Wait();
        }

        public void Dispose()
        {

        }
    }
    /// <summary>
    /// 表示带返回值的延迟操作
    /// </summary>
    public class OperationDeferral<TResult> : IDisposable
    {
        Microsoft.VisualStudio.Threading.AsyncManualResetEvent are = new Microsoft.VisualStudio.Threading.AsyncManualResetEvent(false);
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
            await are.WaitAsync();
            return Result;
        }
        public TResult WaitOne()
        {
            return WaitOneAsync().Result;
        }

        public void Dispose()
        {

        }
    }
}
