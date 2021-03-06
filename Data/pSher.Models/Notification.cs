﻿namespace PSher.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string Text { get; set; }

        public bool Seen { get; set; }

        public DateTime? SeenOn { get; set; }

        public DateTime SendOn { get; set; }
    }
}
