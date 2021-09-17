using System;
using VinylApp.Domain.Models.CoreModels;

namespace VinylApp.Domain.Models.VinylAppModels.AlbumAggregate
{
    public class AlbumItem : BaseModel
    {
        public AlbumItem() { }

        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumArtworkUrl { get; set; }

        public void UpdateAlbumName(string name)
        {
            AlbumName = name;
        }

        public void UpdateArtistName(string name)
        {
            ArtistName = name;
        }

        public void UpdateArtworkUrl(string url)
        {
            AlbumArtworkUrl = url;
        }
    }
}
