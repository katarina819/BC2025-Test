using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampApp.Common.DTOs


{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

   

}

