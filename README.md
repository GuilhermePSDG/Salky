# Salky
> Salky é uma aplicação de chat

### Funcionalidades do projeto

- [x] Autenticação e Autorização
- [x] Sistema de amigos
- [x] Criação de grupos
- [x] Comunicação por texto
- [x] Comunicação por voz (Requer melhorias)
- [x] Gifs/Emoji


## Como funciona
- Back End
    - RestApi escrita em `C#` para algumas operações como cadastro/login e consulta de dados
    - Para a comunicação via `WebSocket` utiliza o pacote [Salky.WebSockets](https://github.com/GuilhermePSDG/Salky.WebSockets)
    - Para armazenar as mensagens está sendo usado MongoDb, para os demais dados o `Entity FrameWork`, que permite a utilização de diversos bancos de dados `SQL`
   
- Frot End
  - Foi escrito em Angular

<br>


