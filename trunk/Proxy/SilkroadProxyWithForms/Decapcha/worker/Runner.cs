
using System;
namespace worker
{


    public sealed class Runner
    {

        private readonly Job runnable;
        private readonly object args;
        private readonly Future future;

        public Runner(Job job, object obj, Future future1)
        {
            future = future1;
            args = obj;
            runnable = job;
        }

        public void run()
        {
            try
            {
                future.setResult(runnable.run(args));
            }
            catch (Exception exception)
            {
                future.setException(exception, this);
            }
        }
    }
}