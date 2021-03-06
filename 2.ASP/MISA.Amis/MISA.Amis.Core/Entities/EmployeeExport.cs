using MISA.Amis.Core.CustomAttrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Amis.Core.Entities
{
    public class EmployeeExport
    {
        #region Properties

        [MISAColumnNameVN("STT", 5)]
        public int Order { get; set; }


        [MISAColumnNameVN("Mã nhân viên", 15)]
        public string EmployeeCode { get; set; }


        [MISAColumnNameVN("Tên nhân viên", 25)]
        public string FullName { get; set; }


        [MISAColumnNameVN("Giới tính", 10)]
        public string GenderName { get; set; }


        [MISAColumnNameVN("Ngày sinh", 15)]
        public string DateOfBirth { get; set; }


        [MISAColumnNameVN("Số CMND", 20)]
        public string IdentityNumber { get; set; }


        [MISAColumnNameVN("Ngày cấp", 15)]
        public string IdentityDate { get; set; }


        [MISAColumnNameVN("Nơi cấp", 20)]
        public string IdentityPlace { get; set; }


        [MISAColumnNameVN("Mã đơn vị", 15)]
        public string UnitCode { get; set; }


        [MISAColumnNameVN("Tên đơn vị", 30)]
        public string UnitName { get; set; }


        [MISAColumnNameVN("Chức danh", 20)]
        public string PositionName { get; set; }


        [MISAColumnNameVN("Email", 30)]
        public string Email { get; set; }


        [MISAColumnNameVN("ĐT di động", 20)]
        public string MobileNumber { get; set; }


        [MISAColumnNameVN("ĐT cố định", 20)]
        public string PhoneNumber { get; set; }


        [MISAColumnNameVN("Số tài khoản", 20)]
        public string BankAccount { get; set; }


        [MISAColumnNameVN("Tên ngân hàng", 20)]
        public string BankName { get; set; }


        [MISAColumnNameVN("Chi nhánh TK ngân hàng", 25)]
        public string BankBranch { get; set; }

        #endregion
    }
}
