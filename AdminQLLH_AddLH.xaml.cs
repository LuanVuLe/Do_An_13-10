using System;
using System.Windows;
using Do_An.BLL;
using System.Data;

namespace Do_An
{
    public partial class AdminQLLH_AddLH : Window
    {
        private readonly LopHocBLL lopHocBLL = new LopHocBLL();

        public AdminQLLH_AddLH()
        {
            InitializeComponent();
            LoadComboData();
        }

        private void LoadComboData()
        {
            // Giả sử bạn chưa có ComboBox, mà đang dùng TextBox
            // => Sau này có thể đổi sang ComboBox cho đúng
            try
            {
                DataTable dsGV = lopHocBLL.LayDanhSachGiaoVien();
                if (dsGV.Rows.Count > 0)
                {
                    // Hiển thị gợi ý tên GV trong tooltip (tạm thời)
                    string dsTenGV = string.Join(", ", dsGV.AsEnumerable().Select(r => r["HoTen"].ToString()));
                    txtGiaoVien.ToolTip = "Danh sách GV: " + dsTenGV;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách giáo viên: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            string tenLop = txtTenLop.Text.Trim();
            string giaoVien = txtGiaoVien.Text.Trim();
            string siSoText = txtSoHocVien.Text.Trim();

            if (string.IsNullOrWhiteSpace(tenLop))
            {
                MessageBox.Show("Vui lòng nhập tên lớp học.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTenLop.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(giaoVien))
            {
                MessageBox.Show("Vui lòng nhập tên giáo viên phụ trách.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtGiaoVien.Focus();
                return;
            }

            if (!int.TryParse(siSoText, out int siSo) || siSo <= 0)
            {
                MessageBox.Show("Sĩ số học viên phải là số nguyên dương.", "Sai định dạng", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSoHocVien.Focus();
                return;
            }

            // ==================
            // Giả sử bạn có hàm lấy mã GV và mã MH, ta tạm cho giá trị mặc định để test
            // ==================
            int maGV = 1; // (tạm thời, sau này bạn có thể thay bằng mã GV thật)
            int maMH = 1; // (tạm thời)
            string phong = "P101";
            string thoiGian = DateTime.Now.ToShortDateString();
            string trangThai = "Đang mở";

            string kq = lopHocBLL.TaoLopHoc(tenLop, phong, thoiGian, siSo, trangThai, maGV, maMH);

            if (kq.Contains("thành công"))
            {
                MessageBox.Show(kq, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(kq, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
