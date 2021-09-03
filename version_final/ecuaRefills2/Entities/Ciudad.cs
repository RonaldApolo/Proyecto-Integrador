using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecuaRefills2.Entities
{
    public class Ciudad
    {
        public int CiudadId { get; set; }
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        //[ForeignKey("Provincia")]
        [DisplayName("Provincia")]
        public int ProvinciaId { get; set; }
        public Provincia Provincia { get; set; }
    }
}