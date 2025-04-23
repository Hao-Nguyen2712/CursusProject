using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.Admin
{
    public interface IAdminRepository
    {
        void BlockAccount(int accountId);
        void UnBlockAccount(int accountId);
        List<Domain.Models.Account> getListAccount();

        void ApproveCourse(int courseId);
        void RejectCourse(int courseId);
        
        List<Domain.Models.Course> getCourseApprove();

        void ApproveInstructorRegis(int accountId);
        void RejectInstructorRegis(int accountId);

        List<Domain.Models.Account> getListInstructorRegis();
        



    }   
}