using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LostInTranslation {
	public sealed partial class TranslatorResult:UserControl {
		public TranslatorResult() {
			this.InitializeComponent();
		}
		private Translation[] translation;
		public Translation[] Translation {
			get {
				return translation;
			}
			set {
				StringBuilder builder = new StringBuilder();
				int topIndex = value.Length - 3;
				for(int i = 0;i<topIndex;i++) {
					builder.Append($"{value[i].ISO639.Value}, ");
				}
				builder.Append($"{value[topIndex + 1].ISO639.Value}");
				TranslationOrderBlock.Text = builder.ToString();
				TranslationOutputBlock.Text = value[topIndex + 2].Text;
				translation = value;
			}
		}
	}
}
