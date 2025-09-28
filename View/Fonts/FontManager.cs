using MessagePack;

namespace Comparatist.View.Fonts
{
	internal class FontManager
	{
		private const string FileName = "Font";
		private static FontManager? _instance;

		private Font _font;

		public static FontManager Instance {
			get {
				if(_instance == null)
					_instance = new FontManager();

				return _instance;
			}
		}

		private FontManager()
		{
			_font = Load();
		}

		public Font Font {
			get => _font;
			set {
				_font = value;
				Save();
			}
		}

		private Font Load()
		{
			if(File.Exists(FileName))
			{
				using var fs = File.OpenRead(FileName);
				var fontData = MessagePackSerializer.Deserialize<FontData>(fs);
				return new Font(fontData.Family, fontData.Size);
			}
			else
			{
				return SystemFonts.DefaultFont;
			}
		}

		private void Save()
		{
			using var fs = File.Create(FileName);
			var fontData = new FontData { Family = _font.FontFamily.Name, Size = _font.Size };
			MessagePackSerializer.Serialize(fs, fontData);
		}
	}
}
