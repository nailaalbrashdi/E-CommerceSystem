using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_CommerceSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",ErrorMessage = "Password must be at least 8 characters and contain uppercase, number, and special character.")]
        public string Password { get; set; }

        [Required]
        
        public string Phone { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        //relations 
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }


    }
}
