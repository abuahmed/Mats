using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MATS.Core.Models
{
    [Table("Roles")]
    public class RoleDTO : EntityBase
    {
        public RoleDTO()
        {
            Users = new List<UsersInRoles>();
        }
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(255)]
        [MaxLength(255, ErrorMessage = "Exceeded 255 letters")]
        public string RoleDescription
        {
            get { return GetValue(() => RoleDescription); }
            set { SetValue(() => RoleDescription, value); }
        }

        public string RoleDescriptionShort
        {
            get { return GetValue(() => RoleDescriptionShort); }
            set { SetValue(() => RoleDescriptionShort, value); }
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

        public ICollection<UsersInRoles> Users
        {
            get { return GetValue(() => Users); }
            set { SetValue(() => Users, value); }
        }
    }
    [Table("UsersInRoles")]
    public class UsersInRoles : EntityBase
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserDTO User
        {
            get { return GetValue(() => User); }
            set { SetValue(() => User, value); }
        }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public RoleDTO Role
        {
            get { return GetValue(() => Role); }
            set { SetValue(() => Role, value); }
        }

    }

}
