using AddressProcessing.Address;
using AddressProcessing.Address.v1;
using NUnit.Framework;
using System.IO;
using System.Reflection;
namespace AddressProcessing.Tests
{
    [TestFixture]
    public class AddressFileProcessorTests
    {
        private FakeMailShotService _fakeMailShotService;
        private const string TestInputFile =  @"test_data\contacts.csv";

        [SetUp]
        public void SetUp()
        {
            _fakeMailShotService = new FakeMailShotService();
        }

        [Test]
        public void Should_send_mail_using_mailshot_service()
        {
            var processor = new AddressFileProcessor(_fakeMailShotService);
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + TestInputFile;
            processor.Process(filePath);

            Assert.That(_fakeMailShotService.Counter, Is.EqualTo(229));
        }

        internal class FakeMailShotService : IMailShot
        {
            internal int Counter { get; private set; }

            public void SendMailShot(string name, string address)
            {
                Counter++;
            }
        }
    }
}