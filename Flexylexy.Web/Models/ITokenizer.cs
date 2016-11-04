namespace Flexylexy.Web.Models
{
    public interface ITokenizer
    {
        string CreateToken(string data);

        string GetData(string token);
    }
}