namespace Entities
{
    public class Person : Entity
    {
        private PersonName name;
        
        public Person(PersonId id, PersonName name)
        {
            Identity = id;
            this.name = name;
        }        
    }
}