﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Xamarin.iOS.Binding.Transformer.Attributes;
using Xamarin.iOS.Binding.Transformer.Models.Collections;

namespace Xamarin.iOS.Binding.Transformer
{
    public class ApiDelegate : ApiObject
    {
        [ChangeIgnore]
        protected internal override string NodeName => $"delegate[@name='{Name}']";

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "returns")]
        public string ReturnType { get; set; }

        [ChangeIgnore]
        [XmlElement(ElementName = "parameter")]
        public List<ApiParameter> Parameters { get; set; }

        public ApiDelegate()
        {
            Parameters = new List<ApiParameter>();
        }

        internal protected override void SetParent(ApiObject parent)
        {
            base.SetParentInternal(parent);

            foreach (var aObject in Parameters)
            {
                aObject.SetParent(this);
            }

        }

        internal protected override void UpdatePathList(ref Dictionary<string, ApiObject> dict)
        {
            dict.Add(Path, this);

            foreach (var aNamespace in Parameters)
            {
                aNamespace.UpdatePathList(ref dict);
            }
        }

        internal override void Add(ApiObject item)
        {
            if (item is ApiParameter)
            {
                Parameters.Add((ApiParameter)item);
            }
        }

        internal override void Remove(ApiObject item)
        {
            if (item is ApiParameter)
            {
                Parameters.Remove((ApiParameter)item);
            }
        }

        public override void RemovePrefix(string prefix)
        {
            if (Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                Name = Name.Replace(prefix, "");
            }

            foreach (var aParam in Parameters)
            {
                aParam.RemovePrefix(prefix);
            }
        }
    }
}