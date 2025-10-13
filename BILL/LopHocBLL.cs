using System;
using System.Data;
using Do_An.DAL;
using static Do_An.DAL.Database;

namespace Do_An.BLL
{
    public class LopHocBLL
    {
        private readonly LopHocDAL lopHocDAL = new LopHocDAL();

        public DataTable LayDanhSachLopHoc() => lopHocDAL.LayTatCaLopHoc();
        public DataTable LayDanhSachMonHoc() => lopHocDAL.LayDanhSachMonHoc();
        public DataTable LayDanhSachGiaoVien() => lopHocDAL.LayDanhSachGiaoVien();

        public string TaoLopHoc(string tenLop, string phong, string thoiGian,
                                int siSoToiDa, string trangThai, int maGV, int maMH)
        {
            if (string.IsNullOrWhiteSpace(tenLop))
                return "Tên lớp không được để trống.";
            if (maGV <= 0)
                return "Vui lòng chọn giáo viên phụ trách.";
            if (maMH <= 0)
                return "Vui lòng chọn môn học.";
            if (siSoToiDa <= 0)
                return "Sĩ số tối đa phải lớn hơn 0.";

            try
            {
                int result = lopHocDAL.ThemLopHoc(tenLop, phong, thoiGian,
                                                  siSoToiDa, trangThai, maGV, maMH);

                return result > 0 ? "Tạo lớp học thành công!" : "Không thể thêm lớp học.";
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm lớp học: " + ex.Message;
            }
        }
        public DataTable LayDanhSachLopTheoGiangVien(int maGV)
        {
            if (maGV <= 0)
                throw new ArgumentException("Mã giảng viên không hợp lệ.");

            return lopHocDAL.LayLopTheoGiangVien(maGV);
        }

        public string CapNhatTrangThai(int maLop, string trangThai)
        {
            if (maLop <= 0)
                return "Mã lớp không hợp lệ.";
            if (string.IsNullOrWhiteSpace(trangThai))
                return "Trạng thái không được để trống.";

            try
            {
                int result = lopHocDAL.CapNhatTrangThaiLop(maLop, trangThai);
                return result > 0 ? "Cập nhật trạng thái thành công!" : "Không thể cập nhật trạng thái lớp.";
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật trạng thái: " + ex.Message;
            }
        }
        public DataTable LayDanhSachLopHoc_Admin()
        {
            try
            {
                // Gọi thẳng DAL (DAL phải có phương thức này: LayThongTinLopHoc_DanhSach)
                return lopHocDAL.LayThongTinLopHoc_DanhSach();
            }
            catch (MissingMethodException)
            {
                // Nếu DAL chưa implement aggregation, fallback lấy tất cả lớp
                return lopHocDAL.LayTatCaLopHoc();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tải danh sách lớp học (Admin): " + ex.Message, ex);
            }
        }

        // Xóa lớp học (Admin)
        // Trả về số dòng bị ảnh hưởng (int) — giống kiểu trả về trong DAL
        public int XoaLopHoc_Admin(int maLop)
        {
            if (maLop <= 0)
                throw new ArgumentException("Mã lớp không hợp lệ.");

            try
            {
                // Gọi trực tiếp DAL.XoaLopHoc (DAL phải có hàm này và trả int)
                return lopHocDAL.XoaLopHoc(maLop);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa lớp học: " + ex.Message, ex);
            }
        }
    }
}
