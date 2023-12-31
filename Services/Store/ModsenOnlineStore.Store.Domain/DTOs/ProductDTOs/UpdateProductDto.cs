﻿namespace ModsenOnlineStore.Store.Domain.DTOs.ProductDTOs
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public int ProductTypeId { get; set; }
    }
}
