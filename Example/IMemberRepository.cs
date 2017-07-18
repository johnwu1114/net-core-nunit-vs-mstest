namespace Example
{
    public interface IMemberRepository
    {
        Member Authenticate(string loginName, string password, out string message);

        bool ChangePassword(string loginName, string oldPassword, string newPassword);
    }
}