using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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
            dgvSinhVien.CellClick += dgvSinhVien_CellClick;
        }

        private void quanLiSinhVienToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void QuanliSinhVien_Page_QueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
        {

        }

        private void QuanliSinhVien_Page_Load(object sender, EventArgs e)
        {
            try
            {
                var dsLop = db.lophocs.ToList().Select(lh => new
                {
                    id = lh.id,
                    Display = $"{lh.malop} - {lh.tenlop}"
                }).ToList();
                cbLop.DataSource = dsLop;
                cbLop.DisplayMember = "Display";
                cbLop.ValueMember = "id";
                cbGioiTinh.SelectedIndex = 0;

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo trang: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                string searchKeyword = txtTimKiem.Text.Trim().ToLower();
                var query = db.sinhviens.AsQueryable();

                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    query = query.Where(sv =>
                        sv.masv.ToLower().Contains(searchKeyword) ||
                        sv.hoten.ToLower().Contains(searchKeyword) ||
                        sv.lophoc.tenlop.ToLower().Contains(searchKeyword) ||
                        sv.lophoc.malop.ToLower().Contains(searchKeyword)
                    );
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi tải dữ liệu sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                txtMaSV.Text = row.Cells["masv"].Value?.ToString();
                txtHoTen.Text = row.Cells["hoten"].Value?.ToString();

                string gioiTinh = row.Cells["gioitinh"].Value?.ToString();
                if (gioiTinh == "Nam") cbGioiTinh.SelectedIndex = 0;
                else if (gioiTinh == "Nữ") cbGioiTinh.SelectedIndex = 1;
                else cbGioiTinh.SelectedIndex = -1;

                if (row.Cells["ngaysinh"].Value != null && row.Cells["ngaysinh"].Value != DBNull.Value)
                {
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["ngaysinh"].Value);
                }
                else
                {
                    dtpNgaySinh.Value = DateTime.Now;
                }

                if (row.Cells["lophoc_id"].Value != null && row.Cells["lophoc_id"].Value != DBNull.Value)
                {
                    cbLop.SelectedValue = Convert.ToInt32(row.Cells["lophoc_id"].Value);
                }
                else
                {
                    cbLop.SelectedIndex = -1;
                }

                txtMaSV.Enabled = true;
            }
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string masv = txtMaSV.Text.Trim();
                if (string.IsNullOrEmpty(masv))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập Họ tên sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbLop.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                sinhvien sv = db.sinhviens.SingleOrDefault(s => s.masv == masv);
                if (sv != null)
                {
                    sv.hoten = txtHoTen.Text.Trim();
                    sv.gioitinh = cbGioiTinh.Text;
                    sv.ngaysinh = dtpNgaySinh.Value;
                    sv.lophoc_id = (int)cbLop.SelectedValue;

                    db.SubmitChanges();
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên có mã: " + masv, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi sửa: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string masv = txtMaSV.Text.Trim();
                if (string.IsNullOrEmpty(masv))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên có mã {masv}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    sinhvien sv = db.sinhviens.SingleOrDefault(s => s.masv == masv);
                    if (sv != null)
                    {
                        db.sinhviens.DeleteOnSubmit(sv);
                        db.SubmitChanges();
                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        btnLamMoi_Click(sender, e);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên có mã: " + masv, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi xóa: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            cbGioiTinh.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Now;
            if (cbLop.Items.Count > 0) cbLop.SelectedIndex = 0;
            else cbLop.SelectedIndex = -1;

            txtMaSV.Enabled = true;
        }


        private void cbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
