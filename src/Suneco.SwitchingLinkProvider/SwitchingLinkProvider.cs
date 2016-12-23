namespace Suneco.SwitchingLinkProvider
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using Business;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Links;
    using Sitecore.StringExtensions;
    using Sitecore.Web;

    public class SwitchingLinkProvider : LinkProvider
    {
        #region Fields

        private LinkProviderWrapperCollection _wrappers;
        private Business.ILogger _logger;
        private Business.ISitecoreService _sitecoreService;

        #endregion Fields

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether to add the 'aspx' extension to generated URLs.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the 'aspx' extension should be added to generated URLs; otherwise, <c>false</c>.
        /// </value>
        public override bool AddAspxExtension => ContextProvider.AddAspxExtension;

        /// <summary>
        ///     Gets a value indicating whether to always add the current server URL to generated URLs.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the server URL should always be added to generated URLs; otherwise, <c>false</c>.
        /// </value>
        public override bool AlwaysIncludeServerUrl => ContextProvider.AlwaysIncludeServerUrl;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SwitchingLinkProvider"/> is debug.
        /// </summary>
        /// <value>
        ///   <c>true</c> if debug; otherwise, <c>false</c>.
        /// </value>
        public bool Debug
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets a value indicating whether to encode the names making up the URL of the link.
        /// </summary>
        /// <value>
        ///     <c>true</c> if encode names; otherwise, <c>false</c>.
        /// </value>
        public override bool EncodeNames => ContextProvider.EncodeNames;

        /// <summary>
        ///     Gets a value controlling if and how to embed language in the URL.
        /// </summary>
        /// <value>The embed language option.</value>
        public override LanguageEmbedding LanguageEmbedding => ContextProvider.LanguageEmbedding;

        /// <summary>
        ///     Gets the language location.
        /// </summary>
        /// <value>The location within the URL where the language should be added.</value>
        public override LanguageLocation LanguageLocation => ContextProvider.LanguageLocation;

        /// <summary>
        ///     Gets a value indicating whether to render lowercase URLs.
        /// </summary>
        /// <value>
        ///     <c>true</c> if renders lowercase URLs; otherwise, <c>false</c>.
        /// </value>
        public override bool LowercaseUrls => ContextProvider.LowercaseUrls;

        /// <summary>
        ///     Gets a value indicating whether to shorten generated URLs.
        /// </summary>
        /// <value>
        ///     <c>true</c> if URLs should be shortened; otherwise, <c>false</c>.
        /// </value>
        public override bool ShortenUrls => ContextProvider.ShortenUrls;

        /// <summary>
        ///     Gets a value indicating whether to use the <see cref="P:Sitecore.Data.Items.Item.DisplayName" /> of items when generating URLs.
        /// </summary>
        /// <value>
        ///     <c>true</c> if display names should be used; otherwise, <c>false</c>.
        /// </value>
        public override bool UseDisplayName => ContextProvider.UseDisplayName;

        /// <summary>
        /// Gets the context provider.
        /// </summary>
        /// <value>
        /// The context provider.
        /// </value>
        private LinkProvider ContextProvider
        {
            get
            {
                try
                {
                    Uri requestUri = _sitecoreService.GetRequestUri();

                    string site = "default";

                    // if no provider is set, return default provider
                    if (_sitecoreService.Sites != null && requestUri != null)
                    {
                        foreach (SiteInfo info in _sitecoreService.Sites.Where(info => info.Matches(requestUri.Host, requestUri.LocalPath, requestUri.Port)))
                        {
                            site = info.Name;
                            break;
                        }
                    }

                    var item = _wrappers.GetWrapper(site);

                    if (item == null)
                    {
                        return _wrappers.DefaultWrapper.Provider;
                    }
                    return item.Provider;
                }
                catch (Exception ex)
                {
                    _logger.Error($"SwitchingLinkProvider : {ex.Message}", this);
                }

                return null;
            }
        }

        #endregion Properties

        public SwitchingLinkProvider(ILogger logger, ISitecoreService scService)
        {
            _logger = logger;
            _sitecoreService = scService;
        }

        public SwitchingLinkProvider()
        {
            _logger = new Business.SitecoreLogger();
            _sitecoreService = new SitecoreService();
        }

        #region Methods

        /// <summary>
        ///  Expands all dynamic links embedded in a text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The expanded dynamic links.</returns>
        [Obsolete("This method is never called by LinkManager. Implement ExpandDynamicLinks(string text, bool resolveSites) instead.")]
        public override string ExpandDynamicLinks(string text)
        {
            return ExpandDynamicLinks(text, false);
        }

        /// <summary>
        ///  Expands the dynamic links.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="resolveSites">
        ///     Set this to <c>true</c> to resolve site information when expanding dynamic links.
        /// </param>
        /// <returns>The expanded dynamic links.</returns>
        public override string ExpandDynamicLinks(string text, bool resolveSites)
        {
            return ContextProvider.ExpandDynamicLinks(text, false);
        }

        /// <summary>
        ///     Gets (a clone of) the default URL options.
        /// </summary>
        /// <returns>The default URL options.</returns>
        public override UrlOptions GetDefaultUrlOptions()
        {
            return ContextProvider.GetDefaultUrlOptions();
        }

        /// <summary>
        ///     Gets the dynamic URL for an item.
        /// </summary>
        /// <param name="item">The item to create an URL to.</param>
        /// <param name="options">The options.</param>
        /// <returns>The dynamic URL.</returns>
        public override string GetDynamicUrl(Item item, LinkUrlOptions options)
        {
            return ContextProvider.GetDynamicUrl(item, options);
        }

        /// <summary>
        ///     Gets the (friendly) URL of an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="options">The options.</param>
        /// <returns>The item URL.</returns>
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            if (Debug)
            {
                // Get call stack
                StackTrace stackTrace = new StackTrace();
                var frames = stackTrace.GetFrames();
                string sitename = Sitecore.Context.GetSiteName();

                if (frames != null && frames.Length > 1)
                {
                    Log.Info(
                        "SwitchingLinkProvider.GetItemUrl called from {0} files: {2} Site : {1}. ".FormatWith(
                            frames[1].GetMethod().Name, sitename, frames[1].GetFileName()), this);
                }
                else
                {
                    Log.Info(
                        "SwitchingLinkProvider.GetItemUrl called from (not found) Site : {1}. ".FormatWith(sitename), this);
                }

            }

            return ContextProvider.GetItemUrl(item, options);
        }

        /// <summary>
        ///     Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        /// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
        /// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     An attempt is made to call
        ///     <see
        ///         cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     on a provider after the provider has already been initialized.
        /// </exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(config, "config");

            base.Initialize(name, config);
            Debug = Sitecore.Configuration.Settings.GetBoolSetting("SwitchingLinkProvider.Debug", false);

            SwitchingLinkProvider linkProviderWrapperList = this;
            NameValueCollection nameValueCollection = config;
            SwitchingLinkProvider switchingMembershipProvider = this;

            linkProviderWrapperList._wrappers = new LinkProviderWrapperCollection(nameValueCollection, switchingMembershipProvider, providerName => LinkManager.Providers[providerName]);
        }

        /// <summary>
        ///     Determines whether the specified link text represents a dynamic link.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <returns>
        ///     <c>true</c> if [is dynamic link] [the specified link text]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsDynamicLink(string linkText)
        {
            if (ContextProvider != null)
                return ContextProvider.IsDynamicLink(linkText);
            return false;
        }

        /// <summary>
        ///     Parses a dynamic link.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <returns>The dynamic link.</returns>
        public override DynamicLink ParseDynamicLink(string linkText)
        {
            return ContextProvider.ParseDynamicLink(linkText);
        }

        /// <summary>
        ///     Parses a request URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The request URL.</returns>
        public override RequestUrl ParseRequestUrl(HttpRequest request)
        {
            return ContextProvider.ParseRequestUrl(request);
        }

        /// <summary>
        ///     Creates a link builder.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The link builder.</returns>
        protected override LinkBuilder CreateLinkBuilder(UrlOptions options)
        {
            return new LinkBuilder(options);
        }

        /// <summary>
        ///     Creates a link parser.
        /// </summary>
        /// <returns>The link parser.</returns>
        protected override LinkParser CreateLinkParser()
        {
            return new LinkParser();
        }

        #endregion Methods
    }
}