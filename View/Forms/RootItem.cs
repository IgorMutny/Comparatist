using Comparatist.Data.Entities;

namespace Comparatist
{
    public partial class StemEditForm
    {
        private class RootItem
        {
            public string Value { get; }
            public Guid Id { get; }

            public RootItem(Root root)
            {
                Value = root.Value;
                Id = root.Id;
            }

            public override string ToString() => Value;
        }
    }
}