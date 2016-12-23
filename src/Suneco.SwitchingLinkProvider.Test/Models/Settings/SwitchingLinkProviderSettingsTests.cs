namespace Suneco.SwitchingLinkProvider.Test.Models.Settings
{
    using System.Linq;
    using System.Text;
    using System.Xml;
    using FluentAssertions;
    using Suneco.SwitchingLinkProvider.Models.Settings;
    using Xunit;

    /// <summary>
    /// Switching link provider settings tests
    /// </summary>
    public class SwitchingLinkProviderSettingsTests
    {
        /// <summary>
        /// Should load module configuration without settings.
        /// </summary>
        [Fact]
        public void ShouldLoadModuleConfigurationWithoutSettings()
        {
            var sb = new StringBuilder();
            sb.Append("<sitecore>");
            sb.Append("</sitecore>");

            var xdoc = new XmlDocument();
            xdoc.LoadXml(sb.ToString());

            var settings = new SwitchingLinkProviderSettings(xdoc);
            settings.LogDebugInfo.Should().BeFalse(because: "Without any configuration the default value should be false");
        }

        /// <summary>
        /// Should load module configuration with settings.
        /// </summary>
        [Fact]
        public void ShouldLoadModuleConfigurationWithSettings()
        {
            var sb = new StringBuilder();
            sb.Append("<sitecore>");
            sb.Append("  <suneco.switchingLinkProvider>");
            sb.Append("    <logDebugInfo value=\"true\" />");
            sb.Append("    <mappings>");
            sb.Append("      <mapping siteName=\"*\" linkProviderName=\"default\" />");
            sb.Append("      <mapping siteName=\"testsite\" linkProviderName=\"testlinkprovider\" />");
            sb.Append("    </mappings>");
            sb.Append("  </suneco.switchingLinkProvider>");
            sb.Append("</sitecore>");

            var xdoc = new XmlDocument();
            xdoc.LoadXml(sb.ToString());

            var settings = new SwitchingLinkProviderSettings(xdoc);
            settings.LogDebugInfo.Should().BeTrue(because: "The log debug info configuration should be true");
            settings.Mappings.Should().HaveCount(2);
            settings.Mappings.FirstOrDefault(x => x.SiteName == "*").LinkProviderName.ShouldBeEquivalentTo("default");
            settings.Mappings.FirstOrDefault(x => x.SiteName == "testsite").LinkProviderName.ShouldBeEquivalentTo("testlinkprovider");
        }
    }
}
