using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class LibroController : Controller
    {
        // GET: Libro
        public ActionResult Index()
        {
            IEnumerable<Libro> lista = null;
            try
            {
                IServiceLibro _ServiceLibro = new ServiceLibro();
                lista = _ServiceLibro.GetLibro();
                ViewBag.title = "Lista Libros";
                //Lista autores
                IServiceAutor _ServiceAutor = new ServiceAutor();
                ViewBag.listaAutores = _ServiceAutor.GetAutors();
                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
        [CustomAuthorize((int)Roles.Administrador,(int)Roles.Procesos)]
        public ActionResult IndexAdmin()
        {
            IEnumerable<Libro> lista = null;
            try
            {
                IServiceLibro _ServiceLibro = new ServiceLibro();
                lista = _ServiceLibro.GetLibro();
                //Lista de autocompletado de autores
                ViewBag.listaNombres=_ServiceLibro.GetLibroNombres();

                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
        public ActionResult librosxNombre(string filtro)
        {
            IEnumerable<Libro> lista = null;
            IServiceLibro _ServiceLibro = new ServiceLibro();
            if (string.IsNullOrEmpty(filtro))
            {
                lista=_ServiceLibro.GetLibro();
            }
            else
            {
                lista=_ServiceLibro.GetLibroByNombre(filtro);
            }

            return PartialView("_PartialViewLibroAdmin", lista);
        }
        public PartialViewResult librosxAutor(int? id)
        {
            IEnumerable<Libro> lista = null;
            IServiceLibro _ServiceLibro = new ServiceLibro();
            if (id != null)
            {
                if (id == 0)
                {
                    lista = _ServiceLibro.GetLibro();
                }
                else
                {
                    lista = _ServiceLibro.GetLibroByAutor((int)id);
                }
            }
            return PartialView("_PartialViewLibro", lista);
        }
        public ActionResult AjaxDetails(int id)
        {
            IServiceLibro _ServiceLibro=new ServiceLibro();
            Libro libro = _ServiceLibro.GetLibroByID(id);
            return PartialView("_PartialViewDetail", libro);
        }

        // GET: Libro/Details/5
        public ActionResult Details(int? id)
        {
            ServiceLibro _ServiceLibro = new ServiceLibro();
            Libro libro = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                libro = _ServiceLibro.GetLibroByID(Convert.ToInt32(id));
                if (libro == null)
                {
                    TempData["Message"] = "No existe el libro solicitado";
                    TempData["Redirect"] = "Libro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                return View(libro);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Libro";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: Libro/Create
        [HttpGet]
        public ActionResult Create()
        {
            //Que recursos necesito para crear un Libro
            //Autores
            ViewBag.idAutor = listAutores();
            //Categorías
            ViewBag.idCategoria = listaCategorias();

            return View();
        }
        private SelectList listAutores(int idAutor = 0)
        {
            IServiceAutor _ServiceAutor = new ServiceAutor();
            IEnumerable<Autor> lista = _ServiceAutor.GetAutors();
            return new SelectList(lista, "IdAutor", "Nombre", idAutor);
        }

        private MultiSelectList listaCategorias(ICollection<Categoria> categorias = null)
        {
            IServiceCategoria _ServiceCategoria = new ServiceCategoria();
            IEnumerable<Categoria> lista = _ServiceCategoria.GetCategoria();
            //Seleccionar categorias
            int[] listaCategoriasSelect = null;
            if (categorias != null)
            {
                listaCategoriasSelect = categorias.Select(c => c.IdCategoria).ToArray();
            }

            return new MultiSelectList(lista, "IdCategoria", "Nombre", listaCategoriasSelect);
        }


        // GET: Libro/Edit/5
        public ActionResult Edit(int? id)
        {
            ServiceLibro _ServiceLibro = new ServiceLibro();
            Libro libro = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                libro = _ServiceLibro.GetLibroByID(Convert.ToInt32(id));
                if (libro == null)
                {
                    TempData["Message"] = "No existe el libro solicitado";
                    TempData["Redirect"] = "Libro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                //Listados
                ViewBag.IdAutor = listAutores(libro.IdAutor);
                ViewBag.IdCategoria = listaCategorias(libro.Categoria);
                return View(libro);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Libro";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // POST: Libro/Edit/5
        [HttpPost]
        public ActionResult Save(Libro libro, HttpPostedFileBase ImageFile, string[] selectedCategorias)
        {
            //Gestión de archivos
            MemoryStream target = new MemoryStream();
            //Servicio Libro
            IServiceLibro _ServiceLibro = new ServiceLibro();
            try
            {
                //Insertar la imagen
                if (libro.Imagen == null)
                {
                    if (ImageFile != null)
                    {
                        ImageFile.InputStream.CopyTo(target);
                        libro.Imagen = target.ToArray();
                        ModelState.Remove("Imagen");
                    }
                }
                if (ModelState.IsValid)
                {
                    Libro oLibroI = _ServiceLibro.Save(libro, selectedCategorias);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Utils.Util.ValidateErrors(this);
                    ViewBag.idAutor = listAutores(libro.IdAutor);
                    ViewBag.idCategoria = listaCategorias(libro.Categoria);
                    //Cargar la vista crear o actualizar
                    //Lógica para cargar vista correspondiente
                    if (libro.IdLibro > 0)
                    {
                        return (ActionResult)View("Edit", libro);
                    }
                    else
                    {
                        return View("Create", libro);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Libro";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

    }
}
