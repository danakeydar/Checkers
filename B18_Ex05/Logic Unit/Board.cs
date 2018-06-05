using System;
using System.Collections.Generic;
using B18_Ex05.Enums;

namespace B18_Ex05
{
    public class Board
    {
        public Cell[,] GameBoard { get; private set; }

        public int BlackCurrentScore { get; private set; }

        public int Dimension { get; private set; }

        public int RedCurrentScore { get; private set; }

        public e_PlayerColor CurrentPlayerTurn { get; private set; }

        public Player PlayerA { get; set; }

        public Player PlayerB { get; set; }

        public void SetNewBoard(int i_Dimension)
        {
            GameBoard = new Cell[i_Dimension, i_Dimension];

            for (int x = 0; x < i_Dimension; x++)
            {
                for (int y = 0; y < i_Dimension; y++)
                {
                    GameBoard[x, y] = new Cell();
                }
            }

            Dimension = i_Dimension;
            SetBoardForNewGame();
        }

        public void SetBoardForNewGame()
        {
            BlackCurrentScore = 0;
            RedCurrentScore = 0;
            CurrentPlayerTurn = e_PlayerColor.Black;
            for (int x = 0; x < Dimension; x++)
            {
                for (int y = 0; y < Dimension; y++)
                {
                    if (x < (Dimension / 2) - 1)
                    {
                        if ((x % 2 == 0 && y % 2 == 1) || (x % 2 == 1 && y % 2 == 0))
                        {
                            GameBoard[x, y].CellContent = e_CellContent.Red;
                            RedCurrentScore++;
                        }
                    }
                    else if (x > (Dimension / 2))
                    {
                        if ((x % 2 == 1 && y % 2 == 0) || (x % 2 == 0 && y % 2 == 1))
                        {
                            GameBoard[x, y].CellContent = e_CellContent.Black;
                            BlackCurrentScore++;
                        }
                    }
                    else
                    {
                        GameBoard[x, y].CellContent = e_CellContent.Empty;
                    }
                }
            }
        }

        public int GetWinnerScore(e_PlayerColor i_PlayerColor)
        {
            int playerScore = getPlayerScore(i_PlayerColor);
            int opponentScore = getPlayerScore(Player.GetMyOpponentColor(i_PlayerColor));

            return playerScore - opponentScore;
        }

        public e_PlayerColor GetCurrUnderDog()
        {
            e_PlayerColor underDog;

            if (BlackCurrentScore > RedCurrentScore)
            {
                underDog = e_PlayerColor.Red;
            }
            else if (BlackCurrentScore < RedCurrentScore)
            {
                underDog = e_PlayerColor.Black;
            }
            else
            {
                underDog = e_PlayerColor.Tie;
            }

            return underDog;
        }

        public bool IsGameOver(ref e_PlayerColor o_winnerColor, e_PlayerColor i_CurPlayerColor)
        {
            bool isGameOver = false;
            e_PlayerColor opponentColor = Player.GetMyOpponentColor(i_CurPlayerColor);

            o_winnerColor = GetCurrWinner();
            if (getPlayerScore(i_CurPlayerColor) == 0)
            {
                o_winnerColor = opponentColor;
                isGameOver = true;
            }
            else
            {
                List<Move> curPlayerValidMoves = getAllValidMoves(i_CurPlayerColor);
                List<Move> opponentPlayerValidMoves = getAllValidMoves(opponentColor);

                if (curPlayerValidMoves.Count == 0)
                {
                    if (opponentPlayerValidMoves.Count == 0)
                    {
                        o_winnerColor = e_PlayerColor.Tie;
                        isGameOver = true;
                    }
                    else
                    {
                        o_winnerColor = opponentColor;
                        isGameOver = true;
                    }
                }
                else
                {
                    isGameOver = false;
                }
            }

            return isGameOver;
        }

        public Player GetPlayerByColor(e_PlayerColor i_Color)
        {
            Player player;

            if (i_Color == e_PlayerColor.Black)
            {
                player = PlayerA;
            }
            else
            {
                player = PlayerB;
            }

            return player;
        }

        public bool PlayTurn(Move i_move)
        {
            bool isValidMove = false;
            Player currPlayer = GetPlayerByColor(CurrentPlayerTurn);
            if (currPlayer.PlayerType == e_PlayerType.Human)
            {
                if (MovePlayerPiece(currPlayer.PlayerColor, i_move) == true)
                {
                    isValidMove = true;
                }
                else
                {
                    isValidMove = false;
                }
            }

            if (currPlayer.PlayerType == e_PlayerType.Computer)
            {
                isValidMove = true;
                i_move = GetComputerMove();
                MovePlayerPiece(currPlayer.PlayerColor, i_move);
            }

            return isValidMove;
        }

        public void PlayComputerTurn()
        {
            while (CurrentPlayerTurn == e_PlayerColor.Red)
            {
                Move computerMove = GetComputerMove();
                if (CurrentPlayerTurn == e_PlayerColor.Red)
                {
                    PlayTurn(computerMove);
                }
            }
        }

        private Move GetComputerMove()
        {
            Random rnd = new Random();
            List<Move> computerValidMoves = getAllValidMoves(e_PlayerColor.Red);
            int numberOfMoves = computerValidMoves.Count;
            int randMove = rnd.Next(0, numberOfMoves - 1);
            return computerValidMoves[randMove];
        }

        private List<Move> GetAllCurEatingMoves(e_PlayerColor i_PlayerColor, int i_CurCol, int i_CurRow)
        {
            List<Move> curPlayerValidMoves = getAllValidMoves(i_PlayerColor);
            List<Move> curEatingMoves = new List<Move>();

            for (int i = 0; i < curPlayerValidMoves.Count; i++)
            {
                if (curPlayerValidMoves[i].FromColumn == i_CurCol
                    && curPlayerValidMoves[i].FromRow == i_CurRow
                    && curPlayerValidMoves[i].IsEatingMove())
                {
                    curEatingMoves.Add(curPlayerValidMoves[i]);
                }
            }

            return curEatingMoves;
        }

        private void setNextTurnPlayer(e_PlayerColor i_CurPlayerColor, Move i_Move)
        {
            if ((i_Move.IsEatingMove() == true) && (GetAllCurEatingMoves(i_CurPlayerColor, i_Move.ToColumn, i_Move.ToRow).Count > 0))
            {
                CurrentPlayerTurn = i_CurPlayerColor;
            }
            else
            {
                if (i_CurPlayerColor == e_PlayerColor.Black)
                {
                    CurrentPlayerTurn = e_PlayerColor.Red;
                }
                else
                {
                    CurrentPlayerTurn = e_PlayerColor.Black;
                }
            }
        }

        private int getPlayerScore(e_PlayerColor i_PlayerColor)
        {
            if (i_PlayerColor == e_PlayerColor.Black)
            {
                return BlackCurrentScore;
            }
            else
            {
                return RedCurrentScore;
            }
        }

        private e_PlayerColor GetCurrWinner()
        {
            e_PlayerColor underDog;

            if (BlackCurrentScore > RedCurrentScore)
            {
                underDog = e_PlayerColor.Black;
            }
            else if (BlackCurrentScore < RedCurrentScore)
            {
                underDog = e_PlayerColor.Red;
            }
            else
            {
                underDog = e_PlayerColor.Tie;
            }

            return underDog;
        }

        private bool MovePlayerPiece(e_PlayerColor i_CurPlayerColor, Move i_Move)
        {
            bool validMove = isValidMove(i_CurPlayerColor, i_Move);

            if (validMove == true)
            {
                updateBoard(i_CurPlayerColor, i_Move);
                setNextTurnPlayer(i_CurPlayerColor, i_Move);
            }
            else
            {
                validMove = false;
            }

            return validMove;
        }

        private bool isValidMove(e_PlayerColor i_PlayerColor, Move i_move)
        {
            List<Move> validMoves = getAllValidMoves(i_PlayerColor);
            bool validMove = false;

            foreach (Move move in validMoves)
            {
                if (i_move.Equals(move))
                {
                    validMove = true;
                }
            }

            return validMove;
        }

        private List<Move> getAllValidMoves(e_PlayerColor i_PlayerColor)
        {
            List<Move> validMoves = new List<Move>();
            List<Move> validJumpMoves = new List<Move>();

            for (int x = 0; x < Dimension; x++)
            {
                for (int y = 0; y < Dimension; y++)
                {
                    if (i_PlayerColor == e_PlayerColor.Black)
                    {
                        if (GameBoard[x, y].IsBlackPiece())
                        {
                            Piece piece = new Piece(GameBoard[x, y].CellContent, x, y);
                            checkIfPieceCanMove(piece, ref validMoves, ref validJumpMoves);
                        }
                    }

                    if (i_PlayerColor == e_PlayerColor.Red)
                    {
                        if (GameBoard[x, y].IsRedPiece())
                        {
                            Piece piece = new Piece(GameBoard[x, y].CellContent, x, y);
                            checkIfPieceCanMove(piece, ref validMoves, ref validJumpMoves);
                        }
                    }
                }
            }

            if (validJumpMoves.Count > 0)
            {
                validMoves = validJumpMoves;
            }

            return validMoves;
        }

        private void updateBoard(e_PlayerColor i_PlayerColor, Move i_Move)
        {
            updateBoardForPlainMove(i_PlayerColor, i_Move);
            updateBoardForJumpMove(i_PlayerColor, i_Move);

            GameBoard[i_Move.FromRow, i_Move.FromColumn].CellContent = e_CellContent.Empty;
        }

        private void updateBoardForPlainMove(e_PlayerColor i_PlayerColor, Move i_Move)
        {
            if (i_PlayerColor == e_PlayerColor.Red && i_Move.ToRow == Dimension - 1)
            {
                GameBoard[i_Move.ToRow, i_Move.ToColumn].CellContent = e_CellContent.RedKing;
                RedCurrentScore += 3;
            }
            else if (i_PlayerColor == e_PlayerColor.Black && i_Move.ToRow == 0)
            {
                GameBoard[i_Move.ToRow, i_Move.ToColumn].CellContent = e_CellContent.BlackKing;
                BlackCurrentScore += 3;
            }
            else
            {
                GameBoard[i_Move.ToRow, i_Move.ToColumn].CellContent = GameBoard[i_Move.FromRow, i_Move.FromColumn].CellContent;
            }
        }

        private void updateBoardForJumpMove(e_PlayerColor i_PlayerColor, Move i_Move)
        {
            int fromRow = i_Move.FromRow;
            int formColumn = i_Move.FromColumn;
            int toRow = i_Move.ToRow;
            int toColumn = i_Move.ToColumn;

            if (Math.Abs(fromRow - i_Move.ToRow) == 2)
            {
                if (fromRow > i_Move.ToRow)
                {
                    if (formColumn > toColumn)
                    {
                        updatePlayerScoreAfterJump(GameBoard[fromRow - 1, formColumn - 1].CellContent);
                        GameBoard[fromRow - 1, formColumn - 1].CellContent = e_CellContent.Empty;
                    }
                    else
                    {
                        updatePlayerScoreAfterJump(GameBoard[fromRow - 1, formColumn + 1].CellContent);
                        GameBoard[fromRow - 1, formColumn + 1].CellContent = e_CellContent.Empty;
                    }
                }
                else
                {
                    if (formColumn > toColumn)
                    {
                        updatePlayerScoreAfterJump(GameBoard[fromRow + 1, formColumn - 1].CellContent);
                        GameBoard[fromRow + 1, formColumn - 1].CellContent = e_CellContent.Empty;
                    }
                    else
                    {
                        updatePlayerScoreAfterJump(GameBoard[fromRow + 1, formColumn + 1].CellContent);
                        GameBoard[fromRow + 1, formColumn + 1].CellContent = e_CellContent.Empty;
                    }
                }
            }
        }

        private void updatePlayerScoreAfterJump(e_CellContent i_CellContent)
        {
            if (i_CellContent == e_CellContent.Black)
            {
                BlackCurrentScore--;
            }

            if (i_CellContent == e_CellContent.BlackKing)
            {
                BlackCurrentScore -= 4;
            }

            if (i_CellContent == e_CellContent.Red)
            {
                RedCurrentScore--;
            }

            if (i_CellContent == e_CellContent.RedKing)
            {
                RedCurrentScore -= 4;
            }
        }

        private void checkIfPieceCanMove(Piece i_Piece, ref List<Move> o_ValidMoves, ref List<Move> o_ValidJumpMoves)
        {
            checkIfcanMoveUpAndRight(i_Piece, ref o_ValidMoves);
            checkIfcanMoveUpAndLeft(i_Piece, ref o_ValidMoves);
            checkIfcanMoveDownAndRight(i_Piece, ref o_ValidMoves);
            checkIfcanMoveDownAndLeft(i_Piece, ref o_ValidMoves);
            checkIfcanJumpUpAndRight(i_Piece, ref o_ValidJumpMoves);
            checkIfcanJumpUpAndLeft(i_Piece, ref o_ValidJumpMoves);
            checkIfcanJumpDownAndRight(i_Piece, ref o_ValidJumpMoves);
            checkIfcanJumpDownAndLeft(i_Piece, ref o_ValidJumpMoves);
        }

        private void checkIfcanJumpDownAndRight(Piece i_Piece, ref List<Move> o_ValidJumpMoves)
        {
            int row = i_Piece.Row;
            int column = i_Piece.Column;
            e_CellContent cellContent = i_Piece.CellContent;
            e_CellContent jumpOverPlainPiece = i_Piece.GetMyOpponentPlainPieceType();
            e_CellContent jumpOverKingPiece = i_Piece.GetMyOpponentKingPieceType();

            if (cellContent == e_CellContent.Red || i_Piece.CheckIfKing())
            {
                if (row < (Dimension - 2) && column < (Dimension - 2))
                {
                    e_CellContent pieceToEat = GameBoard[row + 1, column + 1].CellContent;
                    if (pieceToEat == jumpOverPlainPiece || pieceToEat == jumpOverKingPiece)
                    {
                        if (GameBoard[row + 2, column + 2].CellContent == e_CellContent.Empty)
                        {
                            Move move = new Move(row, column, row + 2, column + 2);
                            o_ValidJumpMoves.Add(move);
                        }
                    }
                }
            }
        }

        private void checkIfcanJumpDownAndLeft(Piece i_Piece, ref List<Move> o_ValidJumpMoves)
        {
            int row = i_Piece.Row;
            int column = i_Piece.Column;
            e_CellContent cellContent = i_Piece.CellContent;
            e_CellContent jumpOverPlainPiece = i_Piece.GetMyOpponentPlainPieceType();
            e_CellContent jumpOverKingPiece = i_Piece.GetMyOpponentKingPieceType();

            if (cellContent == e_CellContent.Red || i_Piece.CheckIfKing())
            {
                if (row < (Dimension - 2) && column > 1)
                {
                    e_CellContent pieceToEat = GameBoard[row + 1, column - 1].CellContent;
                    if (pieceToEat == jumpOverPlainPiece || pieceToEat == jumpOverKingPiece)
                    {
                        if (GameBoard[row + 2, column - 2].CellContent == e_CellContent.Empty)
                        {
                            Move move = new Move(row, column, row + 2, column - 2);
                            o_ValidJumpMoves.Add(move);
                        }
                    }
                }
            }
        }

        private void checkIfcanJumpUpAndRight(Piece i_Piece, ref List<Move> o_ValidJumpMoves)
        {
            int row = i_Piece.Row;
            int column = i_Piece.Column;
            e_CellContent cellContent = i_Piece.CellContent;
            e_CellContent jumpOverPlainPiece = i_Piece.GetMyOpponentPlainPieceType();
            e_CellContent jumpOverKingPiece = i_Piece.GetMyOpponentKingPieceType();

            if (cellContent == e_CellContent.Black || i_Piece.CheckIfKing())
            {
                if (row > 1 && column < (Dimension - 2))
                {
                    e_CellContent pieceToEat = GameBoard[row - 1, column + 1].CellContent;
                    if (pieceToEat == jumpOverPlainPiece || pieceToEat == jumpOverKingPiece)
                    {
                        if (GameBoard[row - 2, column + 2].CellContent == e_CellContent.Empty)
                        {
                            Move move = new Move(row, column, row - 2, column + 2);
                            o_ValidJumpMoves.Add(move);
                        }
                    }
                }
            }
        }

        private void checkIfcanJumpUpAndLeft(Piece i_Piece, ref List<Move> o_ValidJumpMoves)
        {
            int row = i_Piece.Row;
            int column = i_Piece.Column;
            e_CellContent cellContent = i_Piece.CellContent;
            e_CellContent jumpOverPlainPiece = i_Piece.GetMyOpponentPlainPieceType();
            e_CellContent jumpOverKingPiece = i_Piece.GetMyOpponentKingPieceType();

            if (cellContent == e_CellContent.Black || i_Piece.CheckIfKing())
            {
                if (row > 1 && column > 1)
                {
                    e_CellContent pieceToEat = GameBoard[row - 1, column - 1].CellContent;
                    if (pieceToEat == jumpOverPlainPiece || pieceToEat == jumpOverKingPiece)
                    {
                        if (GameBoard[row - 2, column - 2].CellContent == e_CellContent.Empty)
                        {
                            Move move = new Move(row, column, row - 2, column - 2);
                            o_ValidJumpMoves.Add(move);
                        }
                    }
                }
            }
        }

        private void checkIfcanMoveDownAndRight(Piece i_Piece, ref List<Move> o_ValidMoves)
        {
            int row = i_Piece.Row;
            int column = i_Piece.Column;
            e_CellContent cellContent = i_Piece.CellContent;

            if (cellContent == e_CellContent.Red || i_Piece.CheckIfKing())
            {
                if (row < (Dimension - 1) && column < (Dimension - 1))
                {
                    if (GameBoard[row + 1, column + 1].CellContent == e_CellContent.Empty)
                    {
                        Move move = new Move(row, column, row + 1, column + 1);
                        o_ValidMoves.Add(move);
                    }
                }
            }
        }

        private void checkIfcanMoveDownAndLeft(Piece i_Piece, ref List<Move> o_ValidMoves)
        {
            int row = i_Piece.Row;
            int column = i_Piece.Column;
            e_CellContent cellContent = i_Piece.CellContent;

            if (cellContent == e_CellContent.Red || i_Piece.CheckIfKing())
            {
                if (row < (Dimension - 1) && column > 0)
                {
                    if (GameBoard[row + 1, column - 1].CellContent == e_CellContent.Empty)
                    {
                        Move move = new Move(row, column, row + 1, column - 1);
                        o_ValidMoves.Add(move);
                    }
                }
            }
        }

        private void checkIfcanMoveUpAndRight(Piece i_Piece, ref List<Move> o_ValidMoves)
        {
            int row = i_Piece.Row;
            int column = i_Piece.Column;
            e_CellContent cellContent = i_Piece.CellContent;

            if (cellContent == e_CellContent.Black || i_Piece.CheckIfKing())
            {
                if (row > 0 && column < (Dimension - 1))
                {
                    if (GameBoard[row - 1, column + 1].CellContent == e_CellContent.Empty)
                    {
                        Move move = new Move(row, column, row - 1, column + 1);
                        o_ValidMoves.Add(move);
                    }
                }
            }
        }

        private void checkIfcanMoveUpAndLeft(Piece i_Piece, ref List<Move> o_ValidMoves)
        {
            int row = i_Piece.Row;
            int column = i_Piece.Column;
            e_CellContent cellContent = i_Piece.CellContent;

            if (cellContent == e_CellContent.Black || i_Piece.CheckIfKing())
            {
                if (row > 0 && column > 0)
                {
                    if (GameBoard[row - 1, column - 1].CellContent == e_CellContent.Empty)
                    {
                        Move move = new Move(row, column, row - 1, column - 1);
                        o_ValidMoves.Add(move);
                    }
                }
            }
        }
    }
}
