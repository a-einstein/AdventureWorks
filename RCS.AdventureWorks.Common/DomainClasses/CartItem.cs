using System.Diagnostics;
using System.Runtime.Serialization;

namespace RCS.AdventureWorks.Common.DomainClasses
{
    [DataContract]
    // Note this is not implemented in Mono.
    [DebuggerDisplay("{Id.HasValue ? Id.Value : 0}, {Name}, {ProductListPrice}, {Quantity}, {Value}")]
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

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                Value = ProductListPrice * Quantity;
                RaisePropertyChanged(nameof(Quantity));
            }
        }

        decimal value;

        public decimal Value
        {
            get { return value; }
            set
            {
                this.value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}
