using AutoMapper.Configuration.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace coreAPI.Models
{
    public class UserModel
    {
        [Ignore]
        public int ID { get; set; }

        [Required]
        [MaxLength(50), MinLength(5)]
        public string Login { get; set; }

        [Required]
        [MaxLength(50), MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50), MinLength(1)]
        public string FirstName { get; set; }

        [MaxLength(4), MinLength(4)]
        public string Clock { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255), MinLength(5)]
        public string Email { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [MaxLength(50), MinLength(1)]
        public string ModifiedBy { get; set; }

    }
}
