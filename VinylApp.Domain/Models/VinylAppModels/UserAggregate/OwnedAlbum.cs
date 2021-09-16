using VinylApp.Domain.Models.CoreModels;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;

namespace VinylApp.Domain.Models.VinylAppModels.UserAggregate
{
    public class OwnedAlbum : BaseModel
    {
        public OwnedAlbum() { }

        public OwnedAlbum(AlbumItem album)
        {
            AlbumItem = album;
        }

        public virtual AlbumItem AlbumItem { get; private set; }
        public string AlbumInfo { get; private set; }
        public int Rating { get; private set; } = 1;
        
        public virtual User User { get; set; }
        public int UserId { get; set; }
        
        public void UpdateAlbumInfo(string info)
        {
            AlbumInfo = info;
        }

        public void UpdateRating(int id)
        {
            Rating = id;
        }
    }
}
