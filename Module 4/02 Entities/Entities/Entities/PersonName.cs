namespace Entities
{
    public struct PersonName
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }

        public PersonName(string name, string surname)
            : this()
        {
            Name = name;
            Surname = surname;
        }
    }
}