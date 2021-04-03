using PenBody_Cad;
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
        public PenBodyParametersListVM PenBodyParametersListVM { get; set; }

        /// <summary>
        /// Конструктор класса вью-модели окна.
        /// </summary>
        public MainWindowVM()
        {
            PenBodyParametersListVM = new PenBodyParametersListVM();
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
                        PenBodyParametersListVM.UpdateAll();
                        SetState(State.Loading);
                        await Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                _penBodyBuilder.Build(PenBodyParametersListVM.GetValidModel());
                            }
                            catch(ArgumentException e)
                            {
                                MessageBox.Show(e.Message, "Предупреждение");
                            }
                        });
                        SetState(State.Ok);
                    },
                    (obj) =>
                    {
                        if (PenBodyParametersListVM.Error == null)
                        {
                            PenBodyParametersListVM.UpdateAll();
                            if (!IsLoading)
                            {
                                SetState(State.Ok);
                            }
                            
                            return true;
                        }
                        SetState(State.Warning);

                        return false;
                    }
                    ));
            }
        }

        /// <summary>
        /// Метод установки состояния плагина.
        /// </summary>
        /// <param name="status">Состояние.</param>
        private void SetState(State status)
        {
            switch (status)
            {
                case State.Ok:
                    IsLoading = false;
                    Message = WAITING_MSG;
                    OkIconColor = new SolidColorBrush(Colors.Green);
                    WarningIconColor = new SolidColorBrush(Colors.Gray);
                    break;
                case State.Warning:
                    Message = ERROR_MSG;
                    WarningIconColor = new SolidColorBrush(Color.FromRgb(247, 198, 0));
                    OkIconColor = new SolidColorBrush(Colors.Gray);
                    IsLoading = false;
                    break;
                case State.Loading:
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
            PenBodyParametersListVM.SetToDefault();
            Message = WAITING_MSG;
            OkIconColor = new SolidColorBrush(Colors.Green);
            WarningIconColor = new SolidColorBrush(Colors.Gray);
        }
    }
}

/// <summary>
/// Перечисление состояний плагина.
/// </summary>
public enum State
{
    Ok,
    Warning,
    Loading
}