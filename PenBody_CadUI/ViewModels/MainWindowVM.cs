using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PenBody_CadUI
{
    public class MainWindowVM : ViewModelBase
    {
        private double _mainLength;
        private double _rubberLength;
        private double _mainDiameter;
        private double _innerDiameter;
        private double _rubberDiameter;
        private RelayCommand _resetCommand;
        private RelayCommand _buildCommand;

        private const double MAIN_LENGTH = 40;
        private const double RUBBER_LENGTH = 20;
        private const double MAIN_DIAMETER = 15;
        private const double INNER_DIAMETER = 5;
        private const double RUBBER_DIAMETER = 10;

        public MainWindowVM()
        {
            SetDefaultParams();
        }

        public double MainLength
        {
            get => _mainLength;
            set
            {
                _mainLength = value;
                OnPropertyChanged(nameof(MainLength));
            }
        }

        public double RubberLength
        {
            get => _rubberLength;
            set
            {
                _rubberLength = value;
                OnPropertyChanged(nameof(RubberLength));
            }
        }

        public double MainDiameter
        {
            get => _mainDiameter;
            set
            {
                _mainDiameter = value;
                OnPropertyChanged(nameof(MainDiameter));
            }
        }

        public double InnerDiameter
        {
            get => _innerDiameter;
            set
            {
                _innerDiameter = value;
                OnPropertyChanged(nameof(InnerDiameter));
            }
        }

        public double RubberDiameter
        {
            get => _rubberDiameter;
            set
            {
                _rubberDiameter = value;
                OnPropertyChanged(nameof(RubberDiameter));
            }
        }

        public RelayCommand ResetCommand
        {
            get
            {
                return _resetCommand ??
                    (_resetCommand = new RelayCommand((obj) =>
                    {
                        SetDefaultParams();
                    }));
            }
        }

        public RelayCommand BuildCommand
        {
            get
            {
                return _buildCommand ??
                    (_buildCommand = new RelayCommand((obj) =>
                    {
                        MessageBox.Show("Biulder works");
                    }));
            }
        }

        private void SetDefaultParams()
        {
            MainLength = MAIN_LENGTH;
            RubberLength = RUBBER_LENGTH;
            MainDiameter = MAIN_DIAMETER;
            InnerDiameter = INNER_DIAMETER;
            RubberDiameter = RUBBER_DIAMETER;
        }
    }
}
