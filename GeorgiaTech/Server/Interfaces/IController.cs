/// <summary>
/// IController is the toplevel interface which the functional controllers inherit from.
/// </summary>
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Controllers
{
    public interface IController<T>
    {
        public T Insert(T t);
        public T FindByID(int ID);
        public List<T> FindAll();
        public int Update(T t);
        public int Delete(T t);
    }
}
