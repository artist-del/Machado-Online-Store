//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Machado_Store.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    
    public partial class tbl_product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_product()
        {
            this.order_list = new HashSet<order_list>();
        }
    
        public int product_Id { get; set; }
        public string tbl_productName { get; set; }
        public Nullable<decimal> tbl_price { get; set; }
        public string tbl_description { get; set; }
        public Nullable<int> tbl_quantity { get; set; }
        public string product_img { get; set; }
        public Nullable<decimal> tbl_wholesale { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_list> order_list { get; set; }
    }
}