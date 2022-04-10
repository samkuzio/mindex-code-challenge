using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Add(Compensation comp);
        Compensation GetByEmployeeId(String id);
        Compensation Remove(Compensation comp);
        Task SaveAsync();
    }
}