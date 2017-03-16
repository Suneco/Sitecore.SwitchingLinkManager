﻿namespace Suneco.SwitchingLinkProvider
{
    using System;
    using System.Collections.Generic;
    using System.Configuration.Provider;
    using System.Linq;
    using Sitecore.Collections;
    using Sitecore.Diagnostics;
    using Suneco.SwitchingLinkProvider.Services.Interfaces;

    public class LinkProviderWrapperBaseCollection<TProvider, TWrapper> : List<TWrapper>
        where TProvider : ProviderBase
        where TWrapper : LinkProviderBaseWrapper<TProvider, TWrapper>, new()
    {
        private readonly ProviderBase owner;
        private readonly string ownerTypeName;
        private readonly SafeDictionary<string, TWrapper> siteMap = new SafeDictionary<string, TWrapper>();
        private readonly ISitecoreService sitecoreService;
        private TWrapper defaultWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkProviderWrapperBaseCollection{TProvider, TWrapper}" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="getProvider">The get provider.</param>
        /// <param name="sitecoreService">The sitecore service.</param>
        public LinkProviderWrapperBaseCollection(ProviderBase owner, Func<string, TProvider> getProvider, ISitecoreService sitecoreService)
        {
            Assert.ArgumentNotNull(owner, "owner");
            Assert.ArgumentNotNull(getProvider, "getProvider");
            Assert.ArgumentNotNull(sitecoreService, "sitecoreService");

            this.owner = owner;
            this.ownerTypeName = this.owner.GetType().FullName;
            this.sitecoreService = sitecoreService;
            this.Initialize(getProvider);
        }

        /// <summary>
        /// Gets the default wrapper.
        /// </summary>
        /// <value>The default wrapper.</value>
        public virtual TWrapper DefaultWrapper => this.defaultWrapper;

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public virtual ProviderBase Owner => this.owner;

        /// <summary>
        /// Gets the name of the owner type.
        /// </summary>
        /// <value>The name of the owner type.</value>
        public virtual string OwnerTypeName => this.ownerTypeName;

        /// <summary>
        /// Gets the map of domain/wrappers.
        /// </summary>
        /// <value>The map.</value>
        public virtual SafeDictionary<string, TWrapper> SiteMap => this.siteMap;

        /// <summary>
        /// Gets a provider wrapper for a specific Sitename.
        /// </summary>
        /// <param name="sitename">Name of the user.</param>
        /// <returns>The wrapper</returns>
        public virtual TWrapper GetWrapper(string sitename)
        {
            if (string.IsNullOrEmpty(sitename))
            {
                return this.DefaultWrapper;
            }

            var item = this.SiteMap[sitename];
            return item ?? this.DefaultWrapper;
        }

        /// <summary>
        /// Builds the domain/wrapper map.
        /// </summary>
        private void BuildSiteMap()
        {
            foreach (var wrapper in this)
            {
                Assert.IsFalse(
                    this.siteMap.ContainsKey(wrapper.Sitename),
                    $"Duplicate sitename name found in the link provider mapping of the {this.ownerTypeName} '{this.owner.Name}'. sitename: {wrapper.Sitename}");

                this.siteMap.Add(wrapper.Sitename, wrapper);
            }
        }

        /// <summary>
        /// Initializes the wrappers.
        /// </summary>
        /// <param name="getProvider">The get provider delegate.</param>
        private void Initialize(Func<string, TProvider> getProvider)
        {
            var settings = this.sitecoreService.GetLinkProviderSettings();

            if (settings.Mappings != null && settings.Mappings.Any())
            {
                foreach (var mapping in settings.Mappings)
                {
                    var wrapper = Activator.CreateInstance<TWrapper>();
                    wrapper.Initialize(mapping, this, getProvider);
                    this.Add(wrapper);
                }
            }

            this.BuildSiteMap();
            this.defaultWrapper = this.siteMap["*"];
            Assert.IsNotNull(this.defaultWrapper, $"No default provider (\"default\") was found below the configuration node pointed to by the 'mappings' attribute of the {this.ownerTypeName} configuration. Provider: {this.owner.Name}");
        }
    }
}