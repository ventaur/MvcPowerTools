﻿using System.Web.Mvc;
using CavemanTools.Model;
using HtmlConventionsSample.Browse.Posts.ViewModels;
using MvcPowerTools.ControllerHandlers;
namespace HtmlConventionsSample.Browse.Posts
{
    public class PostsListController:QueryController<PagedInput,PostsListModel>
    {
        public override ActionResult Get(PagedInput input)
        {
            return Handle(input);
        }
    }
}