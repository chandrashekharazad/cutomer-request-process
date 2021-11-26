using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cutomer_request_process.Models
{
    public class CustomerRequest
    {
        [Key]
        public Guid request_id { get; set; }
        public Guid userid { get; set; }
        public string request_remarks { get; set; }
        public string request_status { get; set; }
        public string request_type { get; set; }
        public string request_description { get; set; }
    }
}
