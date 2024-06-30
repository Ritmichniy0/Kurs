using Sem4DTO;
using GBLesson4SecurityMarket.Model;

namespace GBLesson4SecurityMarket.Abstraction
{
    public interface IUserRepository
    {
        public void AddUser(string email, string password, UserRoleType userRoleType);
        public UserRoleType CheckUser(string email, string password);
    }
}
