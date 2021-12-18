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

    public partial class Delivery
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Type of Delivery is a required field.")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Status is a required field.")]
        public string Status { get; set; }
        public int CartId { get; set; }
        public int ShippingId { get; set; }
    
        public virtual Cart Cart { get; set; }
        public virtual Shipping Shipping { get; set; }
    }
}
