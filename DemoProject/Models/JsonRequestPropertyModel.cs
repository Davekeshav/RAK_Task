using EPiServer.DataAnnotations;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;


namespace DemoProject.Models
{
    public class JsonRequestPropertyModel
    {
        public virtual ContentLink ContentLink { get; set; }
        public virtual string Name { get; set; }

        public virtual Title Title { get; set; }

        public virtual Content Content { get; set; }

        public virtual ParentLink ParentLink { get; set; }

    }

    public class Title
    {
        public virtual string Value { get; set; }

    }

    public class Content
    {
        public virtual string Value { get; set; }

    }

    public class ContentLink
    {
        public virtual int Id { get; set; }
        public virtual string GuidValue { get; set; }

        public virtual string Url { get; set; }

    }

    public class ParentLink
    {
        public virtual int Id { get; set; }
        public virtual string GuidValue { get; set; }

        public virtual string Url { get; set; }

    }

}
