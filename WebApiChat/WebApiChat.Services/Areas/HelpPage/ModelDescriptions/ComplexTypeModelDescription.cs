namespace WebApiChat.Services.Areas.HelpPage.ModelDescriptions
{
    #region

    using System.Collections.ObjectModel;

    #endregion

    public class ComplexTypeModelDescription : ModelDescription
    {
        public ComplexTypeModelDescription()
        {
            this.Properties = new Collection<ParameterDescription>();
        }

        public Collection<ParameterDescription> Properties { get; private set; }
    }
}