using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Shared.DataAcess.Models;
using Wpf.MVVM.Models;

namespace Wpf.Core
{
    public static class Mapping
    {
        private static MapperConfiguration configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Usuario, UsuarioVM>();
            cfg.CreateMap<UsuarioVM, Usuario>();

            cfg.CreateMap<Contato, ContatoVM>();
            cfg.CreateMap<ContatoVM, Contato>();

            cfg.CreateMap<Message, MessageVM>();
            cfg.CreateMap<MessageVM, Message>();
        });
        private static IMapper mapper = configuration.CreateMapper();

        public static T Map<T>(object source)
        {
            return mapper.Map<T>(source);
        }

    }
}
