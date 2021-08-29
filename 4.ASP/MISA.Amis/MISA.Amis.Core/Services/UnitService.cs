using MISA.Amis.Core.Entities;
using MISA.Amis.Core.Interfaces.Repositiories;
using MISA.Amis.Core.Interfaces.Services;
using System;

namespace MISA.Amis.Core.Services
{
    public class UnitService : BaseService<Unit>, IUnitService
    {
        public UnitService(IUnitRepository repository) : base(repository)
        {
        }
    }
}
