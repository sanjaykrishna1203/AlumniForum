using System;
using System.Collections.Generic;

#nullable disable

namespace AlumniForum.Models
{
    public partial class User
    {
        public User()
        {
            Events = new HashSet<Event>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Degree { get; set; }
        public int YearOfCompletion { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string CurrentJob { get; set; }
        public string CurrentCompany { get; set; }
        public int? Role { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
