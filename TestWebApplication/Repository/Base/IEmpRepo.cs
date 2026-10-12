using TestWebApplication.Models;

namespace TestWebApplication.Repository.Base
{
    public interface IEmpRepo : IRepository<Employees>
    {
        void setPayRoll(Employees Employee);

        decimal getSalary(Employees Employee);
    }
}
