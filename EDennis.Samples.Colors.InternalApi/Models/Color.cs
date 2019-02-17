using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.Colors.InternalApi.Models
{
    public partial class Color : IHasIntegerId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }

    }
}
