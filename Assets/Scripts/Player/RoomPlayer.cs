using Mirror;

namespace Player
{
    public class RoomPlayer : NetworkRoomPlayer
    {
        [SyncVar]
        public string playerName;
        [SyncVar]
        public string character;

        public bool HasValidCharacter()
        {
            return (character == "Alina" || character == "Colin");
        }

        [Command]
        public void SetPlayerName(string value)
        {
            playerName = value;
        }

        [Command]
        public void SetCharacter(string value)
        {
            // If choice is not a character, set it to blank
            if (value != "Alina" && value != "Colin")
            {
                character = "";
                return;
            }

            character = value;
        }
    }
}