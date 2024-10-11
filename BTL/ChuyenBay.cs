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
    public partial class ChuyenBay : Form
    {
        private string connectionString = "Data Source=DESKTOP-JVNF536;Initial Catalog=QLVeMayBay;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adt;
        public ChuyenBay()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void TaoChuyenBayB_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();
                    // Assuming you have textboxes to get user input like txtFlightNumber, txtAirline, etc.
                    string query = "INSERT INTO Flights (FlightNumber, Airline, Departure, Destination, DepartureTime, ArrivalTime,TotalSeats,AvailableSeats) " +
                                   "VALUES (@FlightNumber, @Airline, @Departure, @Destination, @DepartureTime, @ArrivalTime,@TotalSeats,@AvailableSeat)";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FlightNumber", SoChuyenBaytxt.Text);
                        cmd.Parameters.AddWithValue("@Airline", HangHangKhongtxt.Text);
                        cmd.Parameters.AddWithValue("@Departure", NoiKhoiHanhtxt.Text);
                        cmd.Parameters.AddWithValue("@Destination",DiemDentxt.Text);
                        cmd.Parameters.AddWithValue("@DepartureTime", TGKhoiHanhtxt.Value);
                        cmd.Parameters.AddWithValue("@ArrivalTime", TGDentxt.Value);
                        cmd.Parameters.AddWithValue("@TotalSeats", TongGhetxt.Text);
                        cmd.Parameters.AddWithValue("@AvailableSeat", SoGheTrongtxt.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Flight created successfully!");
                        RefreshDataGrid(); // Refresh the data grid to show new record
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            VeMayBay cb = new VeMayBay();
            cb.Show();
            this.Hide();
        }

        private void ChuyenBay_Load(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }
        private void RefreshDataGrid()
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();
                    cmd = new SqlCommand("SELECT FlightNumber,Airline, Departure, Destination, DepartureTime, ArrivalTime,TotalSeats,AvailableSeats FROM Flights", con);
                    adt = new SqlDataAdapter(cmd);
                    SqlCommandBuilder builder = new SqlCommandBuilder();
                    var ds = new DataSet();
                    ds.Clear();
                    adt.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns[0].Width = 72;
                    dataGridView1.Columns[1].Width = 96;
                    dataGridView1.Columns[2].Width = 72;
                    dataGridView1.Columns[3].Width = 72;
                    dataGridView1.Columns[4].Width = 123;
                    dataGridView1.Columns[5].Width = 123;
                    dataGridView1.Columns[6].Width = 72;
                    dataGridView1.Columns[7].Width = 72;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Flights WHERE FlightNumber=@FlightNumber";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FlightNumber", SoChuyenBaytxt.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Flight deleted successfully!");
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
                    string query = "UPDATE Flights SET Airline=@Airline, Departure=@Departure, Destination=@Destination, " +
                                   "DepartureTime=@DepartureTime, ArrivalTime=@ArrivalTime, TotalSeats=@TotalSeats, AvailableSeats=@AvailableSeats " +
                                   "WHERE FlightNumber=@FlightNumber";

                    using (cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FlightNumber", SoChuyenBaytxt.Text);
                        cmd.Parameters.AddWithValue("@Airline", HangHangKhongtxt.Text);
                        cmd.Parameters.AddWithValue("@Departure", NoiKhoiHanhtxt.Text);
                        cmd.Parameters.AddWithValue("@Destination", DiemDentxt.Text);
                        cmd.Parameters.AddWithValue("@DepartureTime", TGKhoiHanhtxt.Value);
                        cmd.Parameters.AddWithValue("@ArrivalTime", TGDentxt.Value);
                        cmd.Parameters.AddWithValue("@TotalSeats", TongGhetxt.Text);
                        cmd.Parameters.AddWithValue("@AvailableSeats", SoGheTrongtxt.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Flight updated successfully!");
                        RefreshDataGrid();
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
                    SoChuyenBaytxt.Text = row.Cells[0].Value.ToString();
                    HangHangKhongtxt.Text = row.Cells[1].Value.ToString();
                    NoiKhoiHanhtxt.Text = row.Cells[2].Value.ToString();
                    DiemDentxt.Text = row.Cells[3].Value.ToString();
                    TGKhoiHanhtxt.Value = DateTime.Parse(row.Cells[4].Value.ToString());
                    TGDentxt.Value = DateTime.Parse(row.Cells[5].Value.ToString());
                    TongGhetxt.Text = row.Cells[6].Value.ToString();
                    SoGheTrongtxt.Text = row.Cells[7].Value.ToString();
                }
       

        }

        private void TimKiemB_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT FlightNumber, Airline, Departure, Destination, DepartureTime, ArrivalTime, TotalSeats, AvailableSeats FROM Flights WHERE 1=1";

                    // Tạo danh sách các điều kiện tìm kiếm nếu người dùng nhập giá trị
                    if (!string.IsNullOrEmpty(SoChuyenBaytxt.Text))
                    {
                        query += " AND FlightNumber LIKE @FlightNumber";
                    }
                    if (!string.IsNullOrEmpty(HangHangKhongtxt.Text))
                    {
                        query += " AND Airline LIKE @Airline";
                    }
                    if (!string.IsNullOrEmpty(NoiKhoiHanhtxt.Text))
                    {
                        query += " AND Departure LIKE @Departure";
                    }
                    if (!string.IsNullOrEmpty(DiemDentxt.Text))
                    {
                        query += " AND Destination LIKE @Destination";
                    }

                    // Tìm kiếm theo ngày khởi hành nếu DateTimePicker được chọn (chỉ tìm theo ngày)
                    if (TGKhoiHanhtxt.Checked)
                    {
                        query += " AND CAST(DepartureTime AS DATE) = @DepartureDate";
                    }

                    // Tìm kiếm theo ngày đến nếu DateTimePicker được chọn (chỉ tìm theo ngày)
                    if (TGDentxt.Checked)
                    {
                        query += " AND CAST(ArrivalTime AS DATE) = @ArrivalDate";
                    }

                    // Tìm kiếm theo tổng số ghế nếu người dùng nhập giá trị
                    if (!string.IsNullOrEmpty(TongGhetxt.Text))
                    {
                        query += " AND TotalSeats = @TotalSeats";
                    }

                    // Tìm kiếm theo số ghế trống nếu người dùng nhập giá trị
                    if (!string.IsNullOrEmpty(SoGheTrongtxt.Text))
                    {
                        query += " AND AvailableSeats = @AvailableSeats";
                    }

                    using (cmd = new SqlCommand(query, con))
                    {
                        // Thêm các tham số vào câu lệnh SQL
                        if (!string.IsNullOrEmpty(SoChuyenBaytxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@FlightNumber", "%" + SoChuyenBaytxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(HangHangKhongtxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@Airline", "%" + HangHangKhongtxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(NoiKhoiHanhtxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@Departure", "%" + NoiKhoiHanhtxt.Text + "%");
                        }
                        if (!string.IsNullOrEmpty(DiemDentxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@Destination", "%" + DiemDentxt.Text + "%");
                        }

                        // Thêm tham số tìm kiếm theo ngày khởi hành nếu người dùng chọn ngày
                        if (TGKhoiHanhtxt.Checked)
                        {
                            cmd.Parameters.AddWithValue("@DepartureDate", TGKhoiHanhtxt.Value.Date);
                        }

                        // Thêm tham số tìm kiếm theo ngày đến nếu người dùng chọn ngày
                        if (TGDentxt.Checked)
                        {
                            cmd.Parameters.AddWithValue("@ArrivalDate", TGDentxt.Value.Date);
                        }

                        // Thêm tham số tìm kiếm theo tổng số ghế nếu có
                        if (!string.IsNullOrEmpty(TongGhetxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@TotalSeats", TongGhetxt.Text);
                        }

                        // Thêm tham số tìm kiếm theo số ghế trống nếu có
                        if (!string.IsNullOrEmpty(SoGheTrongtxt.Text))
                        {
                            cmd.Parameters.AddWithValue("@AvailableSeats", SoGheTrongtxt.Text);
                        }

                        // Đổ dữ liệu kết quả tìm kiếm vào DataGridView
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

        private void TGKhoiHanhtxt_ValueChanged(object sender, EventArgs e)
        {

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
