using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryOrden : IRepositoryOrden
    {
        public IEnumerable<Orden> GetOrden()
        {
            List<Orden> ordenes = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener todas las ordenes incluyendo el cliente y el usuario
                    ordenes = ctx.Orden.Include("Cliente").Include("Usuario").ToList();

                }
                return ordenes;

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
                throw new Exception(mensaje);
            }
        }

        public Orden GetOrdenByID(int id)
        {
            Orden orden = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener una orden incluyendo el cliente, el usuario,
                    //// el detalle de la orden y la infomación de cada libro
                    orden = ctx.Orden.
                        Include("Cliente").
                        Include("Usuario").
                        Include("OrdenDetalle").
                        Include("OrdenDetalle.Libro").
                        Where(o => o.IdOrden == id).
                        FirstOrDefault();


                }
                return orden;

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
                throw new Exception(mensaje);
            }
        }

        public void GetOrdenCountDate(out string etiquetas, out string valores)
        {
            throw new NotImplementedException();
        }

        public Orden Save(Orden pOrden)
        {
            int resultado = 0;
            Orden orden = null;
            try
            {
                //Guardar Orden


                // Buscar la orden que se salvó y reenviarla
                if (resultado >= 0)
                    orden = GetOrdenByID(pOrden.IdOrden);


                return orden;
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
                throw new Exception(mensaje);
            }
        }
    }
}