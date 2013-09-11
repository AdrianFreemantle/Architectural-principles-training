namespace Entities
{
    public class PersonId : Identity
    {
        public PersonId(int id) 
            : base(id)
        {
        }

        public override string GetTag()
        {
            return "Person";
        }        
    }
}