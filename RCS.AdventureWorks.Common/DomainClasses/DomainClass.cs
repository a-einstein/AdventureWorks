using RCS.AdventureWorks.Common.General;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

// TODO Rename to Domain?
namespace RCS.AdventureWorks.Common.DomainClasses
{
    [DataContract]
    [DebuggerDisplay("{Id}, {Name}")]
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
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
