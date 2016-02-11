using Common.General;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Common.DomainClasses
{
    [DataContract]
    [DebuggerDisplay("{Id}, {Name}")]
    public abstract class DomainClass : IEmptyAble, INotifyPropertyChanged
    {
        // TODO Make Id nullable?
        private static int NoId { get { return -1; } }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name; 
        }

        public bool IsEmpty()
        {
            return Id == NoId;
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
