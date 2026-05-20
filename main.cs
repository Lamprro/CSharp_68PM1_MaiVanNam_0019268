using System;
using System.Windows.Forms;

namespace Quanlisinhvien
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String username = textBox1.Text;
            String password = textBox2.Text;
            if (username == "0019268@st.huce.edu.vn" && password == "0019268")
            {
                MessageBox.Show("Login success");
                QuanliHome_Page quanli = new QuanliHome_Page();
                quanli.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Login failed!");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void main_Load(object sender, EventArgs e)
        {

        }

    }
}
