namespace Flexylexy.Web.Models
{
    public interface IClient
    {
        string Name { get; set; }
        string ConnectionToken { get; set; }
    }
}