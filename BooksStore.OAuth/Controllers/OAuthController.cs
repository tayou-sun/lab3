using System.Web.Mvc;

namespace BooksStore.OAuth.Controllers
{
    public class OAuthController : Controller
    {
        #region Авторизация

        // GET: OAuth
        public void Authorize(string clientId)
        {
            //Переходим на создаение AccessToken
            //localhost:54147 - Session
            Response.Redirect("http://localhost:54147/Session/CreateAccessToken?clientId=" + clientId);
        }

        #endregion

        public void RefreshAccessToken(string accessToken, string clientId, string refreshToken)
        {
            Response.Redirect("http://localhost:54147/Session/RefreshAccessToken?accessToken=" + accessToken + "&clientId=" + clientId + "&refreshToken=" + refreshToken);
        }

        #region Поулчение данных о пользователе

        [HttpGet]
        public void Me(string accessToken)
        {
            //Проверяем возможные ошибки
            Response.Redirect("http://localhost:54147/Session/GetTokenUserInfo?accessToken=" + accessToken + "&callbackAction=localhost:54132/OAuth/FinishMe");
        }

        public string FinishMe(string username, string clientId)
        {
            return "{ \"username\":\"" + username + ", \"clientId\":" + clientId + " }";
        }

        #endregion

        #region Работа с магазином

        public void GetBooksInfo(string accessToken, string clientId)
        {
            //Проверяем возможные ошибки
            Response.Redirect("http://localhost:54147/Session/CheckAccessToken?accessToken=" + accessToken + "&clientId=" + clientId + "&callbackAction=localhost:49927/Books/GetBooksInfo");
        }

        public void GetBooksFromPage(string accessToken, string clientId, int pageNumber)
        {
            //Проверяем возможные ошибки
            Response.Redirect("http://localhost:54147/Session/CheckAccessToken?accessToken=" + accessToken + "&clientId=" + clientId + "&callbackAction=localhost:49927/Books/GetBooksFromPage?pageNumber=" + pageNumber);
        }

        #endregion

        #region Работа с покупками

        [HttpGet]
        public void Purchase(string accessToken, string clientId, int id)
        {
            Session["id"] = id;

            //Проверяем возможные ошибки
            Response.Redirect("http://localhost:54147/Session/CheckAccessToken?accessToken=" + accessToken + "&clientId=" + clientId + "&callbackAction=localhost:54132/OAuth/ProcessPurchase?accessToken=" + accessToken);
        }

        public void ProcessPurchase(string accessToken)
        {
            //Получаем имя пользователя
            Response.Redirect("http://localhost:54147/Session/GetTokenUserInfo?accessToken=" + accessToken + "&callbackAction=localhost:54132/OAuth/FinishPurchase");
        }

        public void FinishPurchase(string username, string clientId)
        {
            var id = (int) Session["id"];
            Session.Remove("id");

            Response.Redirect("http://localhost:49927/Books/ProcessPurchase?id=" + id + "&username=" + username);
        }

        #endregion

        #region Работа с биллингом

        [HttpGet]
        public void Billing(string accessToken, string clientId, int id)
        {
            //Проверяем возможные ошибки
            Response.Redirect("http://localhost:54147/Session/CheckAccessToken?accessToken=" + accessToken + "&clientId=" + clientId + "&callbackAction=localhost:54132/OAuth/FinishBilling?id=" + id);
        }

        public void FinishBilling(int id)
        {
            Response.Redirect("http://localhost:49927/Books/ProcessBilling?id=" + id);
        }

        #endregion
    }
}