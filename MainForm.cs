namespace Comparatist
{
    public partial class MainForm : Form
    {
        private InMemoryDatabase _db = new();
        private string _filePath = string.Empty;
        private Guid? _selectedWordId = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Файлы базы данных (*.db)|*.db|Все файлы (*.*)|*.*";
                dialog.Title = "Выберите файл базы данных";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _filePath = dialog.FileName;
                    _db.Load(_filePath);
                    RefreshTable();
                }
            }
        }

        private void dataGridViewWords_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewWords_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewWords.CurrentRow?.Cells["Id"].Value is Guid id)
                _selectedWordId = id;
            else
                _selectedWordId = null;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var newWord = new Word
            {
                Value = "Новое слово",
                Translation = "",
                Comment = "",
                Checked = false,
                SemanticGroups = Array.Empty<SemanticGroup>(),
                Stem = new Stem(),
                Language = new Language { Value = "неизвестно" }
            };

            _db.Words.Add(newWord);
            RefreshTable();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (_selectedWordId != null)
            {
                _db.Words.Delete(_selectedWordId.Value);
                RefreshTable();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Файлы базы данных (*.db)|*.db|Все файлы (*.*)|*.*";
                dialog.Title = "Сохранить базу данных как...";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.FileName;
                    _db.Save(path);
                    _filePath = path;
                }
            }
        }

        private void RefreshTable()
        {
            var list = _db.Words.GetAll()
                .Select((word, index) => new
                {
                    Value = word.Value,
                    Translation = word.Translation,
                    Comment = word.Comment,
                    Checked = word.Checked,
                    Language = word.Language?.Value ?? ""
                })
                .ToList();

            dataGridViewWords.DataSource = list;
        }
    }
}
