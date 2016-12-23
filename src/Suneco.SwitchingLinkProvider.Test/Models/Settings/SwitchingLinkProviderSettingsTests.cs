namespace Suneco.SwitchingLinkProvider.xUnit.Models.Settings
{
    using System.Text;
    using System.Xml;
    using FluentAssertions;
    using SwitchingLinkManager.Models.Settings;
    using Xunit;

    public class SwitchingLinkProviderSettingsTests
    {
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

        [Fact]
        public void ShouldLoadModuleConfigurationWithSettings()
        {
            var sb = new StringBuilder();
            sb.Append("<sitecore>");
            sb.Append("  <suneco.switchinglinkprovider>");
            sb.Append("    <logDebugInfo value=\"true\" />");
            sb.Append("  </suneco.switchinglinkprovider>");
            sb.Append("</sitecore>");

            var xdoc = new XmlDocument();
            xdoc.LoadXml(sb.ToString());

            var settings = new SwitchingLinkProviderSettings(xdoc);
            settings.LogDebugInfo.Should().BeTrue(because: "The log debug info configuration should be true");
        }
    }
}
