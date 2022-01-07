namespace Banks
{
    public class ClientBuilder
    {
        private string _surname;
        private string _address;
        private string _passport;
        private string _name;

        public ClientBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public ClientBuilder SetSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public ClientBuilder SetAddress(string address)
        {
            _address = address;
            return this;
        }

        public ClientBuilder SetPassport(string passport)
        {
            _passport = passport;
            return this;
        }

        public Client GetClient() => new Client(_name, _surname, _address, _passport);
    }
}