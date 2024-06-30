namespace GBLesson4SecurityMarket.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public UserRoleType RoleId { get; set; }

        public virtual Role Role { get; set; }

    }
}
