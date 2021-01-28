using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QulixSystemsTest1.Models
{
	public class Company
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage="Обязательное поле")]
		[Display(Name = "Название компании")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Обязательное поле")]
		[Display(Name = "Орг.-правовая форма")]
		public string OrganizationalForm { get; set; }

		[Display(Name = "Размер компании")]
		public int Size { get; set; }
	}
}
