using System;

namespace B18_Ex05
{
    public class Move
    {
        public int FromRow { get; set; }

        public int FromColumn { get; set; }

        public int ToRow { get; set; }

        public int ToColumn { get; set; }

        public static bool operator ==(Move i_Move1, Move i_Move2)
        {
            return i_Move1.FromColumn == i_Move2.FromColumn
                && i_Move1.FromRow == i_Move2.FromRow
                && i_Move1.ToColumn == i_Move2.ToColumn
                && i_Move1.ToRow == i_Move2.ToRow;
        }

        public static bool operator !=(Move i_Move1, Move i_Move2)
        {
            return i_Move1.FromColumn != i_Move2.FromColumn
                || i_Move1.FromRow != i_Move2.FromRow
                || i_Move1.ToColumn != i_Move2.ToColumn
                || i_Move1.ToRow != i_Move2.ToRow;
        }

        public static bool TryParse(string i_Move, int i_BoardDemension, out Move o_Move)
        {
            o_Move = new Move();
            bool isValidMove = false;

            if (i_Move.Length == 5
                && (i_Move[0] >= 'A' && i_Move[0] <= ('A' + i_BoardDemension)
                && (i_Move[1] >= 'a' && i_Move[1] <= ('a' + i_BoardDemension))
                && (i_Move[3] >= 'A' && i_Move[3] <= ('A' + i_BoardDemension))
                && (i_Move[4] >= 'a' && i_Move[4] <= ('a' + i_BoardDemension)) == true))
            {
                o_Move.FromColumn = i_Move[0] - 'A';
                o_Move.FromRow = i_Move[1] - 'a';
                o_Move.ToColumn = i_Move[3] - 'A';
                o_Move.ToRow = i_Move[4] - 'a';
                isValidMove = true;
            }

            return isValidMove;
        }

        public Move()
        {
            FromColumn = 0;
            FromRow = 0;
            ToColumn = 0;
            ToRow = 0;
        }

        public Move(int i_FromRow, int i_FromColumn, int i_ToRow, int i_ToColumn)
        {
            FromRow = i_FromRow;
            FromColumn = i_FromColumn;
            ToRow = i_ToRow;
            ToColumn = i_ToColumn;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}>{2}{3}", (char)('A' + FromColumn), (char)('a' + FromRow), (char)('A' + ToColumn), (char)('a' + ToRow));
        }

        public override bool Equals(object i_Move)
        {
            if (i_Move == null || GetType() != i_Move.GetType())
            {
                return false;
            }

            Move move = (Move)i_Move;

            return FromRow == move.FromRow && FromColumn == move.FromColumn && ToRow == move.ToRow && ToColumn == move.ToColumn;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool IsEatingMove()
        {
            bool isEatingMove = false;

            if (Math.Abs(ToColumn - FromColumn) == 2 && Math.Abs(ToRow - FromRow) == 2)
            {
                isEatingMove = true;
            }

            return isEatingMove;
        }
    }
}
