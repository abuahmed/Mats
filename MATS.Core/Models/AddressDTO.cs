﻿using MATS.Core.Common;
using System.ComponentModel.DataAnnotations;
using MATS.Validation.CustomValidationAttributes;

namespace MATS.Core.Models
{
    public class AddressDTO : CommonFieldsA
    {
        public string Country
        {
            get { return GetValue(() => Country); }
            set { SetValue(() => Country, value); }
        }
        public string City
        {
            get { return GetValue(() => City); }
            set { SetValue(() => City, value); }
        }

        [MaxLength(250, ErrorMessage = "Exceeded 250 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contains invalid letters")]
        public string StreetAddress
        {
            get { return GetValue(() => StreetAddress); }
            set { SetValue(() => StreetAddress, value); }
        }
        public string SubCity
        {
            get { return GetValue(() => SubCity); }
            set { SetValue(() => SubCity, value); }
        }
        public string Kebele
        {
            get { return GetValue(() => Kebele); }
            set { SetValue(() => Kebele, value); }
        }
        public string HouseNumber
        {
            get { return GetValue(() => HouseNumber); }
            set { SetValue(() => HouseNumber, value); }
        }

        [MaxLength(30, ErrorMessage = "Exceeded 30 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contains invalid letters")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]{10,14}$", ErrorMessage = "Telephone is invalid")]
        public string Telephone
        {
            get { return GetValue(() => Telephone); }
            set { SetValue(() => Telephone, value); }
        }

        [MaxLength(30, ErrorMessage = "Exceeded 30 letters")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contains invalid letters")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]{10,14}$", ErrorMessage = "Mobile Number is invalid")]
        [Required]
        public string Mobile
        {
            get { return GetValue(() => Mobile); }
            set { SetValue(() => Mobile, value); }
        }

        public string PoBox
        {
            get { return GetValue(() => PoBox); }
            set { SetValue(() => PoBox, value); }
        }
        [RegularExpression("^[0-9]{8,14}$", ErrorMessage = "Mobile Number is invalid")]
        public string Fax
        {
            get { return GetValue(() => Fax); }
            set { SetValue(() => Fax, value); }
        }

        [DataType(DataType.EmailAddress)]
        [MaxLength(30, ErrorMessage = "Exceeded 30 letters")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is invalid")]
        [Required]
        public string PrimaryEmail
        {
            get { return GetValue(() => PrimaryEmail); }
            set { SetValue(() => PrimaryEmail, value); }
        }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is invalid")]
        public string AlternateEmail
        {
            get { return GetValue(() => AlternateEmail); }
            set { SetValue(() => AlternateEmail, value); }
        }


    }

}
