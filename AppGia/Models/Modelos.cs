using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AppGia.Models
{
    
    #region Modelo Login
    public class Login
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }
    }

    #endregion

    #region Modelo Response
    public class Response
    {
        public bool MESSAGE { set; get; }
    }
    #endregion

    #region Tabla Usuario
    public class Usuario
    {

        public string userName { get; set; }

        public string displayName { get; set; }


        public int INT_IDUSUARIO_P { get; set; }

        public string STR_USERNAME_USUARIO { get; set; }

        public string STR_PASSWORD_USUARIO { get; set; }

        public string STR_EMAIL_USUARIO { get; set; }

        public bool BOOL_ESTATUS_LOGICO_USUARIO { get; set; }

        public string STR_PUESTO { get; set; }

        public DateTime FEC_MODIF_USUARIO { get; set; }

        public string STR_NOMBRE_USUARIO { get; set; }

    }
    #endregion

    #region Tabla PermisosRoL
    public class Permiso
    {
        public string STR_NOMBRE_PERMISO { get; set; }
        public int INT_IDPERMISO_P { get; set; }
        public int INT_IDROL { get; set; }
        public bool BOOL_ESTATUS_LOGICO_PERM { get; set; }
    }
    #endregion

    #region Tabla Pantallas
    public class Pantalla
    {
        public string STR_NOMBRE_PANTALLA { get; set; }
        public int INT_IDROL_F { get; set; }
        public DateTime FEC_MODIF_PANTALLA { get; set; }
        public int INT_IDPANTALLA_P { get; set; }
        public bool BOOL_ESTATUS_LOGICO_PANT { get; set; }
    }
    #endregion

    #region Tabla Grupos
    public class Grupo 
    {
        public int INT_IDGRUPO_P { get; set; }

        public string STR_NOMBRE_GRUPO { get; set; }

        public bool BOOL_ESTATUS_LOGICO_GRUPO { get; set; }

        public DateTime FEC_MODIF_GRUPO { get; set; }
    }
    #endregion

    #region Tabla Relaciones

    public class Relacion
    {
        public int INT_IDUSUARIO_P { set; get; }
        public int INT_IDGRUPO_P { set; get; }
        public int INT_IDROL_P { set; get; }
        public DateTime FEC_MODIF_RELACIONES { set; get; }
        public bool BOOL_ESTATUS_RELACION { set; get; }
        public int INT_IDRELACION_P { set; get; }
        

    }
    #endregion

    #region Tabla RelacionesRol

    public class RelacionRol
    {
        public int INT_IDRELACION_P { set; get; }
        public int INT_IDPERMISO_F { set; get; }
        public int INT_IDROL_F { set; get; }
        public DateTime FEC_MODIF_RELA_ROL_PERMISO { set; get; }
        public bool BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO { set; get; }


    }
    #endregion

    #region Catalogo Rol
    public class Rol
    {
        public string STR_NOMBRE_ROL { get; set; }

        public bool BOOL_ESTATUS_LOGICO_ROL { get; set; }

        public DateTime FEC_MODIF_ROL { get; set; }

        public int INT_IDROL_P { get; set; }
    }
    #endregion

    #region Catalogo Proyecto
    public class Proyecto
    {
        public string STR_NOMBRE_PROYECTO { get; set; }

        public string STR_RESPONSABLE { get; set; }

        public string STR_IDPROYECTO { get; set; }

        public bool BOOL_ESTATUS_PROYECTO { get; set; }

        public bool BOOL_ESTATUS_LOGICO_PROYECTO { get; set; }

        public DateTime FEC_MODIF { get; set; }

        public int INT_IDPROYECTO_P { get; set; }

        public int INT_IDPANTALLA_F { get; set; }

    }
    #endregion

    #region Catalogo Compañia
    public class Compania
    {
        public string STR_NOMBRE_COMPANIA { get; set; }

        public string STR_ABREV_COMPANIA { get; set; }

        public string STR_IDCOMPANIA { get; set; }

        public bool BOOL_ETL_COMPANIA { get; set; }

        public string STR_HOST_COMPANIA { get; set; }

        public string STR_USUARIO_ETL { get; set; }

        public string STR_CONTRASENIA_ETL { get; set; }

        public string STR_PUERTO_COMPANIA { get; set; }

        public string STR_MONEDA_COMPANIA { get; set; }

        public string STR_BD_COMPANIA { get; set; }

        public bool BOOL_ESTATUS_LOGICO_COMPANIA { get; set; }

        public int INT_IDCOMPANIA_P { get; set; }

        public int INT_IDPROYECTO_F { get; set; }

        public int INT_IDCENTROCOSTO_F { get; set; }

        public DateTime FEC_MODIF_COMPANIA { get; set; }


    }
    #endregion

    #region Modelo Centro de costos
    public class CentroCostos
    {

        public string STR_TIPO_CC { get; set; }

        public string STR_IDCENTROCOSTO { get; set; }

        public string STR_NOMBRE_CC { get; set; }

        public string STR_CATEGORIA_CC { get; set; }

        public bool BOOL_ESTATUS_LOGICO_CENTROCOSTO { get; set; }

        public string STR_GERENTE_CC { get; set; }

        public string STR_ESTATUS_CC { get; set; }

        public int INT_IDCENTROCOSTO_P { get; set; }

        public DateTime FEC_MODIF_CC { get; set; }

       
    }


    #endregion

    #region Modelo de Negocios

    public class ModeloNegocio
    {
        public int INT_IDMODELONEGOCIO_P { get; set; }
        public string STR_NOMBREMODELONEGOCIO { get; set; }
        public string STR_TIPOMONTO { get; set; }
        public string STR_IDCOMPANIA { get; set; }
        public string STR_CUENTASMODELO { get; set; }
        public int INT_COMPANIA_F { get; set; }
        public bool BOOL_ESTATUS_LOGICO_MODE_NEGO { get; set; }
    }
    #endregion



}


