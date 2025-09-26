using PuppeteerSharp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Cursus.Application.Certificate
{
    public class CertificateService : ICertificateService
    {
        private static readonly Dictionary<string, bool> screenshotTaken = new Dictionary<string, bool>();

        // Server-side PDF generation method removed - using client-side PDF generation only
        /*public async void GenerateCertificateToPDF(string fullName, string courseName, string outputPath)
        {
            // Cấu hình giấy phép QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            string htmlContent = $@"
    <html>
      <head>
        <meta charset=""UTF-8"" />
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
        <title>Document</title>
        <link rel=""preconnect"" href=""https://fonts.googleapis.com"" />
        <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin />
        <link href=""https://fonts.googleapis.com/css2?family=Alex+Brush&display=swap"" rel=""stylesheet"" />
        <style>
          .container {{
            position: relative;
            width: 1080px;
            height: 720px;
          }}
          .image {{
            position: relative;
          }}
          .image img {{
            width: 100%;
            height: 100%;
          }}
          .name {{
            position: absolute;
            top: 40%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-size: 63px;
            color: #000;
            font-family: 'Alex Brush', cursive;
          }}
          .course {{
            position: absolute;
            top: 49%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-size: 38px;
            color: #000;
            font-family: 'Poppins', cursive;
            font-weight: 300;
          }}
          p {{
            width: 100%;
            text-align: center;
          }}
        </style>
      </head>
      <body>
        <div class='container'>
          <div class='image'>
            <img src='https://i.imgur.com/0JYjkDb.png' alt='' />
          </div>
          <div>
            <span class=""name""> {fullName} </span>
            <p class=""course"">{courseName}</p>
          </div>
        </div>
      </body>
    </html>";

            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            // Server-side PDF generation removed - using client-side only

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Item().Image(imagePath);
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Certificate generated on ").FontSize(12);
                            text.Span($"{DateTime.Now:f}").FontSize(12);
                        });
                });
            })
            //.ShowInPreviewer();
            .GeneratePdf(Path.Combine(downloadsPath, outputPath));

            // Xóa hình ảnh sau khi tạo PDF
            // if (File.Exists(imagePath))
            // {
            //     File.Delete(imagePath);
            // }
        }*/

        // GenerateImageFromHtml method removed - using client-side PDF generation only
    }
}




