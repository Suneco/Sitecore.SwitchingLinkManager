namespace Suneco.SwitchingLinkProvider.Business
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Web;

    public interface ISitecoreService
    {
        Uri GetRequestUri();

        List<SiteInfo> Sites { get; }
    }
}