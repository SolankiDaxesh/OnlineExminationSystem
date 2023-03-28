using System.Data;

namespace OESRegistraionAPI.IRepository
{
    public interface IRgistrationRepository
    {
        DataTable VerifyUserLogin(string Enrollment, string password);
    }
}
