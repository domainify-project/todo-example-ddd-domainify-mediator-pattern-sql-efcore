using AcceptanceTests;
using System;

namespace AcceptanceTests.ProjectSettingFeature
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