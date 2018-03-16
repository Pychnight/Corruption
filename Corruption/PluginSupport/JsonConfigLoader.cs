using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;

namespace Corruption.PluginSupport
{
	/// <summary>
	/// Common base class for plugin configuration via json files. Provides a uniform means to load, save, and validate such files.
	/// </summary>
	public abstract class JsonConfig
	{
		/// <summary>
		/// Attempts to load a json configuration saved at the specified path. If none exists, one will be created. Also checks for configuration errors, by
		/// calling JsonConfig.Validate().
		/// </summary>
		/// <typeparam name="TConfig">JsonConfig subclass.</typeparam>
		/// <param name="plugin">TerrariaPlugin that houses the JsonConfig.</param>
		/// <param name="filePath">File path at which the config should resided.</param>
		/// <returns></returns>
		public static TConfig LoadOrCreate<TConfig>(TerrariaPlugin plugin, string filePath) where TConfig : JsonConfig, new()
		{
			TConfig result = default(TConfig);

			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));

				if( File.Exists(filePath) )
				{
					var json = File.ReadAllText(filePath);
					result = JsonConvert.DeserializeObject<TConfig>(json);
				}
				else
				{
					result = new TConfig();
					Save(plugin, result, filePath);
				}

				result.Validate();
			}
			catch( Exception ex )
			{
				ServerApi.LogWriter.PluginWriteLine(plugin, ex.Message, TraceLevel.Error);
			}

			return result;
		}

		/// <summary>
		/// Saves the config object to the file path specified.
		/// </summary>
		/// <param name="plugin">TerrariaPlugin that houses this JsonConfig.</param>
		/// <param name="config">JsonConfig type.</param>
		/// <param name="filePath">File path to write the config.</param>
		public static void Save(TerrariaPlugin plugin, JsonConfig config, string filePath)
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));

				var json = JsonConvert.SerializeObject(config, Formatting.Indented);
				File.WriteAllText(filePath, json);
			}
			catch( Exception ex)
			{
				ServerApi.LogWriter.PluginWriteLine(plugin, ex.Message, TraceLevel.Error);
			}
		}

		/// <summary>
		/// JsonConfig types should override this method, and throw for any configuration errors discovered.
		/// LoadOrCreate() runs this method to inform the server administrator of any issues.
		/// </summary>
		public virtual void Validate()
		{
		}
	}
}
