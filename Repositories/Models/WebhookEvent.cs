using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

public partial class WebhookEvent
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Secret { get; set; } = null!;

    // BUY_LONG, BUY_SHORT, SL_SHORT, TP_SHORT, etc.
    [StringLength(50)]
    public string Type { get; set; } = null!;

    [Column(TypeName = "decimal(18, 8)")]
    public decimal Price { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Received"; // Received, Processed, Failed

    [Column(TypeName = "datetime")]
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    [InverseProperty("WebhookEvent")]
    public virtual ICollection<TradeLog> TradeLogs { get; set; } = new List<TradeLog>();
}
