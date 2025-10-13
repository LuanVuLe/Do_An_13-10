using System;
using System.Windows;
using Do_An.BLL;

namespace Do_An
{
    public partial class NVQL_MLH : Window
    {
        private readonly LopHocBLL lopHocBLL = new LopHocBLL();
        private readonly GiaoDien_NVQL parentWindow;

        public NVQL_MLH(GiaoDien_NVQL parent)
        {
            InitializeComponent();
            parentWindow = parent;
            LoadComboBoxData();
        }

        /// <summary>
        /// Load dữ liệu vào các ComboBox (Môn học, Giáo viên)
        /// </summary>
        private void LoadComboBoxData()
        {
            try
            {
                // --- Môn học ---
                cbMonHoc.ItemsSource = lopHocBLL.LayDanhSachMonHoc().DefaultView;
                cbMonHoc.DisplayMemberPath = "TenMH";
                cbMonHoc.SelectedValuePath = "MaMH";

                // --- Giáo viên ---
                cbGiaoVien.ItemsSource = lopHocBLL.LayDanhSachGiaoVien().DefaultView;
                cbGiaoVien.DisplayMemberPath = "HoTen";
                cbGiaoVien.SelectedValuePath = "MaGV";

                // --- Trạng thái lớp ---
                cbTrangThai.SelectedIndex = 0; // Mặc định là "Chờ khai giảng"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Xử lý khi nhấn nút Tạo lớp
        /// </summary>
        private void btnTaoLop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ giao diện
                string tenLop = txtTenLop.Text.Trim();
                string phong = txtPhong.Text.Trim();
                string thoiGian = txtThoiGian.Text.Trim();
                string trangThai = (cbTrangThai.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString() ?? "Chờ khai giảng";

                // Kiểm tra hợp lệ
                if (string.IsNullOrEmpty(tenLop) || cbMonHoc.SelectedValue == null || cbGiaoVien.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Ép kiểu
                int siSoToiDa = int.TryParse(txtSiSoToiDa.Text.Trim(), out int siSo) ? siSo : 20;
                int maGV = Convert.ToInt32(cbGiaoVien.SelectedValue);
                int maMH = Convert.ToInt32(cbMonHoc.SelectedValue);

                // Gọi BLL
                string message = lopHocBLL.TaoLopHoc(tenLop, phong, thoiGian, siSoToiDa, trangThai, maGV, maMH);

                MessageBox.Show(message, "Kết quả", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo lớp học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Làm mới form
        /// </summary>
        private void btnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            txtTenLop.Clear();
            txtPhong.Clear();
            txtThoiGian.Clear();
            txtSiSoToiDa.Clear();
            cbMonHoc.SelectedIndex = -1;
            cbGiaoVien.SelectedIndex = -1;
            cbTrangThai.SelectedIndex = 0;
        }

        /// <summary>
        /// Thoát form
        /// </summary>
        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            parentWindow.Show();
        }
    }
}
