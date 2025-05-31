using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

public partial class SubAccount
{
    [Key]
    public int Id { get; set; }

    public int? ParentUserId { get; set; }

    [StringLength(100)]
    public string? SubAccountName { get; set; }

    [Required]
    [Column("OKXKeyId")]
    public int? OkxkeyId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("OkxkeyId")]
    [InverseProperty("SubAccounts")]
    public virtual Okxkey? Okxkey { get; set; }

    [ForeignKey("ParentUserId")]
    [InverseProperty("SubAccounts")]
    public virtual User? ParentUser { get; set; }
}
