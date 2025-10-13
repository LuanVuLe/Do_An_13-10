using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Do_An.DAL;
using Microsoft.Win32;

namespace Do_An
{
    public partial class NVQL_BC : Window
    {
        private readonly Database db = new Database();
        private DataTable currentTable;
        private readonly GiaoDien_NVQL NVQL;

        public NVQL_BC() : this(null) { }

        public NVQL_BC(GiaoDien_NVQL parent)
        {
            InitializeComponent();
            NVQL = parent;
            Loaded += NVQL_BC_Loaded;
        }

        private void NVQL_BC_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCombos();
            cbReportType.SelectedIndex = 0;
        }

        private void LoadCombos()
        {
            try
            {
                // Lớp
                var dtLop = db.Execute("SELECT MaLop, TenLop FROM LopHoc ORDER BY TenLop");
                cbLop.ItemsSource = dtLop.DefaultView;
                cbLop.SelectedValuePath = "MaLop";
                cbLop.DisplayMemberPath = "TenLop";

                // Môn
                var dtMon = db.Execute("SELECT MaMH, TenMH FROM MonHoc ORDER BY TenMH");
                cbMon.ItemsSource = dtMon.DefaultView;
                cbMon.SelectedValuePath = "MaMH";
                cbMon.DisplayMemberPath = "TenMH";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu combobox:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string reportType = (cbReportType.SelectedItem as ComboBoxItem)?.Content.ToString();
            int? maLop = cbLop.SelectedValue as int? ?? (cbLop.SelectedValue != null ? Convert.ToInt32(cbLop.SelectedValue) : (int?)null);
            int? maMH = cbMon.SelectedValue as int? ?? (cbMon.SelectedValue != null ? Convert.ToInt32(cbMon.SelectedValue) : (int?)null);
            string keyword = txtKeyword.Text?.Trim();

            try
            {
                if (reportType == "Danh sách học viên theo lớp")
                {
                    string sql = @"
                        SELECT hv.MaHV, hv.HoTen, hv.SDT, hv.Email, lh.MaLop, lh.TenLop, mh.MaMH, mh.TenMH
                        FROM HocVien hv
                        LEFT JOIN DangKy dk ON hv.MaHV = dk.MaHV
                        LEFT JOIN LopHoc lh ON dk.MaLop = lh.MaLop
                        LEFT JOIN MonHoc mh ON lh.MaMH = mh.MaMH
                        WHERE 1=1";
                    var pars = new Dictionary<string, object>();
                    if (maLop.HasValue) { sql += " AND lh.MaLop = @MaLop"; pars.Add("@MaLop", maLop.Value); }
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        if (int.TryParse(keyword, out int id)) { sql += " AND hv.MaHV = @MaHV"; pars.Add("@MaHV", id); }
                        else { sql += " AND hv.HoTen LIKE @Name"; pars.Add("@Name", $"%{keyword}%"); }
                    }
                    sql += " ORDER BY lh.TenLop, hv.HoTen";
                    currentTable = db.Execute(sql, pars);
                }
                else if (reportType == "Báo cáo điểm theo lớp / môn")
                {
                    string sql = @"
                        SELECT D.MaHV, HV.HoTen, LH.MaLop, LH.TenLop, MH.MaMH, MH.TenMH,
                               D.DiemGK, D.DiemCK, D.DiemTB
                        FROM Diem D
                        INNER JOIN HocVien HV ON D.MaHV = HV.MaHV
                        INNER JOIN LopHoc LH ON D.MaLop = LH.MaLop
                        INNER JOIN MonHoc MH ON LH.MaMH = MH.MaMH
                        WHERE 1=1";
                    var pars = new Dictionary<string, object>();
                    if (maLop.HasValue) { sql += " AND LH.MaLop = @MaLop"; pars.Add("@MaLop", maLop.Value); }
                    if (maMH.HasValue) { sql += " AND MH.MaMH = @MaMH"; pars.Add("@MaMH", maMH.Value); }
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        if (int.TryParse(keyword, out int id)) { sql += " AND D.MaHV = @MaHV"; pars.Add("@MaHV", id); }
                        else { sql += " AND HV.HoTen LIKE @Name"; pars.Add("@Name", $"%{keyword}%"); }
                    }
                    sql += " ORDER BY LH.TenLop, HV.HoTen";
                    currentTable = db.Execute(sql, pars);
                }
                else // Báo cáo học phí
                {
                    string sql = @"
                        SELECT hp.MaHP, hp.MaHV, hv.HoTen, hp.MaLop, lh.TenLop,
                               hp.SoTien, hp.DaDong, hp.ConNo, hp.PhuongThuc, hp.NgayDong
                        FROM HocPhi hp
                        LEFT JOIN HocVien hv ON hp.MaHV = hv.MaHV
                        LEFT JOIN LopHoc lh ON hp.MaLop = lh.MaLop
                        WHERE 1=1";
                    var pars = new Dictionary<string, object>();
                    if (maLop.HasValue) { sql += " AND hp.MaLop = @MaLop"; pars.Add("@MaLop", maLop.Value); }
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        if (int.TryParse(keyword, out int id)) { sql += " AND hp.MaHV = @MaHV"; pars.Add("@MaHV", id); }
                        else { sql += " AND hv.HoTen LIKE @Name"; pars.Add("@Name", $"%{keyword}%"); }
                    }
                    sql += " ORDER BY hp.NgayDong DESC";
                    currentTable = db.Execute(sql, pars);
                }

                dgReport.ItemsSource = currentTable?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo báo cáo:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            if (currentTable == null || currentTable.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = "report.csv"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    ExportDataTableToCsv(currentTable, dlg.FileName);
                    MessageBox.Show("Xuất CSV thành công.", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất CSV:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportDataTableToCsv(DataTable dt, string filePath)
        {
            var sb = new StringBuilder();

            // header
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append('"').Append(dt.Columns[i].ColumnName.Replace("\"", "\"\"")).Append('"');
                if (i < dt.Columns.Count - 1) sb.Append(',');
            }
            sb.AppendLine();

            // rows
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var s = row[i]?.ToString() ?? "";
                    s = s.Replace("\"", "\"\"");
                    sb.Append('"').Append(s).Append('"');
                    if (i < dt.Columns.Count - 1) sb.Append(',');
                }
                sb.AppendLine();
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // đảm bảo parent hiện lại nếu bị ẩn
            try { NVQL?.Show(); } catch { }
        }

        private void dgReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            NVQL.Show();
        }
    }
}
