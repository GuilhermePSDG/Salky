using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSocket.Shared.DataAcess;
using WebSocket.Shared.DataAcess.Local.Repositories;
using WebSocket.Shared.DataAcess.Models;
using Wpf.Core;

namespace Wpf.Services
{
    public class UserService
    {
        private RepositoryFactory repositoryFactory;
        private string picturefolder;

        public UserService(RepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
            this.picturefolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Salky","pictures");
            Directory.CreateDirectory(picturefolder);
        }

        public ContatoVM? GetContatoByPK(int userid ,byte[] pk)
        {
            ContatoVM? Contato = null;
            using(var repo = repositoryFactory.GetRepository<Usuario>())
            {
                var user = repo.GetById(userid);
                var contato = user.Contatos.FirstOrDefault(x => x.PublicKey.SequenceEqual(pk));
                if(contato != null)
                {
                    Contato = Mapping.Map<ContatoVM>(contato);
                }
            }
            return Contato;
        }

        public void Add(UsuarioVM usuarioVM)
        {
            try
            {
                using (var userRepo = repositoryFactory.GetRepository<Usuario>())
                {
                    if(userRepo.Find(x => x.PublicKey.SequenceEqual(usuarioVM.PublicKey)) != null)
                    {
                        MessageBox.Show("Usuario já cadastrado.","Falha ao adicionar usuario",MessageBoxButton.OK,MessageBoxImage.Warning);
                    }
                    else
                    {
                        var usuario = Mapping.Map<Usuario>(usuarioVM);
                        userRepo.Add(usuario);
                        var saved = userRepo.SaveChanges() > 0;
                        if (!saved)
                            MessageBox.Show("Erro durante o salvamento", "Falha ao adicionar usuario", MessageBoxButton.OK, MessageBoxImage.Warning);
                        else
                            MessageBox.Show("Usuario adicionado com sucesso.", "Adicionar usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro inesperado, {ex.Message}", "Erro ao adicionar usuario", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void AddMessage(int userid, byte[] contatoPublicKey,MessageVM message)
        {
            try
            {
                using (var userRepo = repositoryFactory.GetRepository<Usuario>())
                {
                    var usuario = userRepo.GetById(userid);
                    if (usuario == null)
                    {
                        MessageBox.Show("Usuario não encontrado.", "Falha ao adicionar mensagem", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var contatoIndex = usuario.Contatos.FindIndex(x => x.PublicKey.SequenceEqual(contatoPublicKey));
                    if (contatoIndex == -1)
                    {
                        MessageBox.Show("Contato não encontrado.", "Falha ao adicionar mensagem", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; ;
                    }
                    //
                    var messageDb = Mapping.Map<Message>(message);

                    usuario.Contatos[contatoIndex].Messages.Add(messageDb);
                    //
                    userRepo.Update(usuario);
                    var saved = userRepo.SaveChanges() > 0;
                    message.Id = messageDb.Id;
                    if (!saved)
                    {
                        MessageBox.Show("Erro durante o salvamento, nada foi salvo.", "Falha ao adicionar mensagem", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro inesperado, {ex.Message}", "Erro ao adicionar mensagem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void RefreshPictureContato(int userid,int contatoid,byte[] data)
        {
            using (var userRepo = repositoryFactory.GetRepository<Usuario>())
            {
                var user = userRepo.GetById(userid);
                var contato = user.Contatos.First(x => x.Id.Equals(contatoid));
                var picture_path = Path.Combine(this.picturefolder, $"{contato.Id}{Guid.NewGuid().ToString().Replace("-", "")}.jpg");
                File.WriteAllBytes(picture_path, data);
            }
        }

        public void AddContato(int userid,ContatoVM contato)
        {
            try
            {
                using (var userRepo = repositoryFactory.GetRepository<Usuario>())
                {
                    var user = userRepo.GetById(userid);
                    if(user == null)
                    {
                        MessageBox.Show("Usuario não encontrado.", "Falha ao adicionar o contato", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var contato_db = Mapping.Map<Contato>(contato);
                    if(user.Contatos.Any(x => x.PublicKey.SequenceEqual(contato_db.PublicKey)))
                    {
                        MessageBox.Show("Não foi possivel adicionar o contato, porque você já possui.", "Adicionar contato", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    user.Contatos.Add(contato_db);
                    userRepo.Update(user);
                    var saved = userRepo.SaveChanges() > 0;
                    if (saved)
                    {
                        contato.Id = contato_db.Id;
                        MessageBox.Show("Contato adicionado com sucesso.", "Adicionar contato", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erro durante o salvamento.", "Adicionar contato", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Um erro inesperado ocorreu, {ex.Message}", "Erro ao adicionar contato", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void UpdateContato(int userid, byte[] contatoPublicKey, ContatoVM contatoVM)
        {
            try
            {
                using (var userRepo = repositoryFactory.GetRepository<Usuario>())
                {
                    var user = userRepo.GetById(userid);
                    if (user == null)
                    {
                        MessageBox.Show("Usuario não encontrado.", "Falha ao atualizar o contato", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var contatoindex = user.Contatos.FindIndex(x => x.PublicKey.SequenceEqual(contatoPublicKey));
                    if (contatoindex == -1)
                    {
                        MessageBox.Show("Contato não encontrado.", "Falha ao atualizar o contato", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    contatoVM.Id = user.Contatos[contatoindex].Id;
                    user.Contatos[contatoindex] = Mapping.Map<Contato>(contatoVM);
                    userRepo.Update(user);
                    bool saved = userRepo.SaveChanges() > 0;
                    if (!saved)
                    {
                        MessageBox.Show("Erro durante o salvamento.", "Falha ao atualizar o contatos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Um erro inesperado ocorreu, {ex.Message}", "Erro ao atualizar contato", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
