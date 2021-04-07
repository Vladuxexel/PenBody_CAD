using PenBody_Cad.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenBody_CadUI.ViewModels
{
    public class PenBodyParameterVM
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ParamName ParamName { get; set; }
        public bool IsValid { get; set; }

        public PenBodyParameterVM(string name, string value, ParamName paramName)
        {
            Name = name;
            Value = value;
            ParamName = paramName;
            IsValid = true;
        }
    }
}
