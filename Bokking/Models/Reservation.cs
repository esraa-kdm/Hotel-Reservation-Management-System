using System;
using System.Collections.Generic;

namespace Bokking.Models;

public partial class Reservation
{
    public decimal Id { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? PayementStatus { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Hotelid { get; set; }

    public decimal? Roomid { get; set; }

    public decimal? Serviceid { get; set; }

    public virtual ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();

    public virtual Service? Service { get; set; }

    public virtual Customer? User { get; set; }

    public virtual Room? room { get; set; }
    public virtual Hotel? hotel { get; set; }
}
