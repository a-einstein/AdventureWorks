using System.Diagnostics;
using System.Runtime.Serialization;

namespace Common.DomainClasses
{
    [DataContract]
    [DebuggerDisplay("{ProductID}, {Name}, {ProductListPrice}, {Quantity}, {Value}")]
    public partial class CartItem : DomainClass
    {
        [DataMember]
        public int ProductID { get; set; }

        [DataMember]
        public string ProductSize { get; set; }

        [DataMember]
        public string ProductSizeUnitMeasureCode { get; set; }

        [DataMember]
        public string ProductColor { get; set; }

        [DataMember]
        public decimal ProductListPrice { get; set; }

        int quantity;

        [DataMember]
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                Value = ProductListPrice * Quantity;
                RaisePropertyChanged("Quantity");
            }
        }

        decimal value;

        [DataMember]
        public decimal Value
        {
            get { return value; }
            set
            {
                this.value = value;
                RaisePropertyChanged("Value");
            }
        }
    }
}
