namespace Comparatist.Application.Exceptions
{
    public class CachedRecordNotFoundException: Exception
    {
        public Type RecordType { get; }
        public Guid RecordId { get; }

        public CachedRecordNotFoundException(Type recordType, Guid recordId) :
            base($"{recordType.Name} {recordId} not found in project cache")
        {
            RecordType = recordType;
            RecordId = recordId;
        }
    }
}
