namespace WebApiChat.Services.Areas.HelpPage.ModelDescriptions
{
    #region

    using System.Collections.ObjectModel;

    #endregion

    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            this.Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}