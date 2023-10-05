using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transportes_MVC.Models.DTO_VewModels
{
    public class Camiones_ChoferesList
    {
        public int ID_Camion { get; set; }
        public string matricula { get; set; }
        public string tipo_camion { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public double capacidad { get; set; }
        public double kilometraje { get; set; }
        public string urlfoto { get; set; }
        public bool disponibilidad { get; set; }
        public Nullable<int> chofer_id { get; set; }
        public string Nombre_Chofer {  get; set; }
    }
}