using AutoMapper;
using TESTDB.DTO;
using TESTDB.Models;


namespace WebApplication1
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<AddUserDto, User>();
        }
    }
}
