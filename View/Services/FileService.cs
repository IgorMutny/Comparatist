using Comparatist.Core.Persistence;
using Comparatist.Services.Cache;

namespace Comparatist
{
    public class FileService
    {
        private const string Extension = ".comparatist";

        private Database _db = new();
        private string _filePath = string.Empty;
        private Action _onLoaded;
        private DataCacheService _dataCacheService;

        public FileService(Database db, Action onLoaded, DataCacheService dataCacheService)
        {
            _db = db;
            _onLoaded = onLoaded;
            _dataCacheService = dataCacheService;
        }

        public void Open()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = $"Comparatist DataBase files (*{Extension})|*{Extension}";
                dialog.Title = "Select Comparatist DataBase file";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _filePath = dialog.FileName;
                    _db.Load(_filePath);
                    _dataCacheService.BuildFromDataBase(_db);
                    _onLoaded();
                }
            }
        }

        public void SaveAs()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = $"Comparatist DataBase files (*{Extension})|*{Extension}";
                dialog.Title = "Save Comparatist DataBase as...";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.FileName;
                    _db.Save(path);
                    _filePath = path;
                }
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_filePath))
                SaveAs();
            else
                _db.Save(_filePath);
        }
    }
}
