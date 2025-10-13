using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Do_An.DAL
{
    // --------------------- XỬ LÝ ĐIỂM  ---------------------
    public class DiemDAL
    {
        private readonly Database db = new Database();

        /// <summary>
        /// Lấy tất cả điểm (join với HocVien, LopHoc, MonHoc để hiển thị tên).
        /// </summary>
        public DataTable LayTatCaDiem()
        {
            string sql = @"
                SELECT D.MaHV, HV.HoTen, LH.MaLop, LH.TenLop, MH.MaMH, MH.TenMH,
                       D.DiemGK, D.DiemCK, D.DiemTB
                FROM Diem D
                INNER JOIN HocVien HV ON D.MaHV = HV.MaHV
                INNER JOIN LopHoc LH ON D.MaLop = LH.MaLop
                INNER JOIN MonHoc MH ON LH.MaMH = MH.MaMH";
            return db.Execute(sql, new Dictionary<string, object>());
        }

        /// <summary>
        /// Lấy danh sách lớp (dùng để load combobox)
        /// </summary>
        public DataTable LayDanhSachLopHoc()
        {
            string sql = "SELECT MaLop, TenLop FROM LopHoc ORDER BY TenLop";
            return db.Execute(sql, new Dictionary<string, object>());
        }

        /// <summary>
        /// Lấy danh sách môn học (dùng để load combobox)
        /// </summary>
        public DataTable LayDanhSachMonHoc()
        {
            string sql = "SELECT MaMH, TenMH FROM MonHoc ORDER BY TenMH";
            return db.Execute(sql, new Dictionary<string, object>());
        }

        /// <summary>
        /// Lấy điểm theo bộ lọc: maLop (nullable), maMH (nullable), keyword (mã HV hoặc tên HV).
        /// Nếu tham số null/empty thì sẽ không lọc theo tham số đó.
        /// </summary>
        public DataTable LayDiemTheoLoc(int? maLop, int? maMH, string keyword)
        {
            var sql = @"
                SELECT D.MaHV, HV.HoTen, LH.MaLop, LH.TenLop, MH.MaMH, MH.TenMH,
                       D.DiemGK, D.DiemCK, D.DiemTB
                FROM Diem D
                INNER JOIN HocVien HV ON D.MaHV = HV.MaHV
                INNER JOIN LopHoc LH ON D.MaLop = LH.MaLop
                INNER JOIN MonHoc MH ON LH.MaMH = MH.MaMH
                WHERE 1=1
            ";

            var parameters = new Dictionary<string, object>();

            if (maLop.HasValue)
            {
                sql += " AND LH.MaLop = @MaLop";
                parameters.Add("@MaLop", maLop.Value);
            }

            if (maMH.HasValue)
            {
                sql += " AND MH.MaMH = @MaMH";
                parameters.Add("@MaMH", maMH.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                // Nếu keyword có thể parse thành int -> tìm MaHV, đồng thời tìm theo tên (LIKE)
                if (int.TryParse(keyword.Trim(), out int maHV))
                {
                    sql += " AND (D.MaHV = @SearchMaHV OR HV.HoTen LIKE @SearchName)";
                    parameters.Add("@SearchMaHV", maHV);
                    parameters.Add("@SearchName", $"%{keyword.Trim()}%");
                }
                else
                {
                    sql += " AND HV.HoTen LIKE @SearchName";
                    parameters.Add("@SearchName", $"%{keyword.Trim()}%");
                }
            }

            sql += " ORDER BY LH.TenLop, HV.HoTen";

            return db.Execute(sql, parameters);
        }
    }
}
