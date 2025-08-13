using FifteenGame.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FifteenGame.Business.Services
{

    public class GameService
    { 
        public void Initialize(GameModel model)
        {
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
                {
                    model[row, column] = "E";

                }

            }
            IntializePlaceRandom(model, "C", GameModel.ChestCount);
            IntializePlaceRandom(model, "T", GameModel.TrapCount);
            IntializePlaceRandom(model, "S", GameModel.SkillCount);
            IntializePlaceRandom(model, "P", GameModel.PlaerCount);
            IntializePlaceRandom(model, "B", GameModel.PlaerCount);
            CheckExit(model);
            CheckPlaer(model);

        }
        public void IntializePlaceRandom(GameModel model,string type_boar,int countplace)
        { Random random = new Random();
            int  count = 0;
            while (countplace > count) 
            {
                int row = random.Next(GameModel.RowCount);
                int colum = random.Next(GameModel.ColumnCount);
                if (model[row, colum] == "E")
                {
                    model[row, colum] = type_boar;
                    count++;
                }
            }


        }
        
        public bool CheckMatch(GameModel model)
        {
            model.OldFisrsbuutonRowCol[0] = model.FisrsbuutonRowCol[0];
            model.OldFisrsbuutonRowCol[1] = model.FisrsbuutonRowCol[1];
            if (model[model.FisrsbuutonRowCol[0], model.FisrsbuutonRowCol[1]] == "C")
            {
                model.HealCount++;
            }
            else if ((model[model.FisrsbuutonRowCol[0], model.FisrsbuutonRowCol[1]] == "T")&& 
                (model.FisrsbuutonRowCol[0] != model.OldFisrsbuutonRowCol[0] && model.FisrsbuutonRowCol[1]
                != model.OldFisrsbuutonRowCol[1]))
            {
                model.HealCount--;

            }
            else if (model[model.FisrsbuutonRowCol[0], model.FisrsbuutonRowCol[1]] == "S")
            {
                model.HealCount += 2;

            }
            
            else if (model.FisrsbuutonRowCol[0] == model.PlaerRowCol[0] && model.FisrsbuutonRowCol[1] == model.PlaerRowCol[1])
            {
                return false;
            }
            
            return true;

        }  
        public void NewPlaerRowColumn(GameModel model)
        {
            
            model[model.FisrsbuutonRowCol[0], model.FisrsbuutonRowCol[1]] = model[model.PlaerRowCol[0], model.PlaerRowCol[1]];
            model.OldPlaerRowCol = model.PlaerRowCol;
            model[model.OldPlaerRowCol[0], model.PlaerRowCol[1]] = "";
            CheckPlaer(model);
        }
        public void CheckPlaer(GameModel model)
        {
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
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
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
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
                 
                model.FisrsbuutonRowCol[0]= 4; 
                model.FisrsbuutonRowCol[1]= 4;
                model.HealCount = 10;
                return true; 
            }
            else if (model.ExitRowCol[0] == model.PlaerRowCol[0]&& model.ExitRowCol[1] == model.PlaerRowCol[1])
            {
                model.FisrsbuutonRowCol[0] = 4;
                model.FisrsbuutonRowCol[1] = 4;
                model.HealCount = 10;
                return true;
            }
           

            return false;
        }

      
    }
}
