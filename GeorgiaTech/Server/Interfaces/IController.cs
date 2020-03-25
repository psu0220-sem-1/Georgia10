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
        public T FindByType(T t);
        public List<T> FindAll();
        public T Update(T t);
        public int Delete(T t);
    }
}
