using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ecuaRefills2.Models
{
    public static class ClaimStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Crear Rol", "Crear Rol"),
            new Claim("Editar Rol", "Editar Rol"),
            new Claim("Eliminar Rol", "Eliminar Rol")
        };
    }
}
