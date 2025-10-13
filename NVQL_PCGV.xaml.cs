using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Do_An.BLL;

namespace Do_An
{
    public partial class NVQL_PCGV : Window
    {
        private readonly PhanCongBLL bll = new PhanCongBLL();
        private GiaoDien_NVQL GiaoDien_NVQL;
        private int selectedMaPC = -1; // lưu mã PC khi chọn DataGrid

        public NVQL_PCGV(GiaoDien_NVQL _giaoDien_NVQL)
        {
            InitializeComponent();
            LoadComboBox();
            LoadDataGrid();
            GiaoDien_NVQL = _giaoDien_NVQL;
        }

        private void LoadComboBox()
        {
            // Load lớp học
            cboLopHoc.ItemsSource = bll.LayDanhSachLop().DefaultView;
            cboLopHoc.DisplayMemberPath = "TenLop";
            cboLopHoc.SelectedValuePath = "MaLop";

            // Load giáo viên
            cboGiaoVien.ItemsSource = bll.LayDanhSachGiaoVien().DefaultView;
            cboGiaoVien.DisplayMemberPath = "HoTen";
            cboGiaoVien.SelectedValuePath = "MaGV";
        }

        private void LoadDataGrid()
        {
            dgPhanCong.ItemsSource = bll.LayDanhSachPhanCong().DefaultView;
            selectedMaPC = -1;
            ClearForm();
        }

        private void ClearForm()
        {
            cboLopHoc.SelectedIndex = -1;
            cboGiaoVien.SelectedIndex = -1;
            txtGhiChu.Text = string.Empty;
            dpNgayPhanCong.SelectedDate = null;
        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            if (cboLopHoc.SelectedValue == null || cboGiaoVien.SelectedValue == null || dpNgayPhanCong.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn lớp, giáo viên và ngày phân công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool kq = bll.ThemPhanCong(
                (int)cboLopHoc.SelectedValue,
                (int)cboGiaoVien.SelectedValue,
                dpNgayPhanCong.SelectedDate.Value,
                txtGhiChu.Text
            );

            MessageBox.Show(kq ? "Thêm phân công thành công." : "Thêm thất bại.",
                "Thông báo", MessageBoxButton.OK, kq ? MessageBoxImage.Information : MessageBoxImage.Error);

            LoadDataGrid();
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMaPC == -1)
            {
                MessageBox.Show("Vui lòng chọn phân công cần sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cboLopHoc.SelectedValue == null || cboGiaoVien.SelectedValue == null || dpNgayPhanCong.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn lớp, giáo viên và ngày phân công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool kq = bll.SuaPhanCong(
                selectedMaPC,
                (int)cboLopHoc.SelectedValue,
                (int)cboGiaoVien.SelectedValue,
                dpNgayPhanCong.SelectedDate.Value,
                txtGhiChu.Text
            );

            MessageBox.Show(kq ? "Sửa phân công thành công." : "Sửa thất bại.",
                "Thông báo", MessageBoxButton.OK, kq ? MessageBoxImage.Information : MessageBoxImage.Error);

            LoadDataGrid();
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMaPC == -1)
            {
                MessageBox.Show("Vui lòng chọn phân công cần xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa phân công này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool kq = bll.XoaPhanCong(selectedMaPC);
                MessageBox.Show(kq ? "Xóa phân công thành công." : "Xóa thất bại.",
                    "Thông báo", MessageBoxButton.OK, kq ? MessageBoxImage.Information : MessageBoxImage.Error);
                LoadDataGrid();
            }
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgPhanCong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPhanCong.SelectedItem == null) return;

            DataRowView row = dgPhanCong.SelectedItem as DataRowView;
            if (row != null)
            {
                selectedMaPC = Convert.ToInt32(row["MaPC"]);
                cboLopHoc.SelectedValue = row["MaLop"];
                cboGiaoVien.SelectedValue = row["MaGV"];
                txtGhiChu.Text = row["GhiChu"].ToString();
                dpNgayPhanCong.SelectedDate = Convert.ToDateTime(row["NgayPhanCong"]);
            }
        }
    }
}
