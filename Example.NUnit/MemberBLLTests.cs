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

        [SetUp]
        public void Initialize()
        {
            _memberRepository = Substitute.For<IMemberRepository>();
            _memberBLL = new MemberBLL(_memberRepository);
        }

        [Category("TestCaseAttribute")]
        [TestCase(new object[] { true, "johnwu", "pass.123", "pass.1234" }, TestName = "ChangePassword_Success")]
        [TestCase(new object[] { false, "johnwu", "pass.123", "pass.123" }, TestName = "ChangePassword_Same_Password")]
        [TestCase(new object[] { false, "johnwu", "pass", "pass.123" }, TestName = "ChangePassword_Old_Password_Too_Short")]
        [TestCase(new object[] { false, "johnwu", "pass.123", "pass" }, TestName = "ChangePassword_New_Password_Too_Short")]
        [TestCase(new object[] { false, "john.wu", "pass.123", "pass.123" }, TestName = "ChangePassword_LoginName_Incorrect_Format")]
        [TestCase(new object[] { false, "john", "pass.123", "pass.123" }, TestName = "ChangePassword_LoginName_Too_Short")]
        public void ChangePassword(bool expected, string loginName, string oldPassword, string newPassword)
        {
            // Arrange
            _memberRepository.ChangePassword(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                             .ReturnsForAnyArgs(x => { return true; });

            // Act
            var actual = _memberBLL.ChangePassword(loginName, oldPassword, newPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Category("TestCaseSource")]
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

        [Category("TestCaseSource_From_TestCaseData")]
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