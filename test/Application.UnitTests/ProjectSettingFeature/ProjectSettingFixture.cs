using System;

namespace Application.UnitTests.ProjectSettingFeature
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