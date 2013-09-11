namespace Entities
{
    class Program
    {
        static void Main(string[] args)
        {
            var name1 = new PersonName("Adrian", "Freemantle");
            var name2 = new PersonName("Peter", "Jones");

            var id1 = new PersonId(12345);
            var id2 = new PersonId(99999);

            var p1 = new Person(id1, name1);
            var p2 = new Person(id1, name2);
            var p3 = new Person(id2, name1);

            bool personsAreSame = false;

            personsAreSame = p1 == p2;
            personsAreSame = p1.Equals(p2);
            personsAreSame = ReferenceEquals(p1, p2);

            personsAreSame = p1 == p3;
            personsAreSame = p1.Equals(p3);
            personsAreSame = ReferenceEquals(p1, p3);
        }
    }

    public class PersonSnapshot : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }

}
