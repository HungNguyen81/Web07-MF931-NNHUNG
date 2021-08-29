using System;

namespace MISA.Amis.Core.Entities
{
    public class Unit : BaseEntity
    {

        #region Properties

        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid UnitId { get; set; }


        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        public string UnitName { get; set; }


        /// <summary>
        /// Mô tả nhóm khách hàng
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
