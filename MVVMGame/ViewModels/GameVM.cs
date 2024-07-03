using MVVMGame.Models;
using MVVMGame.Services;
using MVVMGame.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMGame.ViewModels
{
    class GameVM
    {
        private GameBusinessLogic bl;
        public GameVM()
        {
            ObservableCollection<ObservableCollection<Cell>> board = Helper.InitGameBoard();
            bl = new GameBusinessLogic(board);
            GameBoard = CellBoardToCellVMBoard(board);
            bl.GameOver += GameLogic_GameOver;
        }
        private void GameLogic_GameOver(object sender, EventArgs e)
        {
            if (bl != null)
            {
                if (bl != null)
                {
                    if (bl.whitePieceCaptured == 12)
                    {
                        GameOverWindow gameOverWindow = new GameOverWindow(); 
                        ShowGameOverWindow("Alb");
                    }
                    else if (bl.blackPieceCaptured == 12)
                    {
                        GameOverWindow gameOverWindow = new GameOverWindow();
                        ShowGameOverWindow("Negru");
                    }
                }
            }
        }
        private void ShowGameOverWindow(string winner)
        {
            GameOverViewModel viewModel = new GameOverViewModel();
            viewModel.GameOverMessage = winner;

            GameOverWindow gameOverWindow = new GameOverWindow();
            gameOverWindow.DataContext = viewModel;
            gameOverWindow.Show();
        }
        private ObservableCollection<ObservableCollection<CellVM>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<Cell>> board)
        {
            ObservableCollection<ObservableCollection<CellVM>> result = new ObservableCollection<ObservableCollection<CellVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<CellVM> line = new ObservableCollection<CellVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    Cell c = board[i][j];
                    CellVM cellVM = new CellVM(c.X, c.Y, c.DisplayedImage, c.PositionType, c.Color,c.StatusType, bl);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }
        public ObservableCollection<ObservableCollection<CellVM>> GameBoard { get; set; }
    }
}
