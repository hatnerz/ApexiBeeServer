using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Interfaces
{
    public interface IDatabaseService
    {
        bool CreateDbBackup();

        bool RestoreDbFromLastBackup();

        DateTime? GetLastBackupDate();
    }
}
