using System;

namespace Cursus.MVC.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// Gets the default avatar path for a user based on their role
        /// </summary>
        /// <param name="role">User role: 1=Admin, 2=Instructor, 3=Student</param>
        /// <returns>Default avatar path</returns>
        public static string GetDefaultAvatarByRole(int role)
        {
            return role switch
            {
                1 => "/images/admin/img-1.png",           // Admin default
                2 => "/images/instructor/img-default.jpg", // Instructor default
                3 => "/images/student/img-default.jpg",    // Student default
                _ => "/images/student/img-default.jpg"     // Fallback to student default
            };
        }

        /// <summary>
        /// Gets a random avatar from available images for a specific role
        /// </summary>
        /// <param name="role">User role: 1=Admin, 2=Instructor, 3=Student</param>
        /// <returns>Random avatar path</returns>
        public static string GetRandomAvatarByRole(int role)
        {
            var random = new Random();
            
            return role switch
            {
                1 => "/images/admin/img-1.png", // Only one admin image available
                2 => $"/images/instructor/img-{random.Next(1, 8)}.jpg", // img-1.jpg to img-7.jpg
                3 => $"/images/student/img-{random.Next(1, 12)}.jpg",   // img-1.jpg to img-11.jpg
                _ => "/images/student/img-default.jpg"
            };
        }

        /// <summary>
        /// Gets the fallback image path for broken or missing images
        /// </summary>
        /// <param name="role">User role: 1=Admin, 2=Instructor, 3=Student</param>
        /// <returns>Fallback avatar path</returns>
        public static string GetFallbackAvatar(int role)
        {
            return role switch
            {
                1 => "/images/admin/img-1.png",
                2 => "/images/instructor/img-default.jpg",
                3 => "/images/student/img-default.jpg",
                _ => "/images/student/img-default.jpg"
            };
        }

        /// <summary>
        /// Validates if an avatar path exists and returns appropriate fallback if not
        /// </summary>
        /// <param name="avatarPath">Current avatar path</param>
        /// <param name="role">User role for fallback</param>
        /// <returns>Valid avatar path or fallback</returns>
        public static string ValidateAvatarPath(string avatarPath, int role)
        {
            if (string.IsNullOrEmpty(avatarPath))
            {
                return GetDefaultAvatarByRole(role);
            }
            
            // You could add file existence check here if needed
            return avatarPath;
        }
    }
}