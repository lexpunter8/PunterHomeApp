using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.crud
{
    interface ICrudHandler<T>
    {
        Task<bool> Create(T value);
        Task<bool> Create(string value);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        Task<bool> DeletetById(Guid id);
        Task<bool> Update(T value);

    }
}
