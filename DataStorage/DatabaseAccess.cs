using DataStorage.Contracts;
using DataStoring.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStorage
{
    public class DatabaseAccess : IDatabaseAccess
    {
        public IEnumerable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public void InitDBA(string pathToDb)
        {
            throw new NotImplementedException();
        }

        public void InsertTables(IEnumerable<Type> types)
        {
            throw new NotImplementedException();
        }

        public void SaveObject(ISettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
