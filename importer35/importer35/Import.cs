using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace importer
{
    class Import
    {
        public Import(bool allow_duplicates, bool overlay_commissions, string account_tag)
        {
            this.allow_duplicates = allow_duplicates;
            this.overlay_commissions = overlay_commissions;
            this.account_tag = account_tag;
            tags = new List<string>();
            executions = new List<Execution>();
        }

        public void AddTag(string tag)
        {
            tags.Add(tag);
        }

        public void AddExecution(Execution exec)
        {
            executions.Add(exec);
        }

        public string ToJson()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize((object)this);
        }

        public bool allow_duplicates;
        public bool overlay_commissions;
        public string account_tag;
        public List<string> tags;
        public List<Execution> executions;
    }
}
