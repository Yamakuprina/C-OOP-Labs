using System;

namespace Banks
{
    public class Client
    {
        public Client(string name, string surname, string address, string passport)
        {
            if (name == null || surname == null)
            {
                throw new BankException("No Name and Surname found");
            }

            Id = IdGenerator.ClientIdGenerate();
            Name = name;
            Surname = surname;
            Address = address;
            Passport = passport;
        }

        public int Id { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Address { get; }
        public string Passport { get; }
        public bool Verified => !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(Passport);

        public static ClientBuilder Builder(string name, string surname) =>
            new ClientBuilder().SetName(name).SetSurname(surname);

        public static ClientBuilder BuilderPassport(string name, string surname, string passport) =>
            new ClientBuilder().SetName(name).SetSurname(surname).SetPassport(passport);

        public static ClientBuilder BuilderAddress(string name, string surname, string address) =>
            new ClientBuilder().SetName(name).SetSurname(surname).SetAddress(address);

        public void GetUpdate(string message)
        {
            Console.Write(message);
        }
    }
}