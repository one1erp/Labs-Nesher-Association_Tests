using System.Drawing;
using Common;
using DAL;
using Telerik.WinControls;

namespace Association_Tests.Prompts
{
    public abstract class BasePrompt
    {

        protected Color MandatoryColor = NautilusDesign.MandatoryColor;

        public string PromptText { get; set; }

        public abstract RadControl Control { get; }
      
        public string DataBaseName { get; set; }

        public bool Prompted { get; set; }

        public bool Displayed { get; set; }

        public bool IsMandatory
        {
            get { return _isMandatory; }
            set
            {
                _isMandatory = value;
                if (value)
                    MarkedAsMandatory();
            }
        }

        private bool _isMandatory;

        public abstract void MarkedAsMandatory();

        public void SetDefaultBorder()
        {
        }



        public abstract void SetDefaultText(AliquotTemplateField field);

        public abstract void SetValue(string value);

        public abstract string GetValue();




   
    }
}
