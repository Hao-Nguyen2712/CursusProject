using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.Admin
{
    public class AdminService : IAdminService
    {
         private readonly IAdminRepository adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }
        public void BlockAccount(int accountId)
        {
            adminRepository.BlockAccount(accountId);
        }

        List<Domain.Models.Account> IAdminService.getListAccount()
        {
            return this.adminRepository.getListAccount();
        }
        public void UnBlockAccount(int accountId)
        {
            adminRepository.UnBlockAccount(accountId);
        }



        public void ApproveCourse(int courseId)
        {
            adminRepository.ApproveCourse(courseId);
        }

        public void RejectCourse(int courseId)
        {
            adminRepository.RejectCourse(courseId);
        }

        List<Domain.Models.Course> IAdminService.getCourseApprove()
        {
            return this.adminRepository.getCourseApprove();
        }


        public void ApproveInstructorRegis(int accountId)
        {
            adminRepository.ApproveInstructorRegis(accountId);
        }

        public void RejectInstructorRegis(int accountId)
        {
            adminRepository.RejectInstructorRegis(accountId);
        }

        List<Domain.Models.Account> IAdminService.getListInstructorRegis()
        {
            return this.adminRepository.getListInstructorRegis();
        }
    }
}