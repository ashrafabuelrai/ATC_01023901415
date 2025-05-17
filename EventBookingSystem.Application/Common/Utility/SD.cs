using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.Utility
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";
        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
        public static string SessionToken = "JWTToken";
    }
}
