using System;
using System.Collections.Generic;

namespace Bokking.Models;

public partial class Service
{
    public decimal Id { get; set; }

    public string? Nameservice { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public decimal? HotelId { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
