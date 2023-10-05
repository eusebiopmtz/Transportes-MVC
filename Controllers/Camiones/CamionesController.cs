using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using Transportes_MVC.Models;
using Transportes_MVC.Models.DTO_VewModels;
using static Transportes_MVC.Models.Enum;

namespace Transportes_MVC.Controllers.Camiones
{
    public class CamionesController : Controller
    {
        // GET: Camiones
        public ActionResult Index()
        {
            //crear una lista de camiones
            List<CamionesLista> list = new List<CamionesLista>();
            //llenar mi lista de camiones
            using (TransportesEntities db = new TransportesEntities())
            {
                list = (from c in db.Camiones select new CamionesLista
                {
                    ID_Camion = c.ID_Camion,
                    matricula = c.matricula,
                    tipo_camion = c.tipo_camion,
                    marca = c.marca,
                    modelo = c.modelo,
                    capacidad = c.capacidad,
                    kilometraje = c.kilometraje,
                    urlfoto = c.urlfoto,
                    disponibilidad = c.disponibilidad,
                    chofer_id = c.chofer_id,
                    
                }).ToList();
            }

            ViewBag.Title = "Listar Camiones desde vb";
            //ViewData["Title"] = "Listar Camiones desde vd";
            CargarDDL();
            return View(list);
        }

        public ActionResult CamionesVista()
        {
            //crear una lista de camiones
            List<Camiones_ChoferesList> list = new List<Camiones_ChoferesList>();
            //llenar mi lista de camiones
            using (TransportesEntities db = new TransportesEntities())
            {
                list = (from c in db.Camiones_Choferes
                        select new Camiones_ChoferesList
                        {
                            ID_Camion = c.ID_Camion,
                            matricula = c.matricula,
                            tipo_camion = c.tipo_camion,
                            marca = c.marca,
                            modelo = c.modelo,
                            capacidad = c.capacidad,
                            kilometraje = c.kilometraje,
                            urlfoto = c.urlfoto,
                            disponibilidad = c.disponibilidad,
                            chofer_id = c.chofer_id,
                            Nombre_Chofer = c.Nombre_Chofer
                        }).ToList();
            }

            ViewBag.Title = "Listar Camiones desde vb";
            //ViewData["Title"] = "Listar Camiones desde vd";
            ViewBag.titulo = "Camiones desde Vista SQL";
            return View(list);
        }

        public ActionResult ViewLinq()
        {
            List<Camiones_ChoferesList> list = new List<Camiones_ChoferesList>();
            using (TransportesEntities db = new TransportesEntities())
            {
                list = (from c in db.Camiones
                        join ch in db.Choferes on c.chofer_id equals ch.ID_chofer
                        select new Camiones_ChoferesList
                        {
                            ID_Camion = c.ID_Camion,
                            matricula = c.matricula,
                            tipo_camion = c.tipo_camion,
                            marca = c.marca,
                            modelo = c.modelo,
                            capacidad = c.capacidad,
                            kilometraje = c.kilometraje,
                            urlfoto = c.urlfoto,
                            disponibilidad = c.disponibilidad,
                            chofer_id = c.chofer_id,
                            Nombre_Chofer = ch.nombre + " " + ch.apellido_p + " " + ch.apellido_m
                        }).ToList();
            }
            ViewBag.titulo = "Camiones desde vista creada con LinQ";
            return View(list);
        }
        
        public ActionResult Nuevo_Camion()
        {

            CargarDDL();
            return View();
        }
        [HttpPost]
        public ActionResult Nuevo_Camion(CamionDTO model, HttpPostedFileBase imagen)
        {
            try 
            {
                if(ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities()) 
                    {
                        var camion = new Transportes_MVC.Models.Camiones();
                        camion.ID_Camion = model.ID_Camion;
                        camion.matricula = model.matricula;
                        camion.tipo_camion = model.tipo_camion;
                        camion.marca = model.marca;
                        camion.modelo = model.modelo;
                        camion.capacidad = model.capacidad;
                        camion.kilometraje = model.kilometraje;
                        camion.disponibilidad = model.disponibilidad;
                        camion.chofer_id = model.chofer_id;
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Imagenes/Camiones/");
                            if (!Directory.Exists(pathdir))
                            {
                                Directory.CreateDirectory(pathdir);
                            }
                            imagen.SaveAs(pathdir + filename);
                            camion.urlfoto = ("/Imagenes/Camiones/") + filename;
                        }
                      
                        context.Camiones.Add(camion);
                        context.SaveChanges();
                        Alert("Camion Registrado", NotificationType.success);
                        CargarDDL();
                        return View(model);
                    }
                }
                else
                {

                       Alert("Datos no válidos", NotificationType.warnig);
                    CargarDDL();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Alert("Error" + ex.Message,NotificationType.error);
                CargarDDL();
                return View(model);
            }
        }

        public ActionResult Editar_Camion(int id)
        {
            Transportes_MVC.Models.Camiones camion = new Transportes_MVC.Models.Camiones();
            if(id > 0)
            {

            using (TransportesEntities context = new TransportesEntities()) 
            {
                camion = context.Camiones.Where(x => x.ID_Camion == id).FirstOrDefault();
            }
            ViewBag.Title = "Editar camion n°: " + camion.ID_Camion;
                if (camion != null)
                {
                    CargarDDL();
                    return View(camion);
                }
                else
                {
                    CargarDDL();
                    return Redirect("~/Index");

                }
            }
            else
            {
                CargarDDL();
                return Redirect("~/Index");
            }
        }

        [HttpPost]
        public ActionResult Editar_Camion(Transportes_MVC.Models.Camiones model , HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new Transportes_MVC.Models.Camiones();
                        camion.ID_Camion = model.ID_Camion;
                        camion.matricula = model.matricula;
                        camion.tipo_camion = model.tipo_camion;
                        camion.marca = model.marca;
                        camion.modelo = model.modelo;
                        camion.capacidad = model.capacidad;
                        camion.kilometraje = model.kilometraje;
                        camion.disponibilidad = model.disponibilidad;
                        camion.chofer_id = model.chofer_id;
                        
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Imagenes/Camiones/");
                            string urlfoto_old = context.Camiones.Where(x => x.ID_Camion == model.ID_Camion).Select(x => x.urlfoto).FirstOrDefault();
                            if (model.urlfoto.Contains(filename))
                            {
                                camion.urlfoto =model.urlfoto;
                            }
                            else
                            {

                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }
                                try
                                {
                                    string pathdir_old = Server.MapPath("~" + model.urlfoto);
                                    if(System.IO.File.Exists(pathdir_old))
                                    {
                                        System.IO.File.Delete(pathdir_old);
                                    }
                                }
                                catch (Exception e)
                                {
                                    //Alert();
                                    Debug.WriteLine(e);
                                }
                                    imagen.SaveAs(pathdir + filename);
                                    camion.urlfoto = ("/Imagenes/Camiones/") + filename;
                            }
                        }
                        else
                        {
                            camion.urlfoto = model.urlfoto;
                        }
                        //actualizar camion
                        context.Entry(camion).State = System.Data.Entity.EntityState.Modified;
                        CargarDDL();
                        context.SaveChanges();
                        Alert("Correcto", NotificationType.success);
                        Debug.WriteLine("Todo OK");
                    }
                }
                Debug.WriteLine("Datos no válidos");
                CargarDDL();
                return View(model);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                CargarDDL();
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Eliminar_Camion(int id)
        {
            try
            {
                using (TransportesEntities context = new TransportesEntities()) 
                {
                    Transportes_MVC.Models.Camiones _camion = context.Camiones.Where(
                        f => f.ID_Camion == id).FirstOrDefault();
                    if(_camion != null) 
                    {
                        context.Camiones.Remove(_camion);
                        context.SaveChanges();
                    
                        Alert("Registro Eliminado con éxito", NotificationType.success);
                        return Redirect("~/Camiones");
                    }
                    else 
                    {
                
                        Alert("Error al eliminar, no existe el ID :(", NotificationType.info);
                        return Redirect("~/Camiones");
                        
                    }
                }
            }
            catch(Exception ex)
            {
                Alert("Error: "+ ex.Message, NotificationType.error);
                return Redirect("~/Camiones");
            }
        }

        public void CargarDDL()
        {
            List<ChoferDDL> listChoferes = new List<ChoferDDL>();
            listChoferes.Insert(0, new ChoferDDL { ID_Chofer = 0, nombre = "Seleccione un chofer" });
            using (TransportesEntities db = new TransportesEntities())
            {
                foreach (var ch in db.Choferes)
                {
                    ChoferDDL _aux = new ChoferDDL();
                    _aux.ID_Chofer = ch.ID_chofer;
                    _aux.nombre = ch.nombre + " " + ch.apellido_p + " " + ch.apellido_m;
                    listChoferes.Add(_aux);
                }
            }
            ViewBag.ListaChoferes = listChoferes;
        }

        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "<script language='javascript'>Swal.fire('"+notificationType.ToString().ToUpper()+
                "','"+message+"','"+notificationType+"')"+"</script>";
            TempData["notification"] = msg;
        }
    }
}