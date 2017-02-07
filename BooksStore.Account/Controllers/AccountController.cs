using System;
using System.Linq;
using System.Web.Mvc;
using BooksStore.Account.Database;
using BooksStore.Account.Database.Model;
using BooksStore.Account.Models;

namespace BooksStore.Account.Controllers
{
    /// <summary>
    ///     Контроллер для управления аккаунтами и авторизацией
    /// </summary>
    public class AccountController : Controller
    {
        private readonly DatabaseController _databaseController;

        public AccountController()
        {
            _databaseController = new DatabaseController();
        }

        #region Utils

        public ActionResult IsValidClientId(string clientId, string field, string callbackAction)
        {
            return RedirectToAction("ReturnResult", "Account",
                new
                {
                    uri = callbackAction,
                    fieldName = field,
                    result =
                        clientId == null || _databaseController.Users.GetAll().All(u => u.ClientId != int.Parse(clientId))
                            ? "False"
                            : "True"
                });
        }

        public string GetUserInfo(string username)
        {
            return "{ \"username=\":\"" + username + "\", \"clientId\":" +
                   _databaseController.Users.GetAll().FirstOrDefault(u => u.Username == username)?.ClientId + " }";
        }

        #endregion

        #region Редиректы

        /// <summary>
        ///     Для общения с внешним миром (другими микросервисами), возвращает результаты авторизации по указанному адресу
        ///     возрата
        /// </summary>
        public void ReturnAuthorizationResult(string uri, string username, string clientId)
        {
            Response.Redirect("http://" + uri + "?username=" + username + "&clientId=" + clientId);
        }

        /// <summary>
        ///     Для общения с внешним миром (другими микросервисами), возвращает результаты авторизации по указанному адресу
        ///     возрата (в дополнительных функциях)
        /// </summary>
        public void ReturnResult(string uri, string fieldName, string result)
        {
            Response.Redirect("http://" + uri + "?" + fieldName + "=" + result);
        }

        #endregion

        #region Login

        // GET: Account/Login
        public ActionResult Login(string callbackAction)
        {
            Session["callbackAction"] = callbackAction;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            //Если случилась ошибка, то возвращаем наш же вид
            if (!ModelState.IsValid)
                return View();

            //Пытаемся получить пользователя. Если его не будет в БД, то вернется null
            var user =
                _databaseController.Users.GetAll()
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid register attempt (user does not exist).");
                return View(model);
            }

            var callbackAction = Session["callbackAction"].ToString();
            Session.Remove("callbackAction");

            return RedirectToAction("ReturnAuthorizationResult", "Account",
                new {uri = callbackAction, username = user.Username, clientId = user.ClientId});
        }

        #endregion

        #region Register

        // GET: Account/Register
        public ActionResult Register(string callbackAction)
        {
            Session["callbackAction"] = callbackAction;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            //Если случилась ошибка, то возвращаем наш же вид
            if (!ModelState.IsValid)
                return View();

            //Пытаемся получить пользователя. Если его не будет в БД, то вернется null
            var user =
                _databaseController.Users.GetAll()
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            //Пользователь уже есть, не можем проводить регистрацию
            if (user != null)
            {
                ModelState.AddModelError("", "Invalid register attempt (user exists).");
                return View(model);
            }

            //Создаем пользователя
            user = new User {Username = model.Username, Password = model.Password};

            //Генерируем id для клиента
            var rand = new Random();
            do
            {
                user.ClientId = rand.Next();
            } while (_databaseController.Users.GetAll().Any(u => u.ClientId == user.ClientId));

            //Добавляем запись в базу данных
            _databaseController.Users.Create(user);

            var callbackAction = Session["callbackAction"].ToString();
            Session.Remove("callbackAction");

            return RedirectToAction("ReturnAuthorizationResult", "Account",
                new {uri = callbackAction, username = user.Username, clientId = user.ClientId});
        }

        #endregion
    }
}