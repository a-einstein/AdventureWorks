using RCS.AdventureWorks.Common.General;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RCS.AdventureWorks.Common.DomainClasses
{
    // Note these classes in fact are DTO's too.
    // TODO Reconsider namespaces and whether this should be a separately included project elsewhere.
    [DataContract]
    // Note this is not implemented in Mono.
    [DebuggerDisplay("{Id.HasValue ? Id.Value : 0}, {Name}")]
    public abstract class DomainClass : IEmptyAble, INotifyPropertyChanged
    {
        // This has been made nullable for practical reasons, 
        // particularly to enable empty elements in dropdown lists.
        // These elements can also be convenient for filtering.
        // Besides that a new object to be inserted may not have a true Id yet.
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public bool IsEmpty
        {
            get { return !Id.HasValue; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Currently for selective raising of events, before turning properties into full DependencyProperties.
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
