using Comparatist.Data.Entities;
using Comparatist.Data.Persistence;
using System.Text;

namespace Comparatist.View.Stats
{
	internal static class StatsManager
	{
		public static void ShowForm(ProjectMetadata metadata, Dictionary<Type, int> stats)
		{
			var builder = new StringBuilder()
				.AppendLine("Database loaded")
				.AppendLine($"Project id: {metadata.Id}")
				.AppendLine($"Database version: {metadata.Version}")
				.AppendLine($"Created: {metadata.Created}")
				.AppendLine($"Modified: {metadata.Modified}")
				.AppendLine(string.Empty)
				.AppendLine($"Languages: {(stats.TryGetValue(typeof(Language), out var count) ? count : 0)}")
				.AppendLine($"Categories: {(stats.TryGetValue(typeof(Category), out count) ? count : 0)}")
				.AppendLine($"Roots: {(stats.TryGetValue(typeof(Root), out count) ? count : 0)}")
				.AppendLine($"Stems: {(stats.TryGetValue(typeof(Stem), out count) ? count : 0)}")
				.AppendLine($"Words: {(stats.TryGetValue(typeof(Word), out count) ? count : 0)}");

			MessageBox.Show(builder.ToString());
		}
	}
}

