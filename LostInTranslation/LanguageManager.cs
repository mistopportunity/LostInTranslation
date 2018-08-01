using System;
using System.Collections.Generic;

namespace LostInTranslation {

	public struct ISO639 {
		public ISO639(string value,Language language) {
			Value = value;
			Language = language;
		}
		public string Value {
			get;
		}
		public Language Language {
			get;
		}
	}

	public struct Translation {
		public Translation(string text,ISO639 iso639) {
			Text = text;
			ISO639 = iso639;
		}
		public string Text {
			get;
		}
		public ISO639 ISO639 {
			get;
		}
	}

	public enum Language {
		English, Spanish, German, Swedish, Italian, French
	}

	public static class LanguageManager {

		public static ISO639 GetLanguageCode(Language language) {
			switch(language) {
				default:
				case Language.English:
					return new ISO639("en",language);
				case Language.Spanish:
					return new ISO639("es",language);
				case Language.German:
					return new ISO639("de",language);
				case Language.Swedish:
					return new ISO639("sv",language);
				case Language.Italian:
					return new ISO639("it",language);
				case Language.French:
					return new ISO639("fr",language);
			}
		}

		public static string GetLanguageName(Language language) {
			return language.ToString("g");
		}

		public static Language[] GetAllLanguages() {
			return Enum.GetValues(typeof(Language)) as Language[];
		}

		public static ISO639[] GetRandomTargets(Random random,Language[] languages,int layers) {
			
			var targets = new ISO639[layers];

			var runningLanguageList = new List<Language>(languages);

			var lastLanguage = runningLanguageList[
				random.Next(0,runningLanguageList.Count-1)
			];

			runningLanguageList.Remove(lastLanguage);

			for(int i = 1;i<layers;i++) {

				var language = runningLanguageList[
					random.Next(0,runningLanguageList.Count-1)
				];
				runningLanguageList.Remove(language);
				runningLanguageList.Add(lastLanguage);
				lastLanguage = language;

				targets[i] = GetLanguageCode(language);

			}
			return targets;
		}

	}
}
