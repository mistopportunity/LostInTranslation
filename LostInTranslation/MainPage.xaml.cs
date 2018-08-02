using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
				if(TranslationResults.Children.Count > 0) {
					TranslationResults.Children.Clear();
				}
				return;
			}
			var validated = translationService.GetTextValidation(text);

			if(validated.Item1 != null) {

				var inputLanguage = LostInTranslation.Language.English;

				var translations = translationService.GetSuperTranslation(
					validated.Item1,
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

			} else if(validated.Item2 != null) {
				MessageDialog dialog = new MessageDialog(validated.Item2,"Lost in translation") {
					DefaultCommandIndex = 0,
					CancelCommandIndex = 0,
				};
				dialog.Commands.Add(new UICommand("Continue"));
				await dialog.ShowAsync();
			}
		}
	}
}
