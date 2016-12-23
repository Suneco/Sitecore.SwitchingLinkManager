namespace Suneco.SwitchingLinkProvider
{
    using Sitecore.Data.Items;
    using Sitecore.Links;

    /// <summary>
    /// Test link provider
    /// </summary>
    /// <seealso cref="Sitecore.Links.LinkProvider" />
    public class TestLinkProvider1 : LinkProvider
    {
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
            string t = base.GetDynamicUrl(item, options);
            return t + "?g=1";
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
            string t = base.GetItemUrl(item, options);
            return t + "?g=1";
        }
    }
}