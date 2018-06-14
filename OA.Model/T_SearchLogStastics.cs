
namespace OA.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class T_SearchLogStastics
    {
        [Key]
        public string Word { get; set; }
        public long SearchCount { get; set; }
    }
}
