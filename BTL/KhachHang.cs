using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL
{
    public partial class KhachHang : Form
    {
        private string connectionString = "Data Source=DESKTOP-JVNF536;Initial Catalog=QLVeMayBay;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adt;
        public KhachHang()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            VeMayBay cb = new VeMayBay();
            cb.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ChuyenBay cb = new ChuyenBay();
            cb.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            BaoCao cb = new BaoCao();
            cb.Show();
            this.Hide();
        }

        private void MaKhachHangtxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void DatVeB_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO Passengers (FirstName, LastName, CCCD, Phone, UserID) " +
                                   "VALUES (@FirstName, @LastName, @CCCD, @Phone, @UserID)";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", Hotxt.Text);
                        cmd.Parameters.AddWithValue("@LastName", Tentxt.Text);
                        cmd.Parameters.AddWithValue("@CCCD", CCCDtxt.Text);
                        cmd.Parameters.AddWithValue("@Phone", SoDienThoaitxt.Text);
                        cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Usertxt.Text));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("New passenger added successfully!");
                        RefreshDataGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RefreshDataGrid()
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT PassengerID, FirstName, LastName, CCCD, Phone, UserID FROM Passengers", con);
                    var adt = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    ds.Clear();
                    adt.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];

                    // Đặt kích thước cho các cột phù hợp với bảng Passengers
                    dataGridView1.Columns[0].Width = 72;  // PassengerID
                    dataGridView1.Columns[1].Width = 96;  // FirstName
                    dataGridView1.Columns[2].Width = 96;  // LastName
                    dataGridView1.Columns[3].Width = 123; // CCCD
                    dataGridView1.Columns[4].Width = 72;   // Phone
                    dataGridView1.Columns[5].Width = 72;   // UserID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SuaB_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Passengers SET FirstName=@FirstName, LastName=@LastName, CCCD=@CCCD, Phone=@Phone, UserID=@UserID WHERE PassengerID=@PassengerID";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PassengerID", Convert.ToInt32(MaKhachHangtxt.Text));
                        cmd.Parameters.AddWithValue("@FirstName", Hotxt.Text);
                        cmd.Parameters.AddWithValue("@LastName", Tentxt.Text);
                        cmd.Parameters.AddWithValue("@CCCD", CCCDtxt.Text);
                        cmd.Parameters.AddWithValue("@Phone", SoDienThoaitxt.Text);
                        cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Usertxt.Text));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Passenger updated successfully!");
                        RefreshDataGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Passengers WHERE PassengerID=@PassengerID";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PassengerID", Convert.ToInt32(MaKhachHangtxt.Text));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Passenger deleted successfully!");
                        RefreshDataGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TimKiemB_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT PassengerID, FirstName, LastName, CCCD, Phone, UserID FROM Passengers WHERE 1=1";

                    // Kiểm tra từng trường để thêm vào câu truy vấn
                    if (!string.IsNullOrEmpty(MaKhachHangtxt.Text))
                    {
                        query += " AND PassengerID LIKE @PassengerID";
                    }
                    if (!string.IsNullOrEmpty(Hotxt.Text))
                    {
                        query += " AND FirstName LIKE @FirstName";
                    }
                    if (!string.IsNullOrEmpty(Tentxt.Text))
                    {
                        query += " AND LastName LIKE @LastName";
                    }
                    if (!string.IsNullOrEmpty(CCCDtxt.Text))
                    {
                        query += " AND CCCD LIKE @CCCD";
                    }
                    if (!string.IsNullOrEmpty(SoDienThoaitxt.Text))
                    {
                        query += " AND Phone LIKE @Phone";
                    }
                    if (!string.IsNullOrEmpty(Usertxt.Text))
                    {
                        query += " AND UserID LIKE @UserID";
                    }

                    using (cmd = new SqlCommand(query, con))
                    {
                        // Thêm tham số tương ứng cho từng trường
                        if (!string.IsNullOrEmpty(MaKhachHangtxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@PassengerID", "%" + MaKhachHangtxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(Hotxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", "%" + Hotxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(Tentxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@LastName", "%" + Tentxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(CCCDtxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@CCCD", "%" + CCCDtxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(SoDienThoaitxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@Phone", "%" + SoDienThoaitxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(Usertxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@UserID", "%" + Usertxt.Text + "%");
                        }

                        adt = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adt.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                MaKhachHangtxt.Text = row.Cells[0].Value.ToString();  // PassengerID
                Hotxt.Text = row.Cells[1].Value.ToString();    // FirstName
                Tentxt.Text = row.Cells[2].Value.ToString();     // LastName
                CCCDtxt.Text = row.Cells[3].Value.ToString();        // CCCD
                SoDienThoaitxt.Text = row.Cells[4].Value.ToString();        // Phone
                Usertxt.Text = row.Cells[5].Value.ToString();       // UserID
            }
        }

        private void KhachHang_Load(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }
    }
}
