﻿using MusiCom.Infrastructure.Data.Entities.Events;
using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.GenreC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    /// <summary>
    /// Contains data for News' Musical Genres
    /// </summary>
    public class Genre
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The News which are from the given Genre
        /// </summary>
        public ICollection<New> News { get; set; } = new List<New>();

        /// <summary>
        /// The Events which are from the given Genre
        /// </summary>
        public ICollection<Event> Events { get; set; } = new List<Event>();

        [Required]
        public DateTime DateOfCreation { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
