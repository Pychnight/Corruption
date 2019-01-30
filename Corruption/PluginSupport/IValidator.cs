using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corruption.PluginSupport
{
	/// <summary>
	/// Interface for types that can check themselves, or some related data, and return a <see cref="ValidationResult"/> containing error and warning information.
	/// </summary>
	public interface IValidator
	{
		ValidationResult Validate();
	}
}
