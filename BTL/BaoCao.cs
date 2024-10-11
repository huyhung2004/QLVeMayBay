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
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
namespace BTL
{
    public partial class BaoCao : Form
    {
        private string connectionString = "Data Source=DESKTOP-JVNF536;Initial Catalog=QLVeMayBay;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection con;
        public BaoCao()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
        }

        private void BaoCao_Load(object sender, EventArgs e)
        {
            con.Open();
            comboBox1.SelectedIndex = 0;
            LoadMonthlyReport();
        }

        private void ExcelB_Click(object sender, EventArgs e)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    // Tạo một worksheet mới
                    var ws = wb.Worksheets.Add("Báo cáo");

                    ws.Cell(1, 1).Value = "Báo cáo";
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Font.FontSize = 16;
                    // Thêm tiêu đề chính
                    ;
                    if (comboBox1.SelectedIndex != 0)
                    {
                        
                        ws.Range(1, 1, 1, 4).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        ws.Cell(3, 1).Value = "Thông tin dự án";
                        ws.Range(3, 1, 3, 4).Merge().Style.Fill.BackgroundColor = XLColor.DarkBlue;
                        ws.Range(3, 1, 3, 4).Style.Font.FontColor = XLColor.White;
                    }
                    else
                    {
                        ws.Range(1, 1, 1, 6).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        ws.Cell(3, 1).Value = "Thông tin dự án";
                        ws.Range(3, 1, 3, 6).Merge().Style.Fill.BackgroundColor = XLColor.DarkBlue;
                        ws.Range(3, 1, 3, 6).Style.Font.FontColor = XLColor.White;
                    }

                    // Thêm thông tin dự án
                    ws.Cell(4, 1).Value = "Tên dự án";
                    ws.Cell(4, 2).Value = "Quản lý bán vé máy bay";
                    ws.Cell(5, 1).Value = "Ngày báo cáo";
                    ws.Cell(5, 2).Value = "10-10-2024";
                    ws.Cell(6, 1).Value = "Người báo cáo";
                    ws.Cell(6, 2).Value = "Tạ Huy Hùng";

                    // Thêm thông tin bảng báo cáo
                    ws.Cell(8, 1).Value = "Thông tin báo cáo";

                    if (comboBox1.SelectedIndex != 0)
                    {
                        ws.Range(8, 1, 8, 4).Merge().Style.Fill.BackgroundColor = XLColor.DarkBlue;
                        ws.Range(8, 1, 8, 4).Style.Font.FontColor = XLColor.White;
                        // Thêm tiêu đề cột
                        ws.Cell(9, 1).Value = "STT";
                        ws.Cell(9, 2).Value = "Tháng";
                        ws.Cell(9, 3).Value = "Số chuyến bay";
                        ws.Cell(9, 4).Value = "Doanh thu (VND)";
                        ws.Range(9, 1, 9, 4).Style.Font.Bold = true;
                        ws.Range(9, 1, 9, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    }
                    else
                    {
                        ws.Range(8, 1, 8, 6).Merge().Style.Fill.BackgroundColor = XLColor.DarkBlue;
                        ws.Range(8, 1, 8, 6).Style.Font.FontColor = XLColor.White;
                        ws.Cell(9, 1).Value = "STT";
                        ws.Cell(9, 2).Value = "Số chuyến bay";
                        ws.Cell(9, 3).Value = "Nơi khởi hành";
                        ws.Cell(9, 4).Value = "Nơi đến";
                        ws.Cell(9, 5).Value = "Số vé";
                        ws.Cell(9, 6).Value = "Doanh thu";
                        ws.Range(9, 1, 9, 6).Style.Font.Bold = true;
                        ws.Range(9, 1, 9, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    }


                    // Giả sử có dữ liệu DataGridView
                    DataTable dt = (DataTable)dataGridView1.DataSource;
                    if (dt != null && dt.Rows.Count > 0 && comboBox1.SelectedIndex != 0)
                    {
                        // Điền dữ liệu từ DataGridView
                        int row = 10;
                        int stt = 1;
                        foreach (DataRow dr in dt.Rows)
                        {
                            ws.Cell(row, 1).Value = stt++;
                            ws.Cell(row, 2).Value = Convert.ToString(dr["Thang"]); // Đảm bảo rằng tên cột đúng
                            ws.Cell(row, 3).Value = Convert.ToInt32(dr["SoChuyenBay"]); // Đảm bảo rằng tên cột đúng
                            ws.Cell(row, 4).Value = Convert.ToDecimal(dr["DoanhThu"]); // Đảm bảo rằng tên cột đúng

                            row++;
                        }

                        // Tính tổng doanh thu
                        ws.Cell(row, 2).Value = "Tổng doanh thu:";
                        ws.Cell(row, 3).Value = Convert.ToDecimal(dt.Compute("SUM(DoanhThu)", string.Empty)); // Sử dụng tên cột đúng
                        ws.Range(row, 2, row, 3).Style.Font.Bold = true;
                        ws.Range(row, 2, row, 3).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    }
                    else if (dt != null && dt.Rows.Count > 0)
                    {
                        int row = 10;
                        int stt = 1;
                        foreach (DataRow dr in dt.Rows)
                        {
                            ws.Cell(row, 1).Value = stt++;
                            ws.Cell(row, 2).Value = Convert.ToString(dr["FlightNumber"]); // Đảm bảo rằng tên cột đúng
                            ws.Cell(row, 3).Value = Convert.ToString(dr["Departure"]); // Đảm bảo rằng tên cột đúng
                            ws.Cell(row, 4).Value = Convert.ToString(dr["Destination"]); // Đảm bảo rằng tên cột đúng
                            ws.Cell(row, 5).Value = Convert.ToInt32(dr["SoVe"]); // Đảm bảo rằng tên cột đúng
                            ws.Cell(row, 6).Value = Convert.ToDecimal(dr["DoanhThu"]); // Đảm bảo rằng tên cột đúng
                            row++;
                        }

                        // Tính tổng doanh thu
                        ws.Cell(row, 2).Value = "Tổng doanh thu:";
                        ws.Cell(row, 3).Value = Convert.ToDecimal(dt.Compute("SUM(DoanhThu)", string.Empty)); // Sử dụng tên cột đúng
                        ws.Range(row, 2, row, 3).Style.Font.Bold = true;
                        ws.Range(row, 2, row, 3).Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Cell(row, 5).Value = "Tổng Số vé:";
                        ws.Cell(row, 6).Value = Convert.ToInt32(dt.Compute("SUM(SoVe)", string.Empty)); // Sử dụng tên cột đúng
                        ws.Range(row, 5, row, 6).Style.Font.Bold = true;
                        ws.Range(row, 5, row, 6).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    }

                    // Điều chỉnh độ rộng của các cột để phù hợp với nội dung
                    ws.Columns().AdjustToContents();

                    // Lưu file Excel
                    SaveFileDialog sfd = new SaveFileDialog
                    {
                        Filter = "Excel Workbook|*.xlsx",
                        Title = "Save an Excel File",
                        FileName = "BaoCao2024.xlsx"
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        wb.SaveAs(sfd.FileName);
                        MessageBox.Show("Xuất báo cáo ra Excel thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void LoadMonthlyReport()
        {
            string query = @"SELECT FlightNumber, Departure, Destination, COUNT(TicketID) AS SoVe, 
                            SUM(Price) AS DoanhThu
                            FROM Flights f
                            JOIN Tickets t ON f.FlightID = t.FlightID
                            WHERE MONTH(BookingDate) = @Month AND YEAR(BookingDate) = @Year
                            GROUP BY FlightNumber, Departure, Destination";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Month", dateTimePicker1.Value.Month);
            cmd.Parameters.AddWithValue("@Year", dateTimePicker1.Value.Year);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;

            // Cập nhật tổng số chuyến bay, vé và doanh thu
            UpdateSummary(dt);
        }

        private void LoadYearlyReport()
        {
            string query = @"SELECT MONTH(BookingDate) AS Thang, COUNT(DISTINCT f.FlightID) AS SoChuyenBay, 
                            SUM(Price) AS DoanhThu
                            FROM Flights f
                            JOIN Tickets t ON f.FlightID = t.FlightID
                            WHERE YEAR(BookingDate) = @Year
                            GROUP BY MONTH(BookingDate)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Year", dateTimePicker1.Value.Year);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
            
            // Cập nhật tổng số chuyến bay và doanh thu
            UpdateSummary2(dt);
        }

        private void UpdateSummary(DataTable dt)
        {
            int totalFlights = 0;
            int totalTickets = 0;
            decimal totalRevenue = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalFlights++;
                totalTickets += Convert.ToInt32(row["SoVe"]);
                totalRevenue += Convert.ToDecimal(row["DoanhThu"]);
            }
            TongVe.Show();
            TongCB.Text = "Tổng chuyến bay: \n" + totalFlights.ToString();
            TongVe.Text = "Tổng vé: \n" + totalTickets.ToString();
            TongDoanhThu.Text = "Tổng doanh thu: \n" + string.Format("{0:#,##0} ₫", totalRevenue);
            
        }
        private void UpdateSummary2(DataTable dt)
        {
            int totalFlights = 0;
            int totalTickets = 0;
            decimal totalRevenue = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalFlights++;
                totalRevenue += Convert.ToDecimal(row["DoanhThu"]);
            }
            TongCB.Text = "Tổng chuyến bay: \n" + totalFlights.ToString();
            TongVe.Hide();
            TongDoanhThu.Text = "Tổng doanh thu: \n" + string.Format("{0:#,##0} ₫", totalRevenue);
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                dateTimePicker1.CustomFormat = "MM/yyyy";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                //ExcelB.Hide();
                // Báo cáo theo tháng
                LoadMonthlyReport();
            }
            else
            {
                dateTimePicker1.CustomFormat = "yyyy";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                //ExcelB.Show();
                // Báo cáo theo năm
                LoadYearlyReport();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                dateTimePicker1.CustomFormat = "MM/yyyy";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;

                // Báo cáo theo tháng
                LoadMonthlyReport();
            }
            else
            {
                dateTimePicker1.CustomFormat = "yyyy";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                // Báo cáo theo năm
                LoadYearlyReport();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            KhachHang cb = new KhachHang();
            cb.Show();
            this.Hide();
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
    }

}
