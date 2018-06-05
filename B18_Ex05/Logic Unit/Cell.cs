using B18_Ex05.Enums;

namespace B18_Ex05
{
    public class Cell
    {
        public e_CellContent CellContent { get; set; }

        public bool IsBlackPiece()
        {
            return CellContent == e_CellContent.Black || CellContent == e_CellContent.BlackKing;
        }

        public bool IsRedPiece()
        {
            return CellContent == e_CellContent.Red || CellContent == e_CellContent.RedKing;
        }

        public void RemovePiece()
        {
            CellContent = e_CellContent.Empty;
        }
    }
}