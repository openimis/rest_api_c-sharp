using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models
{
    public class GetEnquireModel : GetInsureeModel
    {
        public List<DetailModel> Details { get; set; }

        public GetInsureeModel GetInsuree()
        {
            return new GetInsureeModel()
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
