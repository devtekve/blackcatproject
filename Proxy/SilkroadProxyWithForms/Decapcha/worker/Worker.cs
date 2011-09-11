using System;
using System.Collections.Generic;
using System.Threading;

namespace worker
{

    public class Worker
    {
        public Worker(string s, int i)
        {
            for (int j = 0; j < i; j++)
            {
                WorkerThread workerthread = new WorkerThread(this, (new System.Text.StringBuilder()).Append(s).Append(" #").Append(j).ToString());

                threads.Add(workerthread);

                workerthread.th.Start();

            }
        }

        internal Future asyncExec(Job job, object obj)
        {
            Future future = new Future();
            Runner runner = new Runner(job, obj, future);
            lock (runnables)
            {
                runnables.AddLast(runner);
                Monitor.Pulse(runnables);
            }
            return future;
        }


        internal object syncExec(Job job, object obj)
        {

            if (Thread.CurrentThread.IsThreadPoolThread)
                return job.run(obj);
            else
                return asyncExec(job, obj).waitFor();
        }


        public void execute()
        {
            try
            {
                Runner runner;
                lock (runnables)
                {

                    for (; runnables.Count == 0 || runnables == null; Monitor.Wait(runnables)) ;
                    runner = runnables.First.Value;
                    runnables.RemoveFirst();
                }
                Thread t1 = new Thread(new ThreadStart(runner.run));
                t1.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private readonly LinkedList<Runner> runnables = new LinkedList<Runner>();
        private readonly HashSet<WorkerThread> threads = new HashSet<WorkerThread>();
    }
}