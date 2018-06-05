using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using B18_Ex05.Enums;

namespace B18_Ex05.GUI
{
    public class CheckersForm : Form
    {
        private readonly Board m_Board;
        private readonly SettingForm m_SettingForm;
        private CheckersButton[,] m_BoardButtons;
        private CheckersButton preClickedbutton;
        private Label labelPlayerA;
        private Label labelPlayerB;
        private Label labelCurrentPlayerTurn;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            m_SettingForm.ShowDialog();
            setGame();
        }

        public CheckersForm()
        {
            m_SettingForm = new SettingForm();
            m_Board = new Board();
            labelPlayerA = new Label();
            labelPlayerB = new Label();
            labelCurrentPlayerTurn = new Label();
        }

        private void setGame()
        {
            m_Board.SetNewBoard(m_SettingForm.BoardSize);
            setWindowProperties();
            setBoardLayout();
            setPlayers();
            syncButtonsBoardWithLogicalBoard();
        }

        private void setWindowProperties()
        {
            int boardSize = m_SettingForm.BoardSize;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Width = boardSize * 50;
            this.Height = boardSize * 60;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.MaximumSize = new Size(boardSize * 50, boardSize * 60);
            this.MinimumSize = new Size(boardSize * 50, boardSize * 60);

            labelPlayerA.Left = (this.ClientSize.Width / 100) * 20;
            labelPlayerA.Top = (this.ClientSize.Height / 100) * 10;
            labelPlayerA.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelPlayerA.Text = string.Format("{0}: 0", m_SettingForm.Player1Name);
            this.Controls.Add(labelPlayerA);

            labelPlayerB.Left = (this.ClientSize.Width / 100) * 80;
            labelPlayerB.Top = labelPlayerA.Top;
            labelPlayerB.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelPlayerB.Text = string.Format("{0}: 0", m_SettingForm.Player2Name);
            this.Controls.Add(labelPlayerB);

            labelCurrentPlayerTurn.Left = (this.ClientSize.Width / 2) - (labelCurrentPlayerTurn.Width / 2);
            labelCurrentPlayerTurn.Top = (this.ClientSize.Height / 100) * 2;
            labelCurrentPlayerTurn.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCurrentPlayerTurn.Text = "turn";
            labelCurrentPlayerTurn.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(labelCurrentPlayerTurn);
        }

        private void setBoardLayout()
        {
            int boardSize = m_SettingForm.BoardSize;

            int buttonSize = (this.ClientSize.Width - 30) / boardSize;
            int startingTopPoint = this.ClientSize.Height - ((buttonSize * boardSize) + 15);
            const int startingLeftPoint = 15;
            m_BoardButtons = new CheckersButton[m_SettingForm.BoardSize, m_SettingForm.BoardSize];

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    CheckersButton button = new CheckersButton(i, j); 
                    button.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    button.Size = new Size(buttonSize, buttonSize);
                    button.Top = startingTopPoint + (buttonSize * i);
                    button.Left = startingLeftPoint + (buttonSize * j);
                    if ((i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0))
                    {
                        button.BackColor = Color.WhiteSmoke;
                    }
                    else
                    {
                        button.BackColor = Color.Gray;
                        button.Enabled = false;
                    }

                    this.Controls.Add(button);
                    button.Click += new EventHandler(CheckersButton_Clicked);
                    m_BoardButtons[i, j] = button;
                }
            }
        }

        private void setPlayers()
        {
            m_Board.PlayerA = new Player(e_PlayerColor.Black, m_SettingForm.Player1Name);
            m_Board.PlayerB = new Player(e_PlayerColor.Red, m_SettingForm.Player2Name);

            if (m_SettingForm.AgeinstComputer == true)
            {
                m_Board.PlayerB.PlayerType = e_PlayerType.Computer;
            }
            else
            {
                m_Board.PlayerB.PlayerType = e_PlayerType.Human;
            }
        }

        private void CheckersButton_Clicked(object sender, EventArgs e)
        {
            CheckersButton clickedButton = sender as CheckersButton;

            if (clickedButton != null)
            {
                if (preClickedbutton == null)
                {
                    clickedButton.BackColor = Color.SkyBlue;
                    preClickedbutton = clickedButton;
                }
                else if (preClickedbutton == clickedButton)
                {
                    clickedButton.BackColor = Color.WhiteSmoke;
                    preClickedbutton = null;
                }
                else
                {
                    Move move = new Move(preClickedbutton.RowInBoard, preClickedbutton.ColInBoard, clickedButton.RowInBoard, clickedButton.ColInBoard);
                    if (!m_Board.PlayTurn(move))
                    {
                        MessageBox.Show("Illegal Move!");
                    }

                    handleGameOverAndRematch();

                    preClickedbutton.BackColor = Color.WhiteSmoke;
                    preClickedbutton = null;

                    if (m_SettingForm.AgeinstComputer)
                    {
                        m_Board.PlayComputerTurn();
                    }

                    handleGameOverAndRematch();
                    syncButtonsBoardWithLogicalBoard();
                }
            }
        }

        private void handleGameOverAndRematch()
        {
            e_PlayerColor winnerColor = e_PlayerColor.Tie;

            if (m_Board.IsGameOver(ref winnerColor, m_Board.CurrentPlayerTurn))
            {
                syncButtonsBoardWithLogicalBoard();
                Player winner = m_Board.GetPlayerByColor(winnerColor);
                winner.Score += m_Board.GetWinnerScore(winnerColor);
                StringBuilder messege = new StringBuilder();
                messege.AppendFormat("{0} Won!", winner.PlayerName);
                messege.AppendLine();
                messege.AppendFormat("Another Round?");
                DialogResult result = MessageBox.Show(messege.ToString(), "Damka", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    this.Close();
                }
                else
                {
                    m_Board.SetBoardForNewGame();
                    labelPlayerA.Text = string.Format("{0}: {1}", m_Board.PlayerA.PlayerName, m_Board.PlayerA.Score);
                    labelPlayerB.Text = string.Format("{0}: {1}", m_Board.PlayerB.PlayerName, m_Board.PlayerB.Score);
                }
            }
        }

        private void syncButtonsBoardWithLogicalBoard()
        {
            labelCurrentPlayerTurn.Text = string.Format("{0} turn", m_Board.CurrentPlayerTurn.ToString());
            for (int i = 0; i < m_SettingForm.BoardSize; i++)
            {
                for (int j = 0; j < m_SettingForm.BoardSize; j++)
                {
                    Cell currentLogicalBoardCell = m_Board.GameBoard[i, j];
                    if (currentLogicalBoardCell.CellContent == e_CellContent.Empty)
                    {
                        m_BoardButtons[i, j].Text = " ";
                    }
                    else if (currentLogicalBoardCell.CellContent == e_CellContent.BlackKing)
                    {
                        m_BoardButtons[i, j].Text = "K";
                    }
                    else if (currentLogicalBoardCell.CellContent == e_CellContent.Red)
                    {
                        m_BoardButtons[i, j].Text = "O";
                    }
                    else if (currentLogicalBoardCell.CellContent == e_CellContent.RedKing)
                    {
                        m_BoardButtons[i, j].Text = "U";
                    }
                    else if (currentLogicalBoardCell.CellContent == e_CellContent.Black)
                    {
                        m_BoardButtons[i, j].Text = "X";
                    }
                }
            }
        }
    }
}
