using System;
using System.Collections.Generic;

#nullable disable

namespace AlumniForum.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public DateTime? DateAndTime { get; set; }
        public string EventDescription { get; set; }
        public int? UserRefId { get; set; }

        public virtual User UserRef { get; set; }
    }
}
