
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace FifteenGame.Business.Models
{

    public class GameModel
    {
        public const int RowCount = 4;

        public const int ColumnCount = 4;
        public const int ChestCount = 5;
        public const int TrapCount = 6;
        public const int SkillCount = 3;
        public const int PlaerCount = 1;

        private string[,] _cells = new string[RowCount, ColumnCount];


        public string this[int row, int column]
        {
            get => _cells[row, column];
            internal set => _cells[row, column] = value;
        }
        public int[]  FisrsbuutonRowCol = {4,4};
        public int[] OldFisrsbuutonRowCol = { 4, 4 };
        public int[] PlaerRowCol = {4,4};
        public int[] ExitRowCol = {4,4};
        public int[] OldPlaerRowCol = { 4, 4 };
        public int CountMove = 0;
        public int HealCount = 10;
        



    }
}
