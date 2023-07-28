namespace SuperSimpleContactList.WebApi.Controllers
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Microsoft.AspNetCore.Mvc;
    using NewPlatform.SuperSimpleContactList;
    using static ICSSoft.Services.CurrentUserService;

    /// <summary>
    /// Controller example with DataService and User.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Home : Controller
    {
        private readonly IDataService dataService;
        private readonly IUser user;

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="dataService">DataService.</param>
        /// <param name="user">Current User.</param>
        public Home(IDataService dataService, IUser user)
        {
            this.dataService = dataService;
            this.user = user;
        }

        /// <summary>
        /// GET Request example by url /api/Home.
        /// This is the example method. NOT INCLUDED IN GENERATION.
        /// </summary>
        /// <returns>Contacts names.</returns>
        [HttpGet]
        public string GetContactsNames()
        {
            try
            {
                View view = new View();
                view.DefineClassType = typeof(Contact);
                view.AddProperty("Name");
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Contact), view);

                ObjectStringDataView[] resultList = dataService.LoadStringedObjectView(',', lcs);

                List<string> contactsNames = new List<string>();

                foreach (ObjectStringDataView contactObject in resultList)
                {
                    contactsNames.Add(contactObject.Data);
                }

                string resultMessatge = string.Join(", ", contactsNames.ToArray()) + " for User " + user.Login;

                return resultMessatge;
            }
            catch (Exception ex)
            {
                string errorMessage = "Contacts names read error: " + ex;
                LogService.LogError(errorMessage);

                return errorMessage;
            }
        }
    }
}
