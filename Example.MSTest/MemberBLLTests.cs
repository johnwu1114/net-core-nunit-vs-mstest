using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Example.MSTest
{
    [TestCategory("MSTest")]
    [TestClass]
    public class MemberBLLTests
    {
        private IMemberRepository _memberRepository;
        private MemberBLL _memberBLL;

        [TestInitialize]
        public void Initialize()
        {
            _memberRepository = Substitute.For<IMemberRepository>();
            _memberRepository.ChangePassword(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                             .ReturnsForAnyArgs(x => { return true; });
            _memberBLL = new MemberBLL(_memberRepository);
        }

        #region "ChangePassword"

        [TestMethod]
        public void ChangePassword_Success()
        {
            // Arrange
            var expected = true;
            var loginName = "johnwu";
            var oldPassword = "pass.123";
            var newPassword = "pass.1234";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePassword_Same_Password()
        {
            // Arrange
            var expected = false;
            var loginName = "johnwu";
            var oldPassword = "pass.123";
            var newPassword = "pass.123";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePassword_Old_Password_Too_Short()
        {
            // Arrange
            var expected = false;
            var loginName = "johnwu";
            var oldPassword = "pass";
            var newPassword = "pass.123";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePassword_Old_Password_Too_Long()
        {
            // Arrange
            var expected = false;
            var loginName = "johnwu";
            var oldPassword = "01234567890123456789a";
            var newPassword = "pass.123";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePassword_New_Password_Too_Short()
        {
            // Arrange
            var expected = false;
            var loginName = "johnwu";
            var oldPassword = "pass.123";
            var newPassword = "pass";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePassword_New_Password_Too_Long()
        {
            // Arrange
            var expected = false;
            var loginName = "johnwu";
            var oldPassword = "pass.123";
            var newPassword = "01234567890123456789a";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePassword_LoginName_Too_Short()
        {
            // Arrange
            var expected = false;
            var loginName = "john";
            var oldPassword = "pass.123";
            var newPassword = "pass.1234";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePassword_LoginName_Too_Long()
        {
            // Arrange
            var expected = false;
            var loginName = "01234567890123456789a";
            var oldPassword = "pass.123";
            var newPassword = "pass.1234";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChangePassword_LoginName_Incorrect_Format()
        {
            // Arrange
            var expected = false;
            var loginName = "john.wu";
            var oldPassword = "pass.123";
            var newPassword = "pass.1234";

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion "ChangePassword"

        #region "Login"

        [TestMethod]
        public void Login_Success()
        {
            // Arrange
            var expectedMessage = "Success";
            var loginName = "johnwu";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return new Member { IsActive = true }; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_Member_Not_Found()
        {
            // Arrange
            var expectedMessage = "LoginNameOrPasswordIncorrect";
            var loginName = "johnwu";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return null; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_Member_Is_Inactive()
        {
            // Arrange
            var expectedMessage = "Inactive";
            var loginName = "johnwu";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return new Member { IsActive = false }; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_LoginName_Too_Short()
        {
            // Arrange
            var expectedMessage = "LoginNameOrPasswordIncorrect";
            var loginName = "john";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return null; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_Password_Too_Short()
        {
            // Arrange
            var expectedMessage = "LoginNameOrPasswordIncorrect";
            var loginName = "johnwu";
            var password = "pass";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return null; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_LoginName_Too_Long()
        {
            // Arrange
            var expectedMessage = "LoginNameOrPasswordIncorrect";
            var loginName = "01234567890123456789a";
            var password = "pass.123";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return null; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Login_Password_Too_Long()
        {
            // Arrange
            var expectedMessage = "LoginNameOrPasswordIncorrect";
            var loginName = "johnwu";
            var password = "01234567890123456789a";
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return null; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        #endregion "Login"
    }
}