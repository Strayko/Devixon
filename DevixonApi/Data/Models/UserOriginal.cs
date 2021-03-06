﻿using AutoMapper;
using DevixonApi.Data.Entities;

namespace DevixonApi.Data.Models
{
    public class UserOriginal : Profile
    {
        public UserOriginal()
        {
            this.CreateMap<UserModel, User>()
                .ForMember(d => d.Id, s
                    => s.MapFrom(u => u.Id))
                .ForMember(d => d.FirstName, s
                    => s.MapFrom(u => u.FirstName))
                .ForMember(d => d.LastName, s
                    => s.MapFrom(u => u.LastName))
                .ForMember(d => d.Email, s
                    => s.MapFrom(u => u.Email))
                .ForMember(d => d.CreatedAt, s
                    => s.MapFrom(u => u.CreatedAt));
        }
    }
}