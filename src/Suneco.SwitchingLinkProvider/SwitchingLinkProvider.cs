namespace Suneco.SwitchingLinkProvider
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Links;
    using Sitecore.StringExtensions;
    using Sitecore.Web;
    using Suneco.SwitchingLinkProvider.Services;
    using Suneco.SwitchingLinkProvider.Services.Interfaces;

    public class SwitchingLinkProvider : LinkProvider
    {
        private readonly ILoggingService loggingService;
        private readonly ISitecoreService sitecoreService;
        private LinkProviderWrapperCollection wrappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchingLinkProvider"/> class.
        /// </summary>
        /// <param name="loggingService">The logging service.</param>
        /// <param name="sitecoreService">The sc service.</param>
        public SwitchingLinkProvider(ILoggingService loggingService, ISitecoreService sitecoreService)
        {
            this.loggingService = loggingService;
            this.sitecoreService = sitecoreService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchingLinkProvider"/> class.
        /// </summary>
        public SwitchingLinkProvider()
        {
            this.loggingService = new SitecoreLogging();
            this.sitecoreService = new SitecoreService();
        }

        /// <summary>
        /// Gets a value indicating whether to add the 'aspx' extension to generated URLs.
        /// </summary>
        /// <value>
        /// <c>true</c> if the 'aspx' extension should be added to generated URLs; otherwise, <c>false</c>.
        /// </value>
        public override bool AddAspxExtension => this.ContextProvider.AddAspxExtension;

        /// <summary>
        /// Gets a value indicating whether to always add the current server URL to generated URLs.
        /// </summary>
        /// <value>
        /// <c>true</c> if the server URL should always be added to generated URLs; otherwise, <c>false</c>.
        /// </value>
        public override bool AlwaysIncludeServerUrl => this.ContextProvider.AlwaysIncludeServerUrl;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SwitchingLinkProvider" /> is debug.
        /// </summary>
        /// <value>
        ///   <c>true</c> if debug; otherwise, <c>false</c>.
        /// </value>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets a value indicating whether to encode the names making up the URL of the link.
        /// </summary>
        /// <value>
        ///   <c>true</c> if encode names; otherwise, <c>false</c>.
        /// </value>
        public override bool EncodeNames => this.ContextProvider.EncodeNames;

        /// <summary>
        /// Gets a value controlling if and how to embed language in the URL.
        /// </summary>
        /// <value>
        /// The embed language option.
        /// </value>
        public override LanguageEmbedding LanguageEmbedding => this.ContextProvider.LanguageEmbedding;

        /// <summary>
        /// Gets the language location.
        /// </summary>
        /// <value>
        /// The location within the URL where the language should be added.
        /// </value>
        public override LanguageLocation LanguageLocation => this.ContextProvider.LanguageLocation;

        /// <summary>
        /// Gets a value indicating whether to render lowercase URLs.
        /// </summary>
        /// <value>
        ///   <c>true</c> if renders lowercase URLs; otherwise, <c>false</c>.
        /// </value>
        public override bool LowercaseUrls => this.ContextProvider.LowercaseUrls;

        /// <summary>
        /// Gets a value indicating whether to shorten generated URLs.
        /// </summary>
        /// <value>
        /// <c>true</c> if URLs should be shortened; otherwise, <c>false</c>.
        /// </value>
        public override bool ShortenUrls => this.ContextProvider.ShortenUrls;

        /// <summary>
        /// Gets a value indicating whether to use the <see cref="P:Sitecore.Data.Items.Item.DisplayName" /> of items when generating URLs.
        /// </summary>
        /// <value>
        /// <c>true</c> if display names should be used; otherwise, <c>false</c>.
        /// </value>
        public override bool UseDisplayName => this.ContextProvider.UseDisplayName;

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
                    Uri requestUri = this.sitecoreService.GetRequestUri();

                    string site = "default";

                    // Return the default provider if no provider is set
                    if (this.sitecoreService.Sites != null && requestUri != null)
                    {
                        foreach (SiteInfo info in this.sitecoreService.Sites.Where(info => info.Matches(requestUri.Host, requestUri.LocalPath, requestUri.Port)))
                        {
                            site = info.Name;
                            break;
                        }
                    }

                    var item = this.wrappers.GetWrapper(site);
                    if (item == null)
                    {
                        return this.wrappers.DefaultWrapper.Provider;
                    }

                    return item.Provider;
                }
                catch (Exception ex)
                {
                    this.loggingService.Error($"SwitchingLinkProvider : {ex.Message}", this);
                }

                return null;
            }
        }

        /// <summary>
        /// Expands all dynamic links embedded in a text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The expanded dynamic links.
        /// </returns>
        [Obsolete("This method is never called by LinkProvider. Implement ExpandDynamicLinks(string text, bool resolveSites) instead.")]
        public override string ExpandDynamicLinks(string text)
        {
            return this.ExpandDynamicLinks(text, false);
        }

        /// <summary>
        /// Expands the dynamic links.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="resolveSites">Set this to <c>true</c> to resolve site information when expanding dynamic links.</param>
        /// <returns>
        /// The expanded dynamic links.
        /// </returns>
        public override string ExpandDynamicLinks(string text, bool resolveSites)
        {
            return this.ContextProvider.ExpandDynamicLinks(text, false);
        }

        /// <summary>
        /// Gets (a clone of) the default URL options.
        /// </summary>
        /// <returns>
        /// The default URL options.
        /// </returns>
        public override UrlOptions GetDefaultUrlOptions()
        {
            return this.ContextProvider.GetDefaultUrlOptions();
        }

        /// <summary>
        /// Gets the dynamic URL for an item.
        /// </summary>
        /// <param name="item">The item to create an URL to.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The dynamic URL.
        /// </returns>
        public override string GetDynamicUrl(Item item, LinkUrlOptions options)
        {
            return this.ContextProvider.GetDynamicUrl(item, options);
        }

        /// <summary>
        /// Gets the (friendly) URL of an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The item URL.
        /// </returns>
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            if (this.Debug)
            {
                // Get call stack
                StackTrace stackTrace = new StackTrace();
                var frames = stackTrace.GetFrames();
                string sitename = Sitecore.Context.GetSiteName();

                if (frames != null && frames.Length > 1)
                {
                    Log.Info(
                        "SwitchingLinkProvider.GetItemUrl called from {0} files: {2} Site : {1}. "
                        .FormatWith(frames[1].GetMethod().Name, sitename, frames[1].GetFileName()),
                        this);
                }
                else
                {
                    Log.Info(
                        "SwitchingLinkProvider.GetItemUrl called from (not found) Site : {1}. "
                        .FormatWith(sitename),
                        this);
                }
            }

            return this.ContextProvider.GetItemUrl(item, options);
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        /// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
        /// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
        /// <exception cref="T:System.InvalidOperationException">An attempt is made to call
        /// <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        /// on a provider after the provider has already been initialized.</exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(config, "config");

            base.Initialize(name, config);
            this.Debug = this.sitecoreService.GetLinkProviderSettings().LogDebugInfo;

            SwitchingLinkProvider linkProviderWrapperList = this;
            NameValueCollection nameValueCollection = config;
            SwitchingLinkProvider switchingMembershipProvider = this;

            linkProviderWrapperList.wrappers = new LinkProviderWrapperCollection(nameValueCollection, switchingMembershipProvider, providerName => LinkManager.Providers[providerName]);
        }

        /// <summary>
        /// Determines whether the specified link text represents a dynamic link.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <returns>
        ///   <c>true</c> if [is dynamic link] [the specified link text]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsDynamicLink(string linkText)
        {
            if (this.ContextProvider != null)
            {
                return this.ContextProvider.IsDynamicLink(linkText);
            }

            return false;
        }

        /// <summary>
        /// Parses a dynamic link.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        /// <returns>
        /// The dynamic link.
        /// </returns>
        public override DynamicLink ParseDynamicLink(string linkText)
        {
            return this.ContextProvider.ParseDynamicLink(linkText);
        }

        /// <summary>
        /// Parses a request URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The request URL.
        /// </returns>
        public override RequestUrl ParseRequestUrl(HttpRequest request)
        {
            return this.ContextProvider.ParseRequestUrl(request);
        }

        /// <summary>
        /// Creates a link builder.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The link builder.
        /// </returns>
        protected override LinkBuilder CreateLinkBuilder(UrlOptions options)
        {
            return new LinkBuilder(options);
        }

        /// <summary>
        /// Creates a link parser.
        /// </summary>
        /// <returns>
        /// The link parser.
        /// </returns>
        protected override LinkParser CreateLinkParser()
        {
            return new LinkParser();
        }
    }
}