using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mohbeli.Models;
using System.Data.SqlClient;

namespace mohbeli.Repository
{
    interface IData
    {
        void SaveMohBeliDetails(MohBeliDetail details);

        void SaveMohBeliItems(List<Items> items, SqlConnection con, int id);

        List<MohBeliDetail> GetAllDetail();

        MohBeliDetail GetDetail(int Id);
    }
}
