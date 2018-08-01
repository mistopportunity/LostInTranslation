
using System;
using LostInTranslation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest {
	[TestClass]
	public class LanguageManagerTests {
		[TestMethod]
		public void PureRandomTargetGenerationTest() {
			var random = new Random();
			var languages = LanguageManager.GetAllLanguages();
			var targets = LanguageManager.GetRandomTargets(random,languages,8);
			Assert.IsFalse(containsCongruents(targets));
		}

		[TestMethod]
		public void ControlledRandomTargetGenerationTest1() {
			var random = new Random(123456789);
			var languages = LanguageManager.GetAllLanguages();
			var targets = LanguageManager.GetRandomTargets(random,languages,8);
			Assert.IsFalse(containsCongruents(targets));
		}

		[TestMethod]
		public void ControlledRandomTargetGenerationTest2() {
			var random = new Random(456789213);
			var languages = LanguageManager.GetAllLanguages();
			var targets = LanguageManager.GetRandomTargets(random,languages,8);
			Assert.IsFalse(containsCongruents(targets));
		}


		[TestMethod]
		public void SubListGenerationTest1() {
			var random = new Random(123456789);
			var languages = new Language[] {
				Language.English, Language.French
			};
			var targets = LanguageManager.GetRandomTargets(random,languages,8);
			Assert.IsFalse(containsCongruents(targets));
		}

		[TestMethod]
		public void SubListGenerationTest2() {
			var random = new Random(123456789);
			var languages = new Language[] {
				Language.English, Language.French, Language.Italian, Language.Swedish
			};
			var targets = LanguageManager.GetRandomTargets(random,languages,8);
			Assert.IsFalse(containsCongruents(targets));
		}


		public bool containsCongruents(ISO639[] targets) {
			ISO639 last = targets[0];
			for(int i = 1;i<targets.Length;i++) {
				if(last.Value == targets[i].Value) {
					return true;
				}
				last = targets[i];
			}
			return false;
		}
	}
}
