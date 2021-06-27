using System;
using System.Collections.Generic;
using System.Text;

namespace CheckInProgram
{
    public interface IPersister<T>
    {
        public void SaveObject(T t);

        public T GetObject(string identifier);

        public List<T> GetObjects();

        public void DeleteObject(string identifier);

        public void UpdateObject(T t, string identifier);

    }
}
