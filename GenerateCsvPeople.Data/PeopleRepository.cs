using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCsvPeople.Data
{
    public class PeopleRepository
    {
        private readonly string _connectionString;
        public PeopleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddPplToDb(byte[] bytes)
        {
            using var context = new PeopleDataContext(_connectionString);
            context.People.AddRange(GetFromCsvBytes(bytes));
            context.SaveChanges();
        }

        public List<Person> GetPeople()
        {
            using var context = new PeopleDataContext(_connectionString);
            return context.People.ToList();
        }

        private static List<Person> GetFromCsvBytes(byte[] csvBytes)
        {
            using var memoryStream = new MemoryStream(csvBytes);
            using var reader = new StreamReader(memoryStream);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csvReader.GetRecords<Person>().ToList();
        }

        public void Delete()
        {
            using var context = new PeopleDataContext(_connectionString);
            context.Database.ExecuteSql($"DELETE FROM People");
        }
    }
}
