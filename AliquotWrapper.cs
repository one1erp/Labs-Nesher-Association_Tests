using System.Collections.Generic;
using Association_Tests.Prompts;
using DAL;


namespace Association_Tests
{
    public class AliquotWrapper
    {

        #region Members

        public AliquotTemplate AliquotTemplate { get; set; }
        public List<BasePrompt> Prompts { get; set; }
        public bool OldValue { get; set; }
        public bool NewValue { get; set; }
        
        
        public string SampleNameKey { get; set; }
        public string WorkFlowNameKey { get; set; }
        public string TestTemplateExNameKey { get; set; }

        public bool HasReTest { get; set; }
        public bool AddedRetest { get; set; }
    
        #endregion



        #region Ctor

        public AliquotWrapper(AliquotTemplate aliquotTemplate)
        {
            Prompts = new List<BasePrompt>();

            this.AliquotTemplate = aliquotTemplate;
           

        }

        #endregion


    }



}