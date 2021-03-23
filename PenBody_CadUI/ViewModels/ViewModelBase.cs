using System.ComponentModel;

namespace PenBody_CadUI
{
    /// <summary>
    /// Базовая вью-модель.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод обработки события изменения свойства.
        /// </summary>
        /// <param name="propertyName">Изменяемое свойство.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
