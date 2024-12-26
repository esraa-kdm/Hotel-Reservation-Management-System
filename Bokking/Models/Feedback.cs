using System;
using System.Collections.Generic;

namespace Bokking.Models;

public partial class Feedback
{
    public decimal Id { get; set; }

    public string? Guestname { get; set; }

    public string? Guestemail { get; set; }

    public string? Guestmessage { get; set; }

    public string? Isapproved { get; set; }
}
