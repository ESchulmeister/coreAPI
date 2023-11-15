using System;
using System.Collections.Generic;

#nullable disable

namespace coreAPI.Data
{
    public partial class ContentType
    {
        public ContentType()
        {
            AppUserSettings = new HashSet<AppUserSetting>();
        }

        public int CntId { get; set; }
        public string CntName { get; set; }

        public virtual ICollection<AppUserSetting> AppUserSettings { get; set; }
    }
}
