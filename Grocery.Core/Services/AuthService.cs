using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;
        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }
        public Client? Login(string email, string password)
        {
            Client? client = _clientService.Get(email);
            if (client == null) return null;
            if (PasswordHelper.VerifyPassword(password, client.Password)) return client;
            return null;
        }
        public Client Register(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Velden mogen niet leeg zijn");
            }

            if (_clientService.Get(email) != null)
            {
                throw new ArgumentException("E-mail bestaat al");
            }
            
            string hashedPassword = PasswordHelper.HashPassword(password);
            int id = _clientService.GetAll().Max(c => c.Id);
            Client newClient = new(id + 1, name, email, hashedPassword);
            _clientService.Add(newClient);
            return newClient;
        }
    }
}
