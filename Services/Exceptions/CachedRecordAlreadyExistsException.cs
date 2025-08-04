namespace Comparatist.Services.Exceptions
{
    public class CachedRecordAlreadyExistsException: Exception
    {
        public Type RecordType { get; }
        public Guid RecordId { get; }

        public CachedRecordAlreadyExistsException(Type recordType, Guid recordId) :
            base($"{recordType.Name} {recordId} already exists in project cache")
        {
            RecordType = recordType;
            RecordId = recordId;
        }
    }
}
