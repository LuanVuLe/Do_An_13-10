using System.Windows;
using Do_An.BLL;

namespace Do_An
{
    public partial class QuenMatKhau1 : Window
    {
        private TaiKhoan taiKhoanBLL;

        public QuenMatKhau1()
        {
            InitializeComponent();
            taiKhoanBLL = new TaiKhoan();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string tenDN = txtUsername.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(tenDN))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lấy vai trò được chọn
            string expectedRole = null;
            if (rdQuanLy.IsChecked == true) expectedRole = "Quản lý";
            else if (rdHocSinh.IsChecked == true) expectedRole = "Học viên";
            else if (rdGiaoVien.IsChecked == true) expectedRole = "Giáo viên";

            if (string.IsNullOrEmpty(expectedRole))
            {
                MessageBox.Show("Vui lòng chọn loại tài khoản!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Gọi BLL kiểm tra
            // Vì quên mật khẩu nên password chưa có => truyền rỗng hoặc null
            var result = taiKhoanBLL.KiemTraTaiKhoanTheoTenDN(tenDN, expectedRole);

            switch (result)
            {
                case LoaiNguoiDung.KhongHopLe:
                    MessageBox.Show("Tên đăng nhập không tồn tại trong hệ thống!",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                case LoaiNguoiDung.SaiVaiTro:
                    MessageBox.Show("Tên đăng nhập không khớp với vai trò đã chọn!",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                case LoaiNguoiDung.HocSinh:
                case LoaiNguoiDung.GiaoVien:
                case LoaiNguoiDung.QuanLy:
                    // Nếu đúng thì mở Step2 (nhập email)
                    QuenMatKhau2 step2 = new QuenMatKhau2(tenDN, expectedRole);
                    step2.Owner = this;
                    this.Hide();
                    step2.ShowDialog();
                    this.Show();
                    break;

                default:
                    MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại sau.",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (this.Owner != null)
            {
                this.Owner.Show();
            }
            else
            {
                GiaoDienDangNhap login = new GiaoDienDangNhap();
                login.Show();
            }
        }

        private void rdHocSinh_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
