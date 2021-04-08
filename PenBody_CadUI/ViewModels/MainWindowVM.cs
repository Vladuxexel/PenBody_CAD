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
        /// ��������� ����������� ��������� �������.
        /// </summary>
        private const string WAITING_MSG = "������ ����� � ����������";

        /// <summary>
        /// ��������� ��������� ������� ��� ��������.
        /// </summary>
        private const string LOADING_MSG = "������ ������-3D...";

        /// <summary>
        /// ��������� ��������� ������� ��� ������.
        /// </summary>
        private const string ERROR_MSG = "����������, ������� ���������� ���������";

        /// <summary>
        /// ���� ��������� �������� ������-3D.
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// ���� ������ "��".
        /// </summary>
        private SolidColorBrush _okIconColor;

        /// <summary>
        /// ���� ������ "��������������".
        /// </summary>
        private SolidColorBrush _warningIconColor;

        /// <summary>
        /// ��������� ��������� �������.
        /// </summary>
        private string _message;

        /// <summary>
        /// ���� ��� ������ ������ ���������� ������.
        /// </summary>
        private readonly PenBodyBuilder _penBodyBuilder;

        /// <summary>
        /// �������� ��������� ���������� ������.
        /// </summary>
        public PenBodyParametersListVM PenBodyParametersListVM { get; set; }

        /// <summary>
        /// ����������� ������ ���-������ ����.
        /// </summary>
        public MainWindowVM()
        {
            PenBodyParametersListVM = new PenBodyParametersListVM();
            SetDefaultParams();
            _penBodyBuilder = new PenBodyBuilder();
            ResetCommand = new RelayCommand(SetDefaultParams);
            BuildCommand = new RelayCommand(Build, CanBuild);
        }

        /// <summary>
        /// �������� ��������� ��������� �������.
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
        /// �������� ����� ��������� �������� ������-3D.
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
        /// �������� ����� ������ "��".
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
        /// �������� ����� ������ "��������������".
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
        /// ������� ������ ���������� �� �����������.
        /// </summary>
        public RelayCommand ResetCommand { get; private set; }

        /// <summary>
        /// ������� ������� ���������� ������.
        /// </summary>
        public RelayCommand BuildCommand { get; private set; }

        /// <summary>
        /// ����� ��������� ��������� �������.
        /// </summary>
        /// <param name="status">���������.</param>
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
        /// ����� ������ ���������� ������ �� �����������.
        /// </summary>
        private void SetDefaultParams()
        {
            PenBodyParametersListVM.SetToDefault();
            Message = WAITING_MSG;
            OkIconColor = new SolidColorBrush(Colors.Green);
            WarningIconColor = new SolidColorBrush(Colors.Gray);
        }

        private async void Build()
        {
            SetState(State.Loading);
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    _penBodyBuilder.Build(PenBodyParametersListVM.GetValidModel());
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show(e.Message, "��������������");
                }
            });
            SetState(State.Ok);
        }

        private bool CanBuild()
        {
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
    }
}