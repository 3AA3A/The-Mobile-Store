//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobilesProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Spec
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Spec()
        {
            this.Products = new HashSet<Product>();
        }
    
        public int Id { get; set; }
        [Required(ErrorMessage = "Ram is a required field.")]
        public string Ram { get; set; }
        [Required(ErrorMessage = "Storage is a required field.")]
        public string Storage { get; set; }
        [Required(ErrorMessage = "Camera is a required field.")]
        public string Camera { get; set; }
        [Required(ErrorMessage = "Display is a required field.")]
        public string Display { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
