﻿

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FifteenGame.Wpf.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        
        private bool _isSelected; 
        public string _cellText;
        

        public int Row { get; set; }

        public int Column { get; set; }

        public int[] ColumnRow { get; set; }




        public string TextCell
        {
            get => _cellText;
            set
            {
                _cellText = value;
                OnPropertyChanged(nameof(TextCell));
            }
        }
        public  string TextButoon => IsFaceUp ? TextCell : "Secret";
        public string BruhButon => IsFaceUp ? "Orange" : "Gray";
        public bool IsFaceUp
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsFaceUp));
                // Изменяем цвет кнопки при выделении
                OnPropertyChanged(nameof(ColorButtonPen)); 
                OnPropertyChanged(nameof(TextButoon));
                OnPropertyChanged(nameof(BruhButon));

            }
        }
        public string _сolorButtonPen = "MediumOrchid"; // Начальный цвет границы

        public string ColorButtonPen => IsFaceUp ? "Cyan" : _сolorButtonPen;
        

        // Метод для проверки совпадения

        public event PropertyChangedEventHandler PropertyChanged;


        

        public void MoveRight()
        {
            if (Column >= 3)
            {
                return;
            }

            Column++;
            OnPropertyChanged(nameof(Column));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
