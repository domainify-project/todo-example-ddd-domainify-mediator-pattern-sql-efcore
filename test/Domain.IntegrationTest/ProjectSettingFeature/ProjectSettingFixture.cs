using System;

namespace Application.IntegrationTest.ProjectSettingFeature
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