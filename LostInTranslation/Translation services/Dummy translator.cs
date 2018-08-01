using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTranslation.TranslationServices {
	internal sealed class DummyTranslator:ITranslationService {

		private readonly Random random = new Random(123456789);

		public IEnumerable<Translation[]> GetSuperTranslation(string text,ISO639 source,int threads,int layers) {

			var languages = LanguageManager.GetAllLanguages();

			for(int i = 0;i<threads;i++) {

				var targets = LanguageManager.GetRandomTargets(random,languages,layers);

				var translations = new Translation[layers];

				for(int i2 = 0;i2<layers;i2++) {

					translations[i2] = new Translation(
						text,targets[i2]
					);

				}

				yield return translations;


			}


		}

		public Translation GetTranslation(string text,ISO639 source,ISO639 target) {
			return new Translation(text,target);
		}

		public Translation GetTranslation(string text,ISO639 source,ISO639[] targets) {
			return new Translation(text,targets[targets.Length-1]);
		}
	}
}
