using System;
using System.Data;
// using static Do_An.DAL.Database; // Dòng này có thể bị thừa hoặc gây lỗi nếu không đúng
using Do_An.DAL;
using static Do_An.DAL.Database; // Đảm bảo gọi đúng namespace chứa TaiKhoanDAL

namespace Do_An.BLL
{
    public class TaiKhoanBLL
    {
        private readonly Database db = new Database();
        private readonly TaiKhoanDAL taiKhoanDAL = new TaiKhoanDAL();

        public DataTable LayTatCaTaiKhoan()
        {
            return taiKhoanDAL.LayTatCaTaiKhoan();
        }

        // HÀM MỚI: Lấy thông tin chi tiết
        public DataTable LayThongTinTaiKhoan(string tenDN)
        {
            if (string.IsNullOrWhiteSpace(tenDN))
                throw new ArgumentException("Tên đăng nhập không hợp lệ.");
            return taiKhoanDAL.LayThongTinTaiKhoan(tenDN);
        }

        // CẬP NHẬT: Thêm tài khoản (Bây giờ đã khớp với DAL và Form con)
        public bool ThemTaiKhoan(string tenDN, string hoTen, string matKhau, string loaiNguoiDung)
        {
            if (string.IsNullOrWhiteSpace(tenDN) || string.IsNullOrWhiteSpace(matKhau))
                throw new ArgumentException("Tên đăng nhập và mật khẩu không được để trống.");

            if (taiKhoanDAL.KiemTraTonTai(tenDN))
                throw new Exception("Tên đăng nhập này đã tồn tại.");

            // Truyền hoTen vào DAL
            return taiKhoanDAL.ThemTaiKhoan(tenDN, hoTen, matKhau, loaiNguoiDung);
        }

        // CẬP NHẬT: Sửa tài khoản (Bây giờ đã khớp với DAL và Form con)
        public bool CapNhatTaiKhoan(string tenDN, string hoTen, string matKhauMoi, string loaiNguoiDung)
        {
            if (string.IsNullOrWhiteSpace(tenDN))
                throw new ArgumentException("Tên đăng nhập không hợp lệ.");

            // Gọi hàm DAL mới
            return taiKhoanDAL.CapNhatTaiKhoan(tenDN, hoTen, matKhauMoi, loaiNguoiDung);
        }

        public bool XoaTaiKhoan(string tenDN)
        {
            if (string.IsNullOrWhiteSpace(tenDN))
                throw new ArgumentException("Tên đăng nhập không hợp lệ.");

            return taiKhoanDAL.XoaTaiKhoan(tenDN);
        }

        // ... (Giữ nguyên các hàm KhoaTaiKhoan, MoKhoaTaiKhoan, TimKiemTaiKhoan, CapNhatTrangThai)

        public bool CapNhatTrangThai(string tenDN, string trangThaiMoi)
        {
            if (string.IsNullOrWhiteSpace(tenDN))
                throw new ArgumentException("Tên đăng nhập không hợp lệ.");

            if (string.IsNullOrWhiteSpace(trangThaiMoi))
                throw new ArgumentException("Trạng thái không hợp lệ.");

            return taiKhoanDAL.CapNhatTrangThai(tenDN, trangThaiMoi);
        }

        public DataTable TimKiemTaiKhoan(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return LayTatCaTaiKhoan();
            return taiKhoanDAL.TimKiemTaiKhoan(keyword);
        }
        public int DemSoLuongNhanVien()
        {
            string sql = "SELECT COUNT(*) FROM TaiKhoan WHERE Quyen = N'Nhân viên'";
            object result = db.ExecuteScalar(sql);
            return result != null ? Convert.ToInt32(result) : 0;
        }

    }
}