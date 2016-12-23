namespace Suneco.SwitchingLinkProvider.Models.Settings
{
    using System.Collections.Generic;
    using System.Xml;

    public class SwitchingLinkProviderSettings : SettingBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchingLinkProviderSettings"/> class.
        /// </summary>
        public SwitchingLinkProviderSettings()
        {
        }

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
        /// Gets or sets the mappings.
        /// </summary>
        /// <value>
        /// The mappings.
        /// </value>
        public IEnumerable<Mapping> Mappings { get; set; }

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

            var module = configuration.SelectSingleNode("sitecore/suneco.switchingLinkProvider");

            if (module == null)
            {
                return;
            }

            var logDebugInfoNode = module.SelectSingleNode("logDebugInfo");

            if (logDebugInfoNode != null)
            {
                this.LogDebugInfo = this.ConvertToBoolean(logDebugInfoNode.Attributes["value"]);
            }

            var mappings = new List<Mapping>();
            foreach (XmlNode mappingNode in module.SelectNodes("mappings/mapping"))
            {
                var mapping = new Mapping();
                mapping.SiteName = mappingNode.Attributes["siteName"]?.Value;
                mapping.LinkProviderName = mappingNode.Attributes["linkProviderName"]?.Value;
                mappings.Add(mapping);
            }

            this.Mappings = mappings;
        }
    }
}
