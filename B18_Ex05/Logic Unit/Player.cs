using B18_Ex05.Enums;

namespace B18_Ex05
{
    public class Player
    {
        public e_PlayerColor PlayerColor { get; set; }

        public e_PlayerType PlayerType { get; set; }

        public string PlayerName { get; set; }

        public int Score { get; set; }

        public static e_PlayerColor GetMyOpponentColor(e_PlayerColor i_playerColor)
        {
            e_PlayerColor opponentColor;
            if (i_playerColor == e_PlayerColor.Black)
            {
                opponentColor = e_PlayerColor.Red;
            }
            else
            {
                opponentColor = e_PlayerColor.Black;
            }

            return opponentColor;
        }

        public Player(e_PlayerColor i_PlayerColor, string i_PlayerName)
        {
            PlayerColor = i_PlayerColor;
            PlayerName = i_PlayerName;
        }

        public e_PlayerColor OpponentColor
        {
            get
            {
                if (this.PlayerColor == e_PlayerColor.Black)
                {
                    return e_PlayerColor.Red;
                }
                else
                {
                    return e_PlayerColor.Black;
                }
            }
        }
    }
}
