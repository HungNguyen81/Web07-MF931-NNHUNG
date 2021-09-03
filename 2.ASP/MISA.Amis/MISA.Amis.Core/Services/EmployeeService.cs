using MISA.Amis.Core.Entities;
using MISA.Amis.Core.Helpers;
using MISA.Amis.Core.Interfaces.Repositiories;
using MISA.Amis.Core.Interfaces.Services;
using MISA.Amis.Core.Resource;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;

namespace MISA.Amis.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Fields

        IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public EmployeeService(IEmployeeRepository employeeRepository):base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion


        #region Các phương thức GET của riêng Employee

        /// <summary>
        /// Lấy mã nv mới
        /// </summary>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (17-08-2021)
        //@ Modified_By: HungNguyen81 (17-08-2021)
        public ServiceResult GetNewCode()
        {
            _serviceResult.Data = _employeeRepository.GetNewCode();
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        //@ Created_By: HungNguyen81 (17-08-2021)
        //@ Modified_By: HungNguyen81 (17-08-2021)
        public ServiceResult GetByFilter(int pageSize, int pageNumber, string filterString)
        {
            _serviceResult.Data = _employeeRepository.GetByFilter(pageSize, pageNumber, filterString);
            _serviceResult.IsValid = _serviceResult.Data != null;
            return _serviceResult;
        }

        public Stream GetEmployeesExcelFileStream(Stream stream)
        {
            var list = _employeeRepository.Get();
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Hungnn";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = $"Danh sách nhân viên";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("Sheet 1");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[0];

                workSheet.Cells["A1:K1"].Merge = true;
                workSheet.Cells["A2:K2"].Merge = true;
                
                // tạo header
                workSheet.Cells["A1"].LoadFromText("DANH SÁCH NHÂN VIÊN");
                workSheet.Row(1).Style.Font.Size = 16;

                // Đổ data vào Excel file
                workSheet.Cells[3, 1].LoadFromCollection(list, true, TableStyles.Light1);

                // BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        #endregion
    }
}
