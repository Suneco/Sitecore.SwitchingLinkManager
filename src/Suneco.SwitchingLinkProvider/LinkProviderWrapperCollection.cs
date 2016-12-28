namespace Suneco.SwitchingLinkProvider
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using Services.Interfaces;
    using Sitecore.Links;

    /// <summary>
    /// The link provider wrapper collection
    /// </summary>
    /// <seealso cref="Suneco.SwitchingLinkProvider.LinkProviderWrapperBaseCollection{Sitecore.Links.LinkProvider, Suneco.SwitchingLinkProvider.LinkProviderWrapper}" />
    public class LinkProviderWrapperCollection : LinkProviderWrapperBaseCollection<LinkProvider, LinkProviderWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkProviderWrapperCollection" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="getProvider">The get provider.</param>
        /// <param name="sitecoreService">The sitecore service.</param>
        public LinkProviderWrapperCollection(ProviderBase owner, Func<string, LinkProvider> getProvider, ISitecoreService sitecoreService)
            : base(owner, getProvider, sitecoreService)
        {
        }
    }
}