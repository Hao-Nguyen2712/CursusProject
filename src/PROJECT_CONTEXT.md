# Project Cursus: Bối cảnh và Lịch sử Nhiệm vụ

Tài liệu này dùng để ghi lại bối cảnh kỹ thuật và các quyết định đã được đưa ra trong dự án Cursus. Vui lòng cung cấp tài liệu này làm bối cảnh cho các nhiệm vụ trong tương lai để đảm bảo tính nhất quán.

## 1. Kiến trúc Dự án

Dự án được xây dựng theo kiến trúc phân lớp (Layered Architecture) rõ ràng:

-   **`Cursus.Domain`**: Lớp lõi, chứa các thực thể (Entities), `DbContext`, và các mô hình không phụ thuộc vào các lớp khác. Lớp này cũng chứa định nghĩa người dùng (`ApplicationUser`) cho ASP.NET Identity.
-   **`Cursus.Application`**: Lớp nghiệp vụ, định nghĩa các trường hợp sử dụng (use cases) thông qua các interface của Service.
-   **`Cursus.Infrastructure`**: Lớp cơ sở hạ tầng, triển khai logic truy cập dữ liệu (Repositories) bằng Entity Framework Core và tương tác với các dịch vụ bên ngoài.
-   **`Cursus.MVC`**: Lớp trình diễn (UI), chịu trách nhiệm xử lý yêu cầu HTTP và hiển thị giao diện người dùng.

## 2. Công nghệ chính

-   **Framework**: .NET 8
-   **Web**: ASP.NET Core MVC
-   **ORM**: Entity Framework Core 8
-   **Authentication**: ASP.NET Core Identity

## 3. Lịch sử Nhiệm vụ

### Nhiệm vụ: Chuyển đổi từ Database-First sang Code-First với Migrations

Mục tiêu là cho phép ứng dụng tự động tạo các bảng cơ sở dữ liệu (bao gồm cả các bảng Identity) khi khởi động.

-   [x] **Phân tích kiến trúc ban đầu**: Hoàn thành việc phân tích 4 lớp của dự án.
-   [x] **Hợp nhất DbContext**:
    -   Xác định `CursusDBContext` là `DbContext` chính.
    -   Chỉnh sửa `CursusDBContext` để kế thừa từ `IdentityDbContext<ApplicationUser>`.
-   [x] **Giải quyết tham chiếu vòng tròn**:
    -   Di chuyển lớp người dùng (`CursusMVCUser`, đổi tên thành `ApplicationUser`) từ `Cursus.MVC` sang `Cursus.Domain`.
    -   Thêm các gói NuGet `Microsoft.AspNetCore.Identity.EntityFrameworkCore` vào `Cursus.Domain`.
-   [x] **Cập nhật phiên bản gói**: Nâng cấp các gói `Microsoft.EntityFrameworkCore.*` từ `5.0.17` lên `8.0.0` trong `Cursus.Domain` để phù hợp với TargetFramework `net8.0`.
-   [x] **Cập nhật cấu hình khởi động**: Sửa đổi `Program.cs` để đăng ký và sử dụng `CursusDBContext` và `ApplicationUser` đã được cập nhật.
-   [x] **Sửa lỗi Primary Key cho Identity**: Cập nhật `OnModelCreating` trong `DbContext` để `Ignore()` các lớp model Identity cũ từ database-first, giải quyết xung đột với `IdentityDbContext`.
-   [x] **Sửa lỗi Primary Key cho Model ứng dụng**:
    -   Xác định thực thể `Commnent` thiếu khóa chính.
    -   Sửa lỗi chính tả thành `Comment`.
    -   Thêm `[Key]` attribute vào thuộc tính `CmtId`.
    -   Cập nhật `DbSet` trong `DbContext`.
-   [x] **Sửa cảnh báo chất lượng mã**: Sửa cảnh báo `CA2200` trong `CartRepository.cs` bằng cách thay `throw ex;` thành `throw;`.
-   [x] **Hướng dẫn tạo Migration**: Cung cấp lệnh `dotnet ef migrations add InitialCreate --context CursusDBContext` để người dùng chạy.
