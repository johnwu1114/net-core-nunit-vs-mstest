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

                cases.Add(new object[] {
                    "Success",
                    "john.wu",
                    "pass.123",
                    new Member { IsActive = true }
                });

                cases.Add(new object[] {
                    "LoginNameOrPasswordIncorrect",
                    "john.wu",
                    "pass.123",
                    null
                });

                cases.Add(new object[] {
                    "Inactive",
                    "john.wu",
                    "pass.123",
                    new Member()
                });

                cases.Add(new object[] {
                    "LoginNameOrPasswordIncorrect",
                    "john",
                    "pass.123",
                    new Member()
                });

                cases.Add(new object[] {
                    "LoginNameOrPasswordIncorrect",
                    "john.wu",
                    "pass",
                    new Member()
                });

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

        [TestCaseSource(nameof(_testCases))]
        public void Login(string expectedMessage, string loginName, string password, Member member)
        {
            // Arrange
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), ref actualMessage)
                             .ReturnsForAnyArgs(x => { return member; });

            // Act
            _memberBLL.Login(loginName, password, ref actualMessage);

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestCaseSource(typeof(LoginTestCases), nameof(LoginTestCases.TestCases))]
        public string Login(string loginName, string password, Member member)
        {
            // Arrange
            var actualMessage = string.Empty;
            _memberRepository.Authenticate(Arg.Any<string>(), Arg.Any<string>(), ref actualMessage)
                             .ReturnsForAnyArgs(x => { return member; });

            // Act
            _memberBLL.Login(loginName, password, ref actualMessage);

            // Assert
            return actualMessage;
        }
    }
}