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
		}

		private async void BigFriendlyButton_Click(object sender,RoutedEventArgs e) {

			var text = TranslationInputBlock.Text;
			text = text.Trim();
			if(string.IsNullOrEmpty(text)) {
				return;
			}

			var validated = translationService.GetTextValidation(text);
			if(validated.IsValid) {

				var inputLanguage = LostInTranslation.Language.English;

				BigFriendlyButton.Content = "processing..";
				BigFriendlyButton.IsEnabled = false;
				TranslationInputBlock.IsEnabled = false;

				IEnumerable<Translation[]> translations = await translationService.GetSuperTranslation(
					validated.Value,
					LanguageManager.GetLanguageCode(inputLanguage),
					8,
					8
				);

				TranslationResults.Children.Clear();
				foreach(var translation in translations) {

					var translationResult = new TranslatorResult() {
						Translation = translation
					};

					TranslationResults.Children.Add(translationResult);

				}

				BigFriendlyButton.Content = "translate this";
				BigFriendlyButton.IsEnabled = true;
				TranslationInputBlock.IsEnabled = true;


			} else if(validated.Value != null) {
				MessageDialog dialog = new MessageDialog(validated.Value,"Lost in translation") {
					DefaultCommandIndex = 0,
					CancelCommandIndex = 0,
				};
				dialog.Commands.Add(new UICommand("Continue"));
				await dialog.ShowAsync();
			}
		}
	}
}
