﻿//-----------------------------------------------------------------------
// <copyright file="SharedControllerBase.cs" company="(none)">
//  Copyright © 2011 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WebGitNet
{
    using System.Web.Configuration;
    using System.Web.Mvc;

    public abstract partial class SharedControllerBase : Controller
    {
        private readonly FileManager fileManager;
        private readonly BreadCrumbTrail breadCrumbs;
        private bool areWeLimitedReader;

        public SharedControllerBase()
        {
            var reposPath = WebConfigurationManager.AppSettings["RepositoriesPath"];

            this.fileManager = new FileManager(reposPath);

            this.breadCrumbs = new BreadCrumbTrail();
            ViewBag.BreadCrumbs = this.breadCrumbs;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            // Set our user access rights flags
            areWeLimitedReader = areWeInReadOnlyLimitedGroup();
        }

        protected void AddRepoBreadCrumb(string repo)
        {
            this.BreadCrumbs.Append("Browse", "ViewRepo", repo, new { repo });
        }

        public FileManager FileManager
        {
            get
            {
                return this.fileManager;
            }
        }

        public BreadCrumbTrail BreadCrumbs
        {
            get
            {
                return this.breadCrumbs;
            }
        }

        protected bool AreWeLimitedReader
        {
            get
            {
                return this.areWeLimitedReader;
            }
        }

        private bool areWeInConfigGroup(string groupKeyName)
        {
            var clientPrincipal = (System.Security.Principal.WindowsPrincipal)User;
            var groupName = WebConfigurationManager.AppSettings[groupKeyName];
            return clientPrincipal.IsInRole(groupName);
        }

        private bool areWeInReadOnlyLimitedGroup()
        {
            return areWeInConfigGroup("ReadOnlyLimitedGroupName");
        }
    }
}
