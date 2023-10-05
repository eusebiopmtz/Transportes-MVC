using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Transportes_MVC.Models.DTO_VewModels
{
    public class CamionDTO
    {
        public int ID_Camion { get; set; }
        [Required]
        [Display(Name ="Matrícula")]
        public string matricula { get; set; }
        [Required]
        [Display(Name = "Tipo Camion")]
        public string tipo_camion { get; set; }
        [Required]
        [Display(Name = "Marca")]
        public string marca { get; set; }
        [Required]
        [Display(Name = "Modelo")]
        public string modelo { get; set; }
        [Required]
        [Display(Name = "Capacidad")]
        public double capacidad { get; set; }
        [Required]
        [Display(Name = "Kilometraje")]
        public double kilometraje { get; set; }
        
        [DataType(DataType.ImageUrl)]
        public string urlfoto { get; set; }
        public bool disponibilidad { get; set; }
        public Nullable<int> chofer_id { get; set; }
    }
}