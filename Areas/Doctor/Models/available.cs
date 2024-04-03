using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.Areas.Doctor.Models
{
    public class available
    {
        public int Id { get; set; }
        public int User_Id { get; set; }

        public string Name { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Please select a future date.")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan STime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [EndTimeGreaterThanStartTime(ErrorMessage = "End Time must be greater than Start Time.")]
        public TimeSpan ETime { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = (DateTime)value;
            return dt.Date >= DateTime.Now.Date;
        }
    }

    public class EndTimeGreaterThanStartTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var available = (available)validationContext.ObjectInstance;

            if (available.ETime <= available.STime)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
