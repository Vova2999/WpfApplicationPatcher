using log4net;

namespace WpfApplicationPatcher.Helpers {
	public static class Log {
		public static ILog For(string typeName) {
			return LogManager.GetLogger(typeName);
		}

		public static ILog For<TObject>(TObject obj) {
			return LogManager.GetLogger(typeof(TObject));
		}
	}
}