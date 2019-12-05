﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AppGia.Models
{
    #region Busqueda de Servidor
    #region Modelo Login
    public class Login
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }
    }
    #endregion

    //#region Modelo Usuario
    //public class User
    //{
    //    [Required]
    //    public string userName { get; set; }

    //    [Required]
    //    public string displayName { get; set; }
    //}
    //#endregion
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
        //[Required]
        //public string userName { get; set; }

        public string SRT_USERNAME_USUARIO { get; set; }

        //[Required]
        //public string displayName { get; set; }

        public string SRT_DISPLAYNAME_USUARIO { get; set; }

        /// <summary>
        /// ///////////////////
        /// </summary>

        public string STR_NOMBRE_USUARIO { get; set; }

        public string STR_EMAIL_USUARIO { get; set; }

        public bool SRT_PUESTO { get; set; }

        //public ? FEC_MODIF { get; set; }
    }
    #endregion

    #region Tabla Permisos
    public class Permiso
    {
        public string STR_NOMBRE_PERMISO { get; set; }
    }
    #endregion

    #region Tabla Pantallas
    public class Pantalla
    {
        //public ? FEC_MODIF { get; set; }

        public string STR_NOMBRE_PANTALLA { get; set; }
    }
    #endregion

    #region Tabla Grupos
    public class Grupo 
    {
        public string STR_NOMBRE_GRUPO { get; set; }
    }
    #endregion

    #region Catalogo Rol
    public class Rol 
    {
        public string STR_NOMBRE_ROL { get; set; }


        //public bool BOOL_ESTATUS_ROL { get; set; }

        //public ? FEC_MODIF { get; set; }
    }
    #endregion

    #region Catalogo Proyecto
    public class Proyecto 
    {
        public string STR_NOMBRE_PROYECTO { get; set; }

        public string STR_DESCRIPCION { get; set; }

        public string STR_RESPONSABLE { get; set; }

        public string STR_IDPROYECTO { get; set; }

        //public bool BOOL_STATUS_LOGICO { get; set; }

        //public ? FEC_MODIF { get; set; }
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

        //public bool BOOL_ESTATUS_COMPANIA { get; set; }
    }
    #endregion

    #region Modelo Centro de costos
    public class CentroCostos
    {
        [Key]
        public string id_cc { get; set; }

        public string STR_IDCENTROCOSTO { get; set; }
        [Required]
        public string name_cc { get; set; }
        public string STR_NOMBRE_CC { get; set; }
        [Required]
        public string categoria { get; set; }
        public string STR_CATEGORIA_CC { get; set; }
        [Required]
        public string estatus { get; set; }
        public string BOOL_eSTATUS_CC { get; set; }
        [Required]
        public string GERENTE__COMPANIA { get; set; }

        [Required]
        public string gerente { get; set; }
        [Required]
        public string id_empresa { get; set; }
        [Required]
        public string id_proyecto { get; set; }

    }
    #endregion
}

