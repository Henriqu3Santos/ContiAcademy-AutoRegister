using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GSK.HealthProfessional.Data
{
    
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<TEntity> DbSet;

        protected BaseRepository(IMongoContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>(GetCollectionName());
        }

        public virtual void Add(TEntity obj)
        {
             //_context.AddCommand(async () => await DbSet.InsertOneAsync(obj));
              DbSet.InsertOne(obj);
        }

        private static string GetCollectionName()
        {

            var bsonCollectionAttribute = (typeof(TEntity).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute);
            string collectionName;

            if (bsonCollectionAttribute  == null )            
                collectionName = typeof(TEntity).Name;
            else
                collectionName = bsonCollectionAttribute.CollectionName;

            return collectionName;
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.FirstOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        //public virtual async Task Update(TEntity obj)
        //{
        //     _context.AddCommand(async () =>
        //    {
        //        await DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj);
        //    });
        //}

        public virtual IEnumerable<TEntity> Query()
        {
            return DbSet.Find(FilterDefinition<TEntity>.Empty).ToList();
        }
        public virtual IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.Find(filter).ToList();
        }
        public virtual void Remove(Guid id) =>  _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        private readonly string _collectionName;
        public BsonCollectionAttribute(string collectionName)
        {
            _collectionName = collectionName;
        }
        public string CollectionName => _collectionName;


    }
}
