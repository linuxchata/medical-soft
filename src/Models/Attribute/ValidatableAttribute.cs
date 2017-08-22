using System;

namespace Models.Attribute
{
    /// <summary>
    /// Represent validation attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatableAttribute : System.Attribute
    {
    }
}
