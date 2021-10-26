using AutoMapper;

using CodeMaze.Data;
using CodeMaze.Models;

namespace CodeMaze.Notebooks.Extensions
{
    public class MappingProfileHelper : Profile
    {
        public MappingProfileHelper()
        {
            CreateMap<UserEntity, UserModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserModel, UserEntity>()
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Trim().ToLower() ?? string.Empty))
               .ReverseMap();

            CreateMap<NoteBookEntity, NotebookModel>().ReverseMap();
            CreateMap<NotebookModel, NoteBookEntity>().ReverseMap();
        }
    }
}
