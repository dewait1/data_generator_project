using Oracle.ManagedDataAccess.Client;

namespace DB.SqlConn
{
    class DBUtils
    {
        public static OracleConnection GetDBConnection()
        {
            string host = "217.173.198.135";
            int port = 1522;
            string sid = "orcltp.iaii.local";
            string user = "s96483";
            string password = "s96483";

            return DBOracleUtils.GetDBConnection(host, port, sid, user, password);
        }
    }

}