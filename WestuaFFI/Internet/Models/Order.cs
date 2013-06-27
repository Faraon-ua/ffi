using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Internet.Models
{
    public class Order
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public Dictionary<Product, int> Products { get; set; }
        public int Total
        {
            get
            {
                if (Products != null)
                    return Products.Sum(entry => entry.Key.Price * entry.Value);
                return -1;
            }
        }
    }
}