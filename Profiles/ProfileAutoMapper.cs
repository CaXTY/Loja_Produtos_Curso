using AutoMapper;
using LojaProdutosCurso.Dto.Endereco;
using LojaProdutosCurso.Models;

namespace LojaProdutosCurso.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper() 
        { 
            CreateMap<EnderecoModel, EditarEnderecoDto>().ReverseMap();
        }
    }
}
