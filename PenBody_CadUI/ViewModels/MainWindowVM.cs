using PenBody_CadUI.ViewModels;
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
        private bool _isLoading;
        private string _message;
        private RelayCommand _resetCommand;
        private RelayCommand _buildCommand;

        private const double MAIN_LENGTH = 40;
        private const double RUBBER_LENGTH = 20;
        private const double MAIN_DIAMETER = 15;
        private const double INNER_DIAMETER = 5;
        private const double RUBBER_DIAMETER = 10;
        private const string WAITING_MSG = "Ожидается запуск процесса построения";
        private const string LOADING_MSG = "Запуск КОМПАС-3D...";
        private const string ERROR_MSG = "Пожалуйста, введите корректные параметры";

        public PenVM PenVM { get; set; }

        public MainWindowVM()
        {
            PenVM = new PenVM();
            SetDefaultParams();
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

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
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
                        PenVM.UpdateAll();
                        IsLoading = true;
                        Message = LOADING_MSG;
                        await Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(5000);
                        });
                        IsLoading = false;
                        Message = WAITING_MSG;
                    },
                    (obj) =>
                    {
                        if (PenVM.Error == null)
                        {
                            if (!IsLoading)
                            {
                               Message = WAITING_MSG;
                            }
                            
                            return true;
                        }

                        Message = ERROR_MSG;
                        return false;
                    }
                    ));
            }
        }

        private void SetDefaultParams()
        {
            PenVM.MainLength = MAIN_LENGTH;
            PenVM.RubberLength = RUBBER_LENGTH;
            PenVM.MainDiameter = MAIN_DIAMETER;
            PenVM.InnerDiameter = INNER_DIAMETER;
            PenVM.RubberDiameter = RUBBER_DIAMETER;
            Message = WAITING_MSG;
        }
    }
}
