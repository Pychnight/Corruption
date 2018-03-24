using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corruption.PluginSupport
{
	/// <summary>
	/// Collection of Error and Warning information for types that undergo validation procedures.
	/// </summary>
	public class ValidationResult
	{
		List<ValidationResultItem> errors;
		List<ValidationResultItem> warnings;

		public IList<ValidationResultItem> Errors { get; private set; }
		public IList<ValidationResultItem> Warnings { get; private set; }

		public bool HasErrors => Errors.Count > 0;
		public bool HasWarnings => Warnings.Count > 0;

		public ValidationResult()
		{
			errors = new List<ValidationResultItem>();
			warnings = new List<ValidationResultItem>();

			Errors = errors.AsReadOnly();
			Warnings = warnings.AsReadOnly();
		}

		public void AddError(string message = "", string source = "", string location = "")
		{
			var item = new ValidationResultItem(message, source, location);
			errors.Add(item);
		}

		public void AddError(string message, string source, int line, int col)
		{
			AddError(message, source,$"[{line},{col}]");
		}

		public void AddWarning(string message = "", string source = "", string location = "")
		{
			var item = new ValidationResultItem(message, source, location);
			warnings.Add(item);
		}

		public void AddWarning(string message, string source, int line, int col)
		{
			AddWarning(message, source, $"[{line},{col}]");
		}

		/// <summary>
		/// Appends errors and warnings from another ValidationResult.
		/// </summary>
		/// <param name="result">ValidationResult to copy items from.</param>
		/// <param name="copyErrors">True to append errors.</param>
		/// <param name="copyWarnings">True to append warnings.</param>
		public void AddValidationResult(ValidationResult result, bool copyErrors = true, bool copyWarnings = true)
		{
			if(copyErrors)
				errors.AddRange(result.Errors);
			
			if(copyWarnings)	
				warnings.AddRange(result.Warnings);
		}

		/// <summary>
		/// Set all warnings and errors to a new source.
		/// </summary>
		/// <param name="source">New source.</param>
		/// <param name="setErrors">True to set error sources.</param>
		/// <param name="setWarnings">True to set warning sources.</param>
		public void SetSources(string source, bool setErrors = true, bool setWarnings = true)
		{
			if(setErrors)
				errors.ForEach(i => i.Source = source);

			if( setWarnings )
				warnings.ForEach(i => i.Source = source);
		}
	}
}
