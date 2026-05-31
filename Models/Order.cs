using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace E_CommerceSystem.Models
{
    public class Order
    {
        [key]
        public int OrderId { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.Now;


        public List<OrderProducts> OrderProducts { get; set; }= new List<OrderProducts>();

        [Required]
        public decimal TotalAmount { get; set; }
        
 
        //relations

        [ForeignKey("User")]
        public int UserId { get; set; }

        //navigation property
        public virtual User user { get; set; }







    }


}

