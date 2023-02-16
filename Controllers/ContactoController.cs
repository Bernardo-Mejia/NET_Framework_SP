using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoCRUD.Models;

namespace ProyectoCRUD.Controllers
{
    public class ContactoController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();
        // Listado para acceder a las propiedades de esa clase
        private static readonly List<Contacto> oLista = new List<Contacto>();
        // GET: Contacto
        public ActionResult Index()
        {
            oLista.Clear();
            //oLista = new List<Contacto>();
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                /*
                SqlCommand cmd = new SqlCommand("SELECT * FROM Contacto", oConexion);
                cmd.CommandType = CommandType.Text;
                oConexion.Open();

                using(SqlDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        Contacto nuevoContacto = new Contacto();
                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombres = dr["Nombres"].ToString();
                        nuevoContacto.Apellidos = dr["Apellidos"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();

                        oLista.Add(nuevoContacto);
                    }
                }
                */
                SqlCommand cmd = new SqlCommand("sp_Listar", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oConexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contacto nuevoContacto = new Contacto();
                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombres = dr["Nombres"].ToString();
                        nuevoContacto.Apellidos = dr["Apellidos"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();

                        oLista.Add(nuevoContacto);
                    }
                }
            }
            return View(oLista);
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Registrar(Contacto oContacto)
        {
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Registrar", oConexion);
                cmd.Parameters.AddWithValue("Nombres", oContacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", oContacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", oContacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oConexion.Open();
                cmd.ExecuteNonQuery();
                //cmd.ExecuteReader().Close();
            }

            return RedirectToAction("Index", "Contacto");
            //return View();
        }


        public ActionResult Editar(int? idContacto)
        {
            if (idContacto == null)
                return RedirectToAction("Index", "Contacto");

            Contacto oContacto = oLista.Where(x => x.IdContacto == idContacto).FirstOrDefault();

            return View(oContacto);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Editar(Contacto oContacto)
        {
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar", oConexion);
                cmd.Parameters.AddWithValue("IdContacto", oContacto.IdContacto);
                cmd.Parameters.AddWithValue("Nombres", oContacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", oContacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", oContacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oConexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Contacto");
        }

        public ActionResult Eliminar(int? idContacto)
        {
            if (idContacto == null)
                return RedirectToAction("Index", "Contacto");

            Contacto oContacto = oLista.Where(x => x.IdContacto == idContacto).FirstOrDefault();

            return View(oContacto);
        }

        [HttpPost]
        public ActionResult Eliminar(int IdContacto)
        {
            using (SqlConnection OConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar", OConexion);
                cmd.Parameters.AddWithValue("IdContacto", IdContacto);
                cmd.CommandType = CommandType.StoredProcedure;
                OConexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Contacto");
        }

        public ActionResult Detalles(int? idContacto)
        {
            if (idContacto == null)
                return RedirectToAction("Index", "Contacto");

            Contacto oContacto = oLista.Where(x => x.IdContacto == idContacto).FirstOrDefault();

            return View(oContacto);
        }
    }
}