using DataStoring.Contracts;
using System;
using System.Collections.Generic;

namespace DataStorage.Contracts
{
    public interface IDatabaseAccess
    {
        void InitDBA(string pathToDb);
        void InsertTables(IEnumerable<Type> types);
        IEnumerable<T> GetAll<T>();
        void SaveObject(ISettings settings);
    }
}
