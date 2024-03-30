﻿

namespace EventSchedule.Models.DTO
{
    public class ProductDto
    {
       
        public Guid Id { get; set; }

        // Name of the product.
        public string Name { get; set; }

        // Description of the product.
        public string Description { get; set; }

        // Price of the product. Using float, but consider using decimal for monetary values.
        public float Price { get; set; }

     
    }
}
