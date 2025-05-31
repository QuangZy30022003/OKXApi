using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

public partial class TradeLog
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [StringLength(20)]
    public string? Symbol { get; set; }

    // LONG, SHORT
    [StringLength(10)]
    public string? Side { get; set; }

    // MARKET, LIMIT, SL, TP
    [StringLength(20)]
    public string? OrderType { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Quantity { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? EntryPrice { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? ExitPrice { get; set; }

    [StringLength(20)]
    public string? Status { get; set; } // Pending, Executed, Closed, Canceled

    public bool IsCurrentOrder { get; set; } = false;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "datetime")]
    public DateTime? ExecutedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ClosedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TradeLogs")]
    public virtual User? User { get; set; }

    // Link tới WebhookEvent (nếu có)
    public int? WebhookEventId { get; set; }

    [ForeignKey("WebhookEventId")]
    public virtual WebhookEvent? WebhookEvent { get; set; }
}
