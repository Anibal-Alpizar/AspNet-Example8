using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceOrden : IServiceOrden
    {
        public IEnumerable<Orden> GetOrden()
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.GetOrden();
        }

        public Orden GetOrdenByID(int id)
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.GetOrdenByID(id);
        }

        public void GetOrdenCountDate(out string etiquetas1, out string valores1)
        {
            IRepositoryOrden repository = new RepositoryOrden();

            repository.GetOrdenCountDate(out string etiquetas, out string valores);
            etiquetas1 = etiquetas;
            valores1 = valores;
        }

        public Orden Save(Orden orden)
        {
            IRepositoryOrden repository = new RepositoryOrden();
            return repository.Save(orden);
        }
    }
}
