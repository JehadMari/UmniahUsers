using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UmniahUsers_DAL.Interface;
using UmniahUsers_DAL.Models;

namespace UmniahUsers_BAL.Service
{
    public class UserService
    {
        private readonly IRepository<User> _User;

        public UserService(IRepository<User> user)
        {
            _User = user;
        }
        //Get User Details By User Id
        public User GetUserById(int UserId)
        {
            return _User.GetById(UserId);
        }

        //User Login
        public User GetUserByUserName(string username)
        {
            return _User.GetUserByName(username);
        }
        //GET All Users 
        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return _User.GetAll().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Add User
        public async Task<User> AddUser(User User)
        {
            return await _User.Create(User);
        }
        //Delete User 
        public bool DeleteUser(int UserId)
        {
            try
            {
                var user = _User.GetById(UserId);
                _User.Delete(user);
                return true;
            }
            catch (Exception)
            {
                return true;
            }

        }
        //Update User Details
        public bool UpdateUser(User user)
        {
            try
            {
                _User.Update(user);
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }

}
