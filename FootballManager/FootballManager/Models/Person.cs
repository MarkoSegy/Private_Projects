using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager.Models
{
    public class  Person
	{
        public int Id { get; set; }
        
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Min. 3 & Max. 10 Lethers!")]
        public string Firstname { get; set; }
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Min. 3 & Max. 10 Lethers!")]
        public string Lastname { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Birthday { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        //[HiddenInput(DisplayValue = false)]
        public string Password { get; set; } 
		[HiddenInput(DisplayValue = false)]
        public string SaltedPassword { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string PasswordEncrypted { get; set; }
        public PersonRoles PersonRole { get; set; }
        public bool AdminPermission { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public Team Team { get; set; }
        public string ImagePath { get; set; } 
        public bool PlaysImTeam { get; set; }
        public bool isLogedIn { get; set; } = false;


        public enum PersonRoles
        {
            Player,
            Trainer,
            Manager
        }
       
    }
}
