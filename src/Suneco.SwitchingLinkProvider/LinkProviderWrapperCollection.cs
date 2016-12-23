namespace Suneco.SwitchingLinkProvider
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;

    using Sitecore.Links;

    public class LinkProviderWrapperCollection : LinkProviderWrapperBaseCollection<LinkProvider, LinkProviderWrapper>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkProviderWrapperCollection"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="getProvider">The get provider.</param>
        public LinkProviderWrapperCollection(NameValueCollection config, ProviderBase owner, Func<string, LinkProvider> getProvider)
            : base(config, owner, getProvider)
        {
        }

        #endregion Constructors
    }
}