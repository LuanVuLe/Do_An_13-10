using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Do_An.DAL;

namespace Do_An.BILL
{
    internal class HocVienBLL
    {
        private readonly Database db = new Database();

        /// <summary>
        /// Đăng ký học viên mới (thêm vào bảng HocVien + tạo tài khoản trong TaiKhoan)
        /// </summary>
        public int DangKyHocVien(string hoTen, DateTime? ngaySinh, string gioiTinh,
                                 string diaChi, string sdt, string email, string trinhDo)
        {
            // ✅ Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(hoTen) || ngaySinh == null || string.IsNullOrEmpty(gioiTinh))
                throw new Exception("Vui lòng nhập đầy đủ Họ tên, Ngày sinh và Giới tính!");

            // 1️⃣ Thêm học viên vào bảng HocVien và lấy MaHV
            string sqlHV = @"
                INSERT INTO HocVien (HoTen, NgaySinh, GioiTinh, DiaChi, SDT, Email, TrinhDo)
                OUTPUT INSERTED.MaHV
                VALUES (@HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SDT, @Email, @TrinhDo)";

            var parametersHV = new Dictionary<string, object>
            {
                { "@HoTen", hoTen },
                { "@NgaySinh", ngaySinh },
                { "@GioiTinh", gioiTinh },
                { "@DiaChi", string.IsNullOrEmpty(diaChi) ? DBNull.Value : diaChi },
                { "@SDT", string.IsNullOrEmpty(sdt) ? DBNull.Value : sdt },
                { "@Email", string.IsNullOrEmpty(email) ? DBNull.Value : email },
                { "@TrinhDo", string.IsNullOrEmpty(trinhDo) ? DBNull.Value : trinhDo }
            };

            DataTable result = db.Execute(sqlHV, parametersHV);
            if (result.Rows.Count == 0)
                throw new Exception("Không thể thêm học viên mới!");

            int maHV = Convert.ToInt32(result.Rows[0][0]);

            // 2️⃣ Tạo tài khoản học viên
            string tenDN = "HV" + maHV;
            string matKhau = "123456";
            string sqlTK = @"
                INSERT INTO TaiKhoan (TenDN, MatKhau, LoaiNguoiDung, MaHV)
                VALUES (@TenDN, @MatKhau, N'Học viên', @MaHV)";

            var parametersTK = new Dictionary<string, object>
            {
                { "@TenDN", tenDN },
                { "@MatKhau", matKhau },
                { "@MaHV", maHV }
            };

            db.ExecuteNonQuery(sqlTK, parametersTK);

            return maHV;
        }
        public int DemSoLuong()
        {
            string sql = "SELECT COUNT(*) FROM GiaoVien";
            object result = db.ExecuteScalar(sql);
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}

