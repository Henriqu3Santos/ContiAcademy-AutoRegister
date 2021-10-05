using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GSK.HealthProfessional.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit(bool useSession = false);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit(bool useSession = false)
        {
            return await _context.SaveChanges(useSession) > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
