using System;
using System.Collections.Generic;

#nullable disable

namespace AlumniForum.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public int UserRefId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User UserRef { get; set; }
    }
}
