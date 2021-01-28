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
	public abstract class DataContext
	{
		public static string connectionString;
        public abstract string TableName { get; }

        //TODO Было бы отлично сделать все шаблонные методы CRUD в этом классе в качестве простейшего фреймворка, но возникают трудности
        //TODO *? мы можем создать процедуры с описанными инструкциями в самой базе или работать через язык запросов из написанного кода
        //какой подход лучше: сохранить логику работы с базой в программе или реализовать ее в самой базе?

        /// <summary>
        /// Executes a query "SELECT * FROM {tableName}" and returns collection
        /// </summary>
        protected IEnumerable<T> SelectAllFromTable<T>(string tableName)
        {
            return SelectAllFromTable<T>(tableName, @$"SELECT * FROM {tableName}");
        }

        /// <summary>
        /// Executes a query with commandText and returns collection
        /// </summary>
        protected IEnumerable<T> SelectAllFromTable<T>(string tableName, string commandText)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(commandText, connection);
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                if (reader.HasRows)
                    return ConvertDataReaderToObjectList<T>(reader);
                else
                    return new List<T>();
            }
        }

        /// <summary>
        /// Executes a query "SELECT * FROM {tableName} WHERE Id={id}" and returns first object
        /// </summary>
        protected T SelectObjectFromTable<T>(string tableName, int? id)
        {
            if (id == null || id == 0) return (T)Activator.CreateInstance<T>();
            return SelectObjectFromTable<T>(tableName, @$"SELECT * FROM {tableName} WHERE Id={id}");
        }

        /// <summary>
        /// Executes a query with commandText and returns first object
        /// </summary>
        protected T SelectObjectFromTable<T>(string tableName, string commandText)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(commandText, connection);
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                if (reader.HasRows)
                    return ConvertDataReaderToObjectList<T>(reader)[0];
                else
                    return (T)Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// Executes a query "DELETE FROM {tableName} WHERE Id={id}"
        /// </summary>
        protected void DeleteObjectFromTable(string tableName, int id)
		{
            if (id == 0) return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(@$"DELETE FROM {tableName} WHERE Id={id}", connection);
                command.ExecuteNonQuery();
            }
        }





        //Источник: https://stackoverflow.com/questions/41040189/fastest-way-to-map-result-of-sqldatareader-to-object
        /// <summary>
        /// Converts Sql data to List of objects (class properties first)
        /// </summary>
        protected List<T> ConvertDataReaderToObjectList<T>(SqlDataReader reader)
        {
            List<T> result = new List<T>();

            while (reader.Read())
            {
                Type type = typeof(T);
                T obj = (T)Activator.CreateInstance(type);
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        var value = reader[property.Name];
                        if (value != null)
                            property.SetValue(obj, Convert.ChangeType(value.ToString(), property.PropertyType));
                    }
                    catch (Exception exc)
                    {
                        throw new Exception(exc.Message);
                    }
                }
                result.Add(obj);
            }

            return result;
        }
    }
}
