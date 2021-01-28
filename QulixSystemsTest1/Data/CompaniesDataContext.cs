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
    public class CompaniesDataContext : DataContext
    {
		public override string TableName => "Companies";

		public void AddOrEdit(Company company)
		{
            if (company.Id == 0)
                Insert(company);
            else
                Update(company);
        }

        public void Insert(Company company)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    @$"
                        INSERT INTO {TableName} (Name, OrganizationalForm)
                        VALUES (N'{company.Name}', N'{company.OrganizationalForm}')
                    ", connection);
                command.ExecuteNonQuery();
            }
        }

        public void Update(Company company)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    @$"
                            UPDATE {TableName}
                            SET Name = N'{company.Name}',
                            OrganizationalForm = N'{company.OrganizationalForm}'
                            WHERE Id={company.Id}
                    ", connection);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Company> Read()
        {
            return SelectAllFromTable<Company>(TableName, 
                @$"SELECT c.*, (SELECT COUNT(e.Id) FROM Employees e WHERE c.Id = e.CompanyId) as Size FROM {TableName} c");
        }

        public Company Read(int? id)
		{
            if (id == null || id == 0) return new Company();
            return SelectObjectFromTable<Company>(TableName, 
                @$"SELECT c.*, (SELECT COUNT(e.Id) FROM Employees e WHERE c.Id = e.CompanyId) as Size FROM {TableName} c WHERE c.Id={id}");
        }

        public void Delete(int id)
        {
            DeleteObjectFromTable(TableName, id);
        }
    }
}
