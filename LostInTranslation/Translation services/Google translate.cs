using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Data.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace LostInTranslation.TranslationServices {
	internal sealed class GoogleTranslate:ITranslationService {

		private Random random;
		private HttpClient client;

		internal GoogleTranslate() {
			client = new HttpClient();
			random = new Random(1234589);
		}
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

				translations[0] = await GetTranslation(text,source,targets[0]);

				if(translations[0].Text == null) {
					return null;
				}

				for(int i2 = 1;i2<translations.Length;i2++) {

					var translatedText = await GetTranslation(
						translations[i2-1].Text,
						translations[i2-1].ISO639,
						targets[i2]
					);

					if(translatedText.Text == null) {
						return null;
					}

					translations[i2] = new Translation(
						translatedText.Text,targets[i2]
					);

				}

				translationThreads.Add(translations);

			}

			return translationThreads;

		}

		ValidationResult ITranslationService.GetTextValidation(string text) {
			if(text.Length < 5) {
				return new ValidationResult(false,"What is this? A translation for ants?");
			} else if(text.Length > 2000) {
				return new ValidationResult(false,"There's too much text here. Shorten it up. Thanks.");
			} else {
				return new ValidationResult(true,text);
			}
		}

		public async Task<Translation> GetTranslation(string text,ISO639 source,ISO639 target) {

			string key = null; //Use your own key, thanks. Also, don't store it in a string.

			var uri = new Uri($"https://translation.googleapis.com/language/translate/v2?key={key}");

			var postData = new JsonObject();
			postData.Add("source",JsonValue.CreateStringValue(source.Value));
			postData.Add("target",JsonValue.CreateStringValue(target.Value));

			var queryArray = new JsonArray();
			queryArray.Add(JsonValue.CreateStringValue(text));
			postData.Add("q",queryArray);

			var json = new StringContent(
				postData.ToString(),
				Encoding.UTF8,"application/json"
			);

			var response = await client.PostAsync(uri,json);
			var responseText = await response.Content.ReadAsStringAsync();

			if(!response.IsSuccessStatusCode) {
				return new Translation(null,target);
			}

			var responseJson = JsonValue.Parse(responseText).GetObject();

			var responseTranslation = responseJson["data"].GetObject()["translations"].GetArray()[0].GetObject()["translatedText"].GetString();

			responseTranslation = System.Net.WebUtility.HtmlDecode(responseTranslation);

			return new Translation(responseTranslation,target);

		}

		Task<Translation> ITranslationService.GetTranslation(string text,ISO639 source,ISO639[] targets) {
			throw new NotImplementedException();
		}
	}
}
