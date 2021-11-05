using OpenImis.ePayment.Responses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Data
{
    public class ImisBaseLocations
    {
        private IConfiguration Configuration;

        public ImisBaseLocations(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DataMessage GetOfficerVillages(string officerCode)
        {

            var sSQL = @"SELECT LV.LocationId,code,LW.locationname Ward,LV.LocationName Village,LW.LocationID WardID FROM tblOfficerVillages OV
                        INNER JOIN tblOfficer O ON OV.OfficerId = O.OfficerID AND O.ValidityTo IS NULL AND OV.ValidityTo IS NULL
                        LEFT JOIN tblLocations LV ON LV.LocationId = OV.LocationId AND LV.LocationType = 'V' AND LV.ValidityTo IS NULL
                        LEFT JOIN tblLocations LW ON LW.LocationId = LV.ParentLocationId AND LW.ValidityTo IS NULL
		                WHERE O.Code = @EnrollmentOfficerCode";

            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@EnrollmentOfficerCode", officerCode)
            };



            DataMessage message;

            try
            {
                var response = helper.GetDataTable(sSQL, sqlParameters, System.Data.CommandType.Text);
                message = new ImisApiResponse(0, false,response,0).Message;

            }
            catch (Exception e)
            {

                message = new ImisApiResponse(e).Message;
            }

            return message;

        }
    }
}
