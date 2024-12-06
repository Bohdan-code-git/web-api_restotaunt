using AutoMapper;
using TESTDB.DTO;
using TESTDB.Models;
using WebApplication1.Dtos;


namespace WebApplication1
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<AddUserDto, User>();
            CreateMap<User, GetUserDto>();
            CreateMap<CreateOrderDto, Order>();
        }
    }
}
