using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Utils;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            IServiceUsuario _ServiceUsuario= new ServiceUsuario();
            Usuario oUsuario = null;
            try
            {
                ModelState.Remove("Nombre");
                ModelState.Remove("Apellidos");
                ModelState.Remove("IdRol");
                //Verificar las credenciales
                if (ModelState.IsValid)
                {
                    oUsuario=_ServiceUsuario.GetUsuario(usuario.Email, usuario.Password);   
                    if(oUsuario != null)
                    {
                        Session["User"]=oUsuario;
                        Log.Info($"Inicio sesion: {usuario.Email}");
                        TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Login",
                            "Usuario autenticado",Util.SweetAlertMessageType.success
                            );
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        Log.Warn($"Intento de inicio: {usuario.Email}");
                        ViewBag.NotificationMessage= Util.SweetAlertHelper.Mensaje("Login",
                            "Usuario no válido", Util.SweetAlertMessageType.error
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
            return View("Index");
        }
        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "No autorizado";
            if (Session["User"] != null)
            {
                Usuario usuario = Session["User"] as Usuario;
                Log.Warn($"No autorizado {usuario.Email}");
            }
            return View();
        }
        public ActionResult Logout()
        {
            try
            {
                //Eliminar variable de sesion
                Session["User"] = null;
                Session.Remove("User");

                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
    }
}
