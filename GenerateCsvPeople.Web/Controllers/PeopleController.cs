using GenerateCsvPeople.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Globalization;
using Faker;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GenerateCsvPeople.Web.Models;

namespace GenerateCsvPeople.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly string _connectionString;
        public PeopleController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet("generate")]
        public IActionResult Generate(int amount)
        {
            List<Person> ppl = Enumerable.Range(1, amount).Select(_ => new Person
            {
                FirstName = Name.First(),
                LastName = Name.Last(),
                Age = RandomNumber.Next(20, 80),
                Address = GetAddress(),
                Email = Internet.Email()
            }).ToList();

            string csv = GenerateCsv(ppl);

            byte[] csvBytes = Encoding.UTF8.GetBytes(csv);
            return File(csvBytes, "text/csv", "people.csv");
        }

        [HttpPost("upload")]
        public void Upload(FileVM vm)
        {
            int indexOfComma = vm.Base64Data.IndexOf(',');
            string base64 = vm.Base64Data.Substring(indexOfComma + 1);

            var repo = new PeopleRepository(_connectionString);
            repo.AddPplToDb(Convert.FromBase64String(base64));

           
        }

        [HttpGet("getall")]
        public List<Person> GetAll()
        {
            var repo = new PeopleRepository(_connectionString);
            return repo.GetPeople();
        }

        [HttpPost("delete")]
        public void Delete()
        {
            var repo = new PeopleRepository(_connectionString);
            repo.Delete();
        }

        private static string GetAddress()
        {
            string number = Address.StreetAddress();
            string streetName = Address.StreetName();
            string streetType = Address.StreetSuffix();
            string city = Address.City();
            string state = Address.UsState();
            string zip = Address.ZipCode();

            return $"{number} {streetName} {streetType} {city}, {state} {zip}";
        }

        private static string GenerateCsv(List<Person> people)
        {
            var writer = new StringWriter();
            var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csvWriter.WriteRecords(people);

            return writer.ToString();
        }

        

    }
}
