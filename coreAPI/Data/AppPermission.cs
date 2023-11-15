using System;
using System.Collections.Generic;

#nullable disable

namespace coreAPI.Data
{
    public partial class AppPermission
    {
        public AppPermission()
        {
            AppUserPermissions = new HashSet<AppUserPermission>();
        }

        public int ApId { get; set; }
        public int AppId { get; set; }
        public string PermName { get; set; }
        public bool? ApActive { get; set; }
        public string ApCreatedBy { get; set; }
        public DateTime ApCreatedDate { get; set; }
        public string ApModifiedBy { get; set; }
        public DateTime ApModifiedDate { get; set; }

        public virtual Application App { get; set; }
        public virtual ICollection<AppUserPermission> AppUserPermissions { get; set; }
    }
}
