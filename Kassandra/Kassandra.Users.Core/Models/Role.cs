using System;

namespace Kassandra.Users.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; }
    }
}