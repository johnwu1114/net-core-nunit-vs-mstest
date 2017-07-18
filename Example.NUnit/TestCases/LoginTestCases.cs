using System;
using System.Collections;
using NUnit.Framework;

namespace Example.NUnit.TestCases
{
    internal class LoginTestCases
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "john.wu",
                    "pass.123",
                    new Member { IsActive = true }
                )
                .Returns("Success")
                .SetName("Login_Success");

                yield return new TestCaseData(
                    "john.wu",
                    "pass.123",
                    null
                ).Returns("LoginNameOrPasswordIncorrect")
                .SetName("Login_Member_Not_Found");

                yield return new TestCaseData(
                    "john.wu",
                    "pass.123",
                    new Member { IsActive = false }
                ).Returns("Inactive")
                .SetName("Login_Member_Is_Inactive");

                yield return new TestCaseData(
                    "john",
                    "pass.123",
                    new Member()
                ).Returns("LoginNameOrPasswordIncorrect")
                .SetName("Login_LoginName_Too_Short");

                yield return new TestCaseData(
                    "john.wu",
                    "pass",
                    new Member()
                ).Returns("LoginNameOrPasswordIncorrect")
                .SetName("Login_Password_Too_Short");
            }
        }
    }
}