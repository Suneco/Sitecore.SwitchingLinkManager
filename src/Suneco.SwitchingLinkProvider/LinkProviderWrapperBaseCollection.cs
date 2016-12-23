namespace Suneco.SwitchingLinkProvider
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Xml;
    using Sitecore.Collections;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.Xml;

    public class LinkProviderWrapperBaseCollection<TProvider, TWrapper> : List<TWrapper>
        where TProvider : ProviderBase
        where TWrapper : LinkProviderBaseWrapper<TProvider, TWrapper>, new()
    {
        #region Fields

        private readonly ProviderBase _owner;
        private readonly string _ownerTypeName;
        private readonly SafeDictionary<string, TWrapper> _siteMap = new SafeDictionary<string, TWrapper>();

        private TWrapper _defaultWrapper;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="T:Sitecore.Security.AspNetSecurityProviderWrapperList`2" /> class.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="getProvider">The get provider.</param>
        public LinkProviderWrapperBaseCollection(NameValueCollection config, ProviderBase owner,
            Func<string, TProvider> getProvider)
        {
            Assert.ArgumentNotNull(config, "config");
            Assert.ArgumentNotNull(owner, "owner");
            Assert.ArgumentNotNull(getProvider, "getProvider");

            _owner = owner;
            _ownerTypeName = _owner.GetType().FullName;
            Initialize(config, getProvider);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets the default wrapper.
        /// </summary>
        /// <value>The default wrapper.</value>
        public virtual TWrapper DefaultWrapper => _defaultWrapper;

        /// <summary>
        ///     Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public virtual ProviderBase Owner => _owner;

        /// <summary>
        ///     Gets the name of the owner type.
        /// </summary>
        /// <value>The name of the owner type.</value>
        public virtual string OwnerTypeName => _ownerTypeName;

        /// <summary>
        ///     Gets the map of domain/wrappers.
        /// </summary>
        /// <value>The map.</value>
        public virtual SafeDictionary<string, TWrapper> SiteMap => _siteMap;

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Gets a provider wrapper for a specific Sitename.
        /// </summary>
        /// <param name="sitename">Name of the user.</param>
        /// <returns></returns>
        public virtual TWrapper GetWrapper(string sitename)
        {
            if (string.IsNullOrEmpty(sitename)) return DefaultWrapper;

            var item = SiteMap[sitename];
            return item ?? DefaultWrapper;
        }

        /// <summary>
        ///     Builds the domain/wrapper map.
        /// </summary>
        private void BuildSiteMap()
        {
            foreach (var tWrapper in this)
            {
                Assert.IsFalse(_siteMap.ContainsKey(tWrapper.Sitename),
                               $"Duplicate sitename name found in the link provider mapping of the {_ownerTypeName} '{_owner.Name}'. sitename: {tWrapper.Sitename}");
                _siteMap.Add(tWrapper.Sitename, tWrapper);
            }
        }

        /// <summary>
        ///     Gets the provider nodes from config.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        private List<XmlNode> GetProviderNodesFromConfig(NameValueCollection config)
        {
            string item = config["mappings"];
            Assert.IsNotNullOrEmpty(item, $"The configuration for {_ownerTypeName} must have a non-empty 'mappings' attribute pointing to the domain/provider mappings. Provider name: {_owner.Name}");

            var configNode = Factory.GetConfigNode(item);
            Assert.IsNotNull(configNode,
                             $"Could not find the configuration node pointed to by the 'mappings' attribute of the {_ownerTypeName} configuration. Provider: {_owner.Name}, mappings path: {item}");
            var childNodes = XmlUtil.GetChildNodes("provider", configNode);

            var flag = childNodes != null && childNodes.Count > 0;
            Assert.IsTrue(flag,
                          $"Could not find any 'provider' nodes below the configuration node pointed to by the 'mappings' attribute of the {_ownerTypeName} configuration. Provider: { _owner.Name}, mappings path: {item}");
            return childNodes;
        }

        /// <summary>
        ///     Initializes the wrappers.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="getProvider">The get provider delegate.</param>
        private void Initialize(NameValueCollection config, Func<string, TProvider> getProvider)
        {
            List<XmlNode> providerNodesFromConfig = GetProviderNodesFromConfig(config);
            foreach (XmlNode xmlNodes in providerNodesFromConfig)
            {
                var tWrapper = Activator.CreateInstance<TWrapper>();
                tWrapper.Initialize(xmlNodes, this, getProvider);
                Add(tWrapper);
            }
            BuildSiteMap();
            _defaultWrapper = _siteMap["*"];
            Assert.IsNotNull(_defaultWrapper,
                             $"No default provider (\"default\") was found below the configuration node pointed to by the 'mappings' attribute of the { _ownerTypeName} configuration. Provider: {_owner.Name}");
        }

        #endregion Methods
    }
}