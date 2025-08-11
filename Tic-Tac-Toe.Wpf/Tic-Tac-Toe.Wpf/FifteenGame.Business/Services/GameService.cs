using FifteenGame.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FifteenGame.Business.Services
{

    public class GameService
    { int countPar = 0;
        public void Initialize(GameModel model)
        {
            


            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
                {
                    model[row, column] = "";

                }

            }
        }


        public bool MakeMove(GameModel model)
        {

            if (model[model.FisrsbuutonRowCol[0], model.FisrsbuutonRowCol[1]]=="")
            {
                model[model.FisrsbuutonRowCol[0], model.FisrsbuutonRowCol[1]] = "X";
                ComputerMove(model);
                return true;
            }
            else
            { 
                return false; 
            }
        }  

         public void ComputerMove(GameModel model)
        {   Random random = new Random();
          
            var emptyCells = new List<(int row, int column)>();
            Thread.Sleep(2000);
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
                {
                   if  (model[row, column] == "")
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
            if (IsWingame(model, "X") || IsWingame(model, "O") || IsDraw(model)) {  return true; }


            return false;
        }

        public bool IsWingame(GameModel model,string bor)
        {
            if (IsWinrow(model, bor) || IsWinrcol(model, bor) || IsWinrdig(model, bor))
            {  return true; }
            return false;

           

           

        }
        public bool IsWinrow(GameModel model,string  bor)
        {
            bool win = true;
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
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
            for (int column = 0; column < GameModel.ColumnCount; column++)
            {
                for (int row = 0; row < GameModel.RowCount; row++)
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
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                if (model[row, row] != bor)
                {
                    digright = false;
                   
                }
                if (model[row, GameModel.RowCount - 1 - row] != bor)
                {
                    digleft = false;
                   
                }
                


            }
            return digright || digleft;
        }
        public bool IsDraw(GameModel model)
        {
         
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int col = 0; col < GameModel.ColumnCount; col++)
                {
                    if (model[row, col] == "")
                        return false;
                }
            }
            return true;
        }


    }
}
