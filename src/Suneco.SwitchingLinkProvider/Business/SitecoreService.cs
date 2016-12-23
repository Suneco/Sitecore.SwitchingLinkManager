namespace Suneco.SwitchingLinkProvider.Business
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Sites;
    using Sitecore.Web;

    /// <summary>
    /// Provides access to sitecore methods
    /// </summary>
    /// <seealso cref="Suneco.SwitchingLinkProvider.Business.ISitecoreService" />
    public class SitecoreService : ISitecoreService
    {
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
    }
}