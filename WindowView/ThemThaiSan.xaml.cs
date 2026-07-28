using System;
using System.Data;
using BUS;
using DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QuanLyNhanVien.MessageBox;
using System.Text.RegularExpressions;

namespace QuanLyNhanVien.WindowView
{
    /// <summary>
    /// Interaction logic for ThemThaiSan.xaml
    /// </summary>
    public partial class ThemThaiSan : Window
    {
        public BUS_NHANVIEN busNhanVien = new BUS_NHANVIEN();
        public BUS_SOTHAISAN busSoThaiSan = new BUS_SOTHAISAN();
        public BUS_THAMSO busThamSo = new BUS_THAMSO();
        public DTO_SOTHAISAN suaThaiSan;
        public DTO_NHANVIEN suaNhanVien;
        public DTO_LSCHINHSUA dtoLSChinhSua = new DTO_LSCHINHSUA();
        public bool checkAdd;
        private readonly ObservableCollection<NhanVienComboItem> _nhanVienNguon = new ObservableCollection<NhanVienComboItem>();

        private sealed class NhanVienComboItem
        {
            public NhanVienComboItem(string ma, string hoTen)
            {
                Ma = ma;
                HoTen = hoTen;
            }

            public string Ma { get; }
            public string HoTen { get; }
            public string DisplayText => string.IsNullOrWhiteSpace(HoTen) ? Ma : $"{Ma} - {HoTen}";
        }

        public ThemThaiSan(bool CheckAdd)
        {
            InitializeComponent();
            checkAdd = CheckAdd;
            ComboBoxes_Loaded();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ComboBoxes_Loaded()
        {
            _nhanVienNguon.Clear();
            var danhSachNhanVien = busNhanVien.TongHopMaNhanVienTheoGioiTinh("Nữ") ?? new List<string>();

            if (!danhSachNhanVien.Any())
            {
                danhSachNhanVien = busNhanVien.TongHopMaNhanVien() ?? new List<string>();
            }

            foreach (var maNhanVien in danhSachNhanVien.Distinct())
            {
                var tenNhanVien = busNhanVien.TimTenNVTheoMa(maNhanVien) ?? string.Empty;
                _nhanVienNguon.Add(new NhanVienComboItem(maNhanVien, tenNhanVien));
            }

            maNVCbx.ItemsSource = _nhanVienNguon;

            var coNhanVien = _nhanVienNguon.Any();
            maNVCbx.IsEnabled = coNhanVien;
            btnThemSua.IsEnabled = coNhanVien;

            if (!coNhanVien)
            {
                _ = new MessageBoxCustom("Hiện chưa có nhân viên nào để lập chế độ thai sản.", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                return;
            }

            if (checkAdd)
            {
                maNVCbx.SelectedIndex = 0;
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (maNVCbx.SelectedValue == null || !ngayNghiSinhDpk.SelectedDate.HasValue || !ngayVeSomDpk.SelectedDate.HasValue
                || !ngayLamTLDpk.SelectedDate.HasValue || string.IsNullOrWhiteSpace(troCapTbx.Text))

                {
                    bool? Result = new MessageBoxCustom("Vui lòng điền đầy đủ thông tin!", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                    return;
                }
                DTO_SOTHAISAN dtoSoThaiSan = new DTO_SOTHAISAN();
                dtoSoThaiSan.Manv = int.Parse(maNVCbx.SelectedValue.ToString());
                dtoSoThaiSan.Ngaynghisinh = ngayNghiSinhDpk.SelectedDate.Value;
                dtoSoThaiSan.Ngayvesom = ngayVeSomDpk.SelectedDate.Value;
                dtoSoThaiSan.Ngaylamtrolai = ngayLamTLDpk.SelectedDate.Value;
                dtoSoThaiSan.Trocapcty = double.Parse(troCapTbx.Text);
                dtoSoThaiSan.Ghichu = ghiChuTbx.Text;

                if (maTSTbx.Text == string.Empty)
                {
                    busSoThaiSan.ThemSoThaiSan(dtoSoThaiSan);
                    bool? Result = new MessageBoxCustom("Thêm thai sản thành công!", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                {
                    dtoSoThaiSan.Mats = int.Parse(maTSTbx.Text);
                    busSoThaiSan.SuaSoThaiSan(dtoSoThaiSan);
                    bool? Result = new MessageBoxCustom("Sửa thai sản thành công!", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                this.Close();
            }
            catch
            {
                bool? result = new MessageBoxCustom("Đã xảy ra lỗi khi lưu!\nVui lòng kiểm tra lại dữ liệu.", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }

        private void maTSTbx_Loaded(object sender, RoutedEventArgs e)
        {
            if (checkAdd)
                return;
            maTSTbx.Text = suaThaiSan.Mats.ToString();
            maNVCbx.SelectedValue = suaThaiSan.Manv.ToString();

            ngayNghiSinhDpk.SelectedDate = suaThaiSan.Ngaynghisinh;
            ngayVeSomDpk.SelectedDate = suaThaiSan.Ngayvesom;
            ngayLamTLDpk.SelectedDate = suaThaiSan.Ngaylamtrolai;
            troCapTbx.Text = suaThaiSan.Trocapcty.ToString();
            ghiChuTbx.Text = suaThaiSan.Ghichu.ToString();
        }

        private void ngayNghiSinhDpk_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            int soThangNghiTruocVaSauSinh = int.Parse(busThamSo.Get_soThangNghiSinh().ToString()) / 2;
            var maNhanVien = maNVCbx.SelectedValue?.ToString();
            if (string.IsNullOrWhiteSpace(maNhanVien))
            {
                if (checkAdd)
                {
                    bool? Result = new MessageBoxCustom("Vui lòng chọn nhân viên.", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                }
                ClearYearDpk();
                return;
            }
            DTO_NHANVIEN dtoNhanVien = busNhanVien.GetChiTietNhanVienTheoMa(maNhanVien);

            if (checkAdd)
            {
                if (!ngayNghiSinhDpk.SelectedDate.HasValue)
                {
                    return;
                }

                if (ngayNghiSinhDpk.SelectedDate.Value.Date.AddMonths(-soThangNghiTruocVaSauSinh) < dtoNhanVien.Ngaydangki)
                {
                    bool? Result = new MessageBoxCustom("Không thể nghỉ sinh khi chưa vào làm.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    ClearYearDpk();
                    return;
                }

                if (busSoThaiSan.KiemTraTonTai(maNhanVien))
                {
                    if (ngayNghiSinhDpk.SelectedDate < busSoThaiSan.TimNgayLamTroLai(maNhanVien))
                    {
                        bool? Result = new MessageBoxCustom("Nhân viên chưa kết thúc đợt nghỉ sinh trước.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        ClearYearDpk();
                        return;
                    }
                }
            }

            ngayVeSomDpk.SelectedDate = ngayNghiSinhDpk.SelectedDate.Value.Date.AddMonths(-soThangNghiTruocVaSauSinh);
            ngayLamTLDpk.SelectedDate = ngayNghiSinhDpk.SelectedDate.Value.Date.AddMonths(soThangNghiTruocVaSauSinh);

            if (dtoNhanVien != null && ngayVeSomDpk.SelectedDate.Value.Date.AddMonths(-soThangNghiTruocVaSauSinh) > dtoNhanVien.Ngayhethan)
            {
                bool? Result = new MessageBoxCustom("Không thể hưởng nghỉ sinh sau khi nghỉ việc.", MessageType.Error, MessageButtons.Ok).ShowDialog();

                return;
            }
        }

        private void troCapTbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        public void ClearYearDpk()
        {
            ngayVeSomDpk.SelectedDate = null;
            ngayNghiSinhDpk.SelectedDate = null;
            ngayLamTLDpk.SelectedDate = null;
        }
    }
}
