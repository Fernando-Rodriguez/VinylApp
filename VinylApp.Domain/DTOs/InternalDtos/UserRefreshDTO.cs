namespace VinylApp.Domain.DTOs.InternalDTOs
{
    public class UserRefreshDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}