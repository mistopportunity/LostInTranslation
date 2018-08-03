using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTranslation.TranslationServices {

	//Don't use in production!
	internal sealed class DummyTranslator:ITranslationService {


		private readonly Random random = new Random(123456789);

		async Task<IEnumerable<Translation[]>> ITranslationService.GetSuperTranslation(string text,ISO639 source,int threads,int layers) {

			var translationThreads = new List<Translation[]>();

			var languages = LanguageManager.GetAllLanguages();

			for(int i = 0;i<threads;i++) {

				var targets = LanguageManager.GetRandomTargets(
					random,
					languages,
					source.Language,
					layers
				);

				var translations = new Translation[targets.Length];

				for(int i2 = 0;i2<translations.Length;i2++) {

					translations[i2] = new Translation(
						text,targets[i2]
					);

				}

				translationThreads.Add(translations);

			}

			return translationThreads;

		}

		async Task<Translation> ITranslationService.GetTranslation(string text,ISO639 source,ISO639 target) {
			return new Translation(text,target);
		}

		async Task<Translation> ITranslationService.GetTranslation(string text,ISO639 source,ISO639[] targets) {
			return new Translation(text,targets[targets.Length-1]);
		}

		ValidationResult ITranslationService.GetTextValidation(string text) {
			if(text.Length > 512) {
				return new ValidationResult(false,"The text is too long and there's nothing I want to do about it. Text length must not be greater than 512 characters.");
			} else {
				return new ValidationResult(true,text);
			}
		}
	}
}
