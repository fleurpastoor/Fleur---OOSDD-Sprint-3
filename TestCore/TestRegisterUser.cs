using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;

namespace TestCore
{
    public class TestAuthService
    {
        private Mock<IClientService> _mockClientService;
        private AuthService _authService;
        private List<Client> _clients;

        [SetUp]
        public void Setup()
        {
            _clients = new List<Client>
            {
                new Client(1, "User1", "user1@example.com", PasswordHelper.HashPassword("user1")),
                new Client(2, "User2", "user2@example.com", PasswordHelper.HashPassword("user2"))
            };

            _mockClientService = new Mock<IClientService>();
            _mockClientService.Setup(s => s.Get(It.IsAny<string>()))
                .Returns((string email) => _clients.FirstOrDefault(c => c.EmailAddress == email));
            _mockClientService.Setup(s => s.GetAll()).Returns(_clients);
            _mockClientService.Setup(s => s.Add(It.IsAny<Client>()))
                .Callback((Client c) => _clients.Add(c));
            _authService = new AuthService(_mockClientService.Object);
        }

        // Happy flow: registratie lukt
        [Test]
        public void RegisterNewUserReturnTrue()
        {
            string name = "User3";
            string email = "user3@example.com";
            string password = "user3";

            Client newClient = _authService.Register(name, email, password);

            Assert.IsNotNull(newClient);
            Assert.AreEqual(name, newClient.Name);
            Assert.AreEqual(email, newClient.EmailAddress);
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, newClient.Password));
            Assert.Contains(newClient, _clients);
        }

        // Unhappy flow: Email bestaat al
        [Test]
        public void RegisterExistingEmailCatchException()
        {
            string name = "User1";
            string email = "user1@example.com";
            string password = "user1";

            try
            {
                _authService.Register(name, email, password);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("E-mail bestaat al", ex.Message);
            }
        }
        
        // Unhappy flow: Velden zijn leeg
        [Test]
        public void RegisterEmptyFieldCatchException()
        {
            string name = "";
            string email = "user4@example.com";
            string password = "user1";

            try
            {
                _authService.Register(name, email, password);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Velden mogen niet leeg zijn", ex.Message);
            }
        }
    }
}
