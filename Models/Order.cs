using System;
using System.Collections.Generic;
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

        // Calculated Total Amount
        [NotMapped]
        public decimal TotalAmount
        {
            get
            {
                if (OrderProducts == null || OrderProducts.Count == 0)
                    return 0;

                return OrderProducts.Sum(op =>
                    op.Product.Price * op.Quantity);
            }
        }










        //relations

        [ForeignKey("User")]
        public int UserId { get; set; }

        //navigation property
        public virtual User user { get; set; }







    }


}

