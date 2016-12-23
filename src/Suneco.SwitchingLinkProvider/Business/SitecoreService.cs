namespace Suneco.SwitchingLinkProvider.Business
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Sitecore.Configuration;
    using Sitecore.Sites;
    using Sitecore.Web;
    using Suneco.SwitchingLinkManager.Models.Settings;

    /// <summary>
    /// Provides access to sitecore methods
    /// </summary>
    /// <seealso cref="Suneco.SwitchingLinkProvider.Business.ISitecoreService" />
    public class SitecoreService : ISitecoreService
    {
        private SwitchingLinkProviderSettings linkProviderSettings;

                /// <summary>
        /// Gets the sites.
        /// </summary>
        /// <value>The sites.</value>
        public List<SiteInfo> Sites => SiteContextFactory.Sites;

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
        /// <returns></returns>
        public SwitchingLinkProviderSettings GetLinkProviderSettings()
        {
            if (this.linkProviderSettings == null)
            {
                this.linkProviderSettings = (SwitchingLinkProviderSettings)Activator.CreateInstance(typeof(SwitchingLinkProviderSettings), Factory.GetConfiguration());
            }

            return this.linkProviderSettings;
        }
    }
}