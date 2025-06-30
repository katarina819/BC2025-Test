using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    public class DrinkOrderDto
    {
        
        public Guid UserId { get; set; }
       
        
        public List<DrinkOrderItemDto> Items { get; set; } = new();
    }

}
