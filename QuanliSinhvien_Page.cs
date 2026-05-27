using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlisinhvien
{
    public partial class QuanliSinhVien_Page : UserControl
    {
        DataClasses1DataContext db  = new DataClasses1DataContext();
        public QuanliSinhVien_Page()
        {
            InitializeComponent();
        }

        private void quanLiSinhVienToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void QuanliSinhVien_Page_QueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
        {

        }

        private void QuanliSinhVien_Page_Load(object sender, EventArgs e)
        {
            var dssv = db.sinhviens.Select(sv => new {
                sv.masv,
                sv.hoten,
                sv.gioitinh,
                sv.ngaysinh,
                sv.lophoc_id
            }).ToList();
            dgvSinhVien.DataSource = dssv;

            var dsLop = db.lophocs.ToList().Select(lh => new
            {
                id = lh.id,
                Display = $"{lh.malop} - {lh.tenlop}"
            }).ToList();

            cbLop.DataSource = dsLop;
            cbLop.DisplayMember = "Display";
            cbLop.ValueMember = "id";
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã sinh viên và Họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbLop.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                String mssv = txtMaSV.Text.Trim();
                String hoTen = txtHoTen.Text.Trim();
                String gioiTinh = cbGioiTinh.Text;
                DateTime ngaySinh = dtpNgaySinh.Value;
                if (db.sinhviens.Any(s => s.masv == mssv))
                {
                    MessageBox.Show("Mã sinh viên này đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                sinhvien sinhvien = new sinhvien();
                sinhvien.masv = mssv;
                sinhvien.hoten = hoTen;
                sinhvien.gioitinh = gioiTinh;
                sinhvien.ngaysinh = ngaySinh;
                sinhvien.lophoc_id = (int)cbLop.SelectedValue;

                db.sinhviens.InsertOnSubmit(sinhvien);
                db.SubmitChanges();
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvSinhVien.DataSource = db.sinhviens.Select(sv => new {
                    sv.masv,
                    sv.hoten,
                    sv.gioitinh,
                    sv.ngaysinh,
                    sv.lophoc_id
                }).ToList();
                txtMaSV.Clear();
                txtHoTen.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
