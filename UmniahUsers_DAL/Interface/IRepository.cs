using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UmniahUsers_DAL.Interface
{
    public interface IRepository<T>
    {
        Task<T> Create(T _object);
        void Update(T _object);
        IEnumerable<T> GetAll();
        T GetById(int Id);
        T GetUserByName(string username);
        void Delete(T _object);

    }
}