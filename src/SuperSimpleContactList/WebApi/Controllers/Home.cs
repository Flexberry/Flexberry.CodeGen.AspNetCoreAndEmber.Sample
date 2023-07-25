namespace SuperSimpleContactList.WebApi.Controllers
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
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
        private IDataService dataService;
        private IUser user;

        public Home(IDataService dataService, IUser user)
        {
            this.dataService = dataService;
            this.user = user;
        }

        /// <summary>
        /// GET Request example by url /api/Home.
        /// </summary>
        /// <returns>Contacts names.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Contact), Contact.Views.ContactE);
                List<Contact> resultList = dataService.LoadObjects(lcs).Cast<Contact>().ToList();

                List<string> contactsNames = new List<string>();

                foreach (Contact contact in resultList)
                {
                    contactsNames.Add(contact.Name);
                }

                return new JsonResult(contactsNames);
            }
            catch (Exception ex)
            {
                string errorMessage = "Request error - " + ex;
                LogService.LogError(errorMessage);

                return new JsonResult(errorMessage);
            }
        }
    }
}
