using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MATS.Core.Enumerations;
using MATS.Validation.CustomValidationAttributes;

namespace MATS.Core.Models
{
    [Table("Users")]
    public class UserDTO : EntityBase
    {
        public UserDTO()
        {
            Roles = new HashSet<UsersInRoles>();
        }

        [Key]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(50)]
        [MinLength(6, ErrorMessage = "User Name Can't be less than Six charactes")]
        [MaxLength(10, ErrorMessage = "User Name Can't be greater than 10 charactes")]
        [ExcludeChar("/.,!@#$%", ErrorMessage = "Contains invalid letters")]
        public string UserName
        {
            get { return GetValue(() => UserName); }
            set { SetValue(() => UserName, value); }
        }

        [DataType(DataType.EmailAddress)]
        [MaxLength(50, ErrorMessage = "Exceeded 50 letters")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is invalid")]
        [StringLength(50)]
        public string Email
        {
            get { return GetValue(() => Email); }
            set { SetValue(() => Email, value); }
        }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(150)]
        public string Password
        {
            get { return GetValue(() => Password); }
            set { SetValue(() => Password, value); }
        }

        [DataType(DataType.Password)]        
        [NotMapped]
        public string ConfirmPassword
        {
            get { return GetValue(() => ConfirmPassword); }
            set { SetValue(() => ConfirmPassword, value); }
        }

        [Required]
        [StringLength(150)]
        [MaxLength(150, ErrorMessage = "Exceeded 150 letters")]
        public string FullName
        {
            get { return GetValue(() => FullName); }
            set { SetValue(() => FullName, value); }
        }

        [NotMapped]
        public string NewPassword
        {
            get { return GetValue(() => NewPassword); }
            set { SetValue(() => NewPassword, value); }
        }

        [Required]
        public UserTypes Status
        {
            get { return GetValue(() => Status); }
            set { SetValue(() => Status, value); }
        }

        public int Enabled
        {
            get { return GetValue(() => Enabled); }
            set { SetValue(() => Enabled, value); }
        }

        public DateTime? DateRecordCreated
        {
            get { return GetValue(() => DateRecordCreated); }
            set { SetValue(() => DateRecordCreated, value); }
        }

        public DateTime? DateLastModified
        {
            get { return GetValue(() => DateLastModified); }
            set { SetValue(() => DateLastModified, value); }
        }   

        //public virtual ICollection<RoleDTO> Roles
        //{
        //    get { return GetValue(() => Roles); }
        //    set { SetValue(() => Roles, value); }
        //}
        public ICollection<UsersInRoles> Roles
        {
            get { return GetValue(() => Roles); }
            set { SetValue(() => Roles, value); }
        }
    }
}
