using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceLibro
    {
        IEnumerable<Libro> GetLibro();
        IEnumerable<string> GetLibroNombres();
        IEnumerable<Libro> GetLibroByNombre(String nombre);
        IEnumerable<Libro> GetLibroByAutor(int idAutor);
        IEnumerable<Libro> GetLibroByCategoria(int idCategoria);
        Libro GetLibroByID(int id);
        void DeleteLibro(int id);
        Libro Save(Libro libro, string[] selectedCategorias);
    }
}
