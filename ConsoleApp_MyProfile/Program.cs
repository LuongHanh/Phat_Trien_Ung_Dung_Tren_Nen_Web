using My_Profile;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace ConsoleApp_MyProfile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Title = "My Profile Console App (.NET Framework 2.0)";
            Console.WriteLine("=== Danh sách hồ sơ cá nhân từ Database ===\n");
            Console.WriteLine("=== Chú ý: Nhập họ tên không dấu mới tìm được ===\n");

            string connectionString = "Server=localhost;Database=My_Profile;Trusted_Connection=True;MultipleActiveResultSets=True;";

            Console.Write("Nhập tên cần tìm: ");
            string tenCanTim = Console.ReadLine();

            List<ThongTinCaNhan> danhSach = LayTatCaThongTin(connectionString, tenCanTim);

            if (danhSach.Count == 0)
            {
                Console.WriteLine("\nKhông tìm thấy ai có tên: " + tenCanTim);
            }
            else
            {
                foreach (ThongTinCaNhan cn in danhSach)
                {
                    InThongTin(cn);
                    Console.WriteLine(new string('=', 50));
                }
            }

            Console.WriteLine();
            PrintWatermark();
            Console.WriteLine("\nNhấn Enter để thoát...");
            Console.ReadLine();
        }

        static List<ThongTinCaNhan> LayTatCaThongTin(string connectionString, string tenCanTim)
        {
            List<ThongTinCaNhan> list = new List<ThongTinCaNhan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //lọc theo tên
                string sql = @"
                    SELECT * 
                    FROM ThongTinCaNhan 
                    WHERE HoTen LIKE @ten OR TenKhongDau LIKE @ten";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", "%" + tenCanTim + "%");
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ThongTinCaNhan caNhan = new ThongTinCaNhan
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        HoTen = reader["HoTen"].ToString(),
                        NgaySinh = reader["NgaySinh"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["NgaySinh"]) : null,
                        GioiTinh = reader["GioiTinh"].ToString(),
                        DiaChi = reader["DiaChi"].ToString(),
                        SoDienThoai = reader["SoDienThoai"].ToString(),
                        Email = reader["Email"].ToString(),
                        GhiChu = reader["GhiChu"].ToString()
                    };

                    // Lấy các bảng liên quan
                    caNhan.HocVans = LayHocVan(conn, caNhan.Id);
                    caNhan.KinhNghiems = LayKinhNghiem(conn, caNhan.Id);
                    caNhan.KyNangs = LayKyNang(conn, caNhan.Id);
                    caNhan.SoThichs = LaySoThich(conn, caNhan.Id);

                    list.Add(caNhan);
                }

                reader.Close();
            }

            return list;
        }

        static List<HocVan> LayHocVan(SqlConnection conn, int idCaNhan)
        {
            List<HocVan> list = new List<HocVan>();
            string sql = "SELECT * FROM HocVan WHERE IdCaNhan = @id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", idCaNhan);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HocVan hv = new HocVan
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    IdCaNhan = idCaNhan,
                    Truong = reader["Truong"].ToString(),
                    ChuyenNganh = reader["ChuyenNganh"].ToString(),
                    BangCap = reader["BangCap"].ToString(),
                    NamBatDau = reader["NamBatDau"] != DBNull.Value ? (int?)Convert.ToInt32(reader["NamBatDau"]) : null,
                    NamKetThuc = reader["NamKetThuc"] != DBNull.Value ? (int?)Convert.ToInt32(reader["NamKetThuc"]) : null
                };
                list.Add(hv);
            }
            reader.Close();
            return list;
        }

        static List<KinhNghiemLamViec> LayKinhNghiem(SqlConnection conn, int idCaNhan)
        {
            List<KinhNghiemLamViec> list = new List<KinhNghiemLamViec>();
            string sql = "SELECT * FROM KinhNghiemLamViec WHERE IdCaNhan = @id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", idCaNhan);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                KinhNghiemLamViec kn = new KinhNghiemLamViec
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    IdCaNhan = idCaNhan,
                    CongTy = reader["CongTy"].ToString(),
                    ViTri = reader["ViTri"].ToString(),
                    MoTa = reader["MoTa"].ToString(),
                    NamBatDau = reader["NamBatDau"] != DBNull.Value ? (int?)Convert.ToInt32(reader["NamBatDau"]) : null,
                    NamKetThuc = reader["NamKetThuc"] != DBNull.Value ? (int?)Convert.ToInt32(reader["NamKetThuc"]) : null
                };
                list.Add(kn);
            }
            reader.Close();
            return list;
        }

        static List<KyNang> LayKyNang(SqlConnection conn, int idCaNhan)
        {
            List<KyNang> list = new List<KyNang>();
            string sql = "SELECT * FROM KyNang WHERE IdCaNhan = @id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", idCaNhan);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                KyNang kn = new KyNang
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    IdCaNhan = idCaNhan,
                    TenKyNang = reader["TenKyNang"].ToString(),
                    MucDo = reader["MucDo"].ToString()
                };
                list.Add(kn);
            }
            reader.Close();
            return list;
        }

        static List<SoThich> LaySoThich(SqlConnection conn, int idCaNhan)
        {
            List<SoThich> list = new List<SoThich>();
            string sql = "SELECT * FROM SoThich WHERE IdCaNhan = @id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", idCaNhan);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SoThich st = new SoThich
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    IdCaNhan = idCaNhan,
                    TenSoThich = reader["TenSoThich"].ToString()
                };
                list.Add(st);
            }
            reader.Close();
            return list;
        }

        static void InThongTin(ThongTinCaNhan cn)
        {
            Console.WriteLine("Họ tên     : " + cn.HoTen);
            Console.WriteLine("Ngày sinh  : " + (cn.NgaySinh.HasValue ? cn.NgaySinh.Value.ToShortDateString() : "N/A"));
            Console.WriteLine("Giới tính  : " + cn.GioiTinh);
            Console.WriteLine("Địa chỉ    : " + cn.DiaChi);
            Console.WriteLine("Điện thoại : " + cn.SoDienThoai);
            Console.WriteLine("Email      : " + cn.Email);
            Console.WriteLine("Ghi chú    : " + cn.GhiChu);

            Console.WriteLine("\n--- Học vấn ---");
            foreach (HocVan hv in cn.HocVans)
                Console.WriteLine("* " + hv.Truong + " - " + hv.ChuyenNganh + " (" + hv.NamBatDau + "-" + hv.NamKetThuc + ")");

            Console.WriteLine("\n--- Kinh nghiệm làm việc ---");
            foreach (KinhNghiemLamViec kn in cn.KinhNghiems)
                Console.WriteLine("* " + kn.CongTy + " - " + kn.ViTri + " (" + kn.NamBatDau + "-" + kn.NamKetThuc + ")");

            Console.WriteLine("\n--- Kỹ năng ---");
            foreach (KyNang k in cn.KyNangs)
                Console.WriteLine("* " + k.TenKyNang + " - " + k.MucDo);

            Console.WriteLine("\n--- Sở thích ---");
            foreach (SoThich st in cn.SoThichs)
                Console.WriteLine("* " + st.TenSoThich);
        }

        static void PrintWatermark()
        {
            string text = "Dấu ấn cá nhân: Hạnh Henry (My_Profile)";
            int width = text.Length + 4;
            string border = new string('-', width);
            Console.WriteLine("+" + border + "+");
            Console.WriteLine("|  " + text + "  |");
            Console.WriteLine("+" + border + "+");
        }
    }
}
