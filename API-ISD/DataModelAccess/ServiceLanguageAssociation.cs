//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
namespace DataModelAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceLanguageAssociation
    {
        [Key]
        public int serviceLanguageAssociation1 { get; set; }
        public Nullable<int> serviceID { get; set; }
        public Nullable<int> languageID { get; set; }
    
        public virtual Language Language { get; set; }
        public virtual Service Service { get; set; }
    }
}