using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corruption.PluginSupport
{
	/// <summary>
	/// Base class for errors, warnings, or info within a ValidationResult.
	/// </summary>
	public abstract class ValidationResultItem
	{
		public string Message { get; set; }
		public string Source { get; set; }
		public int Line { get; set; } = -1;
		public int Column { get; set; } = -1;
		
		public override string ToString()
		{
			var sb = new StringBuilder();

			if( !string.IsNullOrWhiteSpace(Source) )
			{
				sb.Append(Source);
				sb.Append(" ");
			}
			
			if (Line>-1)
			{
				if( Column > -1 )
					sb.Append($"[{Line},{Column}]");
				else
					sb.Append($"[{Line}]");
			}
			
			if ( !string.IsNullOrWhiteSpace(Message) )
				sb.Append(Message);

			return sb.ToString();
		}
	}

	//public class ValidationInfo : ValidationResultItem
	//{
	//}

	public class ValidationWarning : ValidationResultItem
	{
		public ValidationWarning() { }
		public ValidationWarning(string message, string source = "", int row = -1, int column = -1)
		{
			Message = message;
			Source = source;
		}
	}

	public class ValidationError : ValidationResultItem
	{
		public ValidationError() { }
		public ValidationError(string message, string source = "", int row = -1, int column = -1)
		{
			Message = message;
			Source = source;
		}
	}
}
