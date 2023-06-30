namespace RoomsApiCrud.Models
{
    public class RoomsApiCrudUser
    {
        public int Id { get; set;} = -1;
        public string UserName { get; set;} 
        public string UserFamilyName { get; set;}
        public string UserSecondFamilyName { get; set;}
        public string NormalizedUserName { get; set;}
        public string Email {get; set;}
        public string NormalizedEmail {get; set;}
        public bool EmailConfirmed {get; set;} = false;
        public string PasswordHash {get; set;}
        public string PhoneNumber {get; set;}
        public bool PhoneNumberConfirmed {get; set;} = false;
        public bool TwoFactorEnabled {get; set;} = false;
    }
}