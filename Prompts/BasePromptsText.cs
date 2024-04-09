using System.Drawing;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Association_Tests.Prompts
{
    class PromptText : BasePrompt
    {
        private readonly RadTextBox radTextBox;
        public PromptText()
        {
            radTextBox=new RadTextBox();
        
          
        }


        public override RadControl Control
        {
            get { return radTextBox; }

        }

        public override void MarkedAsMandatory()
        {
            radTextBox.TextBoxElement.BackColor = MandatoryColor;
        }
        public override void SetDefaultText(DAL.AliquotTemplateField field)
        {
            radTextBox.Text = field.DefaultText;
           
        }

        public override void SetValue(string value)
        {
            radTextBox.Text = value;

        }

        public override string GetValue()
        {
            return radTextBox.Text;
        }
    }
}