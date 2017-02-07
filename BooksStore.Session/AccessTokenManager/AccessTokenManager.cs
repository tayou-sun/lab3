using System;
using System.Collections.Generic;
using System.Linq;

namespace BooksStore.Session.AccessTokenManager
{
    public static class AccessTokenManager
    {
        public static bool DoExist(string username, int clientId)
        {
            return _accessTokens.Any(at => at.Value.Username == username && at.Value.ClientId == clientId);
        }

        public static AccessToken CreateAccessToken(int clientId, string username)
        {
            if (clientId <= 0)
                return null;

            if (_accessTokens == null)
                _accessTokens = new Dictionary<string, AccessToken>();

            var accessToken = new AccessToken(clientId, username);
            _accessTokens.Add(accessToken.Token, accessToken);

            return accessToken;
        }

        public static AccessToken GetAccessToken(string accessToken)
        {
            return !_accessTokens.ContainsKey(accessToken) ? null : _accessTokens[accessToken];
        }

        public static void Remove(string accessToken)
        {
            if (_accessTokens.ContainsKey(accessToken))
                _accessTokens.Remove(accessToken);
        }

        private static Dictionary<string, AccessToken> _accessTokens;
    }
}