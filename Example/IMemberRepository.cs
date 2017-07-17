namespace Example
{
    public interface IMemberRepository
    {
        Member Authenticate(string loginName, string password, ref string message);
    }
}