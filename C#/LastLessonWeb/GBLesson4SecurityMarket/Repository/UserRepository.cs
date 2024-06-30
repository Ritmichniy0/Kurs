using Sem4DTO;
using GBLesson4SecurityMarket.Abstraction;
using GBLesson4SecurityMarket.Context;
using GBLesson4SecurityMarket.Model;
using System;
using System.Linq;
using System.Text;
using XSystem.Security.Cryptography;

namespace GBLesson4SecurityMarket.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserRoleContext _context;

        public UserRepository(UserRoleContext context)
        {
            _context = context;
        }

        public void AddUser(string email, string password, UserRoleType userRoleType)
        {
            var checkUser = _context.Users.FirstOrDefault(user => user.Email == email);

            if (checkUser == null)
            {

                var newUser = new User() { Email = email, RoleId = userRoleType };
                newUser.Salt = new byte[16];
                new Random().NextBytes(newUser.Salt);
                var data = Encoding.UTF8.GetBytes(password).Concat(newUser.Salt).ToArray();
                newUser.Password = new SHA512Managed().ComputeHash(data);
                _context.Add(newUser);
                _context.SaveChanges();
            }
        }

        public UserRoleType CheckUser(string email, string password)
        {
            var checkUser = _context.Users.FirstOrDefault(user => user.Email == email);
            if (checkUser == null)
            {
                throw new Exception("User not found");
            }

            var data = Encoding.UTF8.GetBytes(password).Concat(checkUser.Salt).ToArray();
            var hash = new SHA512Managed().ComputeHash(data);
            if (checkUser.Password.SequenceEqual(hash))
            {
                return checkUser.RoleId;
            }

            throw new Exception("Some error");
        }
    }
}
