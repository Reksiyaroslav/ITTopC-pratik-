using FifteenGame.Business.Services;
using FifteenGame.Common.BusinessModels;
using FifteenGame.Common.Definitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FifteenGame.Wpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private GameService _gameService = new GameService();

        private GameModel _gameModel = new GameModel();

        private UserModel _user;

        public ObservableCollection<CellViewModel> Cells { get; set; } = new ObservableCollection<CellViewModel>();

        public string UserName => _user?.Name ?? "<нет>";

        public string MoveCountText => (_gameModel?.MoveCount ?? 0).ToString();
        
        int[] FisrsbuutonRowCol = { 0, 0 };
        
        public MainWindowViewModel()
        {
            Initialize();
        }

        public void SetUser(UserModel user)
        {
            _user = user;
            OnPropertyChanged(nameof(UserName));

            _gameModel = _gameService.GetByUserId(_user.Id);
            FromModel(_gameModel);
        }
        public void MakeMove(int[] colrow, Action gameFinishedAction)
        {
            FisrsbuutonRowCol = colrow;
                
            _gameModel = _gameService.MakeMove(_gameModel.Id,FisrsbuutonRowCol);
               
          

            FromModel(_gameModel);
            if (_gameService.IsGameOver(_gameModel))
            {
                _gameService.RemoveGame(_gameModel.Id);
                gameFinishedAction?.Invoke();
            }

        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Initialize()
        {
            _gameModel = new GameModel { MoveCount = 0, };
            
            FromModel(_gameModel);
        }

        public void ReInitialize()
        {
            _gameModel = _gameService.GetByUserId(_user.Id);
            FromModel(_gameModel);
        }

        private void FromModel(GameModel model)
        {
            Cells.Clear();
            
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {

                   
                        Cells.Add(item: new CellViewModel
                        {
                            Row = row,
                            ColumnRow = new int[] { row, column },
                            Column = column,
                            Text = model[row, column],

                           

                        });
                   
                }

                OnPropertyChanged(nameof(MoveCountText));
            }
        }
    }
}
