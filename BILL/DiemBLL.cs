using System;
using System.Data;
using Do_An.DAL;
using static Do_An.DAL.Database;

namespace Do_An.BLL
{
    public class DiemBLL
    {
        private readonly DiemDAL dal = new DiemDAL();

        /// <summary>
        /// Lấy toàn bộ điểm (hiển thị ban đầu)
        /// </summary>
        public DataTable LayTatCaDiem()
        {
            return dal.LayTatCaDiem();
        }

        /// <summary>
        /// Lấy danh sách lớp (dùng cho combobox)
        /// </summary>
        public DataTable LayDanhSachLopHoc()
        {
            return dal.LayDanhSachLopHoc();
        }

        /// <summary>
        /// Lấy danh sách môn học (dùng cho combobox)
        /// </summary>
        public DataTable LayDanhSachMonHoc()
        {
            return dal.LayDanhSachMonHoc();
        }

        /// <summary>
        /// Lọc điểm theo điều kiện
        /// </summary>
        public DataTable TimDiem(int? maLop, int? maMH, string keyword)
        {
            return dal.LayDiemTheoLoc(maLop, maMH, keyword);
        }
    }
}
