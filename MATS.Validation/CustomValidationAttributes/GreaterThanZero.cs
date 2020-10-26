using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATS.Validation.CustomValidationAttributes
{
    public class GreaterThanZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //if (value != null)
            //{
            //    if (int.Parse(value.ToString()) <= 0)
            //    {
            //        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            //    }
            //}
            return ValidationResult.Success;
        }
    }
}
