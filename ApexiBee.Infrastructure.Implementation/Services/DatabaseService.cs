using ApexiBee.Application.Interfaces;
using ApexiBee.Infrastructure.Implementation.Helpers;
using ApexiBee.Persistance.Database;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Infrastructure.Implementation.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string serverName = ".\\SQLEXPRESS";
        private readonly string databaseName = "ApexiBee";
        private readonly string backupName = "db.bak";
        private readonly ConfigurationHelper _configurationHelper;

        public DatabaseService(ConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        public bool CreateDbBackup()
        {
            ServerConnection serverConnection = new ServerConnection(serverName);
            Server server = new Server(serverConnection);

            Backup backup = new Backup();
            backup.Action = BackupActionType.Database;
            backup.Database = databaseName;

            BackupDeviceItem backupDeviceItem = new BackupDeviceItem(backupName, DeviceType.File);
            backup.Devices.Add(backupDeviceItem);

            backup.SqlBackup(server);
            return true;
        }

        public bool RestoreDbFromLastBackup()
        {
            ServerConnection serverConnection = new ServerConnection(serverName);
            Server server = new Server(serverConnection);

            server.ConnectionContext.ExecuteNonQuery($"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

            Restore restore = new Restore();

            restore.Database = databaseName;
            restore.Action = RestoreActionType.Database;
            restore.Devices.AddDevice(backupName, DeviceType.File);

            restore.SqlRestore(server);

            server.ConnectionContext.ExecuteNonQuery($"ALTER DATABASE [{databaseName}] SET MULTI_USER");

            return true;
        }

        public DateTime? GetLastBackupDate()
        {
            string query = $"USE msdb;\r\nSELECT TOP 1 MAX(bs.backup_finish_date) FROM backupset bs WHERE bs.database_name = '{databaseName}'";
            string? connectionString = _configurationHelper.GetConnectionString("default");
            if (connectionString == null)
                return null;

            DateTime? lastBackupDate;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    lastBackupDate = command.ExecuteScalar() as DateTime?;
                }
            }
            if (lastBackupDate.HasValue)
                return lastBackupDate;
            else
                return null;
        }
    }
}
