namespace VinylApp.Domain.DTOs.ExternalDTOs
{
    public class OwnedAlbumDTO
    {
        public int Id { get; set; }
        public virtual AlbumDTO Album{ get; set; }
        public string AlbumInfo { get; set; }
        public int Rating { get; private set; } = 1;
        public int UserId { get; set; }
    }
}