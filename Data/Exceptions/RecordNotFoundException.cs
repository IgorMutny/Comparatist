namespace Comparatist.Data.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public Type RecordType { get; }
        public Guid RecordId { get; }

        public RecordNotFoundException(Type recordType, Guid recordId) :
            base($"{recordType.Name} {recordId} not found in database")
        {
            RecordType = recordType;
            RecordId = recordId;
        }
    }
}
