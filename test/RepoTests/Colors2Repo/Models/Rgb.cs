using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace Colors2.Models
{
    public class Rgb: IHasIntegerId, IEFCoreTemporalModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public string SysUser { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }
        public string SysUserNext { get; set; }
    }
}
