using Microsoft.AspNetCore.Mvc;
using ReportesMVC.Models;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ReportesMVC.Controllers
{
    public class LogueoController : Controller
    {
        private readonly IConfiguration configuration;
       
        public LogueoController (IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario oUsuario)
        {
            bool registrado;
            string mensaje;
            if (oUsuario.Password == oUsuario.ConfirmaPassword)
            {
                oUsuario.Password = ConvertirSha256(oUsuario.Password);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }
            string cadena = configuration.GetConnectionString("LocalConnection");
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_InsRegistraUsuario", cn);
                cmd.Parameters.AddWithValue("username", oUsuario.Username);
                cmd.Parameters.AddWithValue("password", oUsuario.Password);
                cmd.Parameters.AddWithValue("confirma", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["confirma"].Value);
                if (registrado)
                {
                    return RedirectToAction("Login", "Logueo");
                }
                else
                {
                    ViewData["Mensaje"] = "Ocurrió un error, porfavor vuelva a intentar";
                    return View();
                }
            }

        }
        [HttpPost]
        public IActionResult Login(Usuario oUsuario)
        {
            oUsuario.Password = ConvertirSha256(oUsuario.Password);
            string cadena = configuration.GetConnectionString("LocalConnection");
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_SelValidaLogin", cn);
                cmd.Parameters.AddWithValue("username", oUsuario.Username);
                cmd.Parameters.AddWithValue("password", oUsuario.Password);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                oUsuario.IdUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());  
            }
            if (oUsuario.IdUsuario != 0)
                return RedirectToAction("Index", "Reportes");
            else
            {
                ViewData["Mensaje"] = "usuario y/o contraseña inválidos";
                return View();
            }
        }
        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;
                byte[] result = hash.ComputeHash(encoding.GetBytes(texto));
                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }    
            return sb.ToString();
        }
    }
}
