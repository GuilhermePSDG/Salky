using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Salky.API;
using LiteDB;
using Salky.Domain.Models.GroupModels;
using System.Security.Cryptography;
using Salky.Domain.Models.UserModels;
using StackExchange.Redis;
using Salky.WebSocket.Infra.Models;

Console.WriteLine("Hello");

var conR =  ConnectionMultiplexer.Connect("localhost:6379");

var sub = conR.GetSubscriber();
var db = conR.GetDatabase();
var channel = "a";
var h1 = (RedisChannel channel, RedisValue value) =>
{
    Console.WriteLine($"In Handler 1 - From Channel : {channel} - Value : {value}");
};
var h2 = (RedisChannel channel, RedisValue value) =>
{
    Console.WriteLine($"In Handler 2 - From Channel : {channel} - Value : {value}");
};
await sub.SubscribeAsync(channel, h1);
await sub.SubscribeAsync(channel, h2);

await sub.PublishAsync(channel, "Msg1");
await sub.PublishAsync(channel, "Msg2");
await Task.Delay(200);
await sub.UnsubscribeAsync(channel, h1);
Console.WriteLine("Handler 1 out");
await sub.PublishAsync(channel, "Msg3");

while (true) ;