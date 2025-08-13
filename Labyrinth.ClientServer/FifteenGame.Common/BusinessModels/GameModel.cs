using FifteenGame.Common.Definitions;
using System;

namespace FifteenGame.Common.BusinessModels
{
    public class GameModel
    {
        private string[,] _cells = new string[Constants.RowCount, Constants.ColumnCount];

        public int GameId { get; set; }

        public int UserId { get; set; }

        public string this[int row, int column]
        {
            get => _cells[row, column];
            set => _cells[row, column] = value;
        }

        public int MoveCount { get; set; }

        public int HealCount { get; set; }

        

        public int[] ExitRowCol { get; set; }

        public int[] PlaerRowCol { get; set; }


        public GameModel()
        {
            PlaerRowCol = new int[2];
            PlaerRowCol[0] = 0;
            PlaerRowCol[1] = 0;
        }


       

        public DateTime GameStart { get; set; }
    }
}
