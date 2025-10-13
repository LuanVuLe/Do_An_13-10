using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class GiaoDien_Admin : Window
    {
        private LoaiNguoiDung userRole;
        public GiaoDien_Admin(LoaiNguoiDung _userRole)
        {
            InitializeComponent();
            MenuList.SelectionChanged += MenuList_SelectionChanged;
            MenuList.SelectedIndex = 0; // Mặc định mở Trang chủ
            userRole = _userRole;
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem is ListBoxItem item)
            {
                string selected = item.Content.ToString();
                ContentPanel.Children.Clear();

                if (selected.Contains("Trang chủ"))
                {
                    txtTitle.Text = "Trang Chủ";
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "Chào mừng bạn đến với giao diện quản lý hệ thống!",
                        FontSize = 18,
                        Margin = new Thickness(20)
                    });
                }
                else if (selected.Contains("Quản lý tài khoản"))
                {
                    txtTitle.Text = "Quản lý tài khoản";
                    ContentPanel.Children.Add(new Admin_QLTK());
                }
                else if (selected.Contains("Quản lý lớp học"))
                  {
                      txtTitle.Text = "Quản lý lớp học";
                      ContentPanel.Children.Add(new Admin_QLPH());
                  }
                  else if (selected.Contains("Quản lý phân quyền"))
                  {
                      txtTitle.Text = "Quản lý phân quyền";
                      ContentPanel.Children.Add(new Admin_QLPQ());
                  }
                  else if (selected.Contains("Bảo mật hệ thống"))
                  {
                      txtTitle.Text = "Bảo mật hệ thống";
                      ContentPanel.Children.Add(new TextBlock
                      {
                          Text = "Trang bảo mật hệ thống đang được xây dựng.",
                          FontSize = 16,
                          Margin = new Thickness(20)
                      });
                  }
                else if (selected.Contains("Xem báo cáo"))
                {
                    txtTitle.Text = "Xem báo cáo";
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "Trang xem báo cáo đang được phát triển.",
                        FontSize = 16,
                        Margin = new Thickness(20)
                    });
                }
            }
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            GiaoDienDangNhap form = new GiaoDienDangNhap();
            this.Close();
            form.Show();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
