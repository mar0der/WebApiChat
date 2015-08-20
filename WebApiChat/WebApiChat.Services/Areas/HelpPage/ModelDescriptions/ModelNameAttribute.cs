namespace WebApiChat.Services.Areas.HelpPage.ModelDescriptions
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     Use this attribute to change the name of the <see cref="ModelDescription" /> generated for a type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
    public sealed class ModelNameAttribute : Attribute
    {
        public ModelNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}