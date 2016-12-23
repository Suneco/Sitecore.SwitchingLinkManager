namespace Suneco.SwitchingLinkProvider.Test
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Moq;
    using Services.Interfaces;
    using Suneco.SwitchingLinkProvider.Models.Settings;
    using Xunit;

    /// <summary>
    /// Test class for testing the SwitchingLinkProvider class
    /// </summary>
    public class SwitchingLinkProviderTests
    {
        [Fact]
        public void ShouldConstructSwithcingLinkProvider()
        {
            var loggingServiceMock = new Mock<ILoggingService>();
            var sitecoreServiceMock = new Mock<ISitecoreService>();

            var provider = new SwitchingLinkProvider(loggingServiceMock.Object, sitecoreServiceMock.Object);
            provider.Should().NotBeNull();
        }

        [Fact]
        public void ShouldInitializeSwithcingLinkProvider()
        {
            var loggingServiceMock = new Mock<ILoggingService>();
            var sitecoreServiceMock = new Mock<ISitecoreService>();

            var settings = new SwitchingLinkProviderSettings
            {
                LogDebugInfo = false,
                Mappings = new List<Mapping>
                {
                    new Mapping { SiteName = "*", LinkProviderName = "default" },
                    new Mapping { SiteName = "testsite", LinkProviderName = "testlinkprovider" }
                }
            };

            sitecoreServiceMock.Setup(x => x.GetLinkProviderSettings()).Returns(settings);

            var provider = new SwitchingLinkProvider(loggingServiceMock.Object, sitecoreServiceMock.Object);
            provider.Should().NotBeNull();

            provider.Initialize("test", new System.Collections.Specialized.NameValueCollection());
        }
    }
}