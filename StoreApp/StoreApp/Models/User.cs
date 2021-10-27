using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSeller { get; set; }
        public bool IsBuyer { get; set; }
    }
}
