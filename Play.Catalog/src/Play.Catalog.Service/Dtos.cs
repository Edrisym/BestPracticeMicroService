using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service
{
    public class Dtos
    {
        public record ItemDto(Guid Id , string Name , string Description,  decimal Price , DateTimeOffset CreatedDate);
        public record CreateItemDto(string Name , string Description , decimal Price);
        public record UpdateItemDto(string Name , string Description , decimal Price);
    }
}