using FifteenGame.Common.BusinessModels;
using FifteenGame.Common.Definitions;
using FifteenGame.Common.Infrastructure;
using FifteenGame.Common.Services;
using Ninject;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FifteenGame.Wpf.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IGameService _service;

        private GameModel _model = new GameModel();

        private UserModel _userModel;

        public ObservableCollection<CellViewModel> Cells { get; set; } = new ObservableCollection<CellViewModel>();

        public string UserName => _userModel?.Name ?? "<нет>";

        public string MoveCountText => (_model?.MoveCount ?? 0).ToString();

        public string HealCountText => (_model?.HealCount ?? 0).ToString();

        public MainWindowViewModel()
        {
            _service = NinjectKernel.Instance.Get<IGameService>();
        }

        public async void MakeMove(int[] colrow, Action gameFinishedAction)
        {
            int[] FisrsbuutonRowCol = colrow;
            FromModel(_model,FisrsbuutonRowCol);
            await Task.Delay(2000);
            _model = _service.CheckMatch(_model.GameId, FisrsbuutonRowCol);
      
            FromModel(_model);
            if (_service.IsGameOver(_model.GameId))
            {
                _service.RemoveGame(_model.GameId);
                gameFinishedAction();
            }
           
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Initialize()
        {
            _model = _service.GetByUserId(_userModel.Id);
            FromModel(_model);
        }

        public void SetUser(UserModel user)
        {
            _userModel = user;
            Initialize();
            OnPropertyChanged(nameof(UserName));
        }

        private void FromModel(GameModel model, int[] FisrsbuutonRowCol = null)
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
                        && model.PlaerRowCol[0] == row && model.PlaerRowCol[1] == column;


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

                        IsFaceUp = isonerowcol || isplaerrowcol,

                    });


                }

                OnPropertyChanged(nameof(MoveCountText));
                OnPropertyChanged(nameof(HealCountText));
            }
        }
    }
}

