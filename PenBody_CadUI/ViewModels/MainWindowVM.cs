﻿using PenBody_CadUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PenBody_CadUI
{
    public class MainWindowVM : ViewModelBase
    {
        private bool _isLoading;
        private SolidColorBrush _okIconColor;
        private SolidColorBrush _warningIconColor;
        private string _message;
        private RelayCommand _resetCommand;
        private RelayCommand _buildCommand;

        private const double MAIN_LENGTH = 40;
        private const double RUBBER_LENGTH = 20;
        private const double MAIN_DIAMETER = 15;
        private const double INNER_DIAMETER = 5;
        private const double RUBBER_DIAMETER = 10;
        private const string WAITING_MSG = "Введенные данные корректны";
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

        public SolidColorBrush OkIconColor
        {
            get => _okIconColor;
            set
            {
                _okIconColor = value;
                OnPropertyChanged(nameof(OkIconColor));
            }
        }

        public SolidColorBrush WarningIconColor
        {
            get => _warningIconColor;
            set
            {
                _warningIconColor = value;
                OnPropertyChanged(nameof(WarningIconColor));
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
                        SetStatus(States.Loading);
                        await Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(5000);
                        });
                        SetStatus(States.Ok);
                    },
                    (obj) =>
                    {
                        if (PenVM.Error == null)
                        {
                            if (!IsLoading)
                            {
                                SetStatus(States.Ok);
                            }
                            
                            return true;
                        }
                        SetStatus(States.Warning);

                        return false;
                    }
                    ));
            }
        }

        private void SetStatus(States status)
        {
            switch (status)
            {
                case States.Ok:
                    IsLoading = false;
                    Message = WAITING_MSG;
                    OkIconColor = new SolidColorBrush(Colors.Green);
                    WarningIconColor = new SolidColorBrush(Colors.Gray);
                    break;
                case States.Warning:
                    Message = ERROR_MSG;
                    WarningIconColor = new SolidColorBrush(Color.FromRgb(247, 198, 0));
                    OkIconColor = new SolidColorBrush(Colors.Gray);
                    IsLoading = false;
                    break;
                case States.Loading:
                    IsLoading = true;
                    Message = LOADING_MSG;
                    OkIconColor = new SolidColorBrush(Colors.Gray);
                    WarningIconColor = new SolidColorBrush(Colors.Gray);
                    break;
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
            OkIconColor = new SolidColorBrush(Colors.Green);
            WarningIconColor = new SolidColorBrush(Colors.Gray);
        }
    }
}

public enum States
{
    Ok,
    Warning,
    Loading
}