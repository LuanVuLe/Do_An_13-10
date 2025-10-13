using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Do_An.DAL
{
    // --------------------- LỚP TÀI KHOẢN ---------------------
    public class TaiKhoanDAL
    {
        private readonly Database db = new Database();

        // Lấy toàn bộ tài khoản
        public DataTable LayTatCaTaiKhoan()
        {
            string sql = @"SELECT TenDN, MatKhau, LoaiNguoiDung, MaHV, MaGV, TrangThai 
                   FROM TaiKhoan";
            return db.Execute(sql);
        }
        // THÊM HÀM NÀY VÀO LỚP TaiKhoanDAL
        public DataTable LayThongTinTaiKhoan(string tenDN)
        {
            // Truy vấn lấy chi tiết tài khoản. 
            // Nếu HoTen nằm trong bảng khác (ví dụ: ThongTinNguoiDung), bạn cần JOIN.
            string sql = @"
    SELECT 
        TK.TenDN, 
        TK.MatKhau, 
        TK.LoaiNguoiDung, 
        TK.TrangThai,
        -- Dùng COALESCE để chọn HoTen đầu tiên không bị NULL:
        COALESCE(HV.HoTen, GV.HoTen, NV.HoTen, '') AS HoTen 
    FROM 
        TaiKhoan TK
    LEFT JOIN 
        HocVien HV ON TK.MaHV = HV.MaHV
    LEFT JOIN 
        GiaoVien GV ON TK.MaGV = GV.MaGV
    LEFT JOIN
        NhanVien NV ON TK.MaNV = NV.MaNV  -- Giả định có bảng NhanVien
    WHERE 
        TK.TenDN = @TenDN";
            var parameters = new Dictionary<string, object>
            {
                {"@TenDN", tenDN}
            };

            return db.Execute(sql, parameters);
        }

        // Thêm tài khoản
        public bool ThemTaiKhoan(string tenDN, string hoTen, string matKhau, string loaiNguoiDung)
        {
            // Giả định: Bạn đã có logic để THÊM HoTen vào bảng liên quan (nếu HoTen không nằm trong TaiKhoan)

            // Sửa SQL để chỉ thêm các trường cần thiết (không cần MaHV, MaGV)
            string sql = @"INSERT INTO TaiKhoan (TenDN, MatKhau, LoaiNguoiDung, TrangThai)
               VALUES (@TenDN, @MatKhau, @LoaiNguoiDung, 1)"; // Mặc định trạng thái là 1 (Hoạt động)
            var parameters = new Dictionary<string, object>
            {
                 {"@TenDN", tenDN},
                 {"@MatKhau", matKhau},
                 {"@LoaiNguoiDung", loaiNguoiDung},
                 // Nếu HoTen nằm trong bảng TaiKhoan, bạn cần thêm @HoTen vào đây
             };
            return db.ExecuteNonQuery(sql, parameters) > 0;
        }
        // Thêm hàm này: Cập nhật tài khoản (khớp với BLL)
        public bool CapNhatTaiKhoan(string tenDN, string hoTen, string matKhauMoi, string loaiNguoiDung)
        {
            // 1. Cập nhật LoaiNguoiDung
            string sqlUpdateTaiKhoan = @"
            UPDATE TaiKhoan 
            SET LoaiNguoiDung = @LoaiNguoiDung
            WHERE TenDN = @TenDN";

            var parametersTaiKhoan = new Dictionary<string, object>
            {
                {"@TenDN", tenDN},
                {"@LoaiNguoiDung", loaiNguoiDung},
            };

            // 2. Cập nhật Mật khẩu nếu matKhauMoi không rỗng
            if (!string.IsNullOrEmpty(matKhauMoi))
            {
                string sqlUpdateMatKhau = "UPDATE TaiKhoan SET MatKhau = @MatKhau WHERE TenDN = @TenDN";
                var parametersMatKhau = new Dictionary<string, object>
    {
        {"@TenDN", tenDN},
        {"@MatKhau", matKhauMoi}
    };
                db.ExecuteNonQuery(sqlUpdateMatKhau, parametersMatKhau);
            }

            // 3. Cập nhật Họ Tên vào bảng thông tin người dùng (GIẢ ĐỊNH)
            // Nếu HoTen nằm trong bảng Tài khoản, bạn có thể gộp vào bước 1

            // Giả sử chỉ cần 1 trong các lệnh ExecuteNonQuery thành công là OK
            return db.ExecuteNonQuery(sqlUpdateTaiKhoan, parametersTaiKhoan) > 0;
        }

        // ...

        // Xóa tài khoản
        public bool XoaTaiKhoan(string tenDN)
        {
            string sql = "DELETE FROM TaiKhoan WHERE TenDN = @TenDN";
            var parameters = new Dictionary<string, object>
    {
        {"@TenDN", tenDN}
    };
            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        // Khóa tài khoản
        public bool KhoaTaiKhoan(string tenDN)
        {
            string sql = "UPDATE TaiKhoan SET TrangThai = 0 WHERE TenDN = @TenDN";
            var parameters = new Dictionary<string, object>
    {
        {"@TenDN", tenDN}
    };
            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        // Mở khóa tài khoản
        public bool MoKhoaTaiKhoan(string tenDN)
        {
            string sql = "UPDATE TaiKhoan SET TrangThai = 1 WHERE TenDN = @TenDN";
            var parameters = new Dictionary<string, object>
    {
        {"@TenDN", tenDN}
    };
            return db.ExecuteNonQuery(sql, parameters) > 0;
        }

        // Tìm kiếm tài khoản theo tên đăng nhập hoặc loại người dùng
        public DataTable TimKiemTaiKhoan(string keyword)
        {
            string sql = @"SELECT TenDN, MatKhau, LoaiNguoiDung, MaHV, MaGV, TrangThai
                   FROM TaiKhoan
                   WHERE TenDN LIKE @Keyword OR LoaiNguoiDung LIKE @Keyword";
            var parameters = new Dictionary<string, object>
    {
        {"@Keyword", "%" + keyword + "%"}
    };
            return db.Execute(sql, parameters);
        }

        // Kiểm tra tài khoản tồn tại (phục vụ khi thêm mới)
        public bool KiemTraTonTai(string tenDN)
        {
            string sql = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDN = @TenDN";
            var parameters = new Dictionary<string, object>
    {
        {"@TenDN", tenDN}
    };
            var result = db.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result) > 0;
        }
        public bool CapNhatTrangThai(string tenDN, string trangThaiMoi)
        {
            try
            {
                string query = "UPDATE TaiKhoan SET TrangThai = @TrangThai WHERE TenDN = @TenDN";

                var parameters = new Dictionary<string, object>
    {
        { "@TrangThai", trangThaiMoi },
        { "@TenDN", tenDN }
    };

                return db.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái tài khoản: " + ex.Message);
            }
        }
    }
}


