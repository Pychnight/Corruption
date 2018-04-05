using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;

namespace Corruption.PluginSupport
{
	public static class TerrariaPluginExtensions
	{
		public static void LogPrint(this TerrariaPlugin plugin, string message, TraceLevel kind = TraceLevel.Info )
		{
			ServerApi.LogWriter.PluginWriteLine(plugin, message, kind);
		}
	}
}
