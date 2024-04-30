using System;

namespace AcceptanceTests.TaskFeature
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