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
        public string HealCountText => (_gameModel?.HealCount ?? 0).ToString();


       
        
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
        public async void MakeMove(int[] colrow, Action gameFinishedAction)
        {
            int [] FisrsbuutonRowCol = colrow;
            FromModel(_gameModel);
            await Task.Delay(300);
            _gameModel = _gameService.CheckMatch(_gameModel.Id,FisrsbuutonRowCol);
               
          

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
            _gameModel = new GameModel { MoveCount = 0,HealCount=10 };
            
            FromModel(_gameModel);
        }

        public void ReInitialize()
        {
            _gameModel = _gameService.GetByUserId(_user.Id);
            FromModel(_gameModel);
        }

        private void FromModel(GameModel model, int[] FisrsbuutonRowCol=null)
        {
            Cells.Clear();
            
            for (int row = 0; row < Constants.RowCount; row++)
            {
                for (int column = 0; column < Constants.ColumnCount; column++)
                {
                    bool isonerowcol = FisrsbuutonRowCol != null
                       && FisrsbuutonRowCol.Length == 2 &&
                       row == FisrsbuutonRowCol[0] && column == FisrsbuutonRowCol[1];
                    bool isplaerrowcol = model.PlaerRowCol != null && model.PlaerRowCol.Length == 2 
                        && model.PlaerRowCol[0]==row&& model.PlaerRowCol[1]==column;


                    if (string.IsNullOrEmpty(model[row, column]))
                    {
                        continue;
                    }

                    Cells.Add(item: new CellViewModel
                    {
                        Row = row,
                        ColumnRow = new int[] { row, column },
                        Column = column,
                        TextCell = model[row, column],

                        IsFaceUp = isonerowcol|| isplaerrowcol,

                    });
                    

                }

                OnPropertyChanged(nameof(MoveCountText));
                OnPropertyChanged(nameof(HealCountText));
            }
        }
    }
}
