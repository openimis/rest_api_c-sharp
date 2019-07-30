using OpenImis.ModulesV1.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Repositories
{
    public interface IFamilyRepository
    {
        List<FamilyModel> Get(string insureeNumber);
        List<FamilyModelv2> GetMember(string insureeNumber, int order);
        DataMessage AddNew(FamilyModelv3 model);
        int UserId { get; set; }
        DataMessage Edit(EditFamily model);
        DataMessage AddMember(FamilyMember model);
        DataMessage EditMember(EditFamilyMember model);
        DataMessage DeleteMember(string insureeNumber);
    }
}
