# Project Cursus: Bối cảnh và Lịch sử Nhiệm vụ

Tài liệu này dùng để ghi lại bối cảnh kỹ thuật và các quyết định đã được### **Cloudinary Integration**: M### Đã hoàn thành
- ✅ **Database & Migrations**: Hệ thống database Code-First hoạt động ổn định với Entity Framework Core 8.
- ✅ **Identity System**: ASP.NET Core Identity được cấu hình đúng với `ApplicationUser`.
- ✅ **Database Seeding**: DatabaseSeeder tạo đầy đủ dữ liệu mẫu bao gồm admin, instructors, students, courses.
- ✅ **Authentication**: Admin login hoạt động với credentials (`admin@cursus.com` / `TempPassword123!`).
- ✅ **Course Display**: Courses hiển thị chính xác trên home page sau khi sửa status mismatch.
- ✅ **Dark Mode Infrastructure**: CSS và JavaScript cho dark mode đã sẵn sàng.
- ✅ **Image Upload System**: Cloudinary integration hoàn tất với auto-optimization và smart cropping.
- ✅ **Admin System Monitoring**: Dashboard analytics với enrollment stats, completion rates, user demographics.
- ✅ **Admin Content Moderation**: Comment approval workflow và report management system. from Azure Blob Storage to Cloudinary for image upload functionality.
- ⚠️ **Production Settings**: Cần review và cấu hình các settings cho production environment.ưa ra trong dự án Cursus. Vui lòng cung cấp tài liệu này làm bối cảnh cho các nhiệm vụ trong tương lai để đảm bảo tính nhất quán.

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

### Nhiệm vụ: Giải quyết lỗi IDENTITY_INSERT và thiết lập Database Seeding

Mục tiêu là sửa lỗi seeding dữ liệu và thiết lập hệ thống database seeding hoạt động ổn định.

-   [x] **Phân tích lỗi IDENTITY_INSERT**: Xác định nguyên nhân lỗi do việc chỉ định ID tường minh cho các bảng có IDENTITY column.
-   [x] **Sửa DatabaseSeeder.cs**:
    -   Loại bỏ các ID tường minh khỏi việc tạo Categories, Accounts, Courses, và các entities khác.
    -   Cập nhật logic để sử dụng auto-generated IDs từ database.
    -   Sửa thứ tự seeding để đảm bảo dependencies được tạo trước.
-   [x] **Cấu hình Identity chính xác**:
    -   Đảm bảo `Program.cs` sử dụng `ApplicationUser` thay vì `CursusMVCUser`.
    -   Cập nhật tất cả Controllers để sử dụng `ApplicationUser`.
    -   Cập nhật tất cả Identity Pages để sử dụng `ApplicationUser`.
-   [x] **Sửa xung đột Identity trong Controllers**:
    -   Cập nhật `CartController.cs`, `AdminController.cs`, `CertificateController.cs`.
    -   Thay đổi `using Cursus.MVC.Areas.Identity.Data` thành `using Cursus.Domain.Models`.
    -   Cập nhật `UserManager<CursusMVCUser>` và `SignInManager<CursusMVCUser>` thành `ApplicationUser`.
-   [x] **Cập nhật Layout Files**:
    -   Sửa `_LoginPartial.cshtml`, `_LayoutStudent.cshtml`, `_LayoutInstructor.cshtml`, `_LayoutAdmin.cshtml`.
    -   Cập nhật dependency injection để sử dụng `ApplicationUser`.
-   [x] **Tạo database thành công**: Database được tạo và seeded với dữ liệu mẫu bao gồm admin, instructors, students, courses, và categories.

### Nhiệm vụ: Điều tra và Sửa lỗi hiển thị Courses

Mục tiêu là tìm hiểu tại sao không có courses nào hiển thị trên home page.

-   [x] **Phân tích logic hiển thị courses**: Xác định HomeController sử dụng HomePageService để lấy dữ liệu courses.
-   [x] **Phát hiện mismatch CourseStatus**: DatabaseSeeder tạo courses với status "Active" nhưng HomePageService filter theo status "Approved".
-   [x] **Sửa DatabaseSeeder**: Cập nhật tất cả courses từ `CourseStatus = "Active"` thành `CourseStatus = "Approved"`.
-   [x] **Kiểm tra course seeding logic**: Xác nhận DatabaseSeeder tạo 5 courses với các categories và instructors tương ứng.

### Nhiệm vụ: Xử lý Admin Account và Seeding System

Mục tiêu là làm rõ hệ thống seeding và đảm bảo admin account có thể đăng nhập.

-   [x] **Phân tích conflict seeding systems**: Xác định có 2 hệ thống seeding song song:
    -   `AccountSeed.cs` + `DataSeeder.cs` (không được sử dụng)
    -   `DatabaseSeeder.cs` (đang hoạt động)
-   [x] **Làm rõ admin credentials**: Admin account được tạo bởi DatabaseSeeder với:
    -   Email: `admin@cursus.com`
    -   Password: `TempPassword123!`
-   [x] **Dọn dẹp unused files**: Xóa các file seeding không sử dụng:
    -   Xóa `AccountSeed.cs`, `AspNetUserSeed.cs`, `CartSeed.cs`, `CategorySeed.cs`, `CommentSeed.cs`, `CourseSeed.cs`, `EnrollSeed.cs`, `LessonSeed.cs`, `ProgressSeed.cs`, `RateSeed.cs`, `TradingSeed.cs`.
    -   Xóa `DataSeeder.cs`.

### Nhiệm vụ: Điều tra Dark Mode và UI Issues

Mục tiêu là tìm hiểu tại sao dark mode không hoạt động và sửa các UI issues.

-   [x] **Phân tích dark mode infrastructure**: Xác định có đầy đủ CSS (`night-mode.css`) và JavaScript (`night-mode.js`).
-   [x] **Xác nhận dark mode toggle tồn tại**: Tìm thấy toggle button với `id="night-mode"` đã có trong layout files.
-   [x] **Sửa lỗi hiển thị năm**: Cập nhật `_LayoutDashboard.cshtml` từ `{DateTime.Now.Year}` thành `@DateTime.Now.Year`.
-   [x] **Cập nhật Identity references**: Sửa `_LayoutDashboard.cshtml` từ `CursusMVCUser` thành `ApplicationUser`.

### Nhiệm vụ: Phân tích Image Upload System

Mục tiêu là tìm hiểu hệ thống upload hình ảnh cho instructors.

-   [x] **Mapping upload endpoints**: Xác định 3 endpoints chính:
    -   `/Instructor/UploadImage` - Course thumbnails (line 270)
    -   `/Instructor/UploadAvatar` - Instructor avatars (line 537)  
    -   `/Instructor/UploadAvatarStudent` - Student avatars (line 569)
-   [x] **Phân tích frontend integration**: 
    -   `addCourse.js` - Handles course image uploads
    -   `validateImage.js` - Handles avatar uploads with validation
-   [x] **Xác định configuration issue**: `blobStorageConnectionString` đang để trống, cần cấu hình Azure Blob Storage.

### Nhiệm vụ: Migration từ Azure Blob Storage sang Cloudinary

Mục tiêu là chuyển đổi hệ thống upload từ Azure Blob Storage sang Cloudinary để có performance và features tốt hơn.

-   [x] **Phân tích Azure Blob implementation**: Review code hiện tại trong InstructorController.cs để hiểu flow upload.
-   [x] **Cập nhật project dependencies**: Thay `Azure.Storage.Blobs` bằng `CloudinaryDotNet` package.
-   [x] **Cập nhật configuration**: Thêm Cloudinary settings vào appsettings.json.
-   [x] **Refactor InstructorController**: 
    -   Thay Azure BlobContainerClient bằng Cloudinary SDK
    -   Cập nhật 3 upload endpoints với Cloudinary upload logic
    -   Thêm image transformations (resizing, optimization)
    -   Implement proper error handling
-   [x] **Dependency Injection setup**: Register Cloudinary service trong Program.cs.

### Nhiệm vụ: Triển khai System Monitoring và Content Moderation cho Admin Dashboard

Mục tiêu là phát triển hệ thống giám sát hệ thống và quản lý nội dung cho admin dashboard.

-   [x] **Phân tích yêu cầu**: Xác định cần thiết kế System Monitoring analytics và Content Moderation workflow.
-   [x] **Tạo View Models**:
    -   Tạo `SystemMonitoringViewModel.cs` với metrics về enrollment, completion rates, user analytics
    -   Tạo `ContentModerationViewModel.cs` với pending comments và report management
    -   Thêm nested view models cho charts data và monthly trends
-   [x] **Mở rộng Service Layer**:
    -   Thêm `GetAllEnrolls()` method vào `IEnrollService` và `EnrollService`
    -   Thêm `GetAllComments()` method vào `ICommentService` và `CommentService`
    -   Cập nhật Application layer với new service methods
-   [x] **Cập nhật AdminController**:
    -   Thêm `SystemMonitoring()` action với comprehensive analytics calculations
    -   Thêm `ContentModeration()` action với comment approval workflow
    -   Implement null safety checks và error handling
-   [x] **Sửa Dependency Injection Issues**:
    -   Xác định và sửa duplicate `_accountService` field declarations
    -   Sửa constructor parameter assignments từ `_accountService` sang `accountService`
    -   Loại bỏ tất cả references đến undefined `_accountService` field
-   [x] **Testing và Validation**:
    -   Build successful với 0 errors
    -   Application runs correctly trên `http://localhost:5205`
    -   Database operations hoạt động bình thường
    -   Admin dashboard features accessible và functional

## 4. Tình trạng Hệ thống Hiện tại

### Đã hoàn thành
- ✅ **Database & Migrations**: Hệ thống database Code-First hoạt động ổn định với Entity Framework Core 8.
- ✅ **Identity System**: ASP.NET Core Identity được cấu hình đúng với `ApplicationUser`.
- ✅ **Database Seeding**: DatabaseSeeder tạo đầy đủ dữ liệu mẫu bao gồm admin, instructors, students, courses.
- ✅ **Authentication**: Admin login hoạt động với credentials (`admin@cursus.com` / `TempPassword123!`).
- ✅ **Course Display**: Courses hiển thị chính xác trên home page sau khi sửa status mismatch.
- ✅ **Dark Mode Infrastructure**: CSS và JavaScript cho dark mode đã sẵn sàng.

### Cần cấu hình
- ⚠️ **Cloudinary Configuration**: Cần cấu hình credentials cho image upload functionality.
- ⚠️ **Production Settings**: Cần review và cấu hình các settings cho production environment.

### Đã dọn dẹp
- 🗑️ **Unused Seeding Files**: Đã xóa các file seeding trùng lặp không sử dụng.
- 🗑️ **Legacy Identity References**: Đã cập nhật toàn bộ từ `CursusMVCUser` sang `ApplicationUser`.

## 5. Các Quyết định Kỹ thuật Quan trọng

### Database Seeding Strategy
- **Quyết định**: Sử dụng service-based seeding (`DatabaseSeeder.cs`) thay vì static seeding (`DataSeeder.cs`).
- **Lý do**: Service-based seeding cho phép dependency injection và xử lý Identity users tốt hơn.
- **Implementation**: Seeding được thực hiện trong `Program.cs` khi environment là Development.

### Identity Configuration
- **Quyết định**: Sử dụng `ApplicationUser` trong `Cursus.Domain` làm Identity user chính.
- **Lý do**: Tránh circular references và tuân theo clean architecture.
- **Implementation**: Tất cả controllers, pages, và layouts đã được cập nhật.

### Course Status Management
- **Quyết định**: Sử dụng "Approved" status cho courses hiển thị public.
- **Lý do**: Phân biệt giữa courses đã được duyệt và courses đang chờ duyệt.
- **Implementation**: DatabaseSeeder tạo courses với status "Approved".

## 6. Cấu trúc File Upload System (Cloudinary)

### Endpoints
```csharp
POST /Instructor/UploadImage        // Course thumbnails (800x600, optimized)
POST /Instructor/UploadAvatar       // Instructor profile pictures (300x300, face-cropped)
POST /Instructor/UploadAvatarStudent // Student profile pictures (300x300, face-cropped)
```

### Frontend Integration
```javascript
// addCourse.js - Course image uploads
// validateImage.js - Avatar uploads with client-side validation
```

### Cloudinary Configuration
```json
{
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key", 
    "ApiSecret": "your-api-secret"
  }
}
```

### Upload Features
- **Organized Folders**: `cursus/course_images/`, `cursus/instructor_avatars/`, `cursus/student_avatars/`
- **Auto Transformations**: Automatic resizing and optimization
- **Face Detection**: Smart cropping for avatars using gravity="face"
- **Format Optimization**: Auto quality and format selection
- **Unique URLs**: GUID-based public IDs to prevent conflicts

## 7. Hướng dẫn cho Developers

### Admin Login
- **URL**: `/Identity/Account/Login`
- **Email**: `admin@cursus.com`
- **Password**: `TempPassword123!`

### Admin Dashboard Features
- **System Monitoring**: `/Admin/SystemMonitoring` - Analytics dashboard với enrollment metrics, completion rates, user demographics
- **Content Moderation**: `/Admin/ContentModeration` - Comment approval workflow và report management
- **User Management**: Existing account management và instructor approval workflows

### Database Reset
```bash
cd Cursus.MVC
dotnet ef database drop --force
dotnet ef database update
```

### Dark Mode Toggle
- Toggle được tích hợp trong profile dropdown của các layout files
- JavaScript tự động lưu preference trong localStorage
- CSS theme được áp dụng thông qua `.night-mode` class

### Cloudinary Configuration
Để enable image upload với Cloudinary, cần cấu hình credentials trong `appsettings.json`:
```json
{
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

### Cloudinary Features Được Triển Khai
- **Smart Cropping**: Avatar images sử dụng face detection
- **Auto Optimization**: Tự động tối ưu chất lượng và format
- **Responsive Images**: Có thể tạo multiple sizes từ một upload
- **Folder Organization**: Images được organize theo folders (course_images, instructor_avatars, student_avatars)
- **Secure URLs**: Tất cả URLs đều sử dụng HTTPS

### System Monitoring Features
- **Real-time Analytics**: Total users, enrollments, completion rates
- **Course Statistics**: Performance metrics và completion tracking
- **User Demographics**: User distribution và activity patterns
- **Monthly Trends**: Enrollment trends với visual charts
- **Performance Metrics**: System health và usage statistics

### Content Moderation Workflow
- **Pending Comments**: Review và approve/reject user comments
- **Bulk Operations**: Mass approval/rejection capabilities
- **Report Management**: Handle user-generated reports
- **Moderation History**: Track moderation actions và approval rates
- **Administrative Controls**: Comprehensive content oversight tools
