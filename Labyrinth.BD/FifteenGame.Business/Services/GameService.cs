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
                HealCount = 10 ,
                ExitRowCol = new int[] {0,0},
                PlaerRowCol = new int[] {0,0} ,

            };

            Initialize(game);
            game.MoveCount = 0;
            game.HealCount = 10;
            dto = ToDto(game);
            var gameId = _gameRepository.Save(dto);

            return GetByGameId(gameId);
        }

        public bool IsGameOver(int gameId)
        {
            var gameDto = _gameRepository.GetByGameId(gameId);
            var result = IsGameOver(FromDto(gameDto));
            return result;
        }

        public void Initialize(GameModel model)
        {
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    model[row, column] = "E";

                }

            }
            IntializePlaceRandom(model, "C", Constants.ChestCount);
            IntializePlaceRandom(model, "T", Constants.TrapCount);
            IntializePlaceRandom(model, "S", Constants.SkillCount);
            IntializePlaceRandom(model, "P", Constants.PlaerCount);
            IntializePlaceRandom(model, "B", Constants.PlaerCount);
            CheckExit(model);
            CheckPlaer(model);

        }
        public void IntializePlaceRandom(GameModel model, string type_boar, int countplace)
        {
            Random random = new Random();
            int count = 0;
            while (countplace > count)
            {
                int row = random.Next(Constants.RowCount);
                int colum = random.Next(Constants.ColumnCount);
                if (model[row, colum] == "E")
                {
                    model[row, colum] = type_boar;
                    count++;
                }
            }


        }

        public bool CheckMatch(GameModel model, int[] FisrsbuutonRowCol)
        {
            if (model[FisrsbuutonRowCol[0], FisrsbuutonRowCol[1]] == "C")
            {
                model.HealCount++;
            }
            else if (model[FisrsbuutonRowCol[0], FisrsbuutonRowCol[1]] == "T") 
            {
                model.HealCount--;

            }
            else if (model[FisrsbuutonRowCol[0], FisrsbuutonRowCol[1]] == "S")
            {
                model.HealCount += 2;

            }

            else if (FisrsbuutonRowCol[0] == model.PlaerRowCol[0] && FisrsbuutonRowCol[1] == model.PlaerRowCol[1])
            {
                return false;
            }

            return true;

        }
        public void NewPlaerRowColumn(GameModel model, int[] FisrsbuutonRowCol)
        {
            int[] OldPlaerRowCol = new int[2];

            model[FisrsbuutonRowCol[0], FisrsbuutonRowCol[1]] = model[model.PlaerRowCol[0], model.PlaerRowCol[1]];
            OldPlaerRowCol = model.PlaerRowCol;
            model[  OldPlaerRowCol[0],  model.PlaerRowCol[1]] = "";
            CheckPlaer(model);
        }
        public void CheckPlaer(GameModel model)
        {
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    if (model[row, column] == "P")
                    {
                        model.PlaerRowCol = new int[] { row, column };
                    }

                }

            }
        }
        public void CheckExit(GameModel model)
        {
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    if (model[row, column] == "B")
                    {
                        model.ExitRowCol = new int[] { row, column };
                    }

                }

            }
        }
        public bool IsGameOver(GameModel model)
        {
            if (model.HealCount == 0)
            {

                
                model.HealCount = 10;
                return true;
            }
            else if (model.ExitRowCol[0] == model.PlaerRowCol[0] && model.ExitRowCol[1] == model.PlaerRowCol[1])
            {
               
                model.HealCount = 10;
                return true;
            }


            return false;
        }
        public GameModel CheckMatch(int gameId, int[] FisrsbuutonRowCol)
        {
            var gameDto = _gameRepository.GetByGameId(gameId);
            var gameModel = FromDto(gameDto);

            CheckMatch(gameModel, FisrsbuutonRowCol);
             NewPlaerRowColumn(gameModel, FisrsbuutonRowCol);
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
                HealCount = dto.HealCount,
                ExitRowCol = new int[]{ dto.ExitRow ,dto.ExitCol},
                PlaerRowCol = new int[] { dto.PlaerRow, dto.PlaerCol },
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
                HealCount = game.HealCount,
                ExitRow = game.ExitRowCol[0],
                ExitCol = game.ExitRowCol[1],
                PlaerRow = game.PlaerRowCol[0] ,
                PlaerCol = game.PlaerRowCol[1]
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
