using DTO;
using QuanLyNhanVien.WindowView;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTO;
using BUS;
using QuanLyNhanVien.MessageBox;
using System.Data;

namespace QuanLyNhanVien.MVVM.View.NhanVien_ThongTinCaNhanSubView
{
    /// <summary>
    /// Interaction logic for ThongTinCaNhanView.xaml
    /// </summary>
    public partial class ThongTinCaNhanView : UserControl
    {
        DTO_NHANVIENHIENTAI dtoNhanVienHienTai = new DTO_NHANVIENHIENTAI();
        BUS_NHANVIENHIENTAI busNhanVienHienTai = new BUS_NHANVIENHIENTAI();
        BUS_NHANVIEN busNhanVien = new BUS_NHANVIEN();
        DTO_NHANVIEN dtoNhanVien = new DTO_NHANVIEN();

        public ThongTinCaNhanView()
        {
            InitializeComponent();
            GetMaNV();            
        }

        public void GetMaNV()
        {            
            busNhanVienHienTai.getNhanVienHienTai();
            maNVTbk.Text = busNhanVienHienTai.getNhanVienHienTai().ToString();
        }

        public void GetDuLieu()
        {
            dtoNhanVien = busNhanVien.GetChiTietNhanVienTheoMa(maNVTbk.Text);
            phongTbk.Text = dtoNhanVien.Maphong;
            tenTbk.Text = dtoNhanVien.Hoten;
            ngaySinhTbk.Text = dtoNhanVien.Ngaysinh.ToString("MM/dd/yyyy");
            cccdTbk.Text = dtoNhanVien.Cmnd_cccd;
            noiCapTbk.Text = dtoNhanVien.Noicap;
            ngayKyTbk.Text = dtoNhanVien.Ngaydangki.ToString("MM/dd/yyyy");
            ngayHetHanTbk.Text = dtoNhanVien.Ngayhethan.ToString("MM/dd/yyyy");
            maLuongTbk.Text = dtoNhanVien.Maluong;
            chucVuTbk.Text = dtoNhanVien.Chucvu;
            loaiNVCbk.Text = dtoNhanVien.Maloainv;
            gioiTinhTbk.Text = dtoNhanVien.Gioitinh;
            danTocTbk.Text = dtoNhanVien.Dantoc;
            soDienThoaiTbk.Text = dtoNhanVien.Sdt;
            hocVanTbk.Text = dtoNhanVien.Hocvan;
            loaiHopDongTbk.Text = dtoNhanVien.Loaihd;
            thoiGianTbk.Text = dtoNhanVien.Thoigian.ToString();
            ghiChuTbx.Text = dtoNhanVien.Ghichu;
        }

        private void lichSuBtn_Click(object sender, RoutedEventArgs e)
        {
            XemLichSuChinhSua xemLichSuChinhSua = new XemLichSuChinhSua();
            xemLichSuChinhSua.MaNV = maNVTbk.Text;
            xemLichSuChinhSua.ShowDialog();
        }

        private void maNVTbk_Loaded(object sender, RoutedEventArgs e)
        {
            GetDuLieu();
        }

        private void chinhSuaBtn_Click(object sender, RoutedEventArgs e)
        {
            DTO_NHANVIEN suaNhanVien = new DTO_NHANVIEN();
            ThemNhanVienForm themNhanVienForm = new ThemNhanVienForm(3);

            suaNhanVien.Manv = int.Parse(maNVTbk.Text);
            suaNhanVien.Maphong = phongTbk.Text;
            suaNhanVien.Maluong = maLuongTbk.Text;
            suaNhanVien.Hoten = tenTbk.Text;
            suaNhanVien.Ngaysinh = DateTime.Parse(ngaySinhTbk.Text);
            suaNhanVien.Gioitinh = gioiTinhTbk.Text;
            suaNhanVien.Dantoc = danTocTbk.Text;
            suaNhanVien.Cmnd_cccd = cccdTbk.Text;
            suaNhanVien.Noicap = noiCapTbk.Text;
            suaNhanVien.Chucvu = chucVuTbk.Text;
            suaNhanVien.Maloainv = loaiNVCbk.Text;
            suaNhanVien.Loaihd = loaiHopDongTbk.Text;
            suaNhanVien.Thoigian = int.Parse(thoiGianTbk.Text);
            suaNhanVien.Ngaydangki = DateTime.Parse(ngayKyTbk.Text);
            suaNhanVien.Ngayhethan = DateTime.Parse(ngayHetHanTbk.Text);
            suaNhanVien.Sdt = soDienThoaiTbk.Text;
            suaNhanVien.Hocvan = hocVanTbk.Text;
            suaNhanVien.Ghichu = ghiChuTbx.Text;

            themNhanVienForm.suaNhanVien = suaNhanVien;
            themNhanVienForm.ShowDialog();
            GetDuLieu();
        }
    }
}
