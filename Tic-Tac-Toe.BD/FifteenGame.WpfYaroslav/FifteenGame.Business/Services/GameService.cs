﻿using FifteenGame.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifteenGame.Business.Services
{

    public class GameService
    { int countPar = 0;
        public void Initialize(GameModel model)
        {
            string[] markCar = { "Ford", "Geely ", "Lada", "BMW", "Tesla", "DS", "Jaguar ", "Skoda " };
            Random random = new Random();


            List<string> markCarPairs = new List<string>();

            foreach (var color in markCar)
            {
                markCarPairs.Add(color);
                markCarPairs.Add(color);
            }



            markCarPairs = markCarPairs.OrderBy(x => random.Next()).ToList();


            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
                {
                    model[row, column] = markCarPairs[row * GameModel.ColumnCount + column];

                }

            }
        }


        public bool CheckMatch(GameModel model)
        {

            if (model.Fistbuuton == null || model.Secondbuuton == null) 
            {
                return false;
            }

            else if (model.Fistbuuton == model.Secondbuuton && (model.FisrsbuutonRowCol[0] != model.SecondbuutonRowCol[0]
                || model.FisrsbuutonRowCol[1] != model.SecondbuutonRowCol[1]) )
            {
                countPar++;

                for (int row = 0; row < GameModel.RowCount; row++)
                {
                    for (int column = 0; column < GameModel.ColumnCount; column++)
                    {
                        if ((row == model.FisrsbuutonRowCol[0] &&
                            column == model.FisrsbuutonRowCol[1]) || (row == model.SecondbuutonRowCol[0] &&
                            column == model.SecondbuutonRowCol[1]))
                        {

                            model[row, column] = "";


                        }
                        else
                        {
                            model[row, column] = model[row, column];
                        }

                    }
                   

                }
                model.SecondbuutonRowCol[0] = 4;
                model.SecondbuutonRowCol[1] = 4;
                model.FisrsbuutonRowCol[0] = 4;
                model.FisrsbuutonRowCol[1] = 4;

                return true;
            }
            else
            { 
                return false; 
            }
       }  

        

        public bool IsGameOver(GameModel model)
        {
            if (countPar == 8)
            {
                model.SecondbuutonRowCol[0]= 4; 
                model.SecondbuutonRowCol[1]= 4; 
                model.FisrsbuutonRowCol[0]= 4; 
                model.FisrsbuutonRowCol[1]= 4;
                countPar = 0;
                return true; 
            }


            return false;
        }

      
    }
}
