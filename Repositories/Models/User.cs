using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

[Index("Email", Name = "UQ__Users__A9D1053478DA7197", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Okxkey> Okxkeys { get; set; } = new List<Okxkey>();

    [InverseProperty("ParentUser")]
    public virtual ICollection<SubAccount> SubAccounts { get; set; } = new List<SubAccount>();

    [InverseProperty("User")]
    public virtual ICollection<TradeLog> TradeLogs { get; set; } = new List<TradeLog>();
}
