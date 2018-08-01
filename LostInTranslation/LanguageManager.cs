using System;

namespace LostInTranslation {

	internal struct ISO639 {
		internal ISO639(string value,Language language) {
			Value = value;
			Language = language;
		}
		internal string Value {
			get;
		}
		internal Language Language {
			get;
		}
	}

	internal struct Translation {
		internal Translation(string text,ISO639 iso639) {
			Text = text;
			ISO639 = iso639;
		}
		internal string Text {
			get;
		}
		internal ISO639 ISO639 {
			get;
		}
	}

	internal enum Language {
		English, Spanish, German, Swedish, Italian, French
	}

	internal static class LanguageManager {

		internal static ISO639 GetLanguageCode(Language language) {
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

		internal static string GetLanguageName(Language language) {
			return language.ToString("g");
		}

		internal static Language GetRandomLanguage(Random random) {
			var languages = Enum.GetValues(typeof(Language));
			return (Language)languages.GetValue(
				random.Next(0,languages.GetUpperBound(0))
			);
		}

	}
}
