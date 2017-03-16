namespace Suneco.SwitchingLinkProvider
{
    using System;
    using System.Configuration.Provider;
    using Sitecore.Diagnostics;
    using Suneco.SwitchingLinkProvider.Models.Settings;

    /// <summary>
    /// The linkprovider base wrapper
    /// </summary>
    /// <typeparam name="TProvider">The type of the provider.</typeparam>
    /// <typeparam name="TWrapper">The type of the wrapper.</typeparam>
    public class LinkProviderBaseWrapper<TProvider, TWrapper>
        where TProvider : ProviderBase
        where TWrapper : LinkProviderBaseWrapper<TProvider, TWrapper>, new()
    {
        private Func<string, TProvider> getProvider;
        private LinkProviderWrapperBaseCollection<TProvider, TWrapper> owner;
        private TProvider provider;
        private string providerName;
        private string sitename;

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public virtual TProvider Provider
        {
            get
            {
                if (this.provider == null)
                {
                    this.provider = this.getProvider(this.providerName);
                    Log.Warn($"Non-existing provider referenced by the 'providerName' attribute in the domain/provider mapping of the {this.owner.OwnerTypeName} '{this.owner.Owner.Name}'. Referenced provider: {this.providerName} ", this);
                }

                return this.provider;
            }
        }

        /// <summary>
        /// Gets the sitename.
        /// </summary>
        /// <value>
        /// The sitename.
        /// </value>
        public virtual string Sitename => this.sitename;

        /// <summary>
        /// Initializes the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="getProvider">The get provider.</param>
        public virtual void Initialize(Mapping mapping, LinkProviderWrapperBaseCollection<TProvider, TWrapper> owner, Func<string, TProvider> getProvider)
        {
            Assert.ArgumentNotNull(owner, "owner");

            this.owner = owner;
            this.getProvider = getProvider;

            this.providerName = mapping.LinkProviderName;

            Assert.IsNotNullOrEmpty(this.providerName, $"The 'linkProviderName' attribute is empty or missing from one of the provider nodes in the mappings config of the switchinglinkprovider of the {this.owner.OwnerTypeName} '{this.owner.Owner.Name}'");
            this.sitename = mapping.SiteName;
        }
    }
}