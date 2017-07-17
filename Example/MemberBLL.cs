namespace Example
{
    public class MemberBLL
    {
        private IMemberRepository _memberRepository;

        public MemberBLL(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public Member Login(string loginName, string password, ref string message)
        {
            if (!IsValidLoginNameOrPassword(loginName, password))
            {
                message = "LoginNameOrPasswordIncorrect";
                return null;
            }

            var member = _memberRepository.Authenticate(loginName, password, ref message);
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

        private bool IsValidLoginNameOrPassword(string username, string password)
        {
            return !((string.IsNullOrEmpty(username) || username.Length < 6
                   || string.IsNullOrEmpty(password) || password.Length < 6));
        }
    }
}