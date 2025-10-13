using System;
using System.Data;
using Do_An.DAL;
using static Do_An.DAL.Database;

namespace Do_An.BLL
{
    public class PhanCongBLL
    {
        private readonly Database db = new Database();
        private readonly PhanCongDAL dal = new PhanCongDAL();
       


        public DataTable LayDanhSachPhanCong()
        {
            return dal.LayDanhSachPhanCong();
        }

        public DataTable LayDanhSachLop()
        {
            return dal.LayDanhSachLop();
        }

        public DataTable LayDanhSachGiaoVien()
        {
            return dal.LayDanhSachGiaoVien();
        }

        public bool ThemPhanCong(int maLop, int maGV, DateTime ngayPhanCong, string ghiChu)
        {
            return dal.ThemPhanCong(maLop, maGV, ngayPhanCong, ghiChu);
        }

        public bool SuaPhanCong(int maPC, int maLop, int maGV, DateTime ngayPhanCong, string ghiChu)
        {
            return dal.SuaPhanCong(maPC, maLop, maGV, ngayPhanCong, ghiChu);
        }

        public bool XoaPhanCong(int maPC)
        {
            return dal.XoaPhanCong(maPC);
        }
        public int DemSoLuong()
        {
            string sql = "SELECT COUNT(*) FROM GiaoVien";
            object result = db.ExecuteScalar(sql);
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}
