namespace Suneco.SwitchingLinkProvider.Services
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Sitecore.Configuration;
    using Sitecore.Links;
    using Sitecore.Sites;
    using Sitecore.Web;
    using Suneco.SwitchingLinkProvider.Models.Settings;
    using Suneco.SwitchingLinkProvider.Services.Interfaces;

    /// <summary>
    /// Provides access to sitecore methods
    /// </summary>
    /// <seealso cref="Suneco.SwitchingLinkProvider.Services.Interfaces.ISitecoreService" />
    public class SitecoreService : ISitecoreService
    {
        /// <summary>
        /// The link provider settings
        /// </summary>
        private SwitchingLinkProviderSettings linkProviderSettings;

        /// <summary>
        /// Gets the sites.
        /// </summary>
        /// <value>The sites.</value>
        public List<SiteInfo> Sites => SiteContextFactory.Sites;

        /// <summary>
        /// Gets the link providers.
        /// </summary>
        /// <value>
        /// The link providers.
        /// </value>
        public LinkProviderCollection LinkProviders { get; }

        /// <summary>
        /// Gets the request URI.
        /// </summary>
        /// <returns>
        /// The URI of the request
        /// </returns>
        public Uri GetRequestUri()
        {
            return WebUtil.GetRequestUri();
        }

        /// <summary>
        /// Gets the link provider settings.
        /// </summary>
        /// <returns>The switching link provider settings</returns>
        public SwitchingLinkProviderSettings GetLinkProviderSettings()
        {
            if (this.linkProviderSettings == null)
            {
                this.linkProviderSettings = (SwitchingLinkProviderSettings)Activator.CreateInstance(typeof(SwitchingLinkProviderSettings), GetSitecoreConfiguration());
            }

            return this.linkProviderSettings;
        }

        public XmlDocument GetSitecoreConfiguration()
        {
            return Factory.GetConfiguration();
        }
    }
}