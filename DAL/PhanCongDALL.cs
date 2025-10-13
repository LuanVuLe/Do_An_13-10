using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Do_An.DAL
{
    // --------------------- LỚP PHÂN CÔNG ---------------------
    public class PhanCongDAL
    {
        private readonly Database db = new Database();

        public DataTable LayDanhSachPhanCong()
        {
            string sql = @"SELECT pc.MaPC, lh.TenLop, gv.HoTen, pc.NgayPhanCong, pc.GhiChu
                       FROM PhanCong pc
                       JOIN LopHoc lh ON pc.MaLop = lh.MaLop
                       JOIN GiaoVien gv ON pc.MaGV = gv.MaGV";
            return db.Execute(sql);
        }

        public DataTable LayDanhSachLop()
        {
            return db.Execute("SELECT MaLop, TenLop FROM LopHoc");
        }

        public DataTable LayDanhSachGiaoVien()
        {
            return db.Execute("SELECT MaGV, HoTen FROM GiaoVien");
        }

        public bool ThemPhanCong(int maLop, int maGV, DateTime ngayPhanCong, string ghiChu)
        {
            string sql = @"INSERT INTO PhanCong(MaLop, MaGV, NgayPhanCong, GhiChu)
                       VALUES (@MaLop, @MaGV, @NgayPhanCong, @GhiChu)";

            var parameters = new Dictionary<string, object>
        {
            {"@MaLop", maLop},
            {"@MaGV", maGV},
            {"@NgayPhanCong", ngayPhanCong},
            {"@GhiChu", ghiChu ?? (object)DBNull.Value}
        };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool SuaPhanCong(int maPC, int maLop, int maGV, DateTime ngayPhanCong, string ghiChu)
        {
            string sql = @"UPDATE PhanCong
                       SET MaLop = @MaLop, MaGV = @MaGV, NgayPhanCong = @NgayPhanCong, GhiChu = @GhiChu
                       WHERE MaPC = @MaPC";

            var parameters = new Dictionary<string, object>
        {
            {"@MaPC", maPC},
            {"@MaLop", maLop},
            {"@MaGV", maGV},
            {"@NgayPhanCong", ngayPhanCong},
            {"@GhiChu", ghiChu ?? (object)DBNull.Value}
        };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool XoaPhanCong(int maPC)
        {
            string sql = "DELETE FROM PhanCong WHERE MaPC = @MaPC";

            var parameters = new Dictionary<string, object>
        {
            {"@MaPC", maPC}
        };

            return db.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
