using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BordspelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        [HttpGet(Name = "GetAllPersons")]
        public IEnumerable<Person> GetPersons()
        {
            throw NotImplementedException();
        }

        [HttpGet(Name = "GetPerson")]
        public Person GetPerson(int id)
        {
            throw NotImplementedException();
        }

        [HttpPost(Name = "AddPerson")]
        public Person addPerson()
        {
            throw NotImplementedException();
        }

        [HttpDelete(Name = "DeletePerson")]
        public Person deletePerson(int id)
        {
            throw NotImplementedException();
        }

        [HttpPut(Name = "updatePerson")]  //kan ook patch ipv put
        public Person putPerson(int id, string newEmail, string newFirstName, string newLastName, Sex newSex, DateTime newBirtdate)
        {
            throw NotImplementedException();
        }





        private Exception NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }
}
