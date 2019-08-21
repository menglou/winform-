using System.ComponentModel;

namespace SkyCar.Coeus.Common.ExtendClass.Interface
{
    public interface IPropertyChanged
    {
        /// <summary>
        /// 表示属性改变
        /// </summary>
        [DefaultValue(false)]
        bool PropertyValueChanged { get; set; }
    }
}
