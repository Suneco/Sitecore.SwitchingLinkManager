namespace Suneco.SwitchingLinkProvider.Business
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Sites;
    using Sitecore.Web;

    public class SitecoreService : ISitecoreService
    {
        public Uri GetRequestUri()
        {
            return WebUtil.GetRequestUri();
        }

        public List<SiteInfo> Sites => SiteContextFactory.Sites;
    }
}