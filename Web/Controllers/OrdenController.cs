using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Util;
using Web.Utils;

namespace Web.Controllers
{
    public class OrdenController : Controller
    {
        // GET: Orden
        public ActionResult Index()
        {
            if (TempData.ContainsKey("NotificationMessage"))
            {
                ViewBag.NotificationMessage = TempData["NotificationMessage"];
            }
            //Listado de clientes
            ViewBag.idCliente = listaClientes();
            ViewBag.DetalleOrden = Carrito.Instancia.Items;


            return View();
        }
        private SelectList listaClientes()
        {
            //Lista de Clientes
            IServiceCliente _ServiceCliente = new ServiceCliente();
            IEnumerable<Cliente> listaClientes = _ServiceCliente.GetCliente();

            return new SelectList(listaClientes, "IdCliente", "IdCliente");
        }

        //Actualizar Vista parcial detalle carrito
        private ActionResult DetalleCarrito()
        {

            return View();
        }
        //Actualizar cantidad de libros en el carrito
        public ActionResult actualizarCantidad(int idLibro, int cantidad)
        {


            return View();

        }
        //Ordenar un libro y agregarlo al carrito
        public ActionResult ordenarLibro(int? idLibro)
        {


            return View();

        }

        //Actualizar solo la cantidad de libros que se muestra en el menú
        public ActionResult actualizarOrdenCantidad()
        {

            return View();

        }
        //Eliminar libro del carrito
        public ActionResult eliminarLibro(int? idLibro)
        {

            return View();
        }


        public ActionResult IndexAdmin()
        {
            IEnumerable<Orden> lista = null;

            try
            {
                IServiceOrden _ServiceOrden = new ServiceOrden();
                lista = _ServiceOrden.GetOrden();
                return View(lista);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Orden";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: Orden/Details/5
        public ActionResult Details(int? id)
        {
            IServiceOrden _ServiceOrden = new ServiceOrden();
            Orden orden = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }

                orden = _ServiceOrden.GetOrdenByID(id.Value);
                if (orden == null)
                {
                    TempData["Message"] = "No existe la orden solicitado";
                    TempData["Redirect"] = "Orden";
                    TempData["Redirect-Action"] = "IndexAdmin";
                    //TempData.Keep();
                    return RedirectToAction("Default", "Error");
                }
                return View(orden);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Orden";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
        [HttpPost]
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Procesos)]
        public ActionResult Save(Orden orden)
        {

            try
            {
                // Si no existe la sesión no hay nada
                if (Carrito.Instancia.Items.Count() <= 0)
                {
                    // Validaciones de datos requeridos.
                    TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("Orden", "Seleccione los libros a ordenar", SweetAlertMessageType.warning);
                    return RedirectToAction("Index");
                }
                else
                {
                    //Obtener datos usuario logueado
                    

                    //Asignar idUsuario que se encuentra logueado
                  

                    //Agregar cada línea de detalle a la orden
                   

                }
                //Guardar la orden
                IServiceOrden _ServiceOrden = new ServiceOrden();
                Orden ordenSave = _ServiceOrden.Save(orden);

                // Limpia el Carrito de compras
                Carrito.Instancia.eliminarCarrito();
                TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("Orden", "Orden guardada satisfactoriamente!", SweetAlertMessageType.success);
                // Reporte orden
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Salvar el error  
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Orden";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
        public ActionResult graficoOrden()
        {
            return View();
        }

    }
}