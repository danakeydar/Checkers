using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace B18_Ex05.GUI
{
    public class CheckersButton : Button
    {
        public int RowInBoard { get; set; }

        public int ColInBoard { get; set; }

        public CheckersButton(int i_RowInBoard, int i_ColInBoard)
        {
            RowInBoard = i_RowInBoard;
            ColInBoard = i_ColInBoard;
        }
    }
}
