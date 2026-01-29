using System.Data.SqlClient;
using System.Drawing;
using Octokit;

namespace ClubManageApp
{
    /// <summary>
    /// Qu?n lý connection string t?p trung cho toàn b? ?ng d?ng
    /// ?? QUAN TR?NG: CH? S?A ? ?ÂY - không s?a ? các file khác
    /// </summary>
    public static class ConnectionHelper
    {
        // ?? CH? S?A CONNECTION STRING ? ?ÂY
        // Sau khi pull code v?, ch? c?n thay Data Source và thông tin login t?i ?ây
    
        private static string _connectionString = @"Data Source=21AK22-COM;Initial Catalog=QL_CLB_LSC;Persist Security Info=True;User ID=sa;Password=912005;Encrypt=True;TrustServerCertificate=True";

        /// <summary>
        /// L?y connection string cho toàn b? ?ng d?ng
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        /// <summary>
        /// Thi?t l?p connection string m?i (runtime)
        /// </summary>
        /// <param name="connectionString">Connection string m?i</param>
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Ki?m tra connection string có h?p l? không
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
