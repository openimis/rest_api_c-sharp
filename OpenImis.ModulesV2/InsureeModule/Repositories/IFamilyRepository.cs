using OpenImis.ModulesV2.InsureeModule.Models;
using OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels;
using System;

namespace OpenImis.ModulesV2.InsureeModule.Repositories
{
    public interface IFamilyRepository
    {
        FamilyModel GetByCHFID(string chfid, Guid userUUID);
        int Create(EnrollFamilyModel model, int userId, int officerId);
        int GetUserIdByUUID(Guid uuid);
        int GetOfficerIdByUserUUID(Guid userUUID);
    }
}
