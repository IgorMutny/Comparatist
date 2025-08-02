using Comparatist.View.Forms;
using MessagePack;

namespace Comparatist.View.Autoreplace
{
    internal class AutoReplaceManager
    {
        private const string FileName = "AutoReplace";

        public static AutoReplaceManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AutoReplaceManager();

                return _instance;
            }
        }

        private Dictionary<string, string> _replaceMap = new();
        private static AutoReplaceManager? _instance;

        private AutoReplaceManager()
        {
            Load();
        }

        public void ShowForm()
        {
            using var form = new AutoReplaceEditorForm(_replaceMap);

            if (form.ShowDialog() == DialogResult.OK)
                _replaceMap = form.Replacements;

            Save();
        }

        public string Apply(string input)
        {
            foreach (var pair in _replaceMap)
                input = input.Replace(pair.Key, pair.Value);

            return input;
        }

        private void Load()
        {
            _replaceMap.Clear();

            if (File.Exists(FileName))
            {
                using var fs = File.OpenRead(FileName);
                _replaceMap = MessagePackSerializer.Deserialize<Dictionary<string, string>>(fs);
            }
        }

        private void Save()
        {
            using var fs = File.Create(FileName);
            MessagePackSerializer.Serialize(fs, _replaceMap);
        }
    }
}
