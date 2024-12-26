using System;
using System.Collections.Generic;

namespace Bokking.Models;

public partial class UserLogin
{
    public decimal Id { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public decimal? Role { get; set; }

    public decimal? Customerid { get; set; }

    public virtual Customer? Customer { get; set; }
}
