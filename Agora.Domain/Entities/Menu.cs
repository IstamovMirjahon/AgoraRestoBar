﻿using Agora.Domain.Abstractions;

namespace Agora.Domain.Entities
{
    public class Menu : BaseParams
    {
        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public string Description { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }

}
