using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using DatingApp.API.Repository;

namespace DatingApp.API.Services
{
    public interface IValueService
    {
        IEnumerable<Value> GetAll();
        Task<bool> InsertAsync(Value value);
    }
    public class ValueService : IValueService
    {
        public IUnitOfWork unitOfWork { get; set; }

        public ValueService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Value> GetAll()
        {
            return unitOfWork.ValueRepository().GetList();
        }

        public async Task<bool> InsertAsync(Value value)
        {
            var result = await unitOfWork.ValueRepository().InsertAsync(value);
            unitOfWork.Save();

            return result;
        }
    }
}