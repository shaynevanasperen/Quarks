using System.Configuration;

namespace Quarks
{
	/// <summary>
	/// Provides a wrapper around ConfigurationManager.AppSettings to allow dependency injections
	/// </summary>
	public interface IAppSettings
	{
		string Get(string key);
	}

	public class AppSettings : IAppSettings
	{
		public string Get(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
