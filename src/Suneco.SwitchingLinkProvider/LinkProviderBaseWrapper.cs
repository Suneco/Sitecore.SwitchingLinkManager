namespace Suneco.SwitchingLinkProvider
{
    using System;
    using System.Configuration.Provider;
    using System.Xml;
    using Sitecore.Diagnostics;
    using Sitecore.Xml;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TProvider">The type of the provider.</typeparam>
    /// <typeparam name="TWrapper">The type of the wrapper.</typeparam>
    public class LinkProviderBaseWrapper<TProvider, TWrapper>
        where TProvider : ProviderBase
        where TWrapper : LinkProviderBaseWrapper<TProvider, TWrapper>, new()
    {
        #region Fields

        private Func<string, TProvider> _getProvider;
        private LinkProviderWrapperBaseCollection<TProvider, TWrapper> _owner;
        private TProvider _provider;
        private string _providerName;
        private string _sitename;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public virtual TProvider Provider
        {
            get
            {
                if (_provider == null)
                {

                    _provider = _getProvider(_providerName);
                    // TODO: must be unit tested.
                    Log.Warn($"Non-existing provider referenced by the 'providerName' attribute in the domain/provider mapping of the {_owner.OwnerTypeName} '{_owner.Owner.Name}'. Referenced provider: {_providerName} ", this);
                }
                return _provider;
            }
        }

        /// <summary>
        /// Gets the sitename.
        /// </summary>
        /// <value>
        /// The sitename.
        /// </value>
        public virtual string Sitename => _sitename;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes the specified config node.
        /// </summary>
        /// <param name="configNode">The config node.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="getProvider">The get provider delegate.</param>
        public virtual void Initialize(XmlNode configNode, LinkProviderWrapperBaseCollection<TProvider, TWrapper> owner, Func<string, TProvider> getProvider)
        {
            Assert.ArgumentNotNull(owner, "owner");

            _owner = owner;
            _getProvider = getProvider;

            _providerName = XmlUtil.GetAttribute("providerName", configNode);
            // todo: Onderstaande melding nog aanpassen.
            Assert.IsNotNullOrEmpty(_providerName, $"The 'providerName' attribute is empty or missing from one of the provider nodes in the domain/provider mapping of the {_owner.OwnerTypeName} '{_owner.Owner.Name}'");
            _sitename = XmlUtil.GetAttribute("sitename", configNode);
        }

        #endregion Methods
    }
}