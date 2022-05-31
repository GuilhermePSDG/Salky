namespace Salky.WebSocket.Infra.Models;

public enum Method
{
    GET,
    GET_RESPONSE,
    POST,
    PUT,
    DELETE,
    REDIRECT,
    ERROR,
    LISTENER,
    CONFIRM,
    //Rotas menores que 0 reservadas para uso interno
    _CONNECTIONCLOSED = -1,
    _AFTERCONNECTIONOPEN = -2
}