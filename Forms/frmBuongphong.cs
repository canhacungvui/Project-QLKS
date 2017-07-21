using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_QLKS.Model;
namespace Project_QLKS.Forms
{
    public partial class frmBuongphong : Form
    {
        bool edit = true;
        TriEntities db = new TriEntities();
        public frmBuongphong()
        {
            InitializeComponent();
        }
        private void frmBuongphong_Load(object sender, EventArgs e)
        {
            var join = (from a in db.tblBuongphongs
                        join b in db.tblLoaiphongs on a.Maloai equals b.Maloai
                        select new { a.Maphong, a.Maloai, a.Tinhtrang, a.Vitri, a.Dienthoai, b.Tenloai, b.Giathuong, b.Giale });
            dgvBuongphong.DataSource = join.ToList();
            ShowRoomDetail();
        }
        public void btnMode()
        {
            if (edit)
            {
                txtMaphong.ReadOnly = true;
                btnAdd.Enabled = btnDelete.Enabled = true;
                btnCancel.Enabled = false;
                //lbValidation.Visible = false;
            }
            else
            {
                btnAdd.Enabled = btnDelete.Enabled = false;
                btnCancel.Enabled = true;
            }

        }
        public void ShowRoomDetail()
        {
            var row = dgvBuongphong.CurrentRow;
            if (row != null)
            {
                txtMaphong.Text = row.Cells["Maphong"].Value.ToString();
                txtMaloai.Text = row.Cells["Maloai"].Value.ToString();
                txtGiathuong.Text = row.Cells["Giathuong"].Value.ToString();
                txtGiale.Text = row.Cells["Giale"].Value.ToString();
                txtDienthoai.Text = row.Cells["Dienthoai"].Value.ToString();
                txtTinhtrang.Text = row.Cells["Tinhtrang"].Value.ToString();
            }
            edit = true;
            btnMode();
        }
    }
}
