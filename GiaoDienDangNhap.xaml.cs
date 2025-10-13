using System.Windows;
using Do_An.BLL;
using DoAn;


namespace Do_An
{
    public partial class GiaoDienDangNhap : Window
    {
        private TaiKhoan taiKhoanBLL;

        public GiaoDienDangNhap()
        {
            InitializeComponent();
            taiKhoanBLL = new TaiKhoan();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Xác định vai trò mong đợi từ RadioButton
            string expectedRole = rbStudent.IsChecked == true ? "Học viên"
                                 : rbTeacher.IsChecked == true ? "Giáo viên"
                                 : rbStaff.IsChecked == true ? "Nhân viên"
                                 : rbAdmin.IsChecked == true ? "Quản lý"
                                 : "";

            if (string.IsNullOrEmpty(expectedRole))
            {
                MessageBox.Show("Vui lòng chọn vai trò đăng nhập!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Gọi BLL để kiểm tra đăng nhập
            LoaiNguoiDung role = taiKhoanBLL.KiemTraDangNhap(username, password, expectedRole);

            switch (role)
            {
                case LoaiNguoiDung.HocSinh:
                    MessageBox.Show("Đăng nhập thành công với vai trò Học viên!",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    GiaoDien_HocVien hocVienForm = new GiaoDien_HocVien(role);
                    hocVienForm.Show();
                    this.Close();
                    break;

                case LoaiNguoiDung.GiaoVien:
                    MessageBox.Show("Đăng nhập thành công với vai trò Giáo viên!",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    GiaoDien_GV giaoVienForm = new GiaoDien_GV(role);
                    giaoVienForm.Show();
                    this.Close();
                    break;

                case LoaiNguoiDung.NhanVien:
                    MessageBox.Show("Đăng nhập thành công với vai trò Nhân viên!",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    GiaoDien_NVQL NhanVienForm = new GiaoDien_NVQL(role);
                    NhanVienForm.Show();
                    this.Close();
                    break;

                case LoaiNguoiDung.QuanLy:
                    MessageBox.Show("Đăng nhập thành công với vai trò Quản lý!",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    GiaoDien_Admin adminForm = new GiaoDien_Admin(role);
                    adminForm.Show();
                    this.Close();
                    break;

                case LoaiNguoiDung.SaiVaiTro:
                    MessageBox.Show($"Bạn đã chọn sai vai trò. Tài khoản này không phải {expectedRole}!",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                case LoaiNguoiDung.KhongHopLe:
                default:
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
        private void ForgotPassword_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            QuenMatKhau1 forgot = new QuenMatKhau1();
            forgot.Owner = this;
            this.Hide();
            forgot.ShowDialog();
            this.Show();
        }
    } 
}
