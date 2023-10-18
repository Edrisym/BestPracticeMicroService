using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service
{
    public class Dtos
    {
        public record ItemDto(Guid Id ,string Name , string Description,  decimal Price , DateTimeOffset CreatedDate);
        public record CreateItemDto( [Required]string Name , string Description , [Range(0,10000)] decimal Price);
        public record UpdateItemDto( [Required]string Name , string Description , [Range(0,10000)] decimal Price);
    }
}