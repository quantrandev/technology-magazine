using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using VNScience.Models.Core;
using VNScience.ViewModels;

namespace VNScience.Common
{
    public class MapperConfig
    {
        public MapperConfig()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PostCategory, PostCategory>();
                cfg.CreateMap<PostViewModel, Post>();
                cfg.CreateMap<Post, PostViewModel>();
                cfg.CreateMap<Menu, Menu>();
                cfg.CreateMap<Menu, MenuViewModel>();
            });
        }
    }
}