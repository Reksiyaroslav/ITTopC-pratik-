﻿using FifteenGame.Business.Models;
using FifteenGame.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FifteenGame.Web.Models
{
    public class GameViewModel
    {
        public int RowCount => GameModel.RowCount;

        public int ColumnCount => GameModel.ColumnCount;

        public CellViewModel[,] Cells { get; } = new CellViewModel[GameModel.RowCount, GameModel.ColumnCount];
    }
}