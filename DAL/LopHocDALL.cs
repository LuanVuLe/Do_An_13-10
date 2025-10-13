using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Do_An.DAL
{
    // --------------------- XỬ LÝ LỚP HỌC  ---------------------
    public class LopHocDAL
    {
        private readonly Database db = new Database();

        public DataTable LayTatCaLopHoc()
        {
            string sql = "SELECT * FROM LopHoc";
            return db.Execute(sql);
        }

        public int ThemLopHoc(string tenLop, string phong, string thoiGian, int siSoToiDa,
                              string trangThai, int maGV, int maMH)
        {
            string sql = @"INSERT INTO LopHoc (TenLop, Phong, ThoiGian, SiSoToiDa, TrangThai, MaGV, MaMH)
                           VALUES (@TenLop, @Phong, @ThoiGian, @SiSoToiDa, @TrangThai, @MaGV, @MaMH)";
            var parameters = new Dictionary<string, object>
            {
                {"@TenLop", tenLop},
                {"@Phong", phong},
                {"@ThoiGian", thoiGian},
                {"@SiSoToiDa", siSoToiDa},
                {"@TrangThai", trangThai},
                {"@MaGV", maGV},
                {"@MaMH", maMH}
            };

            return db.ExecuteNonQuery(sql, parameters);
        }

        public DataTable LayDanhSachMonHoc()
        {
            string sql = "SELECT MaMH, TenMH FROM MonHoc";
            return db.Execute(sql);
        }

        public DataTable LayDanhSachGiaoVien()
        {
            string sql = "SELECT MaGV, HoTen FROM GiaoVien";
            return db.Execute(sql);
        }
        // ================== CHỨC NĂNG MỚI ==================
        // Lấy danh sách lớp theo giảng viên
        public DataTable LayLopTheoGiangVien(int maGV)
        {
            string sql = @"SELECT MaLop, TenLop, Phong, ThoiGian, SiSoToiDa, TrangThai
                           FROM LopHoc WHERE MaGV = @MaGV";
            var parameters = new Dictionary<string, object>
            {
                {"@MaGV", maGV}
            };
            return db.Execute(sql, parameters);
        }

        // Cập nhật trạng thái lớp học (ví dụ: đang học, đã kết thúc, hoãn...)
        public int CapNhatTrangThaiLop(int maLop, string trangThai)
        {
            string sql = @"UPDATE LopHoc SET TrangThai = @TrangThai WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@TrangThai", trangThai},
                {"@MaLop", maLop}
            };
            return db.ExecuteNonQuery(sql, parameters);
        }
        // Lấy danh sách lớp học kèm giáo viên & sĩ số (cho giao diện Admin_QLPH)
        public DataTable LayThongTinLopHoc_DanhSach()
        {
            string sql = @"
                SELECT l.MaLop, l.TenLop, g.HoTen AS GiaoVien, 
                       COUNT(dk.MaHV) AS SiSo
                FROM LopHoc l
                LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                LEFT JOIN DangKy dk ON l.MaLop = dk.MaLop
                GROUP BY l.MaLop, l.TenLop, g.HoTen";
            return db.Execute(sql);
        }

        // Tìm kiếm lớp học theo tên lớp hoặc tên giáo viên
        public DataTable TimKiemLopHoc(string tuKhoa)
        {
            string sql = @"
                SELECT l.MaLop, l.TenLop, g.HoTen AS GiaoVien, 
                       COUNT(dk.MaHV) AS SiSo
                FROM LopHoc l
                LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                LEFT JOIN DangKy dk ON l.MaLop = dk.MaLop
                WHERE l.TenLop LIKE @TuKhoa OR g.HoTen LIKE @TuKhoa
                GROUP BY l.MaLop, l.TenLop, g.HoTen";
            var parameters = new Dictionary<string, object>
            {
                {"@TuKhoa", "%" + tuKhoa + "%"}
            };
            return db.Execute(sql, parameters);
        }

        // Xóa lớp học
        public int XoaLopHoc(int maLop)
        {
            string sql = "DELETE FROM LopHoc WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@MaLop", maLop}
            };
            return db.ExecuteNonQuery(sql, parameters);
        }

        // Sửa thông tin lớp học
        public int SuaLopHoc(int maLop, string tenLop, int maGV)
        {
            string sql = "UPDATE LopHoc SET TenLop = @TenLop, MaGV = @MaGV WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@TenLop", tenLop},
                {"@MaGV", maGV},
                {"@MaLop", maLop}
            };
            return db.ExecuteNonQuery(sql, parameters);
        }

        // ================== BỔ SUNG CHỨC NĂNG LINH HOẠT ==================

        // Lấy thông tin chi tiết lớp học
        public DataTable LayThongTinLopHocChiTiet(string maLop)
        {
            string sql = @"
                SELECT l.*, g.HoTen AS GiaoVien, m.TenMH AS MonHoc
                FROM LopHoc l
                LEFT JOIN GiaoVien g ON l.MaGV = g.MaGV
                LEFT JOIN MonHoc m ON l.MaMH = m.MaMH
                WHERE l.MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@MaLop", maLop}
            };
            return db.Execute(sql, parameters);
        }

        // Đếm số học viên trong lớp
        public int DemSoHocVien(int maLop)
        {
            string sql = "SELECT COUNT(*) FROM HocVien WHERE MaLop = @MaLop";
            var parameters = new Dictionary<string, object>
            {
                {"@MaLop", maLop}
            };
            object result = db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result);
        }
    }
}
