using System;

namespace BooksStore.Session.AccessTokenManager
{
    /// <summary>
    ///     Класс, реализующий функциональность Access Token
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="clientId">Client ID</param>
        /// <param name="username">Имя пользователя</param>
        public AccessToken(int clientId, string username)
        {
            Token = Guid.NewGuid().ToString();
            RefreshToken = Guid.NewGuid().ToString();
            Generated = DateTime.Now;
            ExpiresIn = 1800;
            ClientId = clientId;
            Username = username;
        }

        /// <summary>
        ///     Сам токен
        /// </summary>
        public string Token { get; }

        /// <summary>
        ///     Токен для обновления
        /// </summary>
        public string RefreshToken { get; private set; }

        /// <summary>
        ///     Client Id, с которым связан токен
        /// </summary>
        public int ClientId { get; }

        /// <summary>
        ///     Имя пользоваетеля, с котором связан токен
        /// </summary>
        public string Username { get; }

        /// <summary>
        ///     Момент времени, в который был сгенерирован токен
        /// </summary>
        public DateTime Generated { get; private set; }

        /// <summary>
        ///     Время жизни токена (с момента генерации)
        /// </summary>
        public int ExpiresIn { get; }

        /// <summary>
        ///     Проверяет, истек ли AccessToken
        /// </summary>
        /// <returns>true - если истек, false в противном случае</returns>
        public bool IsExpired()
        {
            return Generated.AddSeconds(ExpiresIn) < DateTime.Now;
        }

        /// <summary>
        ///     Продливает AccessToken
        /// </summary>
        /// <returns>true - если продлен успешно, false в противном случае</returns>
        public bool Extend(string refreshToken)
        {
            //Если Access Token невалидный, то и продлить его нельзя
            if (!IsValid() || refreshToken != RefreshToken)
                return false;

            RefreshToken = Guid.NewGuid().ToString();
            Generated = DateTime.Now;
            return true;
        }

        /// <summary>
        ///     Проверяет AccessToken на валидность
        ///     Валидным он считается, даже если истек, но не превысил время жизни на 10 минут (600 секунд)
        /// </summary>
        /// <returns>true - если валидный, false в противном случае</returns>
        public bool IsValid()
        {
            return Generated.AddSeconds(ExpiresIn + 600) <= DateTime.Now;
        }

        public override string ToString()
        {
            return "{ \"access_token\":\"" + Token + "\", \"refresh_token\":\"" + RefreshToken + "\", \"expires_in\":" +
                   ExpiresIn + " }";
        }
    }
}