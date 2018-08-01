using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTranslation.TranslationServices {
	internal sealed class DummyTranslator:ITranslationService {
		public IEnumerable<Translation[]> GetSuperTranslation(string text,ISO639 source,int threads,int layers) {
			throw new NotImplementedException();
		}

		public Translation GetTranslation(string text,ISO639 source,ISO639 target) {
			throw new NotImplementedException();
		}

		public Translation GetTranslation(string text,ISO639 source,ISO639[] targets) {
			throw new NotImplementedException();
		}
	}
}
