using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IPersonRepository
    {
        public IEnumerable<Person> GetPersons();

        public Person GetPerson(int id);

        public Person AddPerson(Person person);

        public Person UpdatePerson(Person person);

        public void DeletePerson(int id);
    }
}
