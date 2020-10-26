using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AmStock.Core.Models;
//using MATS.Core.Repository;
//using MATS.Core.Repository.Interfaces;

namespace MATS.Validation.CustomValidationAttributes
{
    public class Unqiue : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var contains= CustomerViewModel.SharedViewModel().Customers.Select(x => x.Id).Contains(int.Parse(value.ToString()));

            //if (contains)
            //    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            //else
                return ValidationResult.Success;
        }
    }
    public class UniqueCustomer : ValidationAttribute
    {
        string id ="0";
        public UniqueCustomer(string _id)
        {
            id = _id;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //IUnitOfWork unitOfWork = new UnitOfWork(new MATS.DAL.DbContextFactory().Create());
            //string val = value.ToString();
            //CustomerDTO customer = unitOfWork.Repository<CustomerDTO>().GetAll().Where(c => c.CustomerCode == val).FirstOrDefault();
            ////var contains= CustomerViewModel.SharedViewModel().Customers.Select(x => x.Id).Contains(int.Parse(value.ToString()));

            //if (customer!=null)
            //    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            //else
            return ValidationResult.Success;
        }
    } 
}
