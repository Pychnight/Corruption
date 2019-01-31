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
		List<ValidationError> errors;
		List<ValidationWarning> warnings;
		List<ValidationResult> childResults;

		/// <summary>
		/// Gets an IList of ValidationErrors.
		/// </summary>
		public IList<ValidationError> Errors => errors;
		/// <summary>
		/// Gets an IList of ValidationWarnings.
		/// </summary>
		public IList<ValidationWarning> Warnings => warnings;
		/// <summary>
		/// Gets an IList of ValidationResults, from any sub objects.  
		/// </summary>
		public IList<ValidationResult> ChildResults => childResults;
		/// <summary>
		/// Gets a value determining if <see cref="Errors"/> contains any <see cref="ValidationError"/>s.
		/// </summary>
		public bool HasErrors => Errors.Count > 0;
		/// <summary>
		/// Gets a value determining if <see cref="Warnings"/> contains any <see cref="ValidationWarning"/>s.
		/// </summary>
		public bool HasWarnings => Warnings.Count > 0;
		/// <summary>
		/// Gets a value determining if this ValidationResult has any child results.
		/// </summary>
		public bool HasChildResults => ChildResults.Count > 0;
		/// <summary>
		/// Gets or sets an optional information string that refers to the origin of this <see cref="ValidationResult" />. 
		/// </summary>
		/// <remarks>Source may refer to a file name, or an object, or some other contextual hint.</remarks>
		public string Source { get; set; }

		public ValidationResult()
		{
			errors = new List<ValidationError>();
			warnings = new List<ValidationWarning>();
			childResults = new List<ValidationResult>();
		}
		
		/// <summary>
		/// Appends errors and warnings from another <see cref="ValidationResult"/>.
		/// </summary>
		/// <param name="result">ValidationResult to copy items from.</param>
		/// <param name="copyErrors">True to append errors.</param>
		/// <param name="copyWarnings">True to append warnings.</param>
		public void Concat(ValidationResult result, bool copyErrors = true, bool copyWarnings = true)
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

		/// <summary>
		/// Recursively sums all errors and warnings for this <see cref="ValidationResult"/>, and all <see cref="ChildResults"/>.
		/// </summary>
		/// <param name="totalErrors"></param>
		/// <param name="totalWarnings"></param>
		public void GetTotals(ref int totalErrors, ref int totalWarnings)
		{
			totalErrors += Errors.Count;
			totalWarnings += Warnings.Count;

			foreach(var child in ChildResults)
				child.GetTotals(ref totalErrors, ref totalWarnings);
		}
	}
}
