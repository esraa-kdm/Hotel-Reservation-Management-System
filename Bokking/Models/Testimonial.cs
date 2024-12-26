using System;
using System.Collections.Generic;

namespace Bokking.Models;

public partial class Testimonial
{
    public decimal Id { get; set; }

    public string? Message { get; set; }

    public string? Isapproved { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Hotelid { get; set; }

    public virtual Customer? User { get; set; }

    public virtual Hotel? hotel { get; set; }





}
