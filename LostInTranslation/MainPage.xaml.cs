using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LostInTranslation {
	public sealed partial class MainPage:Page {

		private ITranslationService translationService;

		public MainPage() {
			this.InitializeComponent();
			translationService = new TranslationServices.DummyTranslator();
			StatusUpdate("Have fun in this.. nice.. place. Yep.","Welcome!");
		}

		private void HangUserInterface() {
			BigFriendlyButton.Content = "processing..";
			BigFriendlyButton.IsEnabled = false;
			TranslationInputBlock.IsEnabled = false;
		}

		private void ResumeUserInterface() {
			BigFriendlyButton.Content = "translate this";
			BigFriendlyButton.IsEnabled = true;
			TranslationInputBlock.IsEnabled = true;
		}

		private bool hasTranslations = false;

		private string lastMessage = string.Empty;
		private int repeats = 0;

		private void StatusUpdate(string message,string title) {

			if(hasTranslations) {
				TranslationResults.Children.Clear();
				hasTranslations = false;
			}

			var translationResult = new TranslatorResult();
			translationResult.UseAsErrorProxy(message,title);

			TranslationResults.Children.Add(translationResult);
			if(TranslationResults.Children.Count > 5) {

				if(lastMessage == message) {

					TranslationResults.Children.RemoveAt(4);
					TranslationResults.Children.RemoveAt(4);

					var repeatBlock = new TranslatorResult();
					repeatBlock.UseAsErrorProxy(message,
						$"{title} - {++repeats} repeat{(repeats != 1 ? "s" : "")}"
					);

					TranslationResults.Children.Add(repeatBlock);

				} else {
					TranslationResults.Children.RemoveAt(0);
					repeats = 0;
				}


			}

			lastMessage = message;
		}

		private async void BigFriendlyButton_Click(object sender,RoutedEventArgs e) {

			var text = TranslationInputBlock.Text;
			text = text.Trim();
			if(string.IsNullOrEmpty(text)) {
				StatusUpdate("This text is empty! I'm not a miracle worker!","User error");
				return;
			}

			var validated = translationService.GetTextValidation(text);
			if(validated.IsValid) {
				HangUserInterface();
				var inputLanguage = LostInTranslation.Language.English;
				IEnumerable<Translation[]> translations = null;
				try {
					translations = await translationService.GetSuperTranslation(
						validated.Value,
						LanguageManager.GetLanguageCode(inputLanguage),
						8,
						8
					);
				} catch (Exception exception) {
					StatusUpdate(exception.Message,"Whoops! Internal service error");
					ResumeUserInterface();
					return;
				}

				if(translations == null) {
					StatusUpdate("An error with the translation service has occured. Try again later.","Whoops! Internal service error");
					ResumeUserInterface();
					return;
				}
				TranslationResults.Children.Clear();
				foreach(var translation in translations) {

					var translationResult = new TranslatorResult() {
						Translation = translation
					};

					TranslationResults.Children.Add(translationResult);

				}
				hasTranslations = true;
				ResumeUserInterface();
			} else if(validated.Value != null) {
				StatusUpdate(validated.Value,"User error");
			} else {
				StatusUpdate("You did something wrong. Sorrrrry :(","User error");
			}
		}
	}
}
