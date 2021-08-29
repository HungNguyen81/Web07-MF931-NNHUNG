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
        /// vị trí / chức vụ
        /// </summary>

        public Guid? UnitId { get; set; }

        [MISADbColumnNotMatch]
        public string UnitName { get; set; }

        public string PositionName { get; set; }

        public string BankAccount { get; set; }

        public string BankName { get; set; }

        public string BankBranch { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsProvider { get; set; }

        #endregion
    }
}
