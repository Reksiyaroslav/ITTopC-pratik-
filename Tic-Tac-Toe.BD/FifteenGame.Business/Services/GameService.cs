using FifteenGame.Common.BusinessModels;
using FifteenGame.Common.Definitions;
using FifteenGame.Common.Dtos;
using FifteenGame.Common.Repositories;
using FifteenGame.Common.Services;
using FifteenGame.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FifteenGame.Business.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        int countPar=0;
        public GameService()
        {
            _gameRepository = new GameRepository();
        }

        public GameModel GetByGameId(int gameId)
        {
            var dto = _gameRepository.GetByGameId(gameId);
            return FromDto(dto);
        }

        public GameModel GetByUserId(int userId)
        {
            var dtos = _gameRepository.GetByUserId(userId);
            var dto = dtos.LastOrDefault();
            if (dto != null)
            {
                return FromDto(dto);
            }

            var game = new GameModel
            {
                UserId = userId,
                GameBegin = DateTime.Now,
                MoveCount = 0,
            };

            Initialize(game);
            game.MoveCount = 0;
            dto = ToDto(game);
            var gameId = _gameRepository.Save(dto);

            return GetByGameId(gameId);
        }

        public void Initialize(GameModel model)
        {



            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    model[row, column] = "";

                }

            }
        }

       

        public bool IsGameOver(int gameId)
        {
            var gameDto = _gameRepository.GetByGameId(gameId);
            var result = IsGameOver(FromDto(gameDto));
            return result;
        }

        public bool MakeMove(GameModel model, int[] FisrsbuutonRowCol)
        {
            model.MoveCount++;

            if (model[FisrsbuutonRowCol[0], FisrsbuutonRowCol[1]]=="")
            {
                model[FisrsbuutonRowCol[0], FisrsbuutonRowCol[1]] = "X";
                ComputerMove(model);
                return true;
            }
            else
            { 
                return false; 
            }
        }
        public void ComputerMove(GameModel model)
        {
            model.MoveCount++;
            Random random = new Random();

            var emptyCells = new List<(int row, int column)>();
            Thread.Sleep(2000);
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    if (model[row, column] == "")
                    {
                        emptyCells.Add((row, column));
                    }

                }

            }
            int randomindex = random.Next(emptyCells.Count);
            var (randomRow, randomColumn) = emptyCells[randomindex];
            model[randomRow, randomColumn] = "O";

        }


        public bool IsGameOver(GameModel model)
        {
            if (IsWingame(model, "X") || IsWingame(model, "O") || IsDraw(model)) { return true; }


            return false;
        }

        public bool IsWingame(GameModel model, string bor)
        {
            if (IsWinrow(model, bor) || IsWinrcol(model, bor) || IsWinrdig(model, bor))
            { return true; }
            return false;





        }
        public bool IsWinrow(GameModel model, string bor)
        {
            bool win = true;
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {

                    if (model[row, column] != bor)
                    {
                        win = false;
                        break;
                    }

                }
                if (win) return true;
            }
            return false;
        }
        public bool IsWinrcol(GameModel model, string bor)
        {
            bool win = true;
            for (int column = 0; column < Constants.ColumnCount; column++)
            {
                for (int row = 0; row < Constants.RowCount; row++)
                {

                    if (model[row, column] != bor)
                    {
                        win = false;
                        break;
                    }

                }
                if (win) return true;
            }
            return false;
        }
        public bool IsWinrdig(GameModel model, string bor)
        {
            bool digright = true;
            bool digleft = true;
            for (int row = 0; row < Constants.RowCount; row++)
            {
                if (model[row, row] != bor)
                {
                    digright = false;

                }
                if (model[row, Constants.RowCount - 1 - row] != bor)
                {
                    digleft = false;

                }



            }
            return digright || digleft;
        }
        public bool IsDraw(GameModel model)
        {

            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int col = 0; col < Constants.ColumnCount; col++)
                {
                    if (model[row, col] == "")
                        return false;
                }
            }
            return true;
        }
        public GameModel MakeMove(int gameId, int[] FisrsbuutonRowCol)
        {
            var gameDto = _gameRepository.GetByGameId(gameId);
            var gameModel = FromDto(gameDto);

            MakeMove(gameModel, FisrsbuutonRowCol);

            _gameRepository.Save(ToDto(gameModel));
            return gameModel;
        }

        public void RemoveGame(int gameId)
        {
            _gameRepository.Remove(gameId);
        }

       

        private GameModel FromDto(GameDto dto)
        {
            var result = new GameModel
            {
                Id = dto.Id,
                UserId = dto.UserId,
                MoveCount = dto.MoveCount,
                GameBegin = dto.GameBegin,
            };

            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    result[row, column] = dto.Cells[row, column];
                    if (result[row, column] == "")
                    {
                        continue;
                    }
                }
            }

            return result;
        }

        private GameDto ToDto(GameModel game)
        {
            var dto = new GameDto
            {
                Id = game.Id,
                UserId = game.UserId,
                MoveCount = game.MoveCount,
                GameBegin = game.GameBegin,
            };

            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    dto.Cells[row, column] = game[row, column];
                }
            }

            return dto;
        }
    }
}
