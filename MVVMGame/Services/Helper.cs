using MVVMGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MVVMGame.Models.Cell;

namespace MVVMGame.Services
{
    class Helper
    {
        public static Cell FirstClickPosition { get; set; }
        public static Cell NextPosition { get; set; }

        public static ObservableCollection<ObservableCollection<Cell>> InitGameBoard()
        {   
            var gameBoard = new ObservableCollection<ObservableCollection<Cell>>();

            for (int i = 0; i < 8; i++)
            {
                var rand = new ObservableCollection<Cell>();

                for (int j = 0; j < 8; j++)
                {
                    PositionTypes positionType;
                    PieceColor color;
                    Status status = Status.Normal;
                    string display;

                    if ((i + j) % 2 == 1 && i < 3)
                    {
                        //piesa alba
                        positionType = PositionTypes.Filled;
                        display = "/MVVMGame;component/Resources/piece2.png";
                        color = PieceColor.White;
                    }
                    else if ((i + j) % 2 == 1 && i > 4)
                    {
                        positionType = PositionTypes.Filled;
                        display = "/MVVMGame;component/Resources/piece1.png";
                        color = PieceColor.Black;
                    }
                    else if ((i + j) % 2 == 0)
                    {
                        //fundal dechis
                        positionType = PositionTypes.CannotMoveHere;
                        display = "/MVVMGame;component/Resources/init1.png";
                        color = PieceColor.Nothing;
                    }
                    else if ((i + j) % 2 == 1 && i < 5)
                    {
                        //fundal inchis
                        positionType = PositionTypes.Empty; 
                        display = "/MVVMGame;component/Resources/init2.png";
                        color = PieceColor.Nothing;
                    }
                    else
                    {
                        positionType = PositionTypes.Empty;
                        display = "/MVVMGame;component/Resources/init2.png";
                        color = PieceColor.Nothing;
                    }
                    rand.Add(new Cell(i, j, display, positionType, color, status));
                }

                gameBoard.Add(rand);
            }

            return gameBoard;
        }
    }
}
