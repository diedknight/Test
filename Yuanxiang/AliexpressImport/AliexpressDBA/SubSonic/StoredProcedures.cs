


  
using System;
using SubSonic;
using SubSonic.Schema;
using SubSonic.DataProviders;
using System.Data;

namespace AliexpressDBA{
	public partial class AliexpressDBDB{

        public StoredProcedure CategoryLoadAllPaged(){
            StoredProcedure sp=new StoredProcedure("CategoryLoadAllPaged",this.Provider);
            return sp;
        }
        public StoredProcedure DeleteGuests(){
            StoredProcedure sp=new StoredProcedure("DeleteGuests",this.Provider);
            return sp;
        }
        public StoredProcedure FullText_Disable(){
            StoredProcedure sp=new StoredProcedure("FullText_Disable",this.Provider);
            return sp;
        }
        public StoredProcedure FullText_Enable(){
            StoredProcedure sp=new StoredProcedure("FullText_Enable",this.Provider);
            return sp;
        }
        public StoredProcedure FullText_IsSupported(){
            StoredProcedure sp=new StoredProcedure("FullText_IsSupported",this.Provider);
            return sp;
        }
        public StoredProcedure ImportResourcesNopTemplates(){
            StoredProcedure sp=new StoredProcedure("ImportResourcesNopTemplates",this.Provider);
            return sp;
        }
        public StoredProcedure LanguagePackImport(){
            StoredProcedure sp=new StoredProcedure("LanguagePackImport",this.Provider);
            return sp;
        }
        public StoredProcedure ProductLoadAllPaged(){
            StoredProcedure sp=new StoredProcedure("ProductLoadAllPaged",this.Provider);
            return sp;
        }
        public StoredProcedure ProductLoadAllPagedNopAjaxFilters(){
            StoredProcedure sp=new StoredProcedure("ProductLoadAllPagedNopAjaxFilters",this.Provider);
            return sp;
        }
        public StoredProcedure ProductTagCountLoadAll(){
            StoredProcedure sp=new StoredProcedure("ProductTagCountLoadAll",this.Provider);
            return sp;
        }
	
	}
	
}
 