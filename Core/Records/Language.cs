using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Language: IRecord
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;

        public object Clone()
        {
            return new Language
            {
                Id = Id,
                Value = Value
            };
        }
    }
}
