using MISA.Amis.Core.Entities;
using MISA.Amis.Core.Responses;
using System;
using System.Collections.Generic;

namespace MISA.Amis.Core.Interfaces.Repositiories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Lọc dữ liệu trả về
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        FilterResponse GetByFilter(int pageSize, int pageNumber, string filterString);
       
        /// <summary>
        /// Lấy mã nv mới
        /// </summary>
        /// <returns></returns>
        string GetNewCode();

        List<EmployeeExport> Get(string proc);

    }
}
