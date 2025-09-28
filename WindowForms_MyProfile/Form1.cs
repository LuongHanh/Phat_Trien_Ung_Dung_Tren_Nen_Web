using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using My_Profile;

namespace WindowForms_MyProfile
{
    public class Form1 : Form
    {
        private TextBox txtTen;
        private Button btnTim;
        private TextBox txtThongTin;
        private Label lblTen;

        private string connectionString = "Server=localhost;Database=My_Profile;Trusted_Connection=True;MultipleActiveResultSets=True;";

        public Form1()
        {
            InitializeComponent();
        }

        // ********** Hàm Init: khởi tạo control ở đây (thay thế Designer) **********
        private void InitializeComponent()
        {
            this.txtTen = new TextBox();
            this.btnTim = new Button();
            this.txtThongTin = new TextBox();
            this.lblTen = new Label();

            // 
            // lblTen
            // 
            this.lblTen.Location = new Point(12, 15);
            this.lblTen.Size = new Size(70, 20);
            this.lblTen.Text = "Họ tên:";
            // 
            // txtTen
            // 
            this.txtTen.Location = new Point(85, 12);
            this.txtTen.Size = new Size(260, 22);
            // 
            // btnTim
            // 
            this.btnTim.Location = new Point(355, 11);
            this.btnTim.Size = new Size(80, 24);
            this.btnTim.Text = "Tìm";
            this.btnTim.Click += new EventHandler(this.BtnTim_Click);
            // 
            // txtThongTin
            // 
            this.txtThongTin.Location = new Point(15, 50);
            this.txtThongTin.Size = new Size(420, 220);
            this.txtThongTin.Multiline = true;
            this.txtThongTin.ScrollBars = ScrollBars.Vertical;
            this.txtThongTin.ReadOnly = true;

            // 
            // Form1
            // 
            this.ClientSize = new Size(450, 290);
            this.Controls.Add(this.lblTen);
            this.Controls.Add(this.txtTen);
            this.Controls.Add(this.btnTim);
            this.Controls.Add(this.txtThongTin);
            this.Text = "My Profile - Tra cứu";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // ********** Truy vấn DB **********
        private ThongTinCaNhan GetThongTinCaNhan(string hoTen)
        {
            ThongTinCaNhan caNhan = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1) Lấy thông tin cá nhân
                string sqlCaNhan = "SELECT * FROM ThongTinCaNhan WHERE HoTen = @HoTen OR TenKhongDau = @HoTen";
                using (SqlCommand cmd = new SqlCommand(sqlCaNhan, conn))
                {
                    cmd.Parameters.AddWithValue("@HoTen", hoTen);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            caNhan = new ThongTinCaNhan
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                HoTen = reader["HoTen"].ToString(),
                                NgaySinh = reader["NgaySinh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgaySinh"]),
                                GioiTinh = reader["GioiTinh"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                SoDienThoai = reader["SoDienThoai"].ToString(),
                                Email = reader["Email"].ToString(),
                                GhiChu = reader["GhiChu"].ToString()
                            };
                        }
                    }
                }

                if (caNhan == null) return null;

                // 2) Học vấn
                string sqlHocVan = "SELECT * FROM HocVan WHERE IdCaNhan = @Id";
                using (SqlCommand cmd = new SqlCommand(sqlHocVan, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", caNhan.Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            caNhan.HocVans.Add(new HocVan
                            {
                                Id = (int)reader["Id"],
                                IdCaNhan = (int)reader["IdCaNhan"],
                                Truong = reader["Truong"].ToString(),
                                ChuyenNganh = reader["ChuyenNganh"].ToString(),
                                BangCap = reader["BangCap"].ToString(),
                                NamBatDau = reader["NamBatDau"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["NamBatDau"]),
                                NamKetThuc = reader["NamKetThuc"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["NamKetThuc"])
                            });
                        }
                    }
                }

                // 3) Kinh nghiệm
                string sqlKN = "SELECT * FROM KinhNghiemLamViec WHERE IdCaNhan = @Id";
                using (SqlCommand cmd = new SqlCommand(sqlKN, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", caNhan.Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            caNhan.KinhNghiems.Add(new KinhNghiemLamViec
                            {
                                Id = (int)reader["Id"],
                                IdCaNhan = (int)reader["IdCaNhan"],
                                CongTy = reader["CongTy"].ToString(),
                                ViTri = reader["ViTri"].ToString(),
                                MoTa = reader["MoTa"].ToString(),
                                NamBatDau = reader["NamBatDau"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["NamBatDau"]),
                                NamKetThuc = reader["NamKetThuc"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["NamKetThuc"])
                            });
                        }
                    }
                }

                // 4) Kỹ năng
                string sqlKNang = "SELECT * FROM KyNang WHERE IdCaNhan = @Id";
                using (SqlCommand cmd = new SqlCommand(sqlKNang, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", caNhan.Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            caNhan.KyNangs.Add(new KyNang
                            {
                                Id = (int)reader["Id"],
                                IdCaNhan = (int)reader["IdCaNhan"],
                                TenKyNang = reader["TenKyNang"].ToString(),
                                MucDo = reader["MucDo"].ToString()
                            });
                        }
                    }
                }

                // 5) Sở thích
                string sqlST = "SELECT * FROM SoThich WHERE IdCaNhan = @Id";
                using (SqlCommand cmd = new SqlCommand(sqlST, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", caNhan.Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            caNhan.SoThichs.Add(new SoThich
                            {
                                Id = (int)reader["Id"],
                                IdCaNhan = (int)reader["IdCaNhan"],
                                TenSoThich = reader["TenSoThich"].ToString()
                            });
                        }
                    }
                }
            }

            return caNhan;
        }

        // ********** Event xử lý tìm **********
        private void BtnTim_Click(object sender, EventArgs e)
        {
            string ten = txtTen.Text.Trim();
            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Nhập tên cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ThongTinCaNhan found = GetThongTinCaNhan(ten);

            if (found == null)
            {
                txtThongTin.Text = "Không tìm thấy: " + ten;
                return;
            }

            // format kết quả
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----- HỒ SƠ -----");
            sb.AppendLine("Họ tên: " + found.HoTen);
            sb.AppendLine("Ngày sinh: " + (found.NgaySinh.HasValue ? found.NgaySinh.Value.ToString("dd/MM/yyyy") : "N/A"));
            sb.AppendLine("Giới tính: " + found.GioiTinh);
            sb.AppendLine("Địa chỉ: " + found.DiaChi);
            sb.AppendLine("SĐT: " + found.SoDienThoai);
            sb.AppendLine("Email: " + found.Email);
            sb.AppendLine("Ghi chú: " + found.GhiChu);

            sb.AppendLine();
            sb.AppendLine("--- Học vấn ---");
            foreach (var h in found.HocVans)
                sb.AppendLine($"{h.Truong} - {h.ChuyenNganh} ({h.NamBatDau?.ToString() ?? "-"}-{h.NamKetThuc?.ToString() ?? "-"})");

            sb.AppendLine();
            sb.AppendLine("--- Kinh nghiệm ---");
            foreach (var k in found.KinhNghiems)
                sb.AppendLine($"{k.CongTy} - {k.ViTri} ({k.NamBatDau?.ToString() ?? "-"}-{k.NamKetThuc?.ToString() ?? "-"})");

            sb.AppendLine();
            sb.AppendLine("--- Kỹ năng ---");
            foreach (var kk in found.KyNangs)
                sb.AppendLine($"{kk.TenKyNang} - {kk.MucDo}");

            sb.AppendLine();
            sb.AppendLine("--- Sở thích ---");
            foreach (var s in found.SoThichs)
                sb.AppendLine(s.TenSoThich);

            sb.AppendLine();
            sb.AppendLine("-- Hồ sơ: "+ten+" (My_Profile) --");

            txtThongTin.Text = sb.ToString();
        }
    }
}
