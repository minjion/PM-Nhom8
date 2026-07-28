using QuanLyNhanVien.WindowView;
using BUS;
using DTO;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using QuanLyNhanVien.MessageBox;

namespace QuanLyNhanVien.MVVM.View.SubView
{
    /// <summary>
    /// Interaction logic for BaoHiemNhanVien.xaml
    /// </summary>
    public partial class BaoHiemNhanVienView : UserControl
    {
        public BUS_SOTHAISAN busSoThaiSan = new BUS_SOTHAISAN();
        public BUS_SOBH busSoBH = new BUS_SOBH();
        public BaoHiemNhanVienView()
        {
            InitializeComponent();
            DataGridLoad();
        }

        private void btnThemThaiSan_Click(object sender, RoutedEventArgs e)
        {
            ThemThaiSan thaiSan = new ThemThaiSan(true);
            thaiSan.ShowDialog();
            DataGridLoad();
        }

        public void DataGridLoad()
        {
            dsThaiSanDtg.ItemsSource = busSoThaiSan.getSoThaiSan().DefaultView;
            dtgBaoHiem.ItemsSource = busSoBH.getSoBH().DefaultView;
        }

        private bool TryGetSelectedRow(DataGrid grid, string warningMessage, out DataRowView row)
        {
            row = grid.SelectedItem as DataRowView;
            if (row == null)
            {
                _ = new MessageBoxCustom(warningMessage, MessageType.Warning, MessageButtons.Ok).ShowDialog();
                return false;
            }

            return true;
        }

        private void btnXoaThaiSan_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedRow(dsThaiSanDtg, "Vui lòng chọn thai sản cần xóa!", out DataRowView row))
            {
                return;
            }

            int maThaiSan = int.Parse(row[0].ToString());
            busSoThaiSan.XoaSoThaiSan(maThaiSan);
            DataGridLoad();
            _ = new MessageBoxCustom("Xóa thai sản thành công!", MessageType.Success, MessageButtons.Ok).ShowDialog();
        }

        private void btnSuaThaiSan_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedRow(dsThaiSanDtg, "Vui lòng chọn thai sản cần sửa!", out DataRowView row))
            {
                return;
            }

            DTO_SOTHAISAN suaSoThaiSan = new DTO_SOTHAISAN
            {
                Mats = int.Parse(row[0].ToString()),
                Manv = int.Parse(row[1].ToString()),
                Ngayvesom = DateTime.Parse(row[2].ToString()),
                Ngaynghisinh = DateTime.Parse(row[3].ToString()),
                Ngaylamtrolai = DateTime.Parse(row[4].ToString()),
                Trocapcty = double.Parse(row[5].ToString()),
                Ghichu = row[6].ToString()
            };

            ThemThaiSan themThaiSan = new ThemThaiSan(false)
            {
                suaThaiSan = suaSoThaiSan
            };
            themThaiSan.ShowDialog();
            DataGridLoad();
        }

        private void btnChiTiet_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedRow(dsThaiSanDtg, "Vui lòng chọn thai sản cần xem!", out DataRowView row))
            {
                return;
            }

            DTO_SOTHAISAN ctSoThaiSan = new DTO_SOTHAISAN
            {
                Mats = int.Parse(row[0].ToString()),
                Manv = int.Parse(row[1].ToString()),
                Ngayvesom = DateTime.Parse(row[2].ToString()),
                Ngaynghisinh = DateTime.Parse(row[3].ToString()),
                Ngaylamtrolai = DateTime.Parse(row[4].ToString()),
                Trocapcty = double.Parse(row[5].ToString()),
                Ghichu = row[6].ToString()
            };

            ChiTietThaiSan ctThaiSan = new ChiTietThaiSan
            {
                checkAdd = false,
                ctThaiSan = ctSoThaiSan
            };
            ctThaiSan.ShowDialog();
            DataGridLoad();
        }

        private void bthXoaBaoHiem_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedRow(dtgBaoHiem, "Vui lòng chọn bảo hiểm cần xóa!", out DataRowView row))
            {
                return;
            }

            int maBaoHiem = int.Parse(row[0].ToString());
            busSoBH.XoaSoBH(maBaoHiem);
            DataGridLoad();
            _ = new MessageBoxCustom("Xóa bảo hiểm thành công!", MessageType.Success, MessageButtons.Ok).ShowDialog();
        }

        private void btnThemBaoHiem_Click(object sender, RoutedEventArgs e)
        {
            ThemBaoHiem themBaoHiem = new ThemBaoHiem(true);

            themBaoHiem.ShowDialog();
            DataGridLoad();
        }
        private void btn_SuaBaoHiem_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedRow(dtgBaoHiem, "Vui lòng chọn bảo hiểm cần sửa!", out DataRowView row))
            {
                return;
            }

            DTO_SOBH suaSoBH = new DTO_SOBH
            {
                Mabh = int.Parse(row[0].ToString()),
                Manv = int.Parse(row[1].ToString()),
                Ngaycapso = DateTime.Parse(row[2].ToString()),
                Noicapso = row[3].ToString(),
                Ghichu = row[4].ToString()
            };

            ThemBaoHiem themBaoHiem = new ThemBaoHiem(false)
            {
                suaBaoHiem = suaSoBH
            };
            themBaoHiem.ShowDialog();
            DataGridLoad();
        }

        private void btn_XemChiTiet_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedRow(dtgBaoHiem, "Vui lòng chọn bảo hiểm cần xem!", out DataRowView row))
            {
                return;
            }

            DTO_SOBH ctSoBaoHiem = new DTO_SOBH
            {
                Mabh = int.Parse(row[0].ToString()),
                Manv = int.Parse(row[1].ToString()),
                Ngaycapso = DateTime.Parse(row[2].ToString()),
                Noicapso = row[3].ToString(),
                Ghichu = row[4].ToString()
            };

            ChiTietBaoHiem ctBaoHiem = new ChiTietBaoHiem
            {
                checkAdd = false,
                ctBaoHiem = ctSoBaoHiem
            };
            ctBaoHiem.ShowDialog();
            DataGridLoad();
        }
    }
}
