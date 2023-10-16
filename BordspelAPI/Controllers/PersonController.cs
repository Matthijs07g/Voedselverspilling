using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BordspelAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }

        [HttpGet(Name = "GetAllPersons")]
        public IEnumerable<Person> GetPersons()
        {
            return _personRepository.GetPersons();
        }

        [HttpGet(Name = "GetPerson")]
        public Person GetPerson(int id)
        {
            return _personRepository.GetPerson(id);
        }

        [HttpPost(Name = "AddPerson")]
        public Person addPerson(Person person)
        {
            return _personRepository.AddPerson(person);
        }

        [HttpDelete(Name = "DeletePerson")]
        public void deletePerson(int id)
        {
            _personRepository.DeletePerson(id);
        }

        [HttpPut(Name = "updatePerson")]  //kan ook patch ipv put
        public Person putPerson(Person person)
        {
            return _personRepository.UpdatePerson(person);
        }





        private Exception NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }
}
