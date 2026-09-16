
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CommonLayer.SecurityHelper
{
    public interface ISecurityHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);

        /// <summary>First name plus @123, for example "Adii@123".</summary>
        string GenerateDefaultPassword(string name);

        string GenerateJwtToken(int userId, string username, string role);
    }

}
