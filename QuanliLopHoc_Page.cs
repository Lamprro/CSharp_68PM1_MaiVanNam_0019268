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
    public partial class QuanliLopHoc_Page : UserControl
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public QuanliLopHoc_Page()
        {
            InitializeComponent();
        }

        private void labelTitleLeft_Click(object sender, EventArgs e)
        {

        }

        private void dgvLopHoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void QuanliLopHoc_Page_Load(object sender, EventArgs e)
        {
            List<lophoc> dslh = db.lophocs.ToList();
            dgvLopHoc.DataSource = dslh;
        }
    }
}
