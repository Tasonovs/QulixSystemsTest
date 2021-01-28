using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QulixSystemsTest1.Data;
using QulixSystemsTest1.Models;

namespace QulixSystemsTest1.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeesDataContext dataContext = new EmployeesDataContext();

        // GET: Employees
        public IActionResult Index()
        {
            return View(dataContext.Read());
        }

        // GET: Employees/Edit/5 [id for update or null for insert]
        public IActionResult AddOrEdit(int? id)
        {
            //TODO Для должностей можно и нужно создать отдельную таблицу в БД,
            //т.к. для добавления нового варианта придется редактировать программный код и заново размещать проект
            SelectList positions = new SelectList(new List<string> { "Разработчик", "Тестировщик", "Бизнес-аналитик", "Менеджер" });
            ViewBag.Positions = positions;

            CompaniesDataContext companiesDataContext = new CompaniesDataContext();
            SelectList companies = new SelectList(companiesDataContext.Read(), "Id", "Name");
            ViewBag.Companies = companies;

            return View(dataContext.Read(id));
        }

        // POST: Employees/Edit/5 [id for update or null for insert]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("Id,LastName,FirstName,MiddleName,EmploymentDate,Position,CompanyId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                dataContext.AddOrEdit(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

		// GET: Tests/Delete/5
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0) return NotFound();

			Employee employee = dataContext.Read(id);

			if (employee == null) return NotFound();

			return View(employee);
		}

		// POST: Tests/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			dataContext.Delete(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
