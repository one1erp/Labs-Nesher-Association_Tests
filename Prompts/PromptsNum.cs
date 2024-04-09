using System.Drawing;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Association_Tests.Prompts
{
    sealed class PromptsNum : BasePrompt
    {
        private readonly RadSpinEditor spinEditor;
        public PromptsNum()
        {
            spinEditor = new RadSpinEditor();
        }

        public override RadControl Control
        {
            get { return spinEditor; }

        }

        public override void MarkedAsMandatory()
        {
            spinEditor.SpinElement.BackColor = MandatoryColor;
        }
        public override void SetDefaultText(DAL.AliquotTemplateField field)
        {

            if (field.DefaultNum != null)
            {
                spinEditor.Value = (decimal)field.DefaultNum;


            }

        }

        public override void SetValue(string value)
        {
            decimal x;
            if (value != null && decimal.TryParse(value, out x))
                spinEditor.Value = x;

        }

        public override string GetValue()
        {
            return spinEditor.Value.ToString();
        }
    }
}