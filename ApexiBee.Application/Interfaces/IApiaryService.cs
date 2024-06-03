using ApexiBee.Application.DTO;
using ApexiBee.Application.DTO.Apiary;
using ApexiBee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Interfaces
{
    public interface IApiaryService
    {
        // Creates Apiary + Hub Station
        Task<Apiary> CreateApiary(NewApiaryData apiaryData, NewHubStationData hubData);

        Task DeleteApiary(Guid apiaryId);

        Task<HubStation> CreateNewHubStationWithoutAdding(NewHubStationData hubData);

        Task<Hive> AddNewHive(NewHiveData hiveData);

        Task RemoveHive(Guid hiveId);

        Task CheckHive(Guid hiveId);

        Task<IEnumerable<Apiary>> GetUserApiaries(Guid userId);

        Task<IEnumerable<Hive>> GetApiaryHives(Guid apiaryId);

        Task<IEnumerable<Apiary>> GetAllApiariesWithHives();

        Task<HiveConfiguration> GetHiveConfiguration(Guid hiveId);
    }
}
