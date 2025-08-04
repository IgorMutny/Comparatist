using MessagePack;

namespace Comparatist.Core.Infrastructure
{
    [MessagePackObject]
    public class ProjectMetadata
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
    }
}
