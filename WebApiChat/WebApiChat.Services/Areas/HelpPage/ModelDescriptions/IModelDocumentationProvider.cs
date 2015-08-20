namespace WebApiChat.Services.Areas.HelpPage.ModelDescriptions
{
    #region

    using System;
    using System.Reflection;

    #endregion

    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}