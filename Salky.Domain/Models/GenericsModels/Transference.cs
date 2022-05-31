namespace Salky.Domain.Models.GenericsModels
{
    public class Transference : BaseEntity
    {
        public Transference(Guid from, Guid to, string reason)
        {
            From = from;
            To = to;
            Reason = reason;
        }
        private Transference()
        {

        }

        public Guid From { get; }
        public Guid To { get; }
        public string Reason { get; }


    }
}