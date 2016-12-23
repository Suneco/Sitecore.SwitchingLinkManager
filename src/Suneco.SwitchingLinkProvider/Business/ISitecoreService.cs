namespace Suneco.SwitchingLinkProvider.Business
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Web;
    using Suneco.SwitchingLinkManager.Models.Settings;

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
        /// Gets the link provider settings.
        /// </summary>
        /// <returns></returns>
        SwitchingLinkProviderSettings GetLinkProviderSettings();
        
        /// Gets the request URI.
        /// </summary>
        /// <returns>The URI of the request</returns>
        Uri GetRequestUri();
    }
}