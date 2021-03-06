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

    public partial class Inventory
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A Product ID is required")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "A Color is required")]
        public string Color { get; set; }
        public Nullable<int> Amount { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
