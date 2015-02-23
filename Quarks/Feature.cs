using System.Configuration;

namespace Quarks
{
	/// <summary>
	/// Simple feature-switch class which looks up features in app settings
	/// </summary>
	public static class Feature
	{
		const string EnabledFormat = "{0}.Enabled";

		/// <summary>
		/// Determines whether a feature is specifically enabled by looking for a ".Enabled" app setting for that feature.
		/// Use for features that are disabled by default.
		/// </summary>
		public static bool Enabled(string feature)
		{
			bool enabled;
			bool.TryParse(ConfigurationManager.AppSettings[string.Format(EnabledFormat, feature)], out enabled);
			return enabled;
		}

		/// <summary>
		/// Determines whether a feature has been specifically disabled by looking for a ".Enabled" app setting for that feature.
		/// Use fo features that are enabled by default.
		/// </summary>
		public static bool Disabled(string feature)
		{
			bool enabled;
			return bool.TryParse(ConfigurationManager.AppSettings[string.Format(EnabledFormat, feature)], out enabled) &&
				   !enabled;
		}
	}
}