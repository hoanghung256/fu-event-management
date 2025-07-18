using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using FUEM.Infrastructure.Common.MailSender;

namespace FUEM.Application.UseCases.UserUseCases
{
    public class ForgotPassword : IForgotPassword
    {
        private readonly IOrganizerRepository _organizerRepository;
        private readonly IStudentRepository _studentRepository;

        public ForgotPassword(IOrganizerRepository organizerRepository, IStudentRepository studentRepository)
        {
            _organizerRepository = organizerRepository;
            _studentRepository = studentRepository;
        }

        public async Task<string> RequestPasswordResetByEmailAsync(string email)
        {
            var student = await _studentRepository.GetStudentByEmailAsync(email);
            var organizer = await _organizerRepository.GetOrganizerByEmailAsync(email);
            if (student == null && organizer == null)
            {
                throw new ArgumentException("User not found.");
            }

            string otpCode = GenerateOtpCode();

            try
            {
                await MailSender.SendOTPAsync(email, otpCode);
                return otpCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to send OTP. Please try again later.", ex);
            }
        }

        private string GenerateOtpCode(int length = 6)
        {
            const string chars = "0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> ResetPasswordAsync(string email, string otpInput, string newPassword, string? otpFromSession)
        {
            Student? student = null;
            Organizer? organizer = null;

            student = await _studentRepository.GetStudentByEmailAsync(email);
            if (student == null)
            {
                organizer = await _organizerRepository.GetOrganizerByEmailAsync(email);
            }

            dynamic user = (student != null) ? student : (organizer != null ? organizer : null);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            if (string.IsNullOrEmpty(otpFromSession) || otpFromSession != otpInput)
            {
                throw new ArgumentException("OTP is invalid or expired.");
            }

            string newPasswordHash = Hasher.Hash(newPassword);
            if (Hasher.Verify(newPassword, user.Password))
            {
                throw new ArgumentException("New password must not match the old password.");
            }

            if (student != null)
            {
                await _studentRepository.UpdatePasswordHashAsync(student.Id, newPasswordHash);
            }
            else if (organizer != null)
            {
                await _organizerRepository.UpdatePasswordHashAsync(organizer.Id, newPasswordHash);
            }
            return true;
        }
    }
}
