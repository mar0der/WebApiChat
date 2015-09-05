using System;
using System.Linq.Expressions;
using WebApiChat.Models.Models;
namespace WebApiChat.Web.Models
{
    public class UserSearchViewModel
    {
        public static Expression<Func<User, UserSearchViewModel>> ViewModel
        {
            get
            {
                var onlineUsers = ConnectionManager.Users.Keys;

                return u => new UserSearchViewModel()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                    IsOnline = onlineUsers.Contains(u.UserName)
                };
            }
                    
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool IsOnline { get; set; }

       // public bool IsFriend { get; set; }
    }
}