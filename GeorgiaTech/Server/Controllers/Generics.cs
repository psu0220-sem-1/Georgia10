using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Controllers
{
    public interface IGenerics<T>
    {
        public void Create(T t);

        public void Read(T t);
        public void Update(T t);
        public void Delete(T t);
    }
}
