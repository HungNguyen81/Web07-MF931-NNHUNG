using System;

namespace MISA.Amis.Core.Entities
{
    public class Unit : BaseEntity
    {

        #region Properties

        /// <summary>
        /// Id đơn vị
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string UnitName { get; set; }


        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
