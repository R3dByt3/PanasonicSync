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
            return Array.Empty<T>();
        }

        public void InitDBA(string pathToDb)
        {
        }

        public void InsertTables(IEnumerable<Type> types)
        {
        }

        public void SaveObject(ISettings settings)
        {
        }
    }
}
