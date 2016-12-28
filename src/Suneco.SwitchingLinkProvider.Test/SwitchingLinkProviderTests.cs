﻿namespace Suneco.SwitchingLinkProvider.Test
{
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using FluentAssertions;
    using Moq;
    using Sitecore.Collections;
    using Sitecore.FakeDb;
    using Sitecore.Links;
    using Sitecore.Web;
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

        [Fact]
        public void ShouldGenerateLinkThroughtSwitchingProvider()
        {
            var dbItem = new DbItem("Testpage");

            using (var db = new Db { dbItem })
            {
                var item = db.GetItem(dbItem.ID);

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

                var options = new UrlOptions();

                var sites = new List<SiteInfo>
                {
                    this.CreateSiteInfo("testsite", "www.test.org")
                };
                
                var linkProvider1Mock = new Mock<LinkProvider>();
                var linkProvider2Mock = new Mock<LinkProvider>();

                linkProvider1Mock.Setup(x => x.Name).Returns("default");
                linkProvider1Mock.Setup(x => x.GetItemUrl(item, options)).Returns("/testpage?variant=1").Verifiable();

                linkProvider2Mock.Setup(x => x.Name).Returns("testlinkprovider");
                linkProvider2Mock.Setup(x => x.GetItemUrl(item, options)).Returns("/testpage?variant=2").Verifiable();

                var linkProviders = new LinkProviderCollection();
                linkProviders.Add(linkProvider1Mock.Object);
                linkProviders.Add(linkProvider2Mock.Object);

                sitecoreServiceMock.Setup(x => x.LinkProviders).Returns(linkProviders);
                sitecoreServiceMock.Setup(x => x.Sites).Returns(sites);
                sitecoreServiceMock.Setup(x => x.GetRequestUri()).Returns(new System.Uri("http://www.test.org"));
                sitecoreServiceMock.Setup(x => x.GetLinkProviderSettings()).Returns(settings);

                var provider = new SwitchingLinkProvider(loggingServiceMock.Object, sitecoreServiceMock.Object);
                provider.Initialize("test", new System.Collections.Specialized.NameValueCollection());

                provider.Should().NotBeNull();

                var url = provider.GetItemUrl(item, options);

                url.Should().BeEquivalentTo("/testpage?variant=2");

                linkProvider1Mock.Verify(x => x.GetItemUrl(item, options), Times.Never);
                linkProvider2Mock.Verify(x => x.GetItemUrl(item, options), Times.Once);
            }
        }

        private SiteInfo CreateSiteInfo(string name, string hostName)
        {
            var sd = new StringDictionary();
            sd.Add("name", name);
            sd.Add("hostName", hostName);

            var siteInfo = new SiteInfo(sd);

            return siteInfo;
        }
    }
}