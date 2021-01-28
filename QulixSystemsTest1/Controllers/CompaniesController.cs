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
    public class CompaniesController : Controller
    {
        private readonly IConfiguration configuration;
        private CompaniesDataContext dataContext = new CompaniesDataContext();

        public CompaniesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // GET: Companies
        public IActionResult Index()
        {
            return View(dataContext.Read());
        }


        // GET: Companies/AddOrEdit/5 [id for update or null for insert]
        public IActionResult AddOrEdit(int? id)
        {
            return View(dataContext.Read(id));
        }

        // POST: Companies/AddOrEdit/5 [id for update or null for insert]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("Id,Name,OrganizationalForm")] Company company)
        {
            if (ModelState.IsValid)
            {
                dataContext.AddOrEdit(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Tests/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            Company company = dataContext.Read(id);

            if (company == null) return NotFound();

            return View(company);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
			try
			{
                dataContext.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
			{
                //TODO Следует придумать обработчик ошибок
                return BadRequest("Нельзя удалить компанию, в которой числятся сотрудники");
			}
        }
    }
}
