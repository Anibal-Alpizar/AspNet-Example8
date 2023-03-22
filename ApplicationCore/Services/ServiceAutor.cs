using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceAutor : IServiceAutor
    {
        public Autor GetAutorByID(int id)
        {
            IRepositoryAutor repository = new RepositoryAutor();
            return repository.GetAutorByID(id);
        }

        public IEnumerable<Autor> GetAutors()
        {
            IRepositoryAutor repository = new RepositoryAutor();
            return repository.GetAutors();
        }
    }
}
