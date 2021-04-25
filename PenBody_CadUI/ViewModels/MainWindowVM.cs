using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PenBody_Cad;
using PenBody_CadUI.Enums;
using PenBody_CadUI.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PenBody_CadUI.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        /// <summary>
        /// Сообщение нормального состояния плагина.
        /// </summary>
        private const string WaitingMsg = 
            "Плагин готов к построению";

        /// <summary>
        /// Сообщение состояния плагина при загрузке.
        /// </summary>
        private const string LoadingMsg = 
            "Запуск КОМПАС-3D...";

        /// <summary>
        /// Сообщение состояния плагина при ошибке.
        /// </summary>
        private const string ErrorMsg = 
            "Пожалуйста, введите корректные параметры";

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
        /// Строитель детали.
        /// </summary>
        private readonly PenBodyBuilder _penBodyBuilder;

        /// <summary>
        /// Вью-модель параметров.
        /// </summary>
        public PenBodyParametersListVM 
            PenBodyParametersListVM { get; set; }

        /// <summary>
        /// Конструктор класса вью-модели окна.
        /// </summary>
        public MainWindowVM()
        {
            PenBodyParametersListVM = new PenBodyParametersListVM();
            SetDefaultParams();
            _penBodyBuilder = new PenBodyBuilder();
            ResetCommand = new RelayCommand(SetDefaultParams);
            BuildCommand = new RelayCommand(Build, CanBuild);
            ChangeRibbingCommand = new RelayCommand<bool>(SetRibbing, PenBodyParametersListVM.IsValid());
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
                RaisePropertyChanged(nameof(Message));
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
                RaisePropertyChanged(nameof(IsLoading));
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
                RaisePropertyChanged(nameof(OkIconColor));
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
                RaisePropertyChanged(nameof(WarningIconColor));
            }
        }

        /// <summary>
        /// Команда сброса параметров до стандартных.
        /// </summary>
        public RelayCommand ResetCommand { get; private set; }

        /// <summary>
        /// Команда запуска построения детали.
        /// </summary>
        public RelayCommand BuildCommand { get; private set; }

        /// <summary>
        /// Команда установки флага ребристости корпуса ручки.
        /// </summary>
        public RelayCommand<bool> 
            ChangeRibbingCommand { get; private set; }


        /// <summary>
        /// Метод установки состояния плагина.
        /// </summary>
        /// <param name="status">Статус.</param>
        private void SetState(State status)
        {
            switch (status)
            {
                case State.Ok:
                    IsLoading = false;
                    Message = WaitingMsg;
                    OkIconColor = new SolidColorBrush(Colors.Green);
                    WarningIconColor = 
                        new SolidColorBrush(Colors.Gray);
                    break;
                case State.Warning:
                    Message = ErrorMsg;
                    WarningIconColor = new SolidColorBrush(
                        Color.FromRgb(247, 198, 0));
                    OkIconColor = new SolidColorBrush(Colors.Gray);
                    IsLoading = false;
                    break;
                case State.Loading:
                    IsLoading = true;
                    Message = LoadingMsg;
                    OkIconColor = new SolidColorBrush(Colors.Gray);
                    WarningIconColor =
                        new SolidColorBrush(Colors.Gray);
                    break;
            }
        }

        /// <summary>
        /// Метод сброса параметров модели до стандартных.
        /// </summary>
        private void SetDefaultParams()
        {
            PenBodyParametersListVM.SetToDefault();
            Message = WaitingMsg;
            OkIconColor = new SolidColorBrush(Colors.Green);
            WarningIconColor = new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// Метод построения модели.
        /// </summary>
        private async void Build()
        {
            SetState(State.Loading);
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    _penBodyBuilder
                    .Build(PenBodyParametersListVM.GetValidModel());
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show(e.Message, "Предупреждение");
                }
            });
            SetState(State.Ok);
        }

        /// <summary>
        /// Определение возможности построения модели.
        /// </summary>
        /// <returns></returns>
        private bool CanBuild()
        {
            PenBodyParametersListVM
                .RaisePropertyChanged(string.Empty);
            if (PenBodyParametersListVM.IsValid())
            {
                if (!IsLoading)
                {
                    SetState(State.Ok);
                }

                return true;
            }
            SetState(State.Warning);

            return false;
        }

        /// <summary>
        /// Установка наличия ребер ручки.
        /// </summary>
        /// <param name="isRibbed">Наличие рёбер</param>
        private void SetRibbing(bool isRibbed)
        {
            PenBodyParametersListVM.IsRibbed = isRibbed;
        }
    }
}