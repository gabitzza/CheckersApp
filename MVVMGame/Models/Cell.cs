using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMGame.Models
{
    class Cell : BaseNotification
    {
        public Cell(int x, int y, string displayed, PositionTypes position, PieceColor color, Status status)
        {
            this.X = x;
            this.Y = y;
            this.DisplayedImage = displayed;
            this.PositionType = position;
            this.Color = color;
            this.StatusType = status;
        }

        public int X
        {
            get { return x; }
            set
            {
                x = value;
                NotifyPropertyChanged("X");
            }
        }

        public int Y
        {
            get { return y; }
            set
            {
                y = value;
                NotifyPropertyChanged("Y");
            }
        }

        public string DisplayedImage
        {
            get { return displayedImage; }
            set
            {
                displayedImage = value;
                NotifyPropertyChanged("DisplayedImage");
            }
        }
        public PieceColor Color { get; set; }
        public PositionTypes PositionType { get; set; }
        public Status StatusType { get; set; }

        public enum PieceColor
        {
            Black,
            White,
            Nothing
        }

        public enum PositionTypes {
            Empty,
            Filled,
            CannotMoveHere
        }

        public enum Status
        {
            Normal,
            King
        }

        private int x;
        private int y;
        private string displayedImage;

    }

}
