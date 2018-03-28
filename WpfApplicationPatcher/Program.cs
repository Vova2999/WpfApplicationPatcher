using System;
using System.IO;
using System.Linq;
using Ninject;
using WpfApplicationPatcher.Core.Helpers;

namespace WpfApplicationPatcher {
	public static class Program {
		private static readonly Log log = Log.For(typeof(Program));

		public static void Main(string[] args) {
			try {
				Run(args);
			}
			catch (Exception exception) {
				log.Fatal(exception);
				throw;
			}
		}

		[DoNotAddLogOffset]
		private static void Run(string[] args) {
			if (!args.Any())
				throw new FileNotFoundException("You must specify path to wpf application");

			var wpfApplicationPath = args.FirstOrDefault();
			if (string.IsNullOrEmpty(wpfApplicationPath))
				throw new FileNotFoundException("Path to wpf application can not be empty");

			var availableExtensions = new[] { ".exe", ".dll" };
			var wpfApplicationExtension = Path.GetExtension(wpfApplicationPath);
			if (!availableExtensions.Any(availableExtension => string.Equals(availableExtension, wpfApplicationExtension, StringComparison.InvariantCultureIgnoreCase)))
				throw new ArgumentException($"Extension of wpf application can not be '{wpfApplicationExtension}'. " +
					$"Available extensions: {string.Join(", ", availableExtensions.Select(availableExtension => $"'{availableExtension}'"))}");

			var wpfApplicationFullPath = Path.GetFullPath(wpfApplicationPath);
			if (!File.Exists(wpfApplicationPath))
				throw new FileNotFoundException($"Not found wpf application: {wpfApplicationFullPath}");

			log.Info($"Application was found: {wpfApplicationFullPath}");

			var currentDirectory = Path.GetDirectoryName(wpfApplicationFullPath);
			log.Info($"Current directory: {currentDirectory}");
			Directory.SetCurrentDirectory(currentDirectory ?? throw new Exception());

			var container = new StandardKernel(new WpfApplicationPatcherNinjectModule());
			var processor = container.Get<WpfApplicationPatcherProcessor>();

			processor.PatchApplication(wpfApplicationPath);
		}
	}
}