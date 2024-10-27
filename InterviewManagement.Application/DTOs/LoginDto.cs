namespace InterviewManagement.Application.DTOs
{
    public class LoginDto
    {
        public string Identifier { get; set; } // Can be Email or Username
        public string Password { get; set; }
    }
}
