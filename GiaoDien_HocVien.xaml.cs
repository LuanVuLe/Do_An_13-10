using System.Windows;
using Do_An.BLL;

namespace DoAn
{
    public partial class GiaoDien_HocVien : Window
    {
        private readonly LoaiNguoiDung _userRole;

        public GiaoDien_HocVien(LoaiNguoiDung userRole)
        {
            InitializeComponent();
            _userRole = userRole;
            ConfigureUIBasedOnRole();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (_userRole != LoaiNguoiDung.HocSinh)
            {
                MessageBox.Show($"Quyền truy cập bị từ chối! Vai trò '{_userRole}' phải dùng giao diện khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Stop);
                this.Close();
                return;
            }

            lblContent.Text = "Chào mừng Học viên! Đây là giao diện cá nhân của bạn.";
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            lblContent.Text = "👉 Đang hiển thị Hồ sơ cá nhân / học viên";
        }

        private void BtnStudy_Click(object sender, RoutedEventArgs e)
        {
            lblContent.Text = "👉 Đang hiển thị Lịch học/Bài tập cá nhân";
        }

        private void BtnFee_Click(object sender, RoutedEventArgs e)
        {
            lblContent.Text = "👉 Đang hiển thị Tình trạng học phí cá nhân";
        }

        private void BtnSurvey_Click(object sender, RoutedEventArgs e)
        {
            lblContent.Text = "👉 Đang hiển thị giao diện Khảo sát";
        }
    }
}
