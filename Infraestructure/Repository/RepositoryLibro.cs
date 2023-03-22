using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryLibro : IRepositoryLibro
    {
        public void DeleteLibro(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Libro> GetLibro()
        {
            IEnumerable<Libro> lista = null;
            try
            {


                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener todos los libros incluyendo el autor
                    lista = ctx.Libro.Include("Autor").ToList();

                    //lista = ctx.Libro.Include(x=>x.Autor).ToList();

                }
                return lista;
            }

            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<Libro> GetLibroByAutor(int idAutor)
        {
            IEnumerable<Libro> oLibro = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener libros por Autor
                    oLibro = ctx.Libro.
                        Where(l => l.IdAutor == idAutor).
                        Include("Autor").ToList();

                }
                return oLibro;
            }

            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<Libro> GetLibroByCategoria(int idCategoria)
        {
            IEnumerable<Libro> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener los libros que pertenecen a una categoría
                    lista = ctx.Libro.Include(c => c.Categoria).
                        Where(c => c.Categoria.Any(o => o.IdCategoria == idCategoria))
                        .ToList();

                }
                return lista;
            }

            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public Libro GetLibroByID(int id)
        {
            Libro oLibro = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener libro por ID incluyendo el autor y todas sus categorías
                    oLibro = ctx.Libro.
                        Where(l => l.IdLibro == id).
                        Include("Autor").
                        Include("Categoria").
                        FirstOrDefault();

                }
                return oLibro;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<Libro> GetLibroByNombre(string nombre)
        {
            IEnumerable<Libro> oLibro = null;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oLibro = ctx.Libro.
                    ToList().FindAll(l => l.Nombre.ToLower().Contains(nombre.ToLower()));
            }
            return oLibro;
        }

        public Libro Save(Libro libro, string[] selectedCategorias)
        {
            int retorno = 0;
            Libro oLibro = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oLibro = GetLibroByID((int)libro.IdLibro);
                IRepositoryCategoria _RepositoryCategoria = new RepositoryCategoria();

                if (oLibro == null)
                {

                    //Insertar
                    //Logica para agregar las categorias al libro
                    if (selectedCategorias != null)
                    {

                        libro.Categoria = new List<Categoria>();
                        foreach (var categoria in selectedCategorias)
                        {
                            var categoriaToAdd = _RepositoryCategoria.GetCategoriaByID(int.Parse(categoria));
                            ctx.Categoria.Attach(categoriaToAdd); //sin esto, EF intentará crear una categoría
                            libro.Categoria.Add(categoriaToAdd);// asociar a la categoría existente con el libro


                        }
                    }
                    //Insertar Libro
                    ctx.Libro.Add(libro);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {
                    //Registradas: 1,2,3
                    //Actualizar: 1,3,4

                    //Actualizar Libro
                    ctx.Libro.Add(libro);
                    ctx.Entry(libro).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();

                    //Logica para actualizar Categorias
                    var selectedCategoriasID = new HashSet<string>(selectedCategorias);
                    if (selectedCategorias != null)
                    {
                        ctx.Entry(libro).Collection(p => p.Categoria).Load();
                        var newCategoriaForLibro = ctx.Categoria
                         .Where(x => selectedCategoriasID.Contains(x.IdCategoria.ToString())).ToList();
                        libro.Categoria = newCategoriaForLibro;

                        ctx.Entry(libro).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();
                    }
                }
            }

            if (retorno >= 0)
                oLibro = GetLibroByID((int)libro.IdLibro);

            return oLibro;
        }

    }
}

