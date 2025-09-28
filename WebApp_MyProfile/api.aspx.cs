using My_Profile;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;
using Newtonsoft.Json;

namespace WebApp_MyProfile
{
    public partial class api : System.Web.UI.Page
    {
        // Nếu dùng SQL Server Express thay .\SQLEXPRESS:
        // static string connectionString = "Server=.\\SQLEXPRESS;Database=My_Profile;Trusted_Connection=True;MultipleActiveResultSets=True;";
        static string connectionString = "Server=localhost;Database=My_Profile;User Id=sa;Password=20042002;MultipleActiveResultSets=True;";


        protected void Page_Load(object sender, EventArgs e)
        {
            // Nếu gọi qua query string thì xử lý ở đây
            if (!IsPostBack && Request.QueryString["HoTen"] != null)
            {
                string hoTen = Request.QueryString["HoTen"];

                var result = GetProfile(hoTen);

                // Trả JSON thẳng về browser
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(JsonConvert.SerializeObject(result, Formatting.Indented));
                Response.End();
            }
        }

        [WebMethod]
        public static object GetProfile(string HoTen)
        {
            try
            {
                if (HoTen == null || HoTen.Trim() == "")
                    return new { error = "Tham số HoTen là bắt buộc" };

                var result = TraCuu(HoTen);
                if (result == null)
                    return new { error = "Không tìm thấy thông tin" };

                return result;
            }
            catch (Exception ex)
            {
                return new
                {
                    error = "Lỗi phía server",
                    message = ex.Message
                };
            }
        }

        private static ThongTinCaNhan TraCuu(string hoTen)
        {
            ThongTinCaNhan person = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Lấy thông tin cá nhân (chỉ select cột cần thiết)
                string sqlCaNhan = @"SELECT Id, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, GhiChu 
                                    FROM ThongTinCaNhan WHERE HoTen = @HoTen OR TenKhongDau = @HoTen";
                using (SqlCommand cmd = new SqlCommand(sqlCaNhan, conn))
                {
                    cmd.Parameters.AddWithValue("@HoTen", hoTen);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            person = new ThongTinCaNhan
                            {
                                Id = GetInt(reader, "Id") ?? 0,
                                HoTen = GetString(reader, "HoTen"),
                                NgaySinh = GetDate(reader, "NgaySinh"),
                                GioiTinh = GetString(reader, "GioiTinh"),
                                DiaChi = GetString(reader, "DiaChi"),
                                SoDienThoai = GetString(reader, "SoDienThoai"),
                                Email = GetString(reader, "Email"),
                                GhiChu = GetString(reader, "GhiChu"),

                                HocVans = new List<HocVan>(),
                                KinhNghiems = new List<KinhNghiemLamViec>(),
                                KyNangs = new List<KyNang>(),
                                SoThichs = new List<SoThich>()
                            };
                        }
                    }
                }

                if (person == null) return null;

                // 2. Học vấn
                string sqlHV = @"SELECT Id, IdCaNhan, Truong, ChuyenNganh, BangCap, NamBatDau, NamKetThuc 
                                 FROM HocVan WHERE IdCaNhan=@Id";
                using (SqlCommand cmd = new SqlCommand(sqlHV, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", person.Id);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            person.HocVans.Add(new HocVan
                            {
                                Id = GetInt(r, "Id") ?? 0,
                                IdCaNhan = GetInt(r, "IdCaNhan") ?? person.Id,
                                Truong = GetString(r, "Truong"),
                                ChuyenNganh = GetString(r, "ChuyenNganh"),
                                BangCap = GetString(r, "BangCap"),
                                NamBatDau = GetInt(r, "NamBatDau"),
                                NamKetThuc = GetInt(r, "NamKetThuc")
                            });
                        }
                    }
                }

                // 3. Kinh nghiệm
                string sqlKN = @"SELECT Id, IdCaNhan, CongTy, ViTri, MoTa, NamBatDau, NamKetThuc 
                                 FROM KinhNghiemLamViec WHERE IdCaNhan=@Id";
                using (SqlCommand cmd = new SqlCommand(sqlKN, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", person.Id);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            person.KinhNghiems.Add(new KinhNghiemLamViec
                            {
                                Id = GetInt(r, "Id") ?? 0,
                                IdCaNhan = GetInt(r, "IdCaNhan") ?? person.Id,
                                CongTy = GetString(r, "CongTy"),
                                ViTri = GetString(r, "ViTri"),
                                MoTa = GetString(r, "MoTa"),
                                NamBatDau = GetInt(r, "NamBatDau"),
                                NamKetThuc = GetInt(r, "NamKetThuc")
                            });
                        }
                    }
                }

                // 4. Kỹ năng
                string sqlKNang = @"SELECT Id, IdCaNhan, TenKyNang, MucDo FROM KyNang WHERE IdCaNhan=@Id";
                using (SqlCommand cmd = new SqlCommand(sqlKNang, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", person.Id);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            person.KyNangs.Add(new KyNang
                            {
                                Id = GetInt(r, "Id") ?? 0,
                                IdCaNhan = GetInt(r, "IdCaNhan") ?? person.Id,
                                TenKyNang = GetString(r, "TenKyNang"),
                                MucDo = GetString(r, "MucDo")
                            });
                        }
                    }
                }

                // 5. Sở thích
                string sqlST = @"SELECT Id, IdCaNhan, TenSoThich FROM SoThich WHERE IdCaNhan=@Id";
                using (SqlCommand cmd = new SqlCommand(sqlST, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", person.Id);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            person.SoThichs.Add(new SoThich
                            {
                                Id = GetInt(r, "Id") ?? 0,
                                IdCaNhan = GetInt(r, "IdCaNhan") ?? person.Id,
                                TenSoThich = GetString(r, "TenSoThich")
                            });
                        }
                    }
                }
            }

            return person;
        }

        // Helper: đọc an toàn các cột có thể là DBNull
        private static string GetString(SqlDataReader r, string col)
        {
            if (r == null || string.IsNullOrEmpty(col)) return null;
            var val = r[col];
            return val == DBNull.Value ? null : val.ToString();
        }

        private static int? GetInt(SqlDataReader r, string col)
        {
            if (r == null || string.IsNullOrEmpty(col)) return null;
            var val = r[col];
            return val == DBNull.Value ? (int?)null : Convert.ToInt32(val);
        }

        private static DateTime? GetDate(SqlDataReader r, string col)
        {
            if (r == null || string.IsNullOrEmpty(col)) return null;
            var val = r[col];
            return val == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(val);
        }
    }
}
