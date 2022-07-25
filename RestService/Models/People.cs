using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace RestService.Models
{
    [DataContract(Name = "ListOfPeople")]
    public class People
    {
        [DataMember(Name = "People")]
        public List<Person> people { get; set; }

        public People(List<Person> people)
        {
            this.people = people;
        }

        public People()
        {
        }
    }
}
