namespace ReportesMVC.Models
{
    public class Persona
    {
        public int IIDPERSONA { get; set; }
        public string NOMBRE { get; set; }
        public string APPATERNO { get; set; }
        public string APMATERNO { get; set; }
        public int IIDSEXO { get; set; }
        public string CORREO { get; set; }
        public string TELEFONOCELULAR1 { get; set; }
        public int IIDTIPODOCUMENTO { get; set; }
        public string NUMEROIDENTIFICACION { get; set; }

        public List<TipoDocumentacion> tipoDocumentacions { get; set; }
    }
}
