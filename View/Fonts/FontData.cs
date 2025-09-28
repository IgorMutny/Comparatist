using MessagePack;

namespace Comparatist.View.Fonts
{
	[MessagePackObject]
	public class FontData
	{
		[Key(0)] public string Family { get;set; } = SystemFonts.DefaultFont.FontFamily.Name;
		[Key(1)] public float Size { get; set; } = SystemFonts.DefaultFont.Size;
	}
}
