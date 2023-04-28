using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{

    [Table("user_role")]
    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // not nullable
        [Column("user_id")]
        public int user_id { get; set; }

        [NotMapped]
        public UserRoleEnum _User_Role
        {
            get
            {
                return (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), role);
            }
            set
            {
                this.role = value.ToString();
            }
        }

        private string backingtype { get; set; }
        [Required]
        [Column("role")]
        public string role
        {
            get
            {
                return this.backingtype;
            }
            set
            {
                if(value == null || Enum.IsDefined(typeof(UserRoleEnum), value))
                {
                    this.backingtype = value;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }

        public UserRole() { }

        public UserRole(int uid, UserRoleEnum e)
        {
            user_id = uid;
            _User_Role = e;
        }
    }
}
