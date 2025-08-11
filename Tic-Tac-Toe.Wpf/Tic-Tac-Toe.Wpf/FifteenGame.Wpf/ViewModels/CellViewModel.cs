using FifteenGame.Business.Models;
using FifteenGame.Business.Services;
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
        
        
        public string _Text;
        

        public int Row { get; set; }

        public int Column { get; set; }

        public int[] ColumnRow { get; set; }




        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
       
      
        

      
        

      

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
