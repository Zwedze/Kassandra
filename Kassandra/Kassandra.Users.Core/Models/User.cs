using System;

namespace Kassandra.Users.Core.Models
{
    public class User
    {
        public long Id { get; set; }
        public Guid Uid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
    }
}