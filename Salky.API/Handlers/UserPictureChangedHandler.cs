﻿using Salky.API.WebSocketRoutes.Models;
using Salky.App.Services.Friends;
using Salky.App.Services.Group;
using Salky.Domain.Contracts;
using Salky.Domain.Events.UserEvents;
using Salky.WebSocket.Infra.Interfaces;

namespace Salky.API.Handlers
{
    public class UserPictureChangedHandler : IHandler<UserPictureChanged>
    {
        public IConnectionManager ConnectionMannager { get; }
        public GroupMemberService GroupMemberService { get; }
        public FriendService FriendService { get; }

        public UserPictureChangedHandler(
            IConnectionManager connectionMannager,
            GroupMemberService groupMemberService,
            FriendService friendService

            )
        {
            ConnectionMannager = connectionMannager;
            GroupMemberService = groupMemberService;
            FriendService = friendService;
        }

        public async void Handle(UserPictureChanged args)
        {
            (await GroupMemberService.GetAllMembersOfUser(args.Id))
                .Select(x => new 
                { 
                    GroupId = x.GroupId.ToString() , 
                    MemberId = x.Id,
                    PictureSource = args.Value,
                }
                )
                .ToList()
                .ForEach(async info =>
                {
                    if (ConnectionMannager.TryGetConnectionPool(info.GroupId.ToString(), out var pool))
                    {
                        await pool.SendToAll("group/member/change/picture", Method.PUT, info);
                    }
                });
          
            (await this.FriendService.GetAll(args.Id))
                .Select(x => new
                {
                    FriendId = x.Id.ToString(),
                    PictureSource = args.Value,
                })
                .ToList()
                .ForEach(async info =>
                {
                    if (ConnectionMannager.TryGetConnectionPool(info.FriendId.ToString(), out var pool))
                    {
                        await pool.SendToAll("friend/change/picture", Method.PUT, info);
                    }
                }); 
            ;
        }
    }
}