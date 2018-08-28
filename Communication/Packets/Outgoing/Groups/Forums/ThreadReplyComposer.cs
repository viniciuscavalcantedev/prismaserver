using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Groups.Forums;

namespace Bios.Communication.Packets.Outgoing.Groups
{
	class ThreadReplyComposer : ServerPacket
    {
        public ThreadReplyComposer(GameClient Session, GroupForumThreadPost Post)
            : base(ServerPacketHeader.ThreadReplyMessageComposer)
        {
            var User = Post.GetAuthor();
			WriteInteger(Post.ParentThread.ParentForum.Id);
			WriteInteger(Post.ParentThread.Id);

			WriteInteger(Post.Id); //Post Id
			WriteInteger(Post.ParentThread.Posts.IndexOf(Post)); //Post Index

			WriteInteger(User.Id); //User id
			WriteString(User.Username); //Username
			WriteString(User.Look); //User look

			WriteInteger((int)(BiosEmuThiago.GetUnixTimestamp() - Post.Timestamp)); //User message timestamp
			WriteString(Post.Message); // Message text
			WriteByte(0); // User message oculted by - level
			WriteInteger(0); // User that oculted message ID
			WriteString(""); //Oculted message user name
			WriteInteger(10);
			WriteInteger(Post.ParentThread.GetUserPosts(User.Id).Count); //User messages count
        }
    }
}
