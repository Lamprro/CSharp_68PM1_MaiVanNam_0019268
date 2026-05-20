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
    public partial class QuanliHome_Page : Form
    {
        public QuanliHome_Page()
        {
            InitializeComponent();
        }

        private void menuQlsignVien_Click(object sender, EventArgs e)
        {
            QuanliSinhVien_Page quanliSinhVien_Page = new QuanliSinhVien_Page();
            panel1.Controls.Clear();
            panel1.Controls.Add(quanliSinhVien_Page);
        }

        private void QuanliHome_Page_Load(object sender, EventArgs e)
        {
            QuanliSinhVien_Page quanliSinhVien_Page = new QuanliSinhVien_Page();
            panel1.Controls.Clear();
            panel1.Controls.Add(quanliSinhVien_Page);

        }


        private void menuQllopHoc_Click(object sender, EventArgs e)
        {
            QuanliLopHoc_Page page = new QuanliLopHoc_Page();
            panel1.Controls.Clear();
            panel1.Controls.Add(page);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
