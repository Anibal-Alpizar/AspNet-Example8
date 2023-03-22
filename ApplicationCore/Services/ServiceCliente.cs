using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceCliente : IServiceCliente
    {
        public IEnumerable<Cliente> GetCliente()
        {
            IRepositoryCliente repository = new RepositoryCliente();
            return repository.GetCliente();
        }
    }
}
