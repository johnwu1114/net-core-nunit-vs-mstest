using System.Collections.Generic;
using Example.NUnit.TestCases;
using NSubstitute;
using NUnit.Framework;

namespace Example.NUnit
{
    [TestFixture]
    public class MemberBLLTests
    {
        private static object[] _testCases
        {
            get
            {
                var cases = new List<object>();
                cases.Add(new object[] { "Success", "johnwu", "pass.123", new Member { IsActive = true } });
                cases.Add(new object[] { "LoginNameOrPasswordIncorrect", "johnwu", "pass.123", null });
                cases.Add(new object[] { "Inactive", "johnwu", "pass.123", new Member() });
                cases.Add(new object[] { "LoginNameOrPasswordIncorrect", "john", "pass.123", new Member() });
                cases.Add(new object[] { "LoginNameOrPasswordIncorrect", "johnwu", "pass", new Member() });
                cases.Add(new object[] { "LoginNameOrPasswordIncorrect", "01234567890123456789a", "pass.123", new Member() });
                cases.Add(new object[] { "LoginNameOrPasswordIncorrect", "johnwu", "01234567890123456789a", new Member() });
                return cases.ToArray();
            }
        }

        private IMemberRepository _memberRepository;
        private MemberBLL _memberBLL;

        [OneTimeSetUp]
        public void Initialize()
        {
            _memberRepository = Substitute.For<IMemberRepository>();
            _memberRepository.ChangePassword(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                             .ReturnsForAnyArgs(x => { return true; });
            _memberBLL = new MemberBLL(_memberRepository);
        }

        [Test]
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

        [Category("NUnit_ChangePassword_TestCaseAttribute")]
        [TestCase(new object[] { true, "johnwu", "pass.123", "pass.1234" }, TestName = "ChangePassword_Success")]
        [TestCase(new object[] { false, "johnwu", "pass.123", "pass.123" }, TestName = "ChangePassword_Same_Password")]
        [TestCase(new object[] { false, "johnwu", "pass", "pass.123" }, TestName = "ChangePassword_Old_Password_Too_Short")]
        [TestCase(new object[] { false, "johnwu", "01234567890123456789a", "pass.123" }, TestName = "ChangePassword_Old_Password_Too_Long")]
        [TestCase(new object[] { false, "johnwu", "pass.123", "pass" }, TestName = "ChangePassword_New_Password_Too_Short")]
        [TestCase(new object[] { false, "johnwu", "pass.123", "01234567890123456789a" }, TestName = "ChangePassword_New_Password_Too_Long")]
        [TestCase(new object[] { false, "john", "pass.123", "pass.1234" }, TestName = "ChangePassword_LoginName_Too_Short")]
        [TestCase(new object[] { false, "01234567890123456789a", "pass.123", "pass.1234" }, TestName = "ChangePassword_LoginName_Too_Long")]
        [TestCase(new object[] { false, "john.wu", "pass.123", "pass.1234" }, TestName = "ChangePassword_LoginName_Incorrect_Format")]
        public void ChangePassword(bool expected, string loginName, string oldPassword, string newPassword)
        {
            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Category("NUnit_Login_TestCaseSource")]
        [TestCaseSource(nameof(_testCases))]
        public void Login(string expectedMessage, string loginName, string password, Member member)
        {
            // Arrange
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return member; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Category("NUnit_Login_TestCaseSource_From_TestCaseData")]
        [TestCaseSource(typeof(LoginTestCases), nameof(LoginTestCases.TestCases))]
        public string Login(string loginName, string password, Member member)
        {
            // Arrange
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), out actualMessage)
                             .ReturnsForAnyArgs(x => { return member; });

            // Act
            _memberBLL.Login(loginName, password, out actualMessage);

            // Assert
            return actualMessage;
        }
    }
}