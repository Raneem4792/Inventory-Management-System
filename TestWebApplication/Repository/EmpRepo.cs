using TestWebApplication.Data;
using TestWebApplication.Models;
using TestWebApplication.Repository.Base;

namespace TestWebApplication.Repository
{
    public class EmpRepo : MainRepository<Employees>, IEmpRepo
    {
        public EmpRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;


        decimal IEmpRepo.getSalary(Employees Employee)
        {
            throw new NotImplementedException();
        }

        void IEmpRepo.setPayRoll(Employees Employee)
        {
            throw new NotImplementedException();
        }
    }
}
