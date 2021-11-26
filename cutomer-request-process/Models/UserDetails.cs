using System;
using System.ComponentModel.DataAnnotations;

namespace cutomer_request_process.Models
{
    public class UserDetails
    {
        [Key]
        public Guid userid { get; set; }
        public string username { get; set; }
        public string user_password { get; set; }
        public string user_address { get; set; }
        public string phone_number { get; set; }
        public string email_id { get; set; }
        public string university_name { get; set; }
        public string user_type { get; set; }
        public bool is_enabled { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

    }
}
