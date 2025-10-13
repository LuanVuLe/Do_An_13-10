using System;
using System.Windows;
using System.Windows.Controls;
using Do_An.BILL;

namespace Do_An
{
    public partial class NVQL_DKHV : UserControl
    {
        private readonly HocVienBLL hocVienBLL = new HocVienBLL();

        public NVQL_DKHV()
        {
            InitializeComponent();
        }

        // ==========================
        // NÚT ĐĂNG KÝ HỌC VIÊN
        // ==========================
        private void btnDangKy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string hoTen = txtHoTen.Text.Trim();
                DateTime? ngaySinh = dpNgaySinh.SelectedDate;
                string gioiTinh = rbNam.IsChecked == true ? "Nam" :
                                  rbNu.IsChecked == true ? "Nữ" : null;
                string diaChi = txtDiaChi.Text.Trim();
                string sdt = txtSDT.Text.Trim();
                string email = txtEmail.Text.Trim();
                string trinhDo = (cbTrinhDo.SelectedItem as ComboBoxItem)?.Content.ToString();

                int maHV = hocVienBLL.DangKyHocVien(hoTen, ngaySinh, gioiTinh, diaChi, sdt, email, trinhDo);

                MessageBox.Show(
                    $"Đăng ký học viên thành công!\n\nTài khoản: HV{maHV}\nMật khẩu: 123456",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information
                );

                LamMoiForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi đăng ký học viên:\n" + ex.Message,
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            LamMoiForm();
        }

        private void LamMoiForm()
        {
            txtHoTen.Clear();
            dpNgaySinh.SelectedDate = null;
            rbNam.IsChecked = rbNu.IsChecked = false;
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            cbTrinhDo.SelectedIndex = -1;
            cbKhoaHoc.SelectedIndex = -1;
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            // Ở dạng UserControl, không Close() được
            // => Có thể raise sự kiện để main window xử lý nếu cần
            Visibility = Visibility.Collapsed;
        }
    }
}
