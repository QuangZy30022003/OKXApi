    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    namespace Repositories.Models;

    [Table("OKXKeys")]
    public partial class Okxkey
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        [StringLength(100)]
        public string ApiKey { get; set; } = null!;

        [StringLength(100)]
        public string SecretKey { get; set; } = null!;

        [StringLength(100)]
        public string Passphrase { get; set; } = null!;

        [StringLength(100)]
        public string? Permissions { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [InverseProperty("Okxkey")]
        public virtual ICollection<SubAccount> SubAccounts { get; set; } = new List<SubAccount>();

        [ForeignKey("UserId")]
        [InverseProperty("Okxkeys")]
        public virtual User? User { get; set; }
    }
