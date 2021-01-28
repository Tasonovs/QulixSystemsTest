using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QulixSystemsTest1.Models
{
	public class Employee
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage="Обязательное поле")]
		[Display(Name = "Фамилия")]
		public string LastName { get; set; }

		[Required(ErrorMessage="Обязательное поле")]
		[Display(Name = "Имя")]
		public string FirstName { get; set; }

		[Required(ErrorMessage="Обязательное поле")]
		[Display(Name = "Отчество")]
		public string MiddleName { get; set; }

		[DataType(DataType.Date)]
		[Required(ErrorMessage="Обязательное поле")]
		[Display(Name = "Дата найма")]
		public DateTime EmploymentDate { get; set; }

		[Required(ErrorMessage="Обязательное поле")]
		[Display(Name = "Должность")]
		public string Position { get; set; }

		[Required(ErrorMessage="Обязательное поле")]
		public int CompanyId { get; set; }

		[Display(Name = "Компания")]
		public string CompanyName { get; set; }
	}
}
