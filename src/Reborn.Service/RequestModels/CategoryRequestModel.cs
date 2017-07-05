using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Reborn.Domain.Model;

namespace Reborn.Service.RequestModels
{
    public class CategoryRequestModel:BaseRequestModel
    {
        public int Status { get; set; }
        public bool ShowOnMenu { get; set; }
        public string Slug { get; set; }
    }


    public abstract class BaseRequestModel
    {
        private readonly List<PropertyInfo> ChangedProperties=new List<PropertyInfo>();

        public object this[string propertyName]
        {
            get => this.GetType().GetProperty(propertyName).GetValue(this, null);
            set
            {
                this.GetType().GetProperty(propertyName).SetValue(this, value, null);

                ChangedProperties.Add(this.GetType().GetProperty(propertyName));
            }
        }

        public bool IsChanged(string propertyName)
        {
            return ChangedProperties.Any(s => s.Name==propertyName);
        }
    }
}
