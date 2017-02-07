using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BooksStore.Session.AccessTokenManager;

namespace BooksStore.Session.Controllers
{
    public class SessionController : Controller
    {
        #region Общение с внешним миром

        public string ReturnError(string error, string errorDescription)
        {
            return "{ \"error\":\"" + error + "\",\"error_description\":\"" + errorDescription + "\" }";
        }

        public void RedirectToMicroservice(string uri)
        {
            Response.Redirect("http://" + uri);
        }

        #endregion

        #region Создание AccessToken

        // GET: Session
        /// <summary>
        ///     Начало получения AccessToken
        /// </summary>
        public void CreateAccessToken(string clientId)
        {
            Session["clientId"] = clientId;

            //Редиректим на проверку clientId
            //localhost:53486 - Account
            //localhost:54147 - Session
            Response.Redirect("http://localhost:53486/Account/IsValidClientId?clientId=" + clientId + "&field=wasClientIdCorrect&callbackAction=localhost:54147/Session/ProcessAuthorization");
        }

        /// <summary>
        ///     Пройдена проверка clientId, ее результан передан сюда. Если с ним все хорошо, то переходит к авторизации пользователя
        /// </summary>
        /// <param name="wasClientIdCorrect"></param>
        /// <returns></returns>
        public ActionResult ProcessAuthorization(string wasClientIdCorrect)
        {
            if (!bool.Parse(wasClientIdCorrect))
            {
                Session.Remove("clientId");

                return RedirectToAction("ReturnError", "Session",
                    new {error = "access_denied", errorDescription = "Wrong client id!"});
            }

            return RedirectToAction("ContinueAuthorization", "Session");
        }

        /// <summary>
        ///     Производит саму авторизацию пользователя
        /// </summary>
        public void ContinueAuthorization()
        {
            //Переходим на страницу логина
            //localhost:53486 - Account
            //localhost:54147 - Session
            Response.Redirect("http://localhost:53486/Account/Login?callbackAction=localhost:54147/Session/FinishAuthorization");
        }

        /// <summary>
        ///     Заканчивает авторизацию и возвращает AccessToken
        /// </summary>
        public string FinishAuthorization(string username, string clientId)
        {
            var accessTokenClientId = int.Parse(Session["clientId"].ToString());
            Session.Remove("clientId");

            return AccessTokenManager.AccessTokenManager.CreateAccessToken(accessTokenClientId, username).ToString();
        }

        #endregion

        #region Проверка AccessToken

        /// <summary>
        ///     Проверяет AccessToken. Идея такая:
        ///         если что-то не в порядке, то этот микросервис сам вернет ошибку
        ///         если же все хорошо, то управление просто будет передано дальше
        /// </summary>
        public ActionResult CheckAccessToken(string accessToken, string clientId, string callbackAction)
        {
            var token = AccessTokenManager.AccessTokenManager.GetAccessToken(accessToken);

            if (token == null)
                return RedirectToAction("ReturnError", "Session", new { error = "invalid_access_token", errorDescription = "Unknown access token!" });

            if (token.IsExpired())
                return RedirectToAction("ReturnError", "Session", new { error = "invalid_access_token", errorDescription = "Your access token is obsolete!" });

            if (token.ClientId != int.Parse(clientId))
                return RedirectToAction("ReturnError", "Session", new { error = "access_denied", errorDescription = "Wrong ClientId!" });

            return RedirectToAction("RedirectToMicroservice", "Session", new { uri = callbackAction });
        }

        #endregion

        #region Получение информации о токене

        public ActionResult GetTokenUserInfo(string accessToken, string callbackAction)
        {
            var token = AccessTokenManager.AccessTokenManager.GetAccessToken(accessToken);

            return RedirectToAction("RedirectToMicroservice", "Session",
                new {uri = callbackAction + "?username=" + token.Username + "&clientId=" + token.ClientId});
        }

        #endregion

        #region Обновление токена

        public ActionResult RefreshAccessToken(string accessToken, string clientId, string refreshToken)
        {
            Session["accessToken"] = accessToken;
            Session["refreshToken"] = refreshToken;
           
            return RedirectToAction("CheckAccessToken", "Session", new { accessToken, clientId, callbackAction = "localhost:54147/Session/ProcessRefreshing" });
        }

        public ActionResult ProcessRefreshing()
        {
            var accessToken = Session["accessToken"].ToString();
            Session.Remove("accessToken");

            var refreshToken = Session["refreshToken"].ToString();
            Session.Remove("refreshToken");

            var token = AccessTokenManager.AccessTokenManager.GetAccessToken(accessToken);

            if (token.RefreshToken != refreshToken)
                return RedirectToAction("ReturnError", "Session", new { error = "access_denied", errorDescription = "Wrong refresh token!" });

            return RedirectToAction("FinishRefreshing", "Session", new { result = token.ToString() });
        }

        public string FinishRefreshing(string result)
        {
            return result;
        }

        #endregion
    }
}