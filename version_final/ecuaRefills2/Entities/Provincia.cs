using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecuaRefills2.Entities
{
    public class Provincia
    {
        public int ProvinciaId { get; set; }
        public string Nombre { get; set; }
        public ICollection<Ciudad> Ciudades { get; set; }
    }
}
