//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cleaning_Schedule
    {
        public int id { get; set; }
        public int room_id { get; set; }
        public System.DateTime cleaning_date { get; set; }
        public int user_id { get; set; }
        public int status_id { get; set; }
    
        public virtual Rooms Rooms { get; set; }
        public virtual Users Users { get; set; }
        public virtual Status Status { get; set; }
    }
}
