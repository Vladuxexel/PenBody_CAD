using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenBody_CadUI.ViewModels
{
    public class PenBodyParametersListVM : ViewModelBase, IDataErrorInfo
    {
        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();
    }
}
