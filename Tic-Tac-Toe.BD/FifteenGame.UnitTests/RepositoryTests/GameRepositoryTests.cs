
using FifteenGame.Business.Services;
using FifteenGame.Common.BusinessModels;
using FifteenGame.Common.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FifteenGame.UnitTests
{
    [TestClass]
    public class TicTacToeTests
    {
        private GameService _gameService;
        private GameModel _model;

        [TestInitialize]
        public void Setup()
        {
            _gameService = new GameService();
            _model = new GameModel();
            _gameService.Initialize(_model);
        }

        [TestMethod]
        public void Initialize_CreatesEmptyBoard()
        {
            
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int col = 0; col < Constants.ColumnCount; col++)
                {
                    Assert.AreEqual("", _model[row, col]);
                }
            }
        }

        [TestMethod]
        public void MakeMove_FirstMove_XPlacedCorrectly()
        {
           
            int [] FisrsbuutonRowCol = new int[] { 1, 1 };

            bool result = _gameService.MakeMove(_model, FisrsbuutonRowCol);

            Assert.IsTrue(result);
            Assert.AreEqual("X", _model[1, 1]);
        }

        [TestMethod]
        public void MakeMove_OccupiedCell_ReturnsFalse()
        {
           
            _model[0, 0] = "X";
            int[] FisrsbuutonRowCol = new int[] { 0, 0 };

            // Act
            bool result = _gameService.MakeMove(_model, FisrsbuutonRowCol);

           
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsWinRow_ThreeInRow_ReturnsTrue()
        {
            _model[0, 0] = "X";
            _model[0, 1] = "X";
            _model[0, 2] = "X";

            bool result = _gameService.IsWinrow(_model, "X");

           
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsWinCol_ThreeInColumn_ReturnsTrue()
        {
            
            _model[0, 1] = "O";
            _model[1, 1] = "O";
            _model[2, 1] = "O";

            // Act
            bool result = _gameService.IsWinrcol(_model, "O");

            
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsWinDig_ThreeInDiagonal_ReturnsTrue()
        {
            
            _model[0, 0] = "X";
            _model[1, 1] = "X";
            _model[2, 2] = "X";

           
            bool result = _gameService.IsWinrdig(_model, "X");

           
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDraw_FullBoardNoWinner_ReturnsTrue()
        {
           
            string[,] fullBoard = {
                { "X", "O", "X" },
                { "X", "O", "O" },
                { "O", "X", "X" }
            };

            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int col = 0; col < Constants.ColumnCount; col++)
                {
                    _model[row, col] = fullBoard[row, col];
                }
            }

            // Act
            bool result = _gameService.IsDraw(_model);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ComputerMove_PlacesOInEmptyCell()
        {
            // Arrange
            _model[0, 0] = "X"; // Игрок сделал ход

            // Act
            _gameService.ComputerMove(_model);

            // Assert
            bool oFound = false;
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int col = 0; col < Constants.ColumnCount; col++)
                {
                    if (_model[row, col] == "O")
                    {
                        oFound = true;
                        break;
                    }
                }
            }
            Assert.IsTrue(oFound);
        }

        [TestMethod]
        public void IsGameOver_WinX_ReturnsTrue()
        {
            // Arrange
            _model[0, 0] = "X";
            _model[0, 1] = "X";
            _model[0, 2] = "X";

            // Act
            bool result = _gameService.IsGameOver(_model);

            // Assert
            Assert.IsTrue(result);
        }
    }
}