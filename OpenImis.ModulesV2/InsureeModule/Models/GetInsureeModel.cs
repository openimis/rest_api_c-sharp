using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models
{
    public class GetInsureeModel : GetEnquireModel
    {
        public List<DetailModel> Details { get; set; }

        public GetEnquireModel GetEnquire()
        {
            return new GetEnquireModel()
            {
                CHFID = CHFID,
                DOB = DOB,
                Gender = Gender,
                InsureeName = InsureeName,
                PhotoPath = PhotoPath
            };
        }
    }
}
