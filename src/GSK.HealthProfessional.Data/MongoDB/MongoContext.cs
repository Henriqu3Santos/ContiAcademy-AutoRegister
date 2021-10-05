using Microsoft.Extensions.Configuration;
using Microsoft.Win32.SafeHandles;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GSK.HealthProfessional.Data
{
    public class MongoContext : IMongoContext
    {
        bool disposed = false;
        readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private IMongoDatabase Database { get; set; }
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<Task>> _commands;
        public IClientSessionHandle Session { get; set; }
        public MongoContext(IConfiguration configuration)
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;            
            _commands = new List<Func<Task>>();
            MongoClient = new MongoClient(configuration.GetSection("MongoSettings").GetSection("Connection").Value);
            Database = MongoClient.GetDatabase(configuration.GetSection("MongoSettings").GetSection("DatabaseName").Value);
        }

       

        public async Task<int> SaveChanges(bool useSession = false)
        {

            if (useSession)
            {
                using (Session = await MongoClient.StartSessionAsync())
                {
                    Session.StartTransaction();
                    var commandTasks = _commands.Select(c => c());                  
                    await Task.WhenAll(commandTasks);
                    await Session.CommitTransactionAsync();
                }
            }
            else
            {

                var commandTasks = _commands.Select(c => c());
                await Task.WhenAll(commandTasks);

            }

            return _commands.Count;
        }


        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();                                
            }

            disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }
    }
}
