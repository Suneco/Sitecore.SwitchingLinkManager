namespace Suneco.SwitchingLinkManager.Models.Settings
{
    using System.Xml;

    public class SwitchingLinkProviderSettings : SettingBase
    {
        public SwitchingLinkProviderSettings(XmlDocument configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether [log debug information].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log debug information]; otherwise, <c>false</c>.
        /// </value>
        public bool LogDebugInfo { get; set; }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected override void LoadConfiguration(XmlDocument configuration)
        {
            if (configuration == null)
            {
                return;
            }

            var module = configuration.SelectSingleNode("sitecore/suneco.switchinglinkprovider");

            if (module == null)
            {
                return;
            }

            var logDebugInfoNode = module.SelectSingleNode("logDebugInfo");

            if (logDebugInfoNode != null)
            {
                this.LogDebugInfo = this.ConvertToBoolean(logDebugInfoNode.Attributes["value"]);
            }
        }
    }
}
