
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApplication.Models;
using TestWebApplication.Data;
using Microsoft.AspNetCore.Authorization;


[Area("Employees") ,Authorize]
[Authorize(Roles = "Admin")]
public class EmployeesController : Controller
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: EMPLOYEESS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Employees.ToListAsync());
    }

    // GET: EMPLOYEESS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employees = await _context.Employees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (employees == null)
        {
            return NotFound();
        }

        return View(employees);
    }

    // GET: EMPLOYEESS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EMPLOYEESS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,EmployeeName,EmployeePhone,EmployeeEmail,EmployeeAge,EmployeeSalary")] Employees employees)
    {
        if (ModelState.IsValid)
        {
            _context.Add(employees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(employees);
    }

    // GET: EMPLOYEESS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employees = await _context.Employees.FindAsync(id);
        if (employees == null)
        {
            return NotFound();
        }
        return View(employees);
    }

    // POST: EMPLOYEESS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,EmployeeName,EmployeePhone,EmployeeEmail,EmployeeAge,EmployeeSalary")] Employees employees)
    {
        if (id != employees.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(employees);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(employees.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(employees);
    }

    // GET: EMPLOYEESS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employees = await _context.Employees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (employees == null)
        {
            return NotFound();
        }

        return View(employees);
    }

    // POST: EMPLOYEESS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var employees = await _context.Employees.FindAsync(id);
        if (employees != null)
        {
            _context.Employees.Remove(employees);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EmployeesExists(int? id)
    {
        return _context.Employees.Any(e => e.Id == id);
    }
}
