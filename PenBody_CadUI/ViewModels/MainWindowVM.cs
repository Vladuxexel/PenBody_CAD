using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool _isLoading;
        private string _message;
        private RelayCommand _resetCommand;
        private RelayCommand _buildCommand;

        private const double MAIN_LENGTH = 40;
        private const double RUBBER_LENGTH = 20;
        private const double MAIN_DIAMETER = 15;
        private const double INNER_DIAMETER = 5;
        private const double RUBBER_DIAMETER = 10;
        private const string WAITING_MSG = "Ожидание запуска процесса построения";
        private const string LOADING_MSG = "Запуск КОМПАС-3D...";
        private const string ERROR_MSG = "Пожалуйста, введите корректные параметры";

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

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
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
                    (_buildCommand = new RelayCommand(async (obj) =>
                    {
                        IsLoading = true;
                        Message = LOADING_MSG;
                        await Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(5000);;
                        });
                        IsLoading = false;
                        Message = WAITING_MSG;
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
            Message = WAITING_MSG;
        }
    }
}
