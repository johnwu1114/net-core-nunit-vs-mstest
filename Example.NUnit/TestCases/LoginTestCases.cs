using System.Collections.Generic;
using NUnit.Framework;

namespace Example.NUnit.TestCases
{
    internal class LoginTestCases
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("johnwu", "pass.123", new Member { IsActive = true })
                    .Returns("Success")
                    .SetName("Login_Success");

                yield return new TestCaseData("johnwu", "pass.123", null)
                    .Returns("LoginNameOrPasswordIncorrect")
                    .SetName("Login_Member_Not_Found");

                yield return new TestCaseData("johnwu", "pass.123", new Member { IsActive = false })
                    .Returns("Inactive")
                    .SetName("Login_Member_Is_Inactive");

                yield return new TestCaseData("john", "pass.123", new Member())
                    .Returns("LoginNameOrPasswordIncorrect")
                    .SetName("Login_LoginName_Too_Short");

                yield return new TestCaseData("johnwu", "pass", new Member())
                    .Returns("LoginNameOrPasswordIncorrect")
                    .SetName("Login_Password_Too_Short");

                yield return new TestCaseData("01234567890123456789a", "pass.123", new Member())
                    .Returns("LoginNameOrPasswordIncorrect")
                    .SetName("Login_LoginName_Too_Long");

                yield return new TestCaseData("johnwu", "01234567890123456789a", new Member())
                    .Returns("LoginNameOrPasswordIncorrect")
                    .SetName("Login_Password_Too_Long");
            }
        }
    }
}