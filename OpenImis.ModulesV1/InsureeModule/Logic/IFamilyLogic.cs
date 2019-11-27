using OpenImis.ModulesV1.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Logic
{
    public interface IFamilyLogic
    {
        List<FamilyModel> Get(string insureeNumber);
        List<FamilyModelv2> GetMember(string insureeNumber, int order);
        DataMessage AddNew(FamilyModelv3 model);
        void SetUserId(int userId);
        DataMessage Edit(EditFamily model);
        DataMessage AddMember(FamilyMember model);
        DataMessage EditMember(EditFamilyMember model);
        DataMessage DeleteMember(string insureeNumber);
    }
}
