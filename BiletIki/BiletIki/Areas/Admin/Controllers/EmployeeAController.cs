using BiletIki.DAL;
using BiletIki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Drawing.Imaging;

namespace BiletIki.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeAController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeAController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid) 
            {
                return View(); 
            }
            bool isExistName = await _context.Employees.AnyAsync(c => c.Name.ToLower().Trim() == employee.Name.ToLower().Trim());
            bool isExistLevel = await _context.Employees.AnyAsync(d => d.Level.ToLower().Trim() == employee.Level.ToLower().Trim());
            bool isExistBio = await _context.Employees.AnyAsync(e => e.Bio.ToLower().Trim() == employee.Bio.ToLower().Trim());
            
            if (isExistName)
            {
                ModelState.AddModelError("Name", "Already exist");
                return View();
            } 
            if (isExistLevel)
            {
                ModelState.AddModelError("Level", "Already exist");
                return View();
            }   
            if (isExistBio)
            {
                ModelState.AddModelError("Bio", "Already exist");
                return View();
            }

            if(employee.FormFile is null)
            {
                ModelState.AddModelError("FormFile", "Image can not be null");
                return View();
            }
            if (!employee.FormFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("FormFile", "enter image");
                return View();
            }
            if(employee.FormFile.Length / 1024 / 1024 >= 15)
            {
                ModelState.AddModelError("FormFile", "Image must be under 2MB");
                return View();
            }
            string FileName = Guid.NewGuid() + employee.FormFile.FileName;
            string FolderName = "/assets/img/team/";
            string FullPath = _env.WebRootPath + FolderName+ FileName;
            string path = Path.Combine(FullPath);
            using (FileStream stream = new (path, FileMode.Create))
            {
                employee.FormFile.CopyTo(stream);
            }
            employee.Img = FileName;

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Employee? employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Employee employee)
        {
            Employee? editEmployee = _context.Employees.Find(employee.Id);
            if (editEmployee == null)
            {
                return NotFound();
            }
            if(!ModelState.IsValid)
            {
                return View();
            }
            editEmployee.Name = employee.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Employee? employee = _context.Employees.Find(id);
            if (employee == null) { return NotFound(); }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
    