using System;

namespace AcceptanceTest.TaskFeature
{
    public class TaskFixture : ServiceContext, IDisposable
    {
        public TaskFixture()
        {
        }

        void IDisposable.Dispose()
        {
            EnsureRecreatedDatabase();
            Dispose();
        }
    }
}