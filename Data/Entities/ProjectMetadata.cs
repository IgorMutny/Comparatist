using MessagePack;

namespace Comparatist.Data.Persistence
{
    [MessagePackObject]
    public class ProjectMetadata : ICloneable
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public int Version { get; set; }
        [Key(2)] public DateTime Created { get; set; }
        [Key(3)] public DateTime Modified { get; set; }

        public static ProjectMetadata CreateNew()
        {
            return new ProjectMetadata
            {
                Id = Guid.NewGuid(),
                Version = 1,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
        }

        public object Clone()
        {
            return new ProjectMetadata
            {
                Id = Id,
                Version = Version,
                Created = Created,
                Modified = Modified
            };
        }
    }
}
