//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Moviit.Infrastructure.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ORDER_SANDWICH
    {
        public int Id { get; set; }
        public int SandwichId { get; set; }
        public short Quantity { get; set; }
        public int OrderId { get; set; }
    
        public virtual ORDER ORDER { get; set; }
        public virtual SANDWICH SANDWICH { get; set; }
    }
}
