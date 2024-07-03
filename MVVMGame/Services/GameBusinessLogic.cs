using MVVMGame.Models;
using MVVMGame.ViewModels;
using MVVMGame.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Serialization;

namespace MVVMGame.Services
{
    class GameBusinessLogic
    {
        private ObservableCollection<ObservableCollection<Cell>> cells;
        
        Cell.PieceColor turn= Cell.PieceColor.Black;

        public int whitePieceCaptured = 0, blackPieceCaptured = 0;
        public enum Round
        {
            White,
            Black,
        }
        public GameBusinessLogic(ObservableCollection<ObservableCollection<Cell>> cells)
        {
            this.cells = cells;
        }

   
        public void CheckEndGame()
        {
            if (whitePieceCaptured == 12)
            {
                Console.WriteLine("Alb a castigat");

                EndGame();
            }
            else if (blackPieceCaptured == 12)
            {
                Console.WriteLine("Negru a castigat");
                EndGame();
            }
        
        }

        public event EventHandler GameOver;
        private void EndGame()
        {
            GameOver?.Invoke(this, EventArgs.Empty);
        }

        Cell.PieceColor SwitchRound(Cell.PieceColor color)
        {
            int error = 0;
            if (color == Cell.PieceColor.White)
                return Cell.PieceColor.Black;
            else if (color == Cell.PieceColor.Black)
                return Cell.PieceColor.White;
            
            //conditii pentru eroare
            else
            {
                Console.WriteLine("Eroare la schimbarea rundei");
                error = 1;
            }
            if (error == 1)
                return Cell.PieceColor.Nothing;
            return Cell.PieceColor.Nothing;
        }
        Cell.PieceColor GetOppositeColor(Cell.PieceColor color)
        {
            return color == Cell.PieceColor.Black ? Cell.PieceColor.White :
            color == Cell.PieceColor.White ? Cell.PieceColor.Black : Cell.PieceColor.Nothing;
        }
        public string GetImagePieceByColor(Cell.PieceColor color)
        {
            string WhitePieceDisplay = "/MVVMGame;component/Resources/piece2.png";
            string BlackPieceDisplay = "/MVVMGame;component/Resources/piece1.png";
            string errorDisplay = "/MVVMGame;component/Resources/images.png";
            if (color == Cell.PieceColor.Black)
                return BlackPieceDisplay;
            else if (color == Cell.PieceColor.White)
                return WhitePieceDisplay;
            return errorDisplay;
        }
        public string GetKingImageByColor(Cell.PieceColor color)
        {
            string WhiteQueenPieceDisplay = "/MVVMGame;component/Resources/white-queen.png";
            string BlackQueenPieceDisplay = "/MVVMGame;component/Resources/black-queen.png";
            string errorDisplay = "/MVVMGame;component/Resources/images.png";
            if (color == Cell.PieceColor.Black)
                return BlackQueenPieceDisplay;
            else if (color == Cell.PieceColor.White)
                return WhiteQueenPieceDisplay;
            return errorDisplay;
        }
        public string GetEmptyBackgroundColor()
        {
            string emptyDisplay = "/MVVMGame;component/Resources/init2.png";
            return emptyDisplay;
        }
        public void MakeGreenClickedPieceByColor(Cell firstClickedCell, Cell.PieceColor color)
        {
            if(color==Cell.PieceColor.White)
                firstClickedCell.DisplayedImage = "/MVVMGame;component/Resources/green-piece2.png";
            else if (color==Cell.PieceColor.Black)
                firstClickedCell.DisplayedImage = "/MVVMGame;component/Resources/green-piece1.png";
        }
        private void MoveThePiece(Cell previousPosition, Cell NextPosition)
        {
            if (NextPosition.X == 7 || previousPosition.StatusType == Cell.Status.King)
            {
                NextPosition.DisplayedImage = GetKingImageByColor(previousPosition.Color);
                NextPosition.StatusType = Cell.Status.King;
            }
            else if (NextPosition.X == 0 || previousPosition.StatusType == Cell.Status.King)
            {
                NextPosition.DisplayedImage = GetKingImageByColor(previousPosition.Color);
                NextPosition.StatusType = Cell.Status.King;
            }
            else {
                NextPosition.DisplayedImage = GetImagePieceByColor(previousPosition.Color);
            }
            NextPosition.PositionType = Cell.PositionTypes.Filled;
            NextPosition.Color = previousPosition.Color;
            cells[previousPosition.X][previousPosition.Y] = previousPosition;
            cells[NextPosition.X][NextPosition.Y] = NextPosition;
        }
        private void MakeEmptyThePosition(Cell cell)
        {
            cell.PositionType = Cell.PositionTypes.Empty;
            cell.DisplayedImage = GetEmptyBackgroundColor();
            cell.StatusType = Cell.Status.Normal;
            cells[cell.X][cell.Y] = cell;
        }
        private bool VerifyNullConditionClick2(Cell clickedCell, Cell CellPosition)
        {
            if (clickedCell != null && Helper.FirstClickPosition != null && clickedCell != Helper.FirstClickPosition)
                return true;
            return false;
        }
        public void SimpleMove(Cell FirstClickPosition, Cell clickedCell)
        {
            if (FirstClickPosition.Color == Cell.PieceColor.White) //pentru piesa alba
            {
                if (FirstClickPosition.StatusType == Cell.Status.King)
                {
                    if (FirstClickPosition.X > clickedCell.X) 
                        MoveAndChangedDysplay(clickedCell, FirstClickPosition.Color);
                }
                if (FirstClickPosition.X < clickedCell.X) //daca merge de sus in jos, pentru alb, se poate muta
                    MoveAndChangedDysplay(clickedCell, FirstClickPosition.Color);
            }
            else if (FirstClickPosition.Color == Cell.PieceColor.Black) //daca piese este neagra se merge de jos in sus
            {
                if (FirstClickPosition.StatusType == Cell.Status.King)
                {
                    if (FirstClickPosition.X < clickedCell.X) 
                        MoveAndChangedDysplay(clickedCell, FirstClickPosition.Color);
                }
                if (FirstClickPosition.X > clickedCell.X) //daca merge de sus in jos, pentru negru, se poate muta
                    MoveAndChangedDysplay(clickedCell, FirstClickPosition.Color);
            }
        }
        private void MakeTheJump(Cell FirstClickedPosition, Cell ClickedCell, int x, int y)
        {
            Cell opponentPiece = cells[Helper.FirstClickPosition.X + x][Helper.FirstClickPosition.Y + y];
            if (opponentPiece.Color == GetOppositeColor(FirstClickedPosition.Color) && opponentPiece.PositionType == Cell.PositionTypes.Filled)
            {
                MakeEmptyThePosition(opponentPiece);
                MoveAndChangedDysplay(ClickedCell, ClickedCell.Color);
                turn = SwitchRound(ClickedCell.Color);

                if (FirstClickedPosition.Color == Cell.PieceColor.White)
                {         
                        whitePieceCaptured += 1;
                }
                else if (FirstClickedPosition.Color == Cell.PieceColor.Black)
                {   
                        blackPieceCaptured += 1;
                }
                CheckEndGame();
            }
        }
        private void WhiteJumping(Cell FirstClickPosition, Cell clickedCell)
        {
            if(FirstClickPosition.StatusType==Cell.Status.King)
            {
                if (clickedCell.X < FirstClickPosition.X && clickedCell.Y > FirstClickPosition.Y)
                    MakeTheJump(FirstClickPosition, clickedCell, -1, 1);

                else if (clickedCell.X < FirstClickPosition.X && clickedCell.Y < FirstClickPosition.Y)
                    MakeTheJump(FirstClickPosition,clickedCell, -1, -1);
            }
            if (clickedCell.X > FirstClickPosition.X && clickedCell.Y > FirstClickPosition.Y) // mutare in partea dreapta, jos                                                                                                                    
                MakeTheJump(FirstClickPosition, clickedCell, 1, 1);

            else if (clickedCell.X > FirstClickPosition.X && clickedCell.Y <FirstClickPosition.Y) //mutare in partea stanga, sus
                MakeTheJump(FirstClickPosition, clickedCell, 1, -1);
        }
        private void BlackJumping(Cell FirstClickPosition, Cell clickedCell)
        {
            if (FirstClickPosition.StatusType == Cell.Status.King)
            {
                if (clickedCell.X > FirstClickPosition.X && clickedCell.Y < FirstClickPosition.Y)
                    MakeTheJump(FirstClickPosition, clickedCell, 1, -1);
                else if (clickedCell.X > FirstClickPosition.X && clickedCell.Y > FirstClickPosition.Y) //mutare in partea dreapta, jos
                    MakeTheJump(FirstClickPosition, clickedCell, 1, 1);
            }
            if (clickedCell.X < FirstClickPosition.X && clickedCell.Y < FirstClickPosition.Y) //mutare in partea stanga, sus
                MakeTheJump(FirstClickPosition, clickedCell, -1, -1);
            else if (clickedCell.X < FirstClickPosition.X && clickedCell.Y > FirstClickPosition.Y) // mutare in partea dreapta, sus
                MakeTheJump(FirstClickPosition, clickedCell, -1, 1);
        }
        private void SimpleJump(Cell FirstClickPosition, Cell clickedCell)
        {
            if (FirstClickPosition.Color == Cell.PieceColor.White) // jump daca piesa e alba 
                WhiteJumping(FirstClickPosition, clickedCell);

            else if (FirstClickPosition.Color == Cell.PieceColor.Black)  //jump daca piesa e neagra
                BlackJumping(FirstClickPosition, clickedCell);
        }
        public void MoveAndChangedDysplay(Cell clickedCell, Cell.PieceColor color)
        {
            MoveThePiece(Helper.FirstClickPosition, clickedCell);
            MakeEmptyThePosition(Helper.FirstClickPosition);
            Helper.FirstClickPosition = null;
            turn = SwitchRound(color);
        }
        public void MoveBusinessLogic(Cell clickedCell, Cell.PieceColor color)
        {
            if (VerifyNullConditionClick2(clickedCell, Helper.FirstClickPosition) && Helper.FirstClickPosition.Color == color && clickedCell.PositionType == Cell.PositionTypes.Empty)
            {
                if (Helper.FirstClickPosition.PositionType == Cell.PositionTypes.Filled && clickedCell.PositionType == Cell.PositionTypes.Empty)
                {
                    int deltaX = Math.Abs(clickedCell.X - Helper.FirstClickPosition.X);
                    int deltaY = Math.Abs(clickedCell.Y - Helper.FirstClickPosition.Y);
                    if (deltaX == 1 && deltaY == 1) // daca distanta dintre mutari este de o celula
                        SimpleMove(Helper.FirstClickPosition, clickedCell);
                    else if (deltaX == 2 && deltaY == 2)
                        SimpleJump(Helper.FirstClickPosition, clickedCell);                 
                }         
            }
            else if (clickedCell == Helper.FirstClickPosition)
            {
                if (clickedCell.StatusType == Cell.Status.King)
                {
                    Helper.FirstClickPosition.DisplayedImage = GetKingImageByColor(clickedCell.Color);
                }
                else
                {
                    Helper.FirstClickPosition.DisplayedImage = GetImagePieceByColor(clickedCell.Color);
                }
                Helper.FirstClickPosition = null;
            }
            else if (Helper.FirstClickPosition == null && clickedCell.PositionType == Cell.PositionTypes.Filled && clickedCell.Color == color && clickedCell != null)
            {
                Helper.FirstClickPosition = clickedCell;
                MakeGreenClickedPieceByColor(clickedCell, clickedCell.Color);
            }
        }    
        public void Turn(Cell obj, Cell.PieceColor round)
        {
            if (round == Cell.PieceColor.Black)
            {
                MoveBusinessLogic(obj, round);
            }
            else if (round == Cell.PieceColor.White)
            {
                MoveBusinessLogic(obj, round);
            }       
        }
        public void ClickAction(Cell obj)
        {
            Turn(obj,turn);
        }

    }
}
