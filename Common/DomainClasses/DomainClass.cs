using System.ComponentModel;
using System.Runtime.Serialization;

namespace Common.DomainClasses
{
    [DataContract]
    public abstract class DomainClass : INotifyPropertyChanged
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

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
