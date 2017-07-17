using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Example.MSTest
{
    [TestClass]
    public class MemberBLLTests
    {
        private IMemberRepository _memberRepository;
        private MemberBLL _memberBLL;

        [TestInitialize]
        public void Initialize()
        {
            _memberRepository = Substitute.For<IMemberRepository>();
            _memberBLL = new MemberBLL(_memberRepository);
        }

        [TestMethod]
        public void Login_Success()
        {
            // Arrange
            var expectedMessage = "Success";
            var loginName = "john.wu";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), ref actualMessage)
                             .ReturnsForAnyArgs(x => { return new Member { IsActive = true }; });

            // Act
            _memberBLL.Login(loginName, password, ref actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_User_Not_Found()
        {
            // Arrange
            var expectedMessage = "LoginNameOrPasswordIncorrect";
            var loginName = "john.wu";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), ref actualMessage)
                             .ReturnsForAnyArgs(x => { return null; });

            // Act
            _memberBLL.Login(loginName, password, ref actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_User_Is_Inactive()
        {
            // Arrange
            var expectedMessage = "Inactive";
            var loginName = "john.wu";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), ref actualMessage)
                             .ReturnsForAnyArgs(x => { return new Member(); });

            // Act
            _memberBLL.Login(loginName, password, ref actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_Username_Too_Short()
        {
            // Arrange
            var expectedMessage = "LoginNameOrPasswordIncorrect";
            var loginName = "john";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), ref actualMessage)
                             .ReturnsForAnyArgs(x => { return null; });

            // Act
            _memberBLL.Login(loginName, password, ref actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_Password_Too_Short()
        {
            // Arrange
            var expectedMessage = "LoginNameOrPasswordIncorrect";
            var loginName = "john.wu";
            var password = "pass";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), ref actualMessage)
                             .ReturnsForAnyArgs(x => { return null; });

            // Act
            _memberBLL.Login(loginName, password, ref actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}