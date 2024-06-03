using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexiBee.Application.DTO.Apiary;

namespace ApexiBee.Application.DTO
{
    public class CreateApiaryData
    {
        public NewApiaryData ApiaryData { get; set; }
        public NewHubStationData HubData { get; set; }
    }
}
