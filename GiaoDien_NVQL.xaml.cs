using System;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class GiaoDien_NVQL : Window
    {
        private LoaiNguoiDung userrole;

        public GiaoDien_NVQL(LoaiNguoiDung _userrole)
        {
            InitializeComponent();
            userrole = _userrole;

            // Gắn sự kiện cho menu bên trái
            MenuList.SelectionChanged += MenuList_SelectionChanged;

            // Gắn sự kiện cho các nút trong dashboard
            GanSuKienChoButton();
        }

        // ================== GẮN SỰ KIỆN ==================
        private void GanSuKienChoButton()
        {
            foreach (var element in FindVisualChildren<Button>(this))
            {
                string content = element.Content.ToString();

                // Bỏ qua nút đăng xuất, xử lý riêng
                if (content.Contains("Đăng xuất")) continue;

                element.Click += (s, e) =>
                {
                    XuLyChucNang(content);
                };
            }
        }

        // ================== XỬ LÝ MENU ==================
        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem is ListBoxItem item)
            {
                string muc = item.Content.ToString();
                XuLyChucNang(muc);
                MenuList.SelectedIndex = -1; // reset lại chọn menu
            }
        }

        // ================== XỬ LÝ CHỨC NĂNG ==================
        private void XuLyChucNang(string muc)
        {
            if (muc.Contains("Đăng ký học viên"))
            {
                MainContent.Content = new NVQL_DKHV();;             
            }
            else if (muc.Contains("Mở lớp học"))
            {
                NVQL_MLH moLopHoc = new NVQL_MLH(this);
                this.Hide();
                moLopHoc.ShowDialog();
            }
          else if (muc.Contains("Phân công giáo viên"))
            {
                NVQL_PCGV pcGiaoVien = new NVQL_PCGV(this);
                pcGiaoVien.ShowDialog();
            }
            else if (muc.Contains("Quản lý điểm"))
            {
                NVQL_QLD qlDiem = new NVQL_QLD(this);
                this.Hide();
                qlDiem.ShowDialog();
            }
            else if (muc.Contains("Quản lý học phí"))
            {
                NVQL_HP qlHocPhi = new NVQL_HP(this);
                this.Hide();
                qlHocPhi.ShowDialog();
            }
            else if (muc.Contains("Báo cáo") || muc.Contains("Thống kê"))
            {
                NVQL_BC baoCao = new NVQL_BC(this);
                baoCao.ShowDialog();
            }
        }

        // ================== NÚT ĐĂNG XUẤT ==================
        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
                                "Xác nhận",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GiaoDienDangNhap login = new GiaoDienDangNhap();
                login.Show();
                this.Close();
            }
        }

        // ================== HÀM HỖ TRỢ TÌM KIẾM NÚT ==================
        private static System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;

                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
}
