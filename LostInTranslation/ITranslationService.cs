using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTranslation {

	public struct ValidationResult {
		public ValidationResult(bool isValid,string value) {
			IsValid = isValid;
			Value = value;
		}
		internal readonly bool IsValid;
		internal readonly string Value;
	}

	public interface ITranslationService {

		/// <summary>
		/// Get a source to target translation of a block of text
		/// </summary>
		/// <param name="text">The source text to input to the translation service</param>
		/// <param name="source">An ISO639 object based on the source text</param>
		/// <param name="target">An ISO639 object for the destination language</param>
		/// <returns>A translation object with the translated text and its language</returns>
		Task<Translation> GetTranslation(string text,ISO639 source,ISO639 target);

		/// <summary>
		/// Get a source to target translation of a block of text that puts the text through each target
		/// </summary>
		/// <param name="text">The source text to input to the translation service</param>
		/// <param name="source">An ISO639 object based on the source text</param>
		/// <param name="targets">ISO639 objects for putting the source text through</param>
		/// <returns>A translation object with the final translated text and its language</returns>
		Task<Translation> GetTranslation(string text,ISO639 source,ISO639[] targets);

		/// <summary>
		/// Get multiple threads of translation paths at once
		/// </summary>
		/// <param name="text">The source text to input to the translation service</param>
		/// <param name="source">An ISO639 object based on the source text</param>
		/// <param name="threads">The number of congruent threads to translate the text along</param>
		/// <param name="layers">Layers are how deep each thread goes, that is, how many target language each thread goes through</param>
		/// <returns>An enumerable object of each thread's history</returns>
		Task<IEnumerable<Translation[]>> GetSuperTranslation(string text,ISO639 source,int threads,int layers);

		/// <summary>
		/// Validate text before entering it into a translation method. These methods expect this processing and should not perform it on their own.
		/// </summary>
		/// <param name="text">The text to process. Text input comes trimmed and checked for emptiness.</param>
		/// <returns>Tuple item1 is null if text is invalid. Item2 is an explanation to push to the UI. If text is valid, item 1 is the processed value and item 2 is null</returns>
		ValidationResult GetTextValidation(string text);

	}
}
