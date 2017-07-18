using System.Text.RegularExpressions;

namespace Example
{
    public class MemberBLL
    {
        private IMemberRepository _memberRepository;

        public MemberBLL(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public Member Login(string loginName, string password, out string message)
        {
            if (!ValidateLoginNameFormat(loginName)
             || !ValidatePasswordFormat(password))
            {
                message = "LoginNameOrPasswordIncorrect";
                return null;
            }

            var member = _memberRepository.Authenticate(loginName, password, out message);
            if (member == null)
            {
                message = "LoginNameOrPasswordIncorrect";
            }
            else if (!member.IsActive)
            {
                message = "Inactive";
            }
            else
            {
                message = "Success";
            }

            return member;
        }

        public bool ChangePassword(string loginName, string oldPassword, string newPassword)
        {
            if (!ValidateLoginNameFormat(loginName)
             || !ValidatePasswordFormat(oldPassword)
             || !ValidatePasswordFormat(newPassword)
             || oldPassword.Equals(newPassword))
            {
                return false;
            }

            return _memberRepository.ChangePassword(loginName, oldPassword, newPassword);
        }

        private bool ValidateLoginNameFormat(string loginName)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]{6,20}$");
            return regex.IsMatch(loginName);
        }

        private bool ValidatePasswordFormat(string password)
        {
            var regex = new Regex(@"^.{6,20}$");
            return regex.IsMatch(password);
        }
    }
}