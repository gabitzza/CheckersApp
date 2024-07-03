using MVVMGame.Models;
using MVVMGame.Services;
using MVVMGame.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static MVVMGame.Models.Cell;

namespace MVVMGame.ViewModels
{
    class CellVM
    {
        GameBusinessLogic bl;
        public CellVM(int x, int y, string hidden, PositionTypes positionType, PieceColor color, Status status, GameBusinessLogic bl)
        {
            SimpleCell = new Cell(x, y, hidden, positionType, color, status);
            this.bl = bl;
        }
        //am adus celula din Model in VM
        public ICommand ClickCommand
        {
            get
            {
                if (clickCommand == null)
                {
                    clickCommand = new RelayCommand<Cell>(bl.ClickAction);
                }
                return clickCommand;
            }
        }
        public Cell SimpleCell { get; set; }
       
        private ICommand clickCommand;
    }
}
