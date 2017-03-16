namespace Suneco.SwitchingLinkProvider.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Sitecore.Links;
    using Sitecore.Web;
    using Suneco.SwitchingLinkProvider.Models.Settings;

    /// <summary>
    /// Interface that provides access to sitecore methods
    /// </summary>
    public interface ISitecoreService
    {
        /// <summary>
        /// Gets the sites.
        /// </summary>
        /// <value>The sites.</value>
        List<SiteInfo> Sites { get; }

        /// <summary>
        /// Gets the link providers.
        /// </summary>
        /// <value>
        /// The link providers.
        /// </value>
        LinkProviderCollection LinkProviders { get; }

        /// <summary>
        /// Gets the link provider settings.
        /// </summary>
        /// <returns>The switching link provider settings</returns>
        SwitchingLinkProviderSettings GetLinkProviderSettings();

        /// <summary>
        /// Gets the request URI.
        /// </summary>
        /// <returns>The URI of the request</returns>
        Uri GetRequestUri();

        /// <summary>
        /// Gets the sitecore configuration.
        /// </summary>
        /// <returns>The Sitecore configuration</returns>
        XmlDocument GetSitecoreConfiguration();
    }
}