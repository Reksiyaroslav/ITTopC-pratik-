using FifteenGame.Common.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifteenGame.Common.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string[,] Cells { get; } = new string[Constants.RowCount, Constants.ColumnCount];

        public int MoveCount { get; set; }
        public int HealCount { get; set; }

        public DateTime GameBegin { get; set; }

        public int ExitRow { get; set; }

        public int ExitCol { get; set; }

        public int PlaerRow { get; set; }

        public int PlaerCol { get; set; }
    }
}
