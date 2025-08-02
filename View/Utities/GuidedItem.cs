namespace Comparatist.View.Utities
{
    internal class GuidedItem
    {
        public string Text { get; set; } = string.Empty;
        public Guid Id { get; set; } = Guid.Empty;

        public override string ToString() => Text;
    }
}
