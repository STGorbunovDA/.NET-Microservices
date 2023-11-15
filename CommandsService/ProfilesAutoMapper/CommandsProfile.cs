using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.ProfilesAutoMapper
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();

        }
    }
}
