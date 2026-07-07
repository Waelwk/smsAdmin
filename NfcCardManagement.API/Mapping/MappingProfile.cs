using AutoMapper;
using NfcCardManagement.API.DTOs.Employe;
using NfcCardManagement.API.DTOs.Vehicule;
using NfcCardManagement.API.Models;

namespace NfcCardManagement.API.Mapping;

/// <summary>
/// Profil AutoMapper définissant les mappings entre modèles EF Core et DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // GpEmploye → EmployeListDto
        CreateMap<GpEmploye, EmployeListDto>()
            .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.MotDePasseC != null))
            .ForMember(dest => dest.HasCTag, opt => opt.MapFrom(src => src.TagC != null));

        // GpEmploye → EmployeDetailDto (MotDePasseC jamais exposé)
        CreateMap<GpEmploye, EmployeDetailDto>()
            .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.MotDePasseC != null))
            .ForMember(dest => dest.HasCTag, opt => opt.MapFrom(src => src.TagC != null))
            .ForMember(dest => dest.TypeEmp, opt => opt.MapFrom(src => src.TypeEmp))
            .ForMember(dest => dest.CEquipe, opt => opt.MapFrom(src => src.CEquipe))
            .ForMember(dest => dest.CPosteEmployer, opt => opt.MapFrom(src => src.CPosteEmployer))
            .ForMember(dest => dest.CSociete, opt => opt.MapFrom(src => src.CSociete))
            .ForMember(dest => dest.CSite, opt => opt.MapFrom(src => src.CSite));

        // RefVehicule → VehiculeListDto
        CreateMap<RefVehicule, VehiculeListDto>()
            .ForMember(dest => dest.HasCTag, opt => opt.MapFrom(src => src.CTag != null));

        // RefVehicule → VehiculeDetailDto
        CreateMap<RefVehicule, VehiculeDetailDto>()
            .ForMember(dest => dest.HasCTag, opt => opt.MapFrom(src => src.CTag != null));
    }
}
