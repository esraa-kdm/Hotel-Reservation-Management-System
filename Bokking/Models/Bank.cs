using System;
using System.Collections.Generic;

namespace Bokking.Models;

public partial class Bank
{
    public decimal Id { get; set; }

    public string? Cardname { get; set; }

    public decimal? Cardid { get; set; }

    public decimal? Balance { get; set; }

    public virtual ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();
}
