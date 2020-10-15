using AutoMapper;
using DatingApp.API.DTOSs;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    //  WITHOUT THIS CLASS WE WOULD GET THIS ERROR FOR http:// localhost:5000/api/users/2:

    //AutoMapper.AutoMapperMappingException: Missing type map configuration or unsupported mapping.
    //Mapping types:
    //User -> UserForDetailedDto
    //DatingApp.API.Models.User -> DatingApp.API.DTOSs.UserForDetailedDto

    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl,
                            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age,
                                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl,
                            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age,
                                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();

            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();

            //AutoMapper is convention based that means if we provide above createMap and 2 classes 
            //automaper will map automatically all of the properties with the same name so here 
            // Username or City but not the Age from dto (DateofBirth from user) and PhotoUrl(Photos from user) so
            //we need configuration for these
            //the order here is like this CreateMap<from class to class>
        }
    }
}
