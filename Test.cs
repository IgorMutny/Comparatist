namespace Comparatist
{
    public class SaveTest
    {
        public SaveTest()
        {
            var db = new InMemoryDatabase();

            // Создаём сущности
            var semGroup = new SemanticGroup { Value = "глагол" };
            var source = new Source { Value = "этимологический словарь" };
            var language = new Language { Value = "Русский" };

            var root = new Root
            {
                Value = "бег",
                Translation = "run",
                Comment = "корень",
                Checked = true,
                Source = source,
                SemanticGroups = new[] { semGroup }
            };

            var stem = new Stem
            {
                Value = "беж",
                Translation = "run",
                Comment = "основа",
                Checked = false,
                Roots = new[] { root },
                SemanticGroups = new[] { semGroup }
            };

            var word = new Word
            {
                Value = "бежать",
                Translation = "to run",
                Comment = "слово",
                Checked = false,
                Stem = stem,
                Language = language,
                SemanticGroups = new[] { semGroup }
            };

            // Добавляем в базу
            db.SemanticGroups.Add(semGroup);
            db.Sources.Add(source);
            db.Languages.Add(language);
            db.Roots.Add(root);
            db.Stems.Add(stem);
            db.Words.Add(word);

            // Сохраняем в файл
            db.Save("D:/UnityProjects/Comparatist/test1.dat");
            MessageBox.Show("База сохранена");
        }
    }

    public class LoadTest
    {
        public LoadTest()
        {
            var db = new InMemoryDatabase();
            db.Load("D:/UnityProjects/Comparatist/test1.dat");

            // Получаем все слова
            var words = db.Words.GetAll().ToList();

            if (words.Count > 0)
            {
                var w = words[0];
                MessageBox.Show($"Загружено слово: {w.Value}, перевод: {w.Translation}, язык: {w.Language.Value}");
            }
            else
            {
                MessageBox.Show("Нет слов в базе");
            }
        }
    }
}
