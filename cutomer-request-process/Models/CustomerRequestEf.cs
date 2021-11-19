﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cutomer_request_process
{
    public class CustomerRequestEf
    {
        [Key]
        public Guid account_id { get; set; } = Guid.NewGuid();
        public int  account_number { get; set; }
        public int account_type { get; set; }
        public int balance { get; set; }
        [RegularExpression("Active|Closed|Freezed", ErrorMessage = "Invalid Status")]
        public string account_status { get; set; }
        public int  userid { get; set; }
    }

}