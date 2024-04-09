using System;
using System.Drawing;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Association_Tests.Prompts
{
    class PromptDateTime : BasePrompt
    {
        private readonly RadDateTimePicker dateTimePicker;
        public PromptDateTime()
        {
            dateTimePicker = new RadDateTimePicker();
            //   Value = DateTime.Now;

        }

        public override RadControl Control
        {
            get { return dateTimePicker; }
        }

        public override void MarkedAsMandatory()
        {
            dateTimePicker.DateTimePickerElement.TextBoxElement.BackColor = MandatoryColor;
        }


        public override void SetDefaultText(DAL.AliquotTemplateField field)
        {
            dateTimePicker.Value = field != null ? Convert.ToDateTime(field.DefaultText) : DateTime.Now;
        }

        public override void SetValue(string value)
        {
            dateTimePicker.Value = !string.IsNullOrEmpty(value) ? Convert.ToDateTime(value) : DateTime.Now;

        }

        public override string GetValue()
        {
            return dateTimePicker.Value.ToString();
        }
    }
}