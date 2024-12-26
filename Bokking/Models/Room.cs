using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bokking.Models;

public partial class Room
{
    public decimal Id { get; set; }

    public decimal? Rommnumber { get; set; }

    public string? Roomtype { get; set; }

    public decimal? Pricepernight { get; set; }

    public string? Availability { get; set; }

    public string? Description { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }

    public decimal? HotelId { get; set; }

    public virtual Hotel? Hotel { get; set; }
}
