using System;

namespace Aluguru.Marketplace.Infrastructure.Swagger
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
}
