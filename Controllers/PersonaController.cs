using Microsoft.AspNetCore.Mvc;
using ReportesMVC.Models;
using System.Data;
using System.Data.SqlClient;

namespace ReportesMVC.Controllers
{
    public class PersonaController : Controller
    {
        private readonly IConfiguration configuration;
        public PersonaController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Edita(int idPersona)
        {
             idPersona = Convert.ToInt32(HttpContext.Request.RouteValues["id"].ToString());
            Persona persona = GetPersonaPorId(idPersona);
            persona.tipoDocumentacions = getTipoDocumentacions();
            return View(persona);


        }
        [HttpPost]
        public IActionResult GuardarDatos(Persona persona)
        {
            string cadena = configuration.GetConnectionString("LocalConnection");
            using (SqlConnection cnn = new SqlConnection(cadena))
            {
                using (SqlCommand command = new SqlCommand("ActualizarPersona", cnn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IIDPERSONA", persona.IIDPERSONA);
                    command.Parameters.AddWithValue("@Nombre", persona.NOMBRE);
                    command.Parameters.AddWithValue("@Appaterno", persona.APPATERNO);
                    command.Parameters.AddWithValue("@Apmaterno", persona.APMATERNO);
                    command.Parameters.AddWithValue("@IIDSexo", persona.IIDSEXO);
                    command.Parameters.AddWithValue("@Correo", persona.CORREO);
                    command.Parameters.AddWithValue("@TelefonoCelular1", persona.TELEFONOCELULAR1);
                    command.Parameters.AddWithValue("@IIDTipoDocumento", persona.IIDTIPODOCUMENTO);
                    command.Parameters.AddWithValue("@NumeroIdentificacion", persona.NUMEROIDENTIFICACION);
                    cnn.Open();
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index", "Reportes");
        }
        public Persona GetPersonaPorId(int personaId)
        {
            Persona persona = new Persona();
            string cadena = configuration.GetConnectionString("LocalConnection");
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_selPersona", cn);
                cmd.Parameters.AddWithValue("IdPersona", personaId);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        persona =new Persona
                        {
                            IIDPERSONA = reader.GetInt32(0),
                            NOMBRE = reader.GetString(1),
                            APPATERNO = reader.GetString(2),
                            APMATERNO = reader.GetString(3),
                            IIDSEXO = reader.GetInt32(4),
                            CORREO = reader.GetString(5),
                            TELEFONOCELULAR1 = reader.GetString(6),
                            IIDTIPODOCUMENTO = reader.GetInt32(7),
                            NUMEROIDENTIFICACION = reader.GetString(8)
                        };
                    }
                }

            }
            return persona;
        }
        public List<TipoDocumentacion> getTipoDocumentacions()
        {
            List<TipoDocumentacion> tipos = new List<TipoDocumentacion>();
            string cadena = configuration.GetConnectionString("LocalConnection");
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_SelTipoDocumento", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipos.Add(new TipoDocumentacion
                        {
                            IIDTIPODOCUMENTO = reader.GetInt32(0),
                            NOMBRE = reader.GetString(1)
                        });
                    }
                }
            }
            return tipos;
         }
    }
}
