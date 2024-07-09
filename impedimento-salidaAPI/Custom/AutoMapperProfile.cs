using AutoMapper;
using impedimento_salidaAPI.Models.DTOs;
using impedimento_salidaAPI.Models;


namespace impedimento_salidaAPI.Custom
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            #region Rol
            CreateMap<Role, RoleDTO>().ReverseMap();
            #endregion Rol

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>()
               .ForMember(destino =>
                   destino.RolDesc,
                   opt => opt.MapFrom(origen => origen.Rol.Rol)
               ).ForMember(destino =>
                   destino.EstatusDesc,
                   opt => opt.MapFrom(origen => origen.Estatus.Descripcion)
               );

            CreateMap<UsuarioDTO, Usuario>()
            .ForMember(destino =>
                destino.Rol,
                opt => opt.Ignore()
            ).ForMember(destino =>
                destino.Estatus,
                opt => opt.Ignore()
            );
            #endregion Usuario

            #region Ca
            CreateMap<Ca, CaDTO>().ReverseMap();
            #endregion Ca

            #region CertificacionExistencia
            CreateMap<CertificacionExistencium, CertificacionExistenciumDTO>()
               .ForMember(destino =>
                   destino.EstatusDesc,
                   opt => opt.MapFrom(origen => origen.Estatus.Descripcion)
               );

            CreateMap<CertificacionExistenciumDTO, CertificacionExistencium>()
            .ForMember(destino =>
                destino.Estatus,
                opt => opt.Ignore()
            );
            #endregion CertificacionExistencia

            #region Ciudadano
            CreateMap<Ciudadano, CiudadanoDTO>()
               .ForMember(destino =>
                   destino.RolDesc,
                   opt => opt.MapFrom(origen => origen.Rol.Rol)
               );

            CreateMap<CiudadanoDTO, Ciudadano>()
            .ForMember(destino =>
                destino.Rol,
                opt => opt.Ignore()
            );
            #endregion Ciudadano

            #region Estatus
            CreateMap<Estatus, EstatusDTO>()
               .ForMember(destino =>
                   destino.TipoDesc,
                   opt => opt.MapFrom(origen => origen.TipoCodigoNavigation.Descripcion)
               );

            CreateMap<EstatusDTO, Estatus>()
            .ForMember(destino =>
                destino.TipoCodigoNavigation,
                opt => opt.Ignore()
            );
            #endregion Estatus

            #region Rechazo
            CreateMap<Rechazo, RechazoDTO>().ReverseMap();
            #endregion Rechazo

            #region SolicitudLevantamiento 
            CreateMap<SolicitudLevantamiento, SolicitudLevantamientoDTO>()
               .ForMember(destino =>
                   destino.EstatusDesc,
                   opt => opt.MapFrom(origen => origen.Estatus.Descripcion)
               );

            CreateMap<SolicitudLevantamientoDTO, SolicitudLevantamiento>()
            .ForMember(destino =>
                destino.Estatus,
                opt => opt.Ignore()
            );
            #endregion SolicitudLevantamiento

            #region TipoEstatus
            CreateMap<TipoEstatus, TipoEstatusDTO>().ReverseMap();
            #endregion TipoEstatus

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion Menu

            #region MenuRol 
            CreateMap<Menurol, MenuRolDTO>().ReverseMap();
            #endregion MenuRol

        }
    }
}
