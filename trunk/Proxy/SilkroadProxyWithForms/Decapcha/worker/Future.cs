using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace worker
{
    public sealed class Future
    {
        private bool ready;
        private object result;
        private Exception exception;

        public Future()
        {
            ready = false;
            result = null;
            exception = null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool isReady()
        {
            return ready;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public object waitFor()
        {
            try
            {

                for (; !isReady(); Monitor.Wait(this, 100)) ;   // neu test Monitor.Wait(this) thi se dung im                                              
                return getA();
            }
            catch
            {
                return null;
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public object getA()
        {
            if (exception != null)
                throw exception;
            else
                return result;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void setResult(object obj)
        {
            result = obj;
            ready = true;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void setException(Exception exception1, Object a)
        {
            lock (this)
            {
                exception = exception1;
                ready = true;
                Monitor.PulseAll(this);
            }

        }
    }
}