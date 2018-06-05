using B18_Ex05.Enums;

namespace B18_Ex05
{
    public class Piece
    {
        private e_CellContent m_CellContent;
        private int m_Row;
        private int m_Column;

        public e_CellContent CellContent
        {
            get
            {
                return m_CellContent;
            }

            set
            {
                m_CellContent = value;
            }
        }

        public int Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int Column
        {
            get
            {
                return m_Column;
            }

            set
            {
                m_Column = value;
            }
        }

        public Piece(e_CellContent i_CellContent, int i_Row, int i_Column)
        {
            m_CellContent = i_CellContent;
            m_Row = i_Row;
            m_Column = i_Column;
        }

        public bool CheckIfKing()
        {
            return m_CellContent == e_CellContent.BlackKing || m_CellContent == e_CellContent.RedKing;
        }

        public e_CellContent GetMyOpponentKingPieceType()
        {
            e_CellContent kingType;

            if (m_CellContent == e_CellContent.Black || m_CellContent == e_CellContent.BlackKing)
            {
                kingType = e_CellContent.RedKing;
            }
            else if (m_CellContent == e_CellContent.Red || m_CellContent == e_CellContent.RedKing)
            {
                kingType = e_CellContent.BlackKing;
            }
            else
            {
                kingType = e_CellContent.Empty;
            }

            return kingType;
        }

        public e_CellContent GetMyOpponentPlainPieceType()
        {
            e_CellContent kingType;

            if (m_CellContent == e_CellContent.Black || m_CellContent == e_CellContent.BlackKing)
            {
                kingType = e_CellContent.Red;
            }
            else if (m_CellContent == e_CellContent.Red || m_CellContent == e_CellContent.RedKing)
            {
                kingType = e_CellContent.Black;
            }
            else
            {
                kingType = e_CellContent.Empty;
            }

            return kingType;
        }
    }
}
