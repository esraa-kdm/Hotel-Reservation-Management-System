using System;
using System.Collections.Generic;

namespace Bokking.Models;

public partial class ReservationService
{
    public decimal Id { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Cardid { get; set; }

    public string? InvoiceSent { get; set; }

    public decimal? Reservationid { get; set; }

    public virtual Bank? Card { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
