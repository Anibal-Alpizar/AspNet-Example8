using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModel
{
    public class ViewModelOrdenDetalle
    {
        public int IdOrden { get; set; }
        public int IdLibro { get; set; }
        public byte[] Imagen { get; set; }
        public int Cantidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Precio
        {
            get { return Libro.Precio; }

        }
        public virtual Libro Libro { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal SubTotal
        {
            get
            {
                return calculoSubtotal();
            }
        }
        private decimal calculoSubtotal()
        {
            return this.Precio * this.Cantidad;
        }


        public ViewModelOrdenDetalle(int IdLibro)
        {
            IServiceLibro _ServiceLibro = new ServiceLibro();
            this.IdLibro = IdLibro;
            this.Libro = _ServiceLibro.GetLibroByID(IdLibro);
        }
    }
}