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
		English,
		Spanish,
		German,
		Swedish,
		Italian,
		French,
		Afrikaans,
		Finnish,
		Japanese,
		Polish,
		Russian,
		Slovak
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
				case Language.Afrikaans:
					return new ISO639("af",language);
				case Language.Finnish:
					return new ISO639("fi",language);
				case Language.Japanese:
					return new ISO639("ja",language);
				case Language.Polish:
					return new ISO639("pl",language);
				case Language.Russian:
					return new ISO639("ru",language);
				case Language.Slovak:
					return new ISO639("sk",language);
			}
		}

		public static string GetLanguageName(Language language) {
			return language.ToString("g");
		}

		public static Language[] GetAllLanguages() {
			return Enum.GetValues(typeof(Language)) as Language[];
		}

		public static ISO639[] GetRandomTargets(Random random,Language[] languages,Language startLanguage,int layers) {
			
			var targets = new ISO639[layers+1];

			var runningLanguageList = new List<Language>(languages);

			Language lastLanguage = startLanguage;

			while(lastLanguage == startLanguage) {
				lastLanguage = runningLanguageList[
					random.Next(0,runningLanguageList.Count-1)
				];
			}

			targets[0] = GetLanguageCode(lastLanguage);

			runningLanguageList.Remove(lastLanguage);

			for(int i = 1;i<layers-1;i++) {

				var language = runningLanguageList[
					random.Next(0,runningLanguageList.Count-1)
				];
				runningLanguageList.Remove(language);
				runningLanguageList.Add(lastLanguage);
				lastLanguage = language;

				targets[i] = GetLanguageCode(language);

			}

			var finalLanguage = lastLanguage;
			while(finalLanguage == lastLanguage || finalLanguage == startLanguage) {
				finalLanguage = runningLanguageList[
					random.Next(0,runningLanguageList.Count-1)
				];
			}

			targets[layers-1] = GetLanguageCode(finalLanguage);

			targets[layers] = GetLanguageCode(startLanguage);

			return targets;
		}

	}
}
