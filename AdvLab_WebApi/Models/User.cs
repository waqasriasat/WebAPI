using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System;

namespace AdvLab_WebApi.Models
{
    public class User
    {
        public int ID { get; set; }
        public string? LoginStatus { get; set; }
        public string? LoginStatusIP { get; set; }
        public string? LoginStatusComp { get; set; }
        public string? NSend { get; set; }
        public DateTime? RegDate { get; set; }
        public int? EmpID { get; set; }
        public string? UName { get; set; }
        public string UPassword { get; set; }
        public string? ClientV { get; set; }
        public string? Status { get; set; }
        public string? Place { get; set; }
        public string? PlaceCategory { get; set; }
        public string? CNL { get; set; }
        public string? Location { get; set; }
        public string? Depart { get; set; }
        public string? SubDepart { get; set; }
        public string? Placement { get; set; }
        public string? Designation { get; set; }
        public int? UId { get; set; }
        public string? Idloc { get; set; }
        public string? CompMac { get; set; }
        public string? PayGenerate { get; set; }
        public int? RoleID { get; set; }
        public string? Token { get; set; }
       
    }
}
