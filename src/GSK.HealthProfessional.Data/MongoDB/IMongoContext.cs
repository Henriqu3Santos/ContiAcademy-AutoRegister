using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GSK.HealthProfessional.Data
{
    public interface IMongoContext : IDisposable
    {
         void AddCommand(Func<Task> func);
        Task<int> SaveChanges(bool useSession = false);
        IMongoCollection<T> GetCollection<T>(string name);
    }
}