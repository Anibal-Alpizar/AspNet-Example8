
using ApplicationCore.Utils;
using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceUsuario : IServiceUsuario
    {
        //Login
        public Usuario GetUsuario(string email, string password)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            // Encriptar el password para poder compararlo

            string cryptPassword = Cryptography.EncrypthAES(password);

            return repository.GetUsuario(email, cryptPassword);
        }
        public Usuario GetUsuarioByID(int id)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            Usuario oUsuario = repository.GetUsuarioByID(id);
            // Desencriptar el password para presentarlo
            oUsuario.Password=Cryptography.DecrypthAES(oUsuario.Password);

            return oUsuario;
        }
        public Usuario Save(Usuario usuario)
        {
            IRepositoryUsuario repository = new RepositoryUsuario();
            // Encriptar el password para guardarlo
            usuario.Password = Cryptography.EncrypthAES(usuario.Password);

            return repository.Save(usuario);
        }
    }
}

