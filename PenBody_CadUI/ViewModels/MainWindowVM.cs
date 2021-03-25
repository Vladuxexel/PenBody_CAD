﻿using PenBody_Cad;
using PenBody_CadUI.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PenBody_CadUI
{
    /// <summary>
    /// Вью-модель окна.
    /// </summary>
    public class MainWindowVM : ViewModelBase
    {
        /// <summary>
        /// Длина основной части ручки по умолчанию.
        /// </summary>
        private const double MAIN_LENGTH = 40;

        /// <summary>
        /// Длина части для резинки по умолчанию.
        /// </summary>
        private const double RUBBER_LENGTH = 20;

        /// <summary>
        /// Диаметр основной ручки по умолчанию.
        /// </summary>
        private const double MAIN_DIAMETER = 15;

        /// <summary>
        /// Внутренний диаметр ручки по умолчанию.
        /// </summary>
        private const double INNER_DIAMETER = 5;

        /// <summary>
        /// Диаметр части для резинки по умолчанию.
        /// </summary>
        private const double RUBBER_DIAMETER = 10;

        /// <summary>
        /// Сообщение нормального состояния плагина.
        /// </summary>
        private const string WAITING_MSG = "Плагин готов к построению";

        /// <summary>
        /// Сообщение состояния плагина при загрузке.
        /// </summary>
        private const string LOADING_MSG = "Запуск КОМПАС-3D...";

        /// <summary>
        /// Сообщение состояния плагина при ошибке.
        /// </summary>
        private const string ERROR_MSG = "Пожалуйста, введите корректные параметры";

        /// <summary>
        /// Флаг состояния загрузки Компас-3D.
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// Цвет иконки "Ок".
        /// </summary>
        private SolidColorBrush _okIconColor;

        /// <summary>
        /// Цвет иконки "Предупреждение".
        /// </summary>
        private SolidColorBrush _warningIconColor;

        /// <summary>
        /// Сообщение состояния плагина.
        /// </summary>
        private string _message;

        /// <summary>
        /// Команда сброса параметров до стандартных.
        /// </summary>
        private RelayCommand _resetCommand;

        /// <summary>
        /// Команда запуска построения детали.
        /// </summary>
        private RelayCommand _buildCommand;

        /// <summary>
        /// Поле для вызова метода построения детали.
        /// </summary>
        private PenBodyBuilder _penBodyBuilder;

        /// <summary>
        /// Свойство валидации параметров модели.
        /// </summary>
        public PenBodyParametersVM PenBodyParametersVM { get; set; }

        /// <summary>
        /// Комструктор класса вью-модели окна.
        /// </summary>
        public MainWindowVM()
        {
            PenBodyParametersVM = new PenBodyParametersVM();
            SetDefaultParams();
            _penBodyBuilder = new PenBodyBuilder();
        }

        /// <summary>
        /// Свойство сообщения состояния плагина.
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        /// <summary>
        /// Свойство флага состояния загрузки Компас-3D.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        /// <summary>
        /// Свойство цвета иконки "Ок".
        /// </summary>
        public SolidColorBrush OkIconColor
        {
            get => _okIconColor;
            set
            {
                _okIconColor = value;
                OnPropertyChanged(nameof(OkIconColor));
            }
        }

        /// <summary>
        /// Свойство цвета иконки "Предупреждение".
        /// </summary>
        public SolidColorBrush WarningIconColor
        {
            get => _warningIconColor;
            set
            {
                _warningIconColor = value;
                OnPropertyChanged(nameof(WarningIconColor));
            }
        }

        /// <summary>
        /// Свойство команды сброса параметров до стандартных.
        /// </summary>
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

        /// <summary>
        /// Свойство команды запуска построения детали.
        /// </summary>
        public RelayCommand BuildCommand
        {
            get
            {
                return _buildCommand ??
                    (_buildCommand = new RelayCommand(async (obj) =>
                    {
                        PenBodyParametersVM.UpdateAll();
                        SetState(States.Loading);
                        await Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                _penBodyBuilder.Build(CreatePenBodyModel());
                            }
                            catch(ArgumentException e)
                            {
                                MessageBox.Show(e.Message, "Предупреждение");
                            }
                        });
                        SetState(States.Ok);
                    },
                    (obj) =>
                    {
                        if (PenBodyParametersVM.Error == null)
                        {
                            if (!IsLoading)
                            {
                                SetState(States.Ok);
                            }
                            
                            return true;
                        }
                        SetState(States.Warning);

                        return false;
                    }
                    ));
            }
        }

        /// <summary>
        /// Метод создания объекта параметров модели.
        /// </summary>
        /// <returns>Модель с набором параметров.</returns>
        private PenBodyParameters CreatePenBodyModel()
        {
            return new PenBodyParameters()
            {
                MainLength = PenBodyParametersVM.MainLength,
                RubberLength = PenBodyParametersVM.RubberLength,
                MainDiameter = PenBodyParametersVM.MainDiameter,
                RubberDiameter = PenBodyParametersVM.RubberDiameter,
                InnerDiameter = PenBodyParametersVM.InnerDiameter
            };
        }

        /// <summary>
        /// Метод установки состояния плагина.
        /// </summary>
        /// <param name="status">Состояние.</param>
        private void SetState(States status)
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

        /// <summary>
        /// Метод сброса параметров модели до стандартных.
        /// </summary>
        private void SetDefaultParams()
        {
            PenBodyParametersVM.MainLength = MAIN_LENGTH;
            PenBodyParametersVM.RubberLength = RUBBER_LENGTH;
            PenBodyParametersVM.MainDiameter = MAIN_DIAMETER;
            PenBodyParametersVM.InnerDiameter = INNER_DIAMETER;
            PenBodyParametersVM.RubberDiameter = RUBBER_DIAMETER;
            Message = WAITING_MSG;
            OkIconColor = new SolidColorBrush(Colors.Green);
            WarningIconColor = new SolidColorBrush(Colors.Gray);
        }
    }
}

/// <summary>
/// Перечисление состояний плагина.
/// </summary>
public enum States
{
    Ok,
    Warning,
    Loading
}