using System;

namespace FubuMVC.Swagger.Specification
{
    public class AllowableValuesAttribute : Attribute
    {
        public string[] AllowableValues { get; private set; }

        public AllowableValuesAttribute(params string[] allowableValues)
        {
            AllowableValues = allowableValues;
        }
    }
}