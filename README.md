# BÀI TẬP 01 - PHÁT TRIỂN ỨNG DỤNG TRÊN NỀN WEB
# LƯỜNG VĂN HẠNH - K225480106013
### DEADLINE 28/09/2025
## YÊU CẦU
TẠO SOLUTION GỒM CÁC PROJECT SAU:

1.DLL đa năng, keyword: c# window library -> Class Library (.NET Framework) bắt buộc sử dụng .NET Framework 2.0: giải bài toán bất kỳ, độc lạ càng tốt, phải có dấu ấn cá nhân trong kết quả, biên dịch ra DLL. DLL độc lập vì nó ko nhập, ko xuất, nó nhận input truyền vào thuộc tính của nó, và trả về dữ liệu thông qua thuộc tính khác, hoặc thông qua giá trị trả về của hàm. Nó độc lập thì sẽ sử dụng được trên app dạng console (giao diện dòng lệnh - đen sì), cũng sử dụng được trên app desktop (dạng cửa sổ), và cũng sử dụng được trên web form (web chạy qua iis).

2.Console app, bắt buộc sử dụng .NET Framework 2.0, sử dụng được DLL trên: nhập được input, gọi DLL, hiển thị kết quả, phải có dấu án cá nhân. keyword: c# window Console => Console App (.NET Framework), biên dịch ra EXE

3.Windows Form Application, bắt buộc sử dụng .NET Framework 2.0**, sử dụng được DLL đa năng trên, kéo các control vào để có thể lấy đc input, gọi DLL truyền input để lấy đc kq, hiển thị kq ra window form, phải có dấu án cá nhân; keyword: c# window Desktop => Windows Form Application (.NET Framework), biên dịch ra EXE

4.Web đơn giản, bắt buộc sử dụng .NET Framework 2.0, sử dụng web server là IIS, dùng file hosts để tự tạo domain, gắn domain này vào iis, file index.html có sử dụng html css js để xây dựng giao diện nhập được các input cho bài toán, dùng mã js để tiền xử lý dữ liệu, js để gửi lên backend. backend là api.aspx, trong code của api.aspx.cs thì lấy được các input mà js gửi lên, rồi sử dụng được DLL đa năng trên. kết quả gửi lại json cho client, js phía client sẽ nhận được json này hậu xử lý để thay đổi giao diện theo dữ liệu nhận dược, phải có dấu án cá nhân. keyword: c# window web => ASP.NET Web Application (.NET Framework) + tham khảo link chatgpt thầy gửi. project web này biên dịch ra DLL, phải kết hợp với IIS mới chạy được.
## BÀI TOÁN
Bài toán là làm 1 hệ thống lấy ra hồ sơ cá nhân của bất kỳ ai được lưu trong db.

**Em nhập thông tin cá nhân của em vào db. Em khảng định là không ai biết hết các thông tin này. Đảm bảo tính cái nhân tuyệt đối.**

Db gồm các thông tin về cá nhân, học vấn, kinh nghiệm làm việc, sở thích.

-Tạo 1 dll để lấy ra tất cả thông tin hồ sơ của 1 người. 

-Dùng dll đó để triển khai trên console app.

-Dùng dll đó để triển khai trên windows form app.

-Dùng dll đó để triển khai trên web form app.
## GIẢI QUYẾT BÀI TOÁN
### a.Xây dưng db
<img width="1119" height="815" alt="image" src="https://github.com/user-attachments/assets/00166088-2814-49bc-a5d7-4a589fac4a64" />

### b.Tạo dll
<img width="960" height="540" alt="dll_1" src="https://github.com/user-attachments/assets/0dc0f229-e484-4132-8e9c-42a120de4e49" />

<img width="960" height="539" alt="dll_2" src="https://github.com/user-attachments/assets/7f283ad9-f2c0-4249-b488-cf493be41beb" />

### c.Console app
<img width="960" height="530" alt="console1" src="https://github.com/user-attachments/assets/d7b27340-a504-4eee-a357-2f7aef0ad22e" />

Do app không nhận được tiếng việt từ input trên console nên chỉ có thể tìm theo tên không dấu.
<img width="1919" height="1005" alt="image" src="https://github.com/user-attachments/assets/863f0d56-40a4-45e4-891b-d9e3a518061c" />

### d.Windows form app
Em đang dùng bản Visual studio 2022 nên không có template Windows form app(.NET Framework 2.0). Em đã tạo thủ công và dồn code trong 1 file.
<img width="1894" height="1012" alt="image" src="https://github.com/user-attachments/assets/2e4cd043-6c07-4687-b6ea-ba2b01612be9" />

### e.Web form app
<img width="960" height="540" alt="web" src="https://github.com/user-attachments/assets/7a0b8a7d-1aac-40d6-87e3-43b8fff6f9b0" />
