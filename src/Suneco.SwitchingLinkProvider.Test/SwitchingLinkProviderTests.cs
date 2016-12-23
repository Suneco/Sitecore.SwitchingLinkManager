namespace Suneco.SwitchingLinkProvider.Test
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Moq;
    using Suneco.SwitchingLinkProvider.Models.Settings;
    using Suneco.SwitchingLinkProvider.Services.Interfaces;
    using Xunit;

    /// <summary>
    /// Test class for testing the SwitchingLinkProvider class
    /// </summary>
    public class SwitchingLinkProviderTests
    {
        /// <summary>
        /// Should construct switching link provider.
        /// </summary>
        [Fact]
        public void ShouldConstructSwitchingLinkProvider()
        {
            var loggingServiceMock = new Mock<ILoggingService>();
            var sitecoreServiceMock = new Mock<ISitecoreService>();

            var provider = new SwitchingLinkProvider(loggingServiceMock.Object, sitecoreServiceMock.Object);
            provider.Should().NotBeNull();
        }

        /// <summary>
        /// Should initialize switching link provider.
        /// </summary>
        [Fact]
        public void ShouldInitializeSwitchingLinkProvider()
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