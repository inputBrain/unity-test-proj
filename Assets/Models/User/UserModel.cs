namespace Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public TotalResourceModel TotalResourceModel { get; set; }
    }
}