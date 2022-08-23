# Salky
> Salky é uma aplicação de chat

### Funcionalidades do projeto

- [x] Autenticação e Autorização
- [x] Sistema de amigos
- [x] Criação de grupos
- [x] Comunicação por texto
- [x] Comunicação por voz (Requer melhorias)
- [x] Gifs/Emoji
- [ ] `Chat privado (entre amigos)`
- [ ] `Varios canais/chats por grupo`
- [ ] `Configurações de grupo`

 >[Clique para acessar o aplicativo](https://roaring-empanada-489899.netlify.app)

## Como funciona
- Back End
    - RestApi escrita em `C#` para algumas operações como cadastro/login e consulta de dados
    - Para a comunicação via `WebSocket` utiliza o pacote [Salky.WebSockets](https://github.com/GuilhermePSDG/Salky.WebSockets)
    - Para armazenar as mensagens está sendo usado MongoDb, para os demais dados o `Entity FrameWork`, que permite a utilização de diversos bancos de dados `SQL`
   
- Frot End
  - Foi escrito em Angular

<br>

<h4 align="center"> 🚧 Projeto em construção 🚧 </h4>
