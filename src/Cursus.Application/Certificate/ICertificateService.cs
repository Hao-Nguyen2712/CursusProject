using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursus.Domain;

namespace Cursus.Application.Certificate
{
    public interface ICertificateService
    {
        //public Account GenerateCertificate(Account account, Course course, string outputPath);
        public void GenerateCertificateToPDF(string fullName, string courseName, string outputPath);
    }
}
