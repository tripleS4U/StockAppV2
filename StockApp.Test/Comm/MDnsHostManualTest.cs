using NUnit.Framework;
using StockApp.Comm.MDns;
using StockApp.Comm.NetMqStockTV;

namespace StockApp.Test.Comm
{
    /// <summary>
    /// Regression tests for <see cref="MDnsHost.Create"/> (manual hosts).
    /// Uses manual (non-mDNS) hosts so no real StockTV hardware is required.
    /// </summary>
    internal class MDnsHostManualTest
    {
        private class TestAliveInfo : IStockTVAliveInfo
        {
            public string HostName { get; set; }
            public string AppVersion { get; set; }
            public string IpAddress { get; set; }
        }

        [Test]
        public void Update_TakesIpAddressFromAliveInfo_NotAppVersion()
        {
            var host = MDnsHost.Create("unit-test-host", "192.168.100.250", 4747, 4748, "n.a.");

            host.Update(new TestAliveInfo
            {
                HostName = "unit-test-host",
                AppVersion = "1.2.3.4",
                IpAddress = "192.168.100.251"
            });

            Assert.That(host.IPAddress, Is.EqualTo("192.168.100.251"));
            Assert.That(host.Version, Is.EqualTo("1.2.3.4"));
            Assert.That(host.HostName, Is.EqualTo("unit-test-host"));

        }

        [Test]
        public void Update_UnchangedIpAddress_KeepsValue()
        {
            var host = MDnsHost.Create("unit-test-host", "192.168.100.250", 4747, 4748, "n.a.");

            host.Update(new TestAliveInfo { AppVersion = "1.2.3.4", IpAddress = "192.168.100.250" });

            Assert.That(host.IPAddress, Is.EqualTo("192.168.100.250"));
        }

        [Test]
        public void Update_EmptyOrNullValues_DoNotOverwrite()
        {
            var host = MDnsHost.Create("unit-test-host", "192.168.100.250", 4747, 4748, "1.0.0.0");

            host.Update(new TestAliveInfo { HostName = null, AppVersion = "  ", IpAddress = "" });

            Assert.That(host.IPAddress, Is.EqualTo("192.168.100.250"));
            Assert.That(host.Version, Is.EqualTo("1.0.0.0"));
            Assert.That(host.HostName, Is.EqualTo("unit-test-host"));

        }

        [Test]
        public void Update_Null_DoesNotThrowAndKeepsValues()
        {
            var host = MDnsHost.Create("unit-test-host", "192.168.100.250", 4747, 4748, "1.0.0.0");

            host.Update(null);

            Assert.That(host.IPAddress, Is.EqualTo("192.168.100.250"));
        }
    }
}
