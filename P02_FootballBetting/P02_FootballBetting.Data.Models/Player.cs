﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        public Player() 
        {
            this.PlayersStatistics = new HashSet<PlayerStatistic>();
        }


        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int SquadNumber { get; set; }

        public bool IsInjured { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; } = null!;

        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }

        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }

    }
}
