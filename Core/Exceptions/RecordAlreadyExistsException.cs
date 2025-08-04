namespace Comparatist.Core.Exceptions
{
    public class RecordAlreadyExistsException : Exception
    {
        public Type RecordType { get; }
        public Guid RecordId { get; }

        public RecordAlreadyExistsException(Type recordType, Guid recordId) :
            base($"{recordType.Name} {recordId} already exists in database")
        {
            RecordType = recordType;
            RecordId = recordId;
        }
    }
}
