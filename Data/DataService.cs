using System.Data;
using Aumerial.Data.Nti;
using Dapper;

namespace ConcertScannerAppWASM.Data
{
    public class DataService : IDisposable
    {
        private readonly NTiConnection _dbConnection;

        public DataService(IConfiguration config)
        {
            _dbConnection = new NTiConnection();
            _dbConnection.DefaultDatabase = "CONCERT";
            _dbConnection.Username = Environment.GetEnvironmentVariable("user");
            _dbConnection.Password = Environment.GetEnvironmentVariable("password");
            _dbConnection.Server = Environment.GetEnvironmentVariable("server");
        }

        public Billet GetBooking(string id)
        {
            string sql = @"SELECT * FROM Billets WHERE BilletID = ?";
            return _dbConnection.QueryFirst<Billet>(sql, new { id });
        }

        public string ScanTicket(Billet booking)
        {
            try
            {

            _dbConnection.Execute("UPDATE BILLETS SET Statut = 1, DateScan = CURRENT_TIMESTAMP WHERE BilletId = ?", new { booking.BilletID });
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public void Dispose()
        {
            if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
            {
                _dbConnection.Close();
            }
        }
    }
}
