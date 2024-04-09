using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace Association_Tests.Prompts
{
    class PromptCheckBox : BasePrompt
    {
        private readonly RadCheckBox checkBox;
        public PromptCheckBox()
        {
            checkBox = new RadCheckBox();
            checkBox.IsThreeState = true;
         
        }

        public override RadControl Control
        {
            get { return checkBox; }
        }

        public override void MarkedAsMandatory()
        {
            checkBox.BackColor = MandatoryColor;
        }

        public override void SetDefaultText(DAL.AliquotTemplateField field)
        {
            checkBox.IsChecked = field.DefaultText == "T";
        }

        public override void SetValue(string value)
        {
            checkBox.IsChecked = value == "True";
        }

        public override string GetValue()
        {
            return checkBox.IsChecked.ToString();
        }

    }
}