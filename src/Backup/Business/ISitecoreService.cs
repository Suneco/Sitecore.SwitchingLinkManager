using System;
using System.Collections.Generic;
using Sitecore.Configuration;
using Sitecore.Web;

namespace Suneco.SwitchingLinkManager.Business
{
    public interface ISitecoreService
    {
        Uri GetRequestUri();

        List<SiteInfo> Sites { get; }
    }
}