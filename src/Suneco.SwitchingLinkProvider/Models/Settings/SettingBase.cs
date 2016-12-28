namespace Suneco.SwitchingLinkProvider.Models.Settings
{
    using System.Xml;

    /// <summary>
    /// Abstract base class for all settings
    /// </summary>
    public abstract class SettingBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingBase"/> class.
        /// </summary>
        public SettingBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingBase"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "This is allowed")]
        public SettingBase(XmlDocument configuration)
        {
#pragma warning disable S1699 // Constructors should only call non-overridable methods
            this.LoadConfiguration(configuration);
#pragma warning restore S1699 // Constructors should only call non-overridable methods
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected abstract void LoadConfiguration(XmlDocument configuration);

        /// <summary>
        /// Converts to boolean.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The result</returns>
        protected virtual bool ConvertToBoolean(XmlAttribute attribute)
        {
            if (attribute == null)
            {
                return false;
            }

            return this.ConvertToBoolean(attribute.Value);
        }

        /// <summary>
        /// Converts to boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result</returns>
        protected virtual bool ConvertToBoolean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Equals("true", System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
