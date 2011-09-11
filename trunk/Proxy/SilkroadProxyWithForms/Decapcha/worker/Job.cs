
namespace worker
{
    public abstract class Job
    {
        public Job(Worker worker1)
        {
            worker = worker1;
        }

        public abstract object run(object obj);

        public Future asyncExec(object obj)
        {
            return worker.asyncExec(this, obj);
        }

        public object syncExec(object obj)
        {
            return worker.syncExec(this, obj);
        }

        private Worker worker;
    }
}