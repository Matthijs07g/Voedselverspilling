using Domain.Models;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EFPersonRepository : IPersonRepository
    {
        public IEnumerable<Person> GetPersons()
        {
            throw new NotImplementedException();
        }

        public Person GetPerson(int id)
        {
            throw new NotImplementedException();
        }

        public Person AddPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public Person UpdatePerson(Person person) 
        {
            throw new NotImplementedException();
        }

        public void DeletePerson(int id)
        {
            throw new NotImplementedException();
        }
    }
}
