using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarisbrookeOpenFileService.Helper
{
    public static class SqlExtensions
    {
        public static bool IsAvailable(this SqlConnection conn)
        {
            try
            {
                conn.Open();
                conn.Close();
            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }
    }
}
