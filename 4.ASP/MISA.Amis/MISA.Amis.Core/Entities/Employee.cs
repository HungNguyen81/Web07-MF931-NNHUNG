using MISA.Amis.Core.CustomAttrs;
using System;

namespace MISA.Amis.Core.Entities
{
    public class Employee : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Id nhân viên
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string FullName { get; set; }


        /// <summary>
        /// Giới tính (int)
        /// </summary>
        public int? Gender { get; set; }

        
        /// <summary>
        /// Giới tính (string)
        /// </summary>
        [MISADbColumnNotMatch]
        public string GenderName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        
        public string MobileNumber { get; set; }

        /// <summary>
        /// Số DT cố định
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Số cmnd/căn cước
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string IdentityPlace { get; set; }

        /// <summary>
        /// Id đơn vị
        /// </summary>
        public Guid? UnitId { get; set; }

        /// <summary>
        /// Tên đên vị
        /// </summary>
        [MISADbColumnNotMatch]
        public string UnitName { get; set; }

        /// <summary>
        /// vị trí / chức vụ
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string BankBranch { get; set; }

        /// <summary>
        /// Là khách hàng
        /// </summary>
        public bool IsCustomer { get; set; }

        /// <summary>
        /// Là nhà cung cấp
        /// </summary>
        public bool IsProvider { get; set; }

        #endregion
    }
}
