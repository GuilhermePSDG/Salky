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
using Salky.Domain.Models.GroupModels;
using System.Security.Cryptography;
using Salky.Domain.Models.UserModels;
using StackExchange.Redis;
using Salky.WebSocket.Infra.Models;
using Salky.App.Services;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using Salky.App.Security;
using Salky.Domain;
using Salky.Domain.Contracts;
using Salky.WebSocket.Handler;
using Salky.WebSocket.Infra.Interfaces;


var mapper = new ObjectMapper();
var res = mapper.MapProperties(typeof(B));
var json = JsonSerializer.Serialize(res, new JsonSerializerOptions() {WriteIndented=true });
Console.Write(json);
class ObjectMapper
{
    public ObjectMapper()
    {

    }



    Dictionary<string, Type> mappedTypes = new ();
    public object MapProperties(Type type)
    {
        mappedTypes = new();
        return SafeMapPropertiesLoopingPrevent(type) ?? throw new NullReferenceException();
    }
    private object? SafeMapPropertiesLoopingPrevent(Type? type,MemberInfo? HASMEMBER = null)
    {
        if (type == null) return null;
        if (IsPrimitive(type)) return new SimpleType(HASMEMBER == null ? type.Name : HASMEMBER.Name, type.Name);
        
        mappedTypes.Add(type.Name, type);
        var result = new Dictionary<string, List<object?>>();
        result[type.Name] = new List<object?>();

        foreach (var member in type.GetProperties())
        {
            if (IsPrimitive(member.PropertyType))
            {
                result[type.Name].Add(new SimpleType(member.Name, member.PropertyType.Name));
            }
            else if (IsCollection(member.PropertyType))
            {
                var arrayPropertyType = member.PropertyType.GetElementType() ?? throw new NullReferenceException();
                if (mappedTypes.ContainsKey(arrayPropertyType.Name))
                    result[type.Name].Add(new SimpleType(member.Name, member.PropertyType.Name));
                else
                    result[type.Name].Add(new ArrayType(member.Name, arrayPropertyType.Name, new ComplexType(arrayPropertyType.Name, SafeMapPropertiesLoopingPrevent(arrayPropertyType,member) ?? throw new NullReferenceException())));
            }
            else
            {
                if (mappedTypes.ContainsKey(member.PropertyType.Name))
                    result[type.Name].Add(new SimpleType(member.Name,member.PropertyType.Name));
                else
                    result[type.Name].Add(SafeMapPropertiesLoopingPrevent(member.PropertyType));
            }
        }
        return result;
    }

    bool IsPrimitive(Type type)
    {
        if (type.IsPrimitive)
            return true;
        if (type.Equals(typeof(string)))
            return true;
        if (type.Equals(typeof(DateTime)))
            return true;
        return false;
    }
    bool IsCollection(Type type)
    {
        if (type.IsArray)
            return true;
        if (type.GetInterface(nameof(ICollection)) != null)
            return true;
        return false;
    }
   
    record SimpleType(string Name, string TypeName);
    record ArrayType(string Name,string TypeName , ComplexType ArrayObject);
    record ComplexType(string Name, object ComplexObject);
}


record B(string naime, A[] a);
record A(string Name,string b);

record SafePerson(string Name, DateTime BornAt, Adress[] adress);


record Person(string Name,DateTime BornAt,Person[] Friend,Adress[] adress);
record Adress(string Streat,long HouseNumber);

//Console.WriteLine("Hello");

//var conR =  ConnectionMultiplexer.Connect("localhost:6379");

//var sub = conR.GetSubscriber();
//var db = conR.GetDatabase();
//var channel = "a";
//var h1 = (RedisChannel channel, RedisValue value) =>
//{
//    Console.WriteLine($"In Handler 1 - From Channel : {channel} - Value : {value}");
//};
//var h2 = (RedisChannel channel, RedisValue value) =>
//{
//    Console.WriteLine($"In Handler 2 - From Channel : {channel} - Value : {value}");
//};
//await sub.SubscribeAsync(channel, h1);
//await sub.SubscribeAsync(channel, h2);

//await sub.PublishAsync(channel, "Msg1");
//await sub.PublishAsync(channel, "Msg2");
//await Task.Delay(200);
//await sub.UnsubscribeAsync(channel, h1);
//Console.WriteLine("Handler 1 out");
//await sub.PublishAsync(channel, "Msg3");

//while (true) ;
