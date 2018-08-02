using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTranslation.TranslationServices {
	internal sealed class DummyTranslator:ITranslationService {

		private readonly Random random = new Random(123456789);

		IEnumerable<Translation[]> ITranslationService.GetSuperTranslation(string text,ISO639 source,int threads,int layers) {

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

				yield return translations;

			}

		}

		Translation ITranslationService.GetTranslation(string text,ISO639 source,ISO639 target) {
			return new Translation(text,target);
		}

		Translation ITranslationService.GetTranslation(string text,ISO639 source,ISO639[] targets) {
			return new Translation(text,targets[targets.Length-1]);
		}

		Tuple<string,string> ITranslationService.GetTextValidation(string text) {
			if(text.Length > 512) {
				return new Tuple<string,string>(null,"The text is too long and there's nothing I want to do about it. Text length must not be greater than 512 characters.");
			} else {
				return new Tuple<string,string>(text,null);
			}
		}
	}
}
