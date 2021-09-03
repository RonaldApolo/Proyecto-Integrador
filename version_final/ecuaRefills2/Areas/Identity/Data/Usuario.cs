using ecuaRefills2.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ecuaRefills2.Areas.Identity.Data
{
    public class Usuario : IdentityUser
    {
        [PersonalData]
        public string Cedula { get; set; }
        [PersonalData]
        public string Nombre { get; set; }
        [PersonalData]
        public string NombreCompleto { get; set; }
        [PersonalData]
        public string Telefono { get; set; }
        [PersonalData]
        public string Dirección { get; set; }
    }
}
