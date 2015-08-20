namespace WebApiChat.Services.Areas.HelpPage.ModelDescriptions
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     Describes a type model.
    /// </summary>
    public abstract class ModelDescription
    {
        public string Documentation { get; set; }

        public Type ModelType { get; set; }

        public string Name { get; set; }
    }
}