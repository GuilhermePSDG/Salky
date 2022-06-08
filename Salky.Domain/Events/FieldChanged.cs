namespace Salky.Domain.Events
{
    public class FieldChanged<FieldValueType>
    {
        public FieldChanged(Guid EntityId, FieldValueType value, string FieldName)
        {
            Id = EntityId;
            Value = value;
            this.FieldName = FieldName;
        }

        public Guid Id { get; }
        public FieldValueType Value { get;}
        public string FieldName { get; }
    }
}
