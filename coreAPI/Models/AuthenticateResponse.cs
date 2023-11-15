using coreAPI.Data;
using System;

namespace coreAPI.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Clock { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Token { get; set; }

        public AuthenticateResponse(User entityUser , string token)
        {
            Id = entityUser.UsrId;
            FirstName = entityUser.UsrFirstName;
            LastName = entityUser.UsrLastName;
            Username = entityUser.UsrLogin;
            Clock = entityUser.UsrClock;
            Email = entityUser.UsrEmail;
            CreatedBy = entityUser.UsrCreatedBy;
            CreateDate = entityUser.UsrCreatedDate;
            IsActive = (bool)entityUser.UsrActive;
            ModifiedBy = entityUser.UsrModifiedBy;
            UpdateDate = entityUser.UsrModifiedDate;
            Token = token;
        }
    }
}
