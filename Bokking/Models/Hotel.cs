using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bokking.Models;

public partial class Hotel
{
    public decimal Id { get; set; }

    public string? Hotelname { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public decimal? Phone { get; set; }

    public string? Email { get; set; }

    public string? Imagepath { get; set; }

    [NotMapped]
    public virtual  IFormFile ImageFile { get; set; }

    public string? Description { get; set; }
}
