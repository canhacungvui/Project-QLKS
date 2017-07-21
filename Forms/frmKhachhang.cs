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
    public partial class frmKhachhang : Form
    {
        bool edit = true;
        TriEntities db = new TriEntities();
        public frmKhachhang()
        {
            InitializeComponent();
        }

        private void frmKhachhang_Load(object sender, EventArgs e)
        {
            dgvKhachhang.DataSource = db.tblKhachhangs.ToList();
            ShowCustomerDetail();
        }
        public void ShowCustomerDetail()
        {
            var row = dgvKhachhang.CurrentRow;
            if (row != null)
            {
                txtTenKH.Text = row.Cells[0].Value.ToString();
                txtCMND.Text = row.Cells[1].Value.ToString();
                txtDiaChi.Text = row.Cells[2].Value.ToString();
                if (row.Cells[3].Value.ToString() == "Nam")
                {
                    rbNam.Checked = true;
                }
                else
                {
                    rbNu.Checked = true;
                }
                txtSDT.Text = row.Cells[4].Value.ToString();
                txtQuocTich.Text = row.Cells[5].Value.ToString();
            }
            edit = true;
            btnMode();
        }
        public void btnMode()
        {
            if (edit)
            {
                txtCMND.ReadOnly = true;
                btnAdd.Enabled = btnDelete.Enabled = true;
                btnCancel.Enabled = false;
                lbValidation.Visible = false;
                btnFindReturn.Visible = false;
            }
            else
            {
                btnAdd.Enabled = btnDelete.Enabled = false;
                btnCancel.Enabled = true;
            }

        }
        private void dgvKhachhang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowCustomerDetail();
        }
        private void txtCMND_Leave(object sender, EventArgs e)
        {
            if (edit == false)
            {
                lbValidation.Visible = true;
                var validateCus = db.tblKhachhangs.Find(txtCMND.Text);
                if (txtCMND.Text.Trim().Length == 0)
                {
                    lbValidation.Text = "Bạn chưa nhập CMND";
                    lbValidation.ForeColor = Color.Red;
                    btnUpdate.Enabled = false;
                }
                else if (validateCus == null)
                {
                    lbValidation.Text = "CMND không bị trùng";
                    lbValidation.ForeColor = Color.Green;
                    btnUpdate.Enabled = true;

                }
                else
                {
                    lbValidation.Text = "CMND đã bị trùng";
                    lbValidation.ForeColor = Color.Red;
                    btnUpdate.Enabled = false;
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            edit = false;
            txtCMND.Text = txtTenKH.Text = txtDiaChi.Text = txtQuocTich.Text = txtSDT.Text = "";
            rbNam.Checked = rbNu.Checked = false;
            txtCMND.ReadOnly = false;
            txtCMND.Focus();
            btnMode();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (edit)
            {
                var find = db.tblKhachhangs.Find(txtCMND.Text);
                if (find != null)
                {
                    find.TenKH = txtTenKH.Text;
                    find.Quoctich = txtQuocTich.Text;
                    find.Diachi = txtDiaChi.Text;
                    find.Dienthoai = txtSDT.Text;
                    if (rbNam.Checked == true)
                        find.Gioitinh = "Nam";
                    else
                        find.Gioitinh = "Nữ";
                }
            }
            else
            {
                var checkCus = db.tblKhachhangs.Find(txtCMND.Text);
                if (checkCus == null)
                {
                    var cus = new tblKhachhang();
                    cus.SoCMND = txtCMND.Text;
                    cus.TenKH = txtTenKH.Text;
                    cus.Quoctich = txtQuocTich.Text;
                    cus.Diachi = txtDiaChi.Text;
                    cus.Dienthoai = txtSDT.Text;
                    if (rbNam.Checked == true)
                        cus.Gioitinh = "Nam";
                    else
                        cus.Gioitinh = "Nữ";
                    db.tblKhachhangs.Add(cus);


                }
                else
                {
                    MessageBox.Show("Khách hàng có CMND " + txtCMND.Text + " đã tồn tại trong danh sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            CheckField();
            db.SaveChanges();
            dgvKhachhang.DataSource = db.tblKhachhangs.ToList();
            ShowCustomerDetail();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowCustomerDetail();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int delnumber = 0;
            string str = "";
            foreach (DataGridViewRow row in dgvKhachhang.SelectedRows)
            {
                delnumber += 1;
                str += row.Cells["CMND"].Value.ToString();
            }
            if (MessageBox.Show("Bạn có muốn xóa " + delnumber + " khách hàng không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvKhachhang.SelectedRows)
                {
                    var find = db.tblKhachhangs.Find(row.Cells["CMND"].Value.ToString());
                    if (find != null)
                    {
                        db.tblKhachhangs.Remove(find);
                        db.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Không tồn tại khách hàng có CMND: " + txtCMND.Text, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            dgvKhachhang.DataSource = db.tblKhachhangs.ToList();
            ShowCustomerDetail();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (txtTimCMND.Text.Trim().Length == 0 && txtTimTenKH.Text.Trim().Length == 00)
            {
                MessageBox.Show("Bạn phải nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTimTenKH.Focus();
                return;
            }
            if (txtTimTenKH.Text != "")
            {
                var customer = from cus in db.tblKhachhangs
                               where cus.TenKH.Contains(txtTimTenKH.Text)
                               select new { cus.TenKH, cus.SoCMND, cus.Diachi, cus.Gioitinh, cus.Dienthoai, cus.Quoctich };
                dgvKhachhang.DataSource = customer.ToList();
            }
            if (txtTimCMND.Text != "")
            {
                var customer = from cus in db.tblKhachhangs
                               where cus.SoCMND.Contains(txtTimCMND.Text)
                               select new { cus.TenKH, cus.SoCMND, cus.Diachi, cus.Gioitinh, cus.Dienthoai, cus.Quoctich };
                dgvKhachhang.DataSource = customer.ToList();
            }
            btnFindReturn.Visible = true;
        }
        private void btnFindReturn_Click(object sender, EventArgs e)
        {
            dgvKhachhang.DataSource = db.tblKhachhangs.ToList();
            ShowCustomerDetail();
            btnFindReturn.Visible = false;
        }
        public void AutoComplete()
        {
            var autosource = from name in db.tblKhachhangs where name.TenKH.Contains(txtTimTenKH.Text) select name.TenKH;
            AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
            MyCollection.AddRange(autosource.ToArray());
            txtTimTenKH.AutoCompleteCustomSource = MyCollection;
        }
        public void CheckField()
        {
            if (txtTenKH.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKH.Focus();
                return;
            }

            if (txtDiaChi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập địa chỉ của khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }

            if (txtSDT.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập số điện thoại của khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            if (txtQuocTich.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập quốc tịch của khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuocTich.Focus();
                return;
            }

            if (rbNam.Checked == false && rbNu.Checked == false)
            {
                MessageBox.Show("Bạn chưa chọn giới tính của khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        private void txtTimTenKH_TextChanged(object sender, EventArgs e)
        {
            AutoComplete();
        }
    }
}
