
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmniahUsers_DAL.Data;
using UmniahUsers_DAL.Interface;
using UmniahUsers_DAL.Models;

namespace UmniahUsers_DAL.Repository
{
    public class RepositoryUser : IRepository<User>
    {
        ApplicationDbContext _dbContext;
        public RepositoryUser(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        public async Task<User> Create(User _object)
        {
            var obj = await _dbContext.Users.AddAsync(_object);
            _dbContext.SaveChanges();
            return obj.Entity;
        }

        public void Delete(User _object)
        {
            _dbContext.Remove(_object);
            _dbContext.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return _dbContext.Users.Where(x => x.IsActive).ToList();
            }
            catch (Exception ee)
            {
                throw;
            }
        }

        public User GetById(int Id)
        {
            return _dbContext.Users.Where(x => x.IsActive && x.Id == Id).FirstOrDefault();
        }

        public User GetUserByName(string username)
        {
            return _dbContext.Users.Where(x => x.IsActive && x.UserName == username).FirstOrDefault();
        }

        public void Update(User _object)
        {
            _dbContext.Users.Update(_object);
            _dbContext.SaveChanges();
        }
    }
}
