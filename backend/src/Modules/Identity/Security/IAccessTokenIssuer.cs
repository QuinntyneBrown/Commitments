namespace Identity.Security;

public interface IAccessTokenIssuer
{
    string Issue(Guid userId, string username);
}
