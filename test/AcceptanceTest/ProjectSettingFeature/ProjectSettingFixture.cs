using AcceptanceTest;
using System;

namespace AcceptanceTest.ProjectSettingFeature
{
    public class ProjectSettingFixture : ServiceContext, IDisposable
    {
        public ProjectSettingFixture()
        {
        }

        void IDisposable.Dispose()
        {
            EnsureRecreatedDatabase();
            Dispose();
        }
    }
}