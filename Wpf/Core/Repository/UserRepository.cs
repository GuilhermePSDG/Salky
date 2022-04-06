using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Shared.DataAcess;
using WebSocket.Shared.DataAcess.Models;
using Wpf.MVVM.Models;

namespace Wpf.Core.Repository
{
    //public class UserRepository : IViewModelRepository<UsuarioVM>
    //{
    //    private IRepository<Usuario> userRepo;

    //    public UserRepository(IRepository<Usuario> userRepository)
    //    {
    //        this.userRepo = userRepository;
    //    }
    //    public void Add(UsuarioVM entity)
    //    {
    //        var user = Mapping.Convert(entity);
    //        userRepo.Add(user);
    //    }

    //    public UsuarioVM? Find(Func<UsuarioVM, bool> expression)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public List<UsuarioVM>? FindAll(Func<UsuarioVM, bool> expression)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public List<UsuarioVM>? GetAll()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public UsuarioVM? GetById(int id)
    //    {
    //        var f = userRepo.GetById(id);
    //        if (f == null) return null;
    //        else return Mapping.Convert(f);
    //    }

    //    public void Remove(UsuarioVM entity)
    //    {
    //        userRepo.Remove(entity.Id);
    //    }

    //    public void Remove(int id)
    //    {
    //        userRepo.Remove(id);
    //    }
    //    public int SaveChanges()
    //    {
    //        return userRepo.SaveChanges();
    //    }
    //    public async Task<int> SaveChangesAsync()
    //    {
    //        return await userRepo.SaveChangesAsync();
    //    }
    //    public void Update(UsuarioVM entity)
    //    {
    //        throw new NotImplementedException();
    //    }
    
    
      

    //}
}
