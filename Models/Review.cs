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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Review
    {
        public int Id { get; set; }
        public string CustomerUserName { get; set; }
        [StringLength(50, ErrorMessage = "Title must be less than 50 Characters")]
        public string Title { get; set; }
        public System.DateTime Date { get; set; }
        [StringLength(50, ErrorMessage = "Feedback must be less than 50 Characters")]
        public string Feedback { get; set; }
        public string Reply { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}