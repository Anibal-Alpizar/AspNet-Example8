using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceLibro : IServiceLibro
    {
        public void DeleteLibro(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Libro> GetLibro()
        {
            IRepositoryLibro repository = new RepositoryLibro();
            return repository.GetLibro();
        }
        public IEnumerable<string> GetLibroNombres()
        {
            IRepositoryLibro repository = new RepositoryLibro();
            return repository.GetLibro().Select(x => x.Nombre);
        }

        public IEnumerable<Libro> GetLibroByAutor(int idAutor)
        {
            IRepositoryLibro repository = new RepositoryLibro();
            return repository.GetLibroByAutor(idAutor);
        }

        public Libro GetLibroByID(int id)
        {
            IRepositoryLibro repository = new RepositoryLibro();
            return repository.GetLibroByID(id);
        }

        public IEnumerable<Libro> GetLibroByNombre(string nombre)
        {
            IRepositoryLibro repository = new RepositoryLibro();
            return repository.GetLibroByNombre(nombre);
        }

        public Libro Save(Libro libro, string[] selectedCategorias)
        {
            IRepositoryLibro repository = new RepositoryLibro();
            return repository.Save(libro, selectedCategorias);
        }

        public IEnumerable<Libro> GetLibroByCategoria(int idCategoria)
        {
            IRepositoryLibro repository = new RepositoryLibro();
            return repository.GetLibroByCategoria(idCategoria);
        }
    }
}
