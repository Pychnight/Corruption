using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corruption.PluginSupport
{
	/// <summary>
	/// An individual error or warning within a ValidationResult.
	/// </summary>
	public class ValidationResultItem
	{
		public string Source { get; internal set; }
		public string Location { get; internal set; }
		public string Message { get; internal set; }

		public ValidationResultItem(string message = "", string source = "", string location = "")
		{
			Message = message ?? "";
			Source = source ?? "";
			Location = location ?? "";
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			if( !string.IsNullOrWhiteSpace(Source) )
			{
				sb.Append(Source);
				sb.Append(" ");
			}

			if( !string.IsNullOrWhiteSpace(Location) )
			{
				sb.Append(Location);
				sb.Append(" ");
			}
			
			if( !string.IsNullOrWhiteSpace(Message) )
				sb.Append(Message);

			return sb.ToString();
		}
	}
}
