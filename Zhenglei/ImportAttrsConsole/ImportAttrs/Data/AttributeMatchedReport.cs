using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    //[RID]
    //,[CID]
    //,[PID]
    //,[AttType]
    //,[AttTitleID]
    //,[PM_AttName]
    //,[DR_AttName]
    //,[DR_AttValue_Orignal]
    //,[DR_AttValue_Changed]
    //,[SaveToDB]
    //,[AutoImport]
    //,[CreatedBy]
    //,[CreatedOn]
    public class AttributeMatchedReport
    {
        public int RID { get; set; }
        public int CID { get; set; }
        public int PID { get; set; }
        public int AttType { get; set; }
        public int AttTitleID { get; set; }
        public string PM_AttName { get; set; }
        public string DR_AttName { get; set; }
        public string DR_AttValue_Orignal { get; set; }
        public string DR_AttValue_Changed { get; set; }
        public bool SaveToDB { get; set; }
        public bool AutoImport { get; set; }

        public AttributeMatchedReport(int rId, int cId, int pId, int attType, int attTitleId, string pm_AttName, string dr_AttName, string dr_AttValue_Orignal, string dr_AttValue_Changed, bool saveToDB, bool autoImport)
        {
            RID = rId;
            CID = cId;
            PID = pId;
            AttType = attType;
            AttTitleID = attTitleId;
            PM_AttName = pm_AttName;
            DR_AttName = dr_AttName;
            DR_AttValue_Orignal = dr_AttValue_Orignal;
            DR_AttValue_Changed = dr_AttValue_Changed;
            SaveToDB = saveToDB;
            AutoImport = autoImport;
        }
    }
}