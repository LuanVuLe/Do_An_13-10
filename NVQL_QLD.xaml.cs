using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class NVQL_QLD : Window
    {
        private readonly DiemBLL diemBLL = new DiemBLL();
        private GiaoDien_NVQL menu;
        public NVQL_QLD(GiaoDien_NVQL menu)
        {
            InitializeComponent();
            Loaded += QuanLyDiemWindow_Loaded;
            this.menu = menu;
        }

        private void QuanLyDiemWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadComboBoxes();
                LoadAllDiem();
                // Nếu textbox mã HV mặc định chứa "Nhập mã học viên", giữ nguyên; event LostFocus/GotFocus có thể xử lý placeholder
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo giao diện: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Gán event cho nút (nếu trong XAML không có Click handlers)
            btnTimKiem.Click += BtnTimKiem_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
        }

        private void LoadComboBoxes()
        {
            // Load lớp
            DataTable dtLop = diemBLL.LayDanhSachLopHoc();
            cbLopHoc.ItemsSource = dtLop.DefaultView;
            cbLopHoc.DisplayMemberPath = "TenLop";
            cbLopHoc.SelectedValuePath = "MaLop";
            cbLopHoc.SelectedIndex = -1;

            // Load môn
            DataTable dtMon = diemBLL.LayDanhSachMonHoc();
            cbMonHoc.ItemsSource = dtMon.DefaultView;
            cbMonHoc.DisplayMemberPath = "TenMH";
            cbMonHoc.SelectedValuePath = "MaMH";
            cbMonHoc.SelectedIndex = -1;
        }

        private void LoadAllDiem()
        {
            DataTable dt = diemBLL.LayTatCaDiem();
            dgDiem.ItemsSource = dt.DefaultView;
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? maLop = null;
                int? maMH = null;
                string keyword = null;

                if (cbLopHoc.SelectedValue != null && int.TryParse(cbLopHoc.SelectedValue.ToString(), out int mL))
                    maLop = mL;

                if (cbMonHoc.SelectedValue != null && int.TryParse(cbMonHoc.SelectedValue.ToString(), out int mM))
                    maMH = mM;

                // Lấy keyword: nếu TextBox chứa placeholder text "Nhập mã học viên", bỏ qua
                string raw = txtMaHV.Text?.Trim();
                if (!string.IsNullOrWhiteSpace(raw) && raw != "Nhập mã học viên")
                    keyword = raw;

                DataTable dt = diemBLL.TimDiem(maLop, maMH, keyword);
                dgDiem.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm điểm: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cbLopHoc.SelectedIndex = -1;
                cbMonHoc.SelectedIndex = -1;
                // Reset textbox placeholder
                txtMaHV.Text = "Nhập mã học viên";
                txtMaHV.Foreground = System.Windows.Media.Brushes.Gray;
                LoadAllDiem();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void txtMaHV_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtMaHV.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void btnQuayVe_Click(object sender, RoutedEventArgs e)
        {
            menu.Show();
            this.Close();
        }

    }
}
