using ProyectoCRUD2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoCRUD2.Controllers
{
    public class ContactoController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();
        private static List<Contacto> oLista = new List<Contacto>();
        // GET: Contacto
        public ActionResult Inicio()
        {
            oLista = new List<Contacto>();

            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CONTACTO", oConexion);
                cmd.CommandType = CommandType.Text;
                oConexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contacto nuevoContacto = new Contacto();

                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombre = dr["Nombre"].ToString();
                        nuevoContacto.Apellido = dr["Apellido"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Email = dr["Email"].ToString();

                        oLista.Add(nuevoContacto);
                    }
                }
            }
            return View(oLista);
        }
        public ActionResult Nuevo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Nuevo(Contacto oContacto)
        {
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Nuevo", oConexion);
                cmd.Parameters.AddWithValue("Nombre", oContacto.Nombre);
                cmd.Parameters.AddWithValue("Apellido", oContacto.Apellido);
                cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                cmd.Parameters.AddWithValue("Email", oContacto.Email);
                cmd.CommandType = CommandType.StoredProcedure;
                oConexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Contacto");
        }
        public ActionResult Editar(int? idContacto)
        {
            if (idContacto == null)
            {
                return RedirectToAction("Inicio", "Contacto");
            }
            Contacto oContacto = oLista.Where(x => x.IdContacto == idContacto).FirstOrDefault();

            return View(oContacto);
        }
    }
}