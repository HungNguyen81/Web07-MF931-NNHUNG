using Microsoft.AspNetCore.Mvc;
using MISA.Amis.Core.CustomAttrs;
using MISA.Amis.Core.Entities;
using MISA.Amis.Core.Interfaces.Repositiories;
using MISA.Amis.Core.Interfaces.Services;
using MISA.Amis.Core.Resource;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Amis.API.Controllers
{
    //[Route("api/v1/employees")]
    //[ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        #region Fields

        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Constructors

        public EmployeesController(IEmployeeService employeeService, IEmployeeRepository employeeRepository):base(employeeService)
        {
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Get Requests

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>

        //@ CreateBy  : Hungnn
        //@ ModifiedBy: Hungnn

        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewEmPloyeeCode()
        {
            try
            {
                var serviceResult = _employeeService.GetNewCode();

                return Ok(serviceResult);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Lọc và phân trang theo tiêu chí tìm kiếm, theo phòng ban, vị trí
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filterString"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>

        //@ CreateBy  : Hungnn
        //@ ModifiedBy: Hungnn

        [HttpGet("employeeFilter")]
        public IActionResult GetEmployeeFilter(int pageSize, int pageNumber, string filterString)
        {
            try
            {
                var serviceResult = _employeeService.GetByFilter(pageSize, pageNumber, filterString);

                if (!serviceResult.IsValid)
                {
                    serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return StatusCode(204, serviceResult);
                }

                // Trả dữ liệu về cho client
                return StatusCode(200, serviceResult.Data);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }


        [HttpGet("employeesFile")]
        public async Task<FileStreamResult> ExportEmployees(CancellationToken cancellationToken)
        {
            // query data from database  
            await Task.Yield();
            
            //var columnNames = new List<string>();
            var propNames = new List<string>();
            var headersName = new List<string>();
            var columnWidth = new List<int>();

            var props = typeof(EmployeeExport).GetProperties();

            foreach (var prop in props)
            {
                propNames.Add(prop.Name);

                var attrs = prop.GetCustomAttributes(false);

                if(attrs.Length > 0)
                {
                    var attr = attrs[0] as MISAColumnNameVN;
                    headersName.Add(attr.Name);
                    columnWidth.Add(attr.Width);
                }
            }

            var procName = "Proc_GetExportEmployeeData";

            var list = _employeeRepository.Get(procName);
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                var countColHeader = headersName.Count;

                workSheet.Cells[1, 1].Value = ResourceVN.ExcelFileHeader;
                workSheet.Cells[1, 1, 1, countColHeader].Merge = true;
                workSheet.Cells[2, 1, 2, countColHeader].Merge = true;
                workSheet.Cells[1, 1, 1, countColHeader].Style.Font.Bold = true;
                workSheet.Cells[1, 1, 1, countColHeader].Style.Font.Size = 16;
                workSheet.Cells[1, 1, 1, countColHeader].Style.Font.Name = "Arial";
                workSheet.Cells[1, 1, 1, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                workSheet.Row(1).Height = 20;
                workSheet.Row(2).Height = 20;
                workSheet.View.FreezePanes(4, 4);

                // write các header
                for (int i = 0; i < headersName.Count; i++)
                {
                    var cell = workSheet.Cells[3, i + 1];
                    cell.Value = headersName[i];

                    // set border style
                    var cellBorder = cell.Style.Border;
                    cellBorder.Top.Style = 
                    cellBorder.Bottom.Style = 
                    cellBorder.Left.Style = 
                    cellBorder.Right.Style = ExcelBorderStyle.Thin;

                    // background color
                    var cellFill = cell.Style.Fill;
                    cellFill.PatternType = ExcelFillStyle.Solid;
                    cellFill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                    // font style
                    cell.Style.Font.Size = 10;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Name = "Arial";

                    workSheet.Column(i+1).Width = columnWidth[i];

                    // căn giữa ngày tháng
                    if (propNames[i].Equals("DateOfBirth") || propNames[i].Equals("IdentityDate"))
                    {
                        workSheet.Column(i+1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }

                for (int col = 0; col < headersName.Count; col++)
                {
                    for (int row = 0; row < list.Count; row++)
                    {
                        workSheet.Row(row + 4).Height = 15;
                        var cell = workSheet.Cells[row + 4, col + 1];
                        cell.Value = typeof(EmployeeExport).GetProperty(propNames[col]).GetValue(list[row]);

                        // set border style
                        var cellBorder = cell.Style.Border;
                        cellBorder.Top.Style =
                        cellBorder.Bottom.Style =
                        cellBorder.Left.Style =
                        cellBorder.Right.Style = ExcelBorderStyle.Thin;

                        // font style
                        cell.Style.Font.Size = 11;
                        cell.Style.Font.Bold = false;
                        cell.Style.Font.Name = "Arial";
                    }
                }

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Danh_sach_nhan_vien-{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = excelName
            };
        }
        #endregion

    }
}

