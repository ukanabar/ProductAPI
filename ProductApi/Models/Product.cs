using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        [Required]
        [StringLength(5)]
        public string Code { get; set; }
        public string Description { get; set; }

        [Range(0, 999.99)]
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
