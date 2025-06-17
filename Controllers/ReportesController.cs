using Microsoft.AspNetCore.Mvc;
using ReportesMVC.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ReportesMVC.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IConfiguration configuration;
        public ReportesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            var personas = GetPersonas();
            return View(personas);
        }
        public List<Persona> GetPersonas()
        {
            List<Persona> personas = new List<Persona>();
            string cadena = configuration.GetConnectionString("LocalConnection");
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_selPersona", cn);
                cmd.Parameters.AddWithValue("IdPersona", 0);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        personas.Add(new Persona
                        {
                            IIDPERSONA = reader.GetInt32(0),
                            NOMBRE= reader.GetString(1),
                            APPATERNO= reader.GetString(2),
                            APMATERNO= reader.GetString(3),
                            IIDSEXO= reader.GetInt32(4),
                            CORREO= reader.GetString(5),
                            TELEFONOCELULAR1= reader.GetString(6),
                            IIDTIPODOCUMENTO= reader.GetInt32(7),
                            NUMEROIDENTIFICACION=reader.GetString(8)
                        });
                    }
                }
               
            }

            return personas;
        }
    }
}
