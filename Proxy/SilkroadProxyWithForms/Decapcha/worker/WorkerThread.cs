using System;
using System.Threading;

namespace worker
{
    public sealed class WorkerThread
    {
        public Thread th = null;
        private readonly Worker worker;
        private string s;
        public WorkerThread(Worker worker1, string s)
        {

            this.s = s;
            worker = worker1;
            th = new Thread(new ThreadStart(this.run));
            th.IsBackground = true;
            th.Name = s;

        }

        public void run()
        {
            try
            {
                while (true)
                    worker.execute();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Environment.Exit(1);
        }

        public string getNameThread() { return s; }

    }
}