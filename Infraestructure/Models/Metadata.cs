using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infraestructure.Models
{
    internal partial class LibroMetadata
    {

        public int IdLibro { get; set; }

        [Display(Name = "ISBN")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Isbn { get; set; }

        [Display(Name = "Id Autor")]
        [Required(ErrorMessage = "{0} es un dato requerido")]

        public int IdAutor { get; set; }

        [Display(Name = "Nombre Libro")]
        [Required(ErrorMessage = "{0} es un dato requerido")]

        public string Nombre { get; set; }

        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "solo acepta números, con dos decimales")]
        [Required(ErrorMessage = "{0} es un dato requerido")]

        public decimal Precio { get; set; }

        [Display(Name = "Cantidad")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} solo acepta números")]
        [Required(ErrorMessage = "{0} es un dato requerido")]

        public int Cantidad { get; set; }

        [Display(Name = "Imagen Libro")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public byte[] Imagen { get; set; }

        [Display(Name = "Autor")]
        public virtual Autor Autor { get; set; }
        public virtual ICollection<OrdenDetalle> OrdenDetalle { get; set; }

        [Display(Name = "Categoría")]
        public virtual ICollection<Categoria> Categoria { get; set; }
    }
    internal partial class UsuarioMetadata
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "{0} no tiene formato válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Password { get; set; }
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Apellidos { get; set; }
        public bool Estado { get; set; }

        public virtual Rol Rol { get; set; }
    }
    internal partial class AutorMetadata
    {
        public int IdAutor { get; set; }

        [Display(Name = "Nombre Autor")]
        public string Nombre { get; set; }

        public virtual ICollection<Libro> Libro { get; set; }
    }
    internal partial class OrdenMetadata
    {
        [Display(Name = "Id Orden")]
        public int IdOrden { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string IdCliente { get; set; }

        [Display(Name = "Fecha Orden")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]

        [Required(ErrorMessage = "{0} es un dato requerido")]
        public System.DateTime FechaOrden { get; set; }

        [Display(Name = "Cliente")]
        public virtual Cliente Cliente { get; set; }
        [Display(Name = "Detalle Orden")]
        public virtual ICollection<OrdenDetalle> OrdenDetalle { get; set; }
    }
    internal partial class ClienteMetadata
    {
        [Display(Name = "Cliente")]
        public string IdCliente { get; set; }

        [Display(Name = "Nombre Cliente")]
        public string Nombre { get; set; }

        [Display(Name = "Primer Apellido")]
        public string Apellido1 { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string Apellido2 { get; set; }

        public string Sexo { get; set; }
        public System.DateTime FechaNacimiento { get; set; }

        public virtual ICollection<Orden> Orden { get; set; }
    }
}
