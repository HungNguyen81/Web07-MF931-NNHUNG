using Microsoft.AspNetCore.Mvc;
using MISA.Amis.Core.Entities;
using MISA.Amis.Core.Interfaces.Services;
using System;

namespace MISA.Amis.API.Controllers
{
    //[Route("api/v1/customerGroups")]
    //[ApiController]
    public class UnitsController : BaseController<Unit>
    {
        #region Constructors

        public UnitsController(IUnitService customerGroupService):base(customerGroupService)
        {
                        
        }

        #endregion

    }
}
