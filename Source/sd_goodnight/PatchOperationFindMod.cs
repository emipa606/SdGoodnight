using System.Linq;
using System.Xml;
using Verse;

namespace sd_goodnight
{
    // Token: 0x02000002 RID: 2
    internal class PatchOperationFindMod : PatchOperation
    {
        // Token: 0x04000001 RID: 1
        private string modName;

        // Token: 0x06000001 RID: 1 RVA: 0x00002074 File Offset: 0x00001074
        protected override bool ApplyWorker(XmlDocument xml)
        {
            return !modName.NullOrEmpty() && ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name.Contains(modName));
        }
    }
}