using System;
using DatingApp.API.Data;
using DatingApp.API.Models;

namespace DatingApp.API.Repository
{
    public interface IUnitOfWork
    {
        void Save();
        BaseRepository<Value> ValueRepository();
    }
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public AppDbContext context { get; set; }
        public BaseRepository<Value> valueRepository { get; set; }
        private bool disposed = false;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public BaseRepository<Value> ValueRepository()
        {
            if (this.valueRepository == null)
            {
                this.valueRepository = new BaseRepository<Value>(context);
            }
            return valueRepository;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}