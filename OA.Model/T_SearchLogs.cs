
namespace OA.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class T_SearchLogs
    {
        [Key]
        public System.Guid Id { get; set; }
        public string Word { get; set; }
        public System.DateTime SearchDate { get; set; }
    }
}


