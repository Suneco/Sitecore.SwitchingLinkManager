namespace Suneco.SwitchingLinkManager.Models.Settings
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
        /// <param name="configuration">The configuration.</param>
        public SettingBase(XmlDocument configuration)
        {
            this.LoadConfiguration(configuration);
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
