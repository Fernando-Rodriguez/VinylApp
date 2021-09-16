using System.Collections.Generic;
using System.Linq;
using VinylApp.Domain.Models.CoreModels;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;
using VinylApp.Domain.Models.VinylAppModels.GroupAggregate;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate.Auth;

namespace VinylApp.Domain.Models.VinylAppModels.UserAggregate
{
    public class User : BaseModel
    {
        public User() { }

        public User(string screenName, string userName, string userPass, string generalInfo)
        {
            ScreenName = screenName;
            UserAuthorization = new AuthUser(userName, userPass);
            UserInfo = generalInfo;
        }

        public string ScreenName { get; private set; }
        public virtual AuthUser UserAuthorization { get; private set; }
        public virtual List<OwnedAlbum> Albums { get; private set; } = new();
        public virtual List<Group> Groups { get; private set; } = new ();
        public string UserInfo { get; private set; } = "";

        public void UpdateScreenName(string screenName)
        {
            this.ScreenName = screenName;
        }

        public void UpdateUserName(string newUserName)
        {
            this.UserAuthorization.UserName = newUserName;
        }

        public void UpdateUserPass(string userPass)
        {
            UserAuthorization.UserPass = userPass;
        }

        public void UpdateUserInfo(string info)
        {
            UserInfo = info;
        }

        public void AddAlbum(AlbumItem newAlbum)
        {
            var newOwnedAlbum = new OwnedAlbum(newAlbum);
            Albums.Add(newOwnedAlbum);
        }

        public void RemoveAlbum(int id)
        {
            var albumToDelete = Albums.Find(a => a.Id == id);
            Albums.Remove(albumToDelete);
        }

        public List<Group> GetMyGroups()
        {
            return Groups;
        }

        public Group GetMyGroupbyId(int id)
        {
            return Groups.FirstOrDefault(g => g.Id == id);
        }

        public void AddGroup(Group group)
        {
            Groups.Add(group);
        }

        public List<AlbumItem> GetCoreAlbumInfo()
        {
            return Albums.Select(x => x.AlbumItem).ToList();
        }

        public OwnedAlbum FindAlbumWithId(int id)
        {
            return Albums.FirstOrDefault(a => a.Id == id);
        }
    }
}