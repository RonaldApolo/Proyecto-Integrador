using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ecuaRefills2.Entities
{
    [Index(nameof(Cedula), IsUnique = true)]
    public class Cliente
    {
        public int ClienteId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Cédula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Nombre Completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress (ErrorMessage ="Ingrese un correo electrónico valido")]
        [DisplayName("Correo Electrónico")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayName("Teléfono")]
        public int Telefono { get; set; }

        public int CiudadId { get; set; }
        public Ciudad Ciudad { get; set; }
    }
}
