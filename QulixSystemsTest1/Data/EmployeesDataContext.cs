using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QulixSystemsTest1.Models;

namespace QulixSystemsTest1.Data
{
    public class EmployeesDataContext : DataContext
    {
		public override string TableName => "Employees";

		public void AddOrEdit(Employee employee)
        {
            if (employee.Id == 0)
                Insert(employee);
            else
                Update(employee);
        }

        public void Insert(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    @$"
                        INSERT INTO {TableName} (LastName, FirstName, MiddleName, EmploymentDate, Position, CompanyId)
                        VALUES (N'{employee.LastName}', N'{employee.FirstName}', N'{employee.MiddleName}',
                        '{employee.EmploymentDate}', N'{employee.Position}', {employee.CompanyId})
                    ", connection);
                command.ExecuteNonQuery();
            }
        }

        public void Update(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    @$"
                            UPDATE {TableName}
                            SET LastName = N'{employee.LastName}',
                            FirstName = N'{employee.FirstName}',
                            MiddleName = N'{employee.MiddleName}',
                            EmploymentDate = '{employee.EmploymentDate}',
                            Position = N'{employee.Position}',
                            CompanyId = {employee.CompanyId}
                            WHERE Id={employee.Id}
                    ", connection);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Employee> Read()
        {
            return SelectAllFromTable<Employee>(TableName, 
                $@"SELECT e.*, c.Name as 'CompanyName' FROM {TableName} e JOIN Companies c ON e.CompanyId=c.Id");
        }

        public Employee Read(int? Id)
        {
            if (Id == null || Id == 0) return new Employee();
            return SelectObjectFromTable<Employee>(TableName, 
                $@"SELECT e.*, c.Name as 'CompanyName' FROM {TableName} e JOIN Companies c ON e.CompanyId=c.Id WHERE e.Id={Id}");
        }

        public void Delete(int Id)
		{
            DeleteObjectFromTable(TableName, Id);
		}
    }
}
