namespace BooksStore.Account.Database.Model
{
    /// <summary>
    ///     Описание пользователя в базе данных
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Глобальный id пользователя (для базы данных)
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     Имя пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Для OAuth (вместо отдельной регистрации, чтобы упростить лабораторку)
        /// </summary>
        public int ClientId { get; set; }
    }
}