using System;
using Xunit;
using PipServices.Container.Info;
using PipServices.Container.Config;
using PipServices.Commons.Config;

namespace PipServices.Container.Test.Info
{
    public sealed class ContainerInfoTest
    {
        [Fact]
    public void TestName()
        {
            var containerInfo = new ContainerInfo();

            Assert.Equal(containerInfo.Name, "unknown");

            containerInfo.Name = "new name";

            Assert.Equal(containerInfo.Name, "new name");
        }

        [Fact]
        public void TestDescription()
        {
            var containerInfo = new ContainerInfo();
            Assert.Null(containerInfo.Description);

            containerInfo.Description = "new description";
            Assert.Equal(containerInfo.Description, "new description");
        }

        [Fact]
        public void TestContainerId()
        {
            var containerInfo = new ContainerInfo();

            containerInfo.ContainerId = "new container id";

            Assert.Equal(containerInfo.ContainerId, "new container id");
        }

        [Fact]
        public void TestStartTime()
        {
            var containerInfo = new ContainerInfo();
            Assert.Equal(containerInfo.StartTime.Year, DateTimeOffset.UtcNow.Year);
            Assert.Equal(containerInfo.StartTime.Month, DateTimeOffset.UtcNow.Month);

            containerInfo.StartTime = new DateTimeOffset(1975, 4, 8, 0, 0, 0, 0, TimeSpan.Zero);
            Assert.Equal(containerInfo.StartTime, new DateTimeOffset(1975, 4, 8, 0, 0, 0, 0, TimeSpan.Zero));
        }

        [Fact]
        public void TestFromConfigs()
        {
            var config = ConfigParams.FromTuples(
                "info.name", "new name",
                "info.description", "new description",
                "properties.access_key", "key",
                "properties.store_key", "store key"
                );

            var containerInfo = ContainerInfo.FromConfig(config);
            Assert.Equal(containerInfo.Name, "new name");
            Assert.Equal(containerInfo.Description, "new description");
        }
    }
}
