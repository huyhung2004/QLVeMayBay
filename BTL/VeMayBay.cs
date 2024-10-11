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
    public partial class VeMayBay : Form
    {
        private string connectionString = "Data Source=DESKTOP-JVNF536;Initial Catalog=QLVeMayBay;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adt;
        public VeMayBay()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ChuyenBay cb=new ChuyenBay();
            cb.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void VeMayBay_Load(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }
        private void RefreshDataGrid()
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT TicketID, FlightID, PassengerID, BookingDate, SeatNumber, Price FROM Tickets", con);
                    var adt = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    ds.Clear();
                    adt.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];

                    // Đặt kích thước cho các cột phù hợp với bảng Tickets
                    dataGridView1.Columns[0].Width = 72;  // TicketID
                    dataGridView1.Columns[1].Width = 96;  // FlightID
                    dataGridView1.Columns[2].Width = 72;  // PassengerID
                    dataGridView1.Columns[3].Width = 123; // BookingDate
                    dataGridView1.Columns[4].Width = 72;  // SeatNumber
                    dataGridView1.Columns[5].Width = 72;  // Price
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DatVeB_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string checkSeatsQuery = "SELECT (SELECT COUNT(*) FROM Tickets WHERE FlightID = @FlightID) AS SeatsBooked, " +
                                             "TotalSeats, AvailableSeats FROM Flights WHERE FlightID = @FlightID";

                    using (cmd = new SqlCommand(checkSeatsQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@FlightID", Convert.ToInt32(MaChuyenBaytxt.Text));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int seatsBooked = Convert.ToInt32(reader["SeatsBooked"]); // Số chỗ đã đặt
                                int totalSeats = Convert.ToInt32(reader["TotalSeats"]);   // Tổng số chỗ
                                int availableSeats = Convert.ToInt32(reader["AvailableSeats"]); // Số chỗ trống

                                if (seatsBooked >= availableSeats)
                                {
                                    MessageBox.Show("Không thể đặt thêm vé, chuyến bay đã hết chỗ.");
                                    return;
                                }
                            }
                        }
                    }

                    string query = "INSERT INTO Tickets (FlightID, PassengerID, BookingDate, SeatNumber, Price) " +
                                   "VALUES (@FlightID, @PassengerID, @BookingDate, @SeatNumber, @Price)";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FlightID", Convert.ToInt32(MaChuyenBaytxt.Text));
                        cmd.Parameters.AddWithValue("@PassengerID", Convert.ToInt32(MaKhachHangtxt.Text));
                        cmd.Parameters.AddWithValue("@BookingDate", NgayDatVetxt.Value);
                        cmd.Parameters.AddWithValue("@SeatNumber", SoGhetxt.Text);
                        cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(Giatxt.Text));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đặt vé thành công!");
                        RefreshDataGrid();
                    }
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
                    string query = "UPDATE Tickets SET FlightID=@FlightID, PassengerID=@PassengerID, BookingDate=@BookingDate, " +
                                   "SeatNumber=@SeatNumber, Price=@Price WHERE TicketID=@TicketID";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TicketID", MaVetxt.Text);
                        cmd.Parameters.AddWithValue("@FlightID", MaChuyenBaytxt.Text);
                        cmd.Parameters.AddWithValue("@PassengerID", MaKhachHangtxt.Text);
                        cmd.Parameters.AddWithValue("@BookingDate", NgayDatVetxt.Value);
                        cmd.Parameters.AddWithValue("@SeatNumber", SoGhetxt.Text);
                        cmd.Parameters.AddWithValue("@Price", Giatxt.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ticket updated successfully!");
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
                    string query = "DELETE FROM Tickets WHERE TicketID=@TicketID";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TicketID", MaVetxt.Text); // MaVexTxt là TextBox để nhập mã vé

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ticket deleted successfully!");
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

                    string query = "SELECT TicketID, FlightID, PassengerID, BookingDate, SeatNumber, Price FROM Tickets WHERE 1=1";

                    // Thêm điều kiện tìm kiếm nếu người dùng nhập giá trị
                    if (!string.IsNullOrEmpty(MaVetxt.Text))
                    {
                        query += " AND TicketID LIKE @TicketID";
                    }
                    if (!string.IsNullOrEmpty(MaChuyenBaytxt.Text))
                    {
                        query += " AND FlightID LIKE @FlightID";
                    }
                    if (!string.IsNullOrEmpty(MaKhachHangtxt.Text))
                    {
                        query += " AND PassengerID LIKE @PassengerID";
                    }
                    if (NgayDatVetxt.Checked)
                    {
                        query += " AND CAST(BookingDate AS DATE) = @BookingDate";
                    }
                    if (!string.IsNullOrEmpty(SoGhetxt.Text))
                    {
                        query += " AND SeatNumber LIKE @SeatNumber";
                    }
                    if (!string.IsNullOrEmpty(Giatxt.Text))
                    {
                        query += " AND Price = @Price";
                    }

                    using (cmd = new SqlCommand(query, con))
                    {
                        // Thêm tham số cho câu truy vấn
                        if (!string.IsNullOrEmpty(MaVetxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@TicketID", "%" + MaVetxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(MaChuyenBaytxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@FlightID", "%" + MaChuyenBaytxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(MaKhachHangtxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@PassengerID", "%" + MaKhachHangtxt.Text + "%");
                        }
                        if (NgayDatVetxt.Checked)
                        {
                            cmd.Parameters.AddWithValue("@BookingDate", NgayDatVetxt.Value.Date);
                        }
                        if (!string.IsNullOrEmpty(SoGhetxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@SeatNumber", "%" + SoGhetxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(Giatxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@Price", Giatxt.Text);
                        }

                        // Đổ dữ liệu vào DataGridView
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

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                MaVetxt.Text = row.Cells[0].Value.ToString();      // TicketID
                MaChuyenBaytxt.Text = row.Cells[1].Value.ToString(); // FlightID
                MaKhachHangtxt.Text = row.Cells[2].Value.ToString(); // PassengerID
                NgayDatVetxt.Value = DateTime.Parse(row.Cells[3].Value.ToString()); // BookingDate
                SoGhetxt.Text = row.Cells[4].Value.ToString();       // SeatNumber
                Giatxt.Text = row.Cells[5].Value.ToString();        // Price
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            KhachHang cb = new KhachHang();
            cb.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            BaoCao cb = new BaoCao();
            cb.Show();
            this.Hide();
        }
    }
}
