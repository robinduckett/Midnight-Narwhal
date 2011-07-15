using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace Reddit
{
    public delegate void RedditLoginSuccessHandler();
    public delegate void RedditLoginErrorHandler(string error);

    public class RedditClient
    {
        public static event RedditLoginSuccessHandler LoginSuccess;
        public static event RedditLoginErrorHandler LoginFailure;

        private static string _username;
        private static string _password;
        private static string _modhash;
        private static string _cookie;

        public static void Login(string Username, string Password)
        {
            _username = Username;
            _password = Password;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "http://www.reddit.com/api/login/" + HttpUtility.HtmlEncode(_username)
            );

            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Method = "POST";
            request.BeginGetRequestStream(LoginGetRequestCallback, request);
        }

        public static void LoginGetRequestCallback(IAsyncResult Result)
        {
            // Wrapping this async stuff in using as to keep the streams
            // in scope as well as telling the CLR to dispose of them when
            // they're done.

            var request = (HttpWebRequest)Result.AsyncState;
            using (var postStream = request.EndGetRequestStream(Result))
            {
                using (var memStream = new MemoryStream())
                {

                    var bodyData = string.Format(
                        "api_type=json&user={0}&passwd={1}",
                        HttpUtility.HtmlEncode(_username),
                        HttpUtility.HtmlEncode(_password)
                    );

                    _password = "";

                    var bytes = Encoding.UTF8.GetBytes(bodyData);

                    // Storing body data stream in a buffer as was having
                    // some problems when POSTing data to reddit

                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Position = 0;
                    var tempBuffer = new byte[memStream.Length];
                    memStream.Read(tempBuffer, 0, tempBuffer.Length);
                    postStream.Write(tempBuffer, 0, tempBuffer.Length);
                }
            }

            request.BeginGetResponse(LoginGetResponseCallback, request);
        }

        public static void LoginGetResponseCallback(IAsyncResult Result)
        {
            var request = (HttpWebRequest)Result.AsyncState;
            using (var response = (HttpWebResponse)request.EndGetResponse(Result))
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var streamRead = new StreamReader(responseStream))
                    {
                        string responseString = streamRead.ReadToEnd();
                        LoginParseJson(responseString);
                    }
                }
            }
        }

        public static void LoginParseJson(string json)
        {
            JObject login = JObject.Parse(json);

            // Making sure the LoginFailure / LoginSuccess events are triggered in
            // the UI thread

            if (login["json"]["errors"].Count() > 0)
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    LoginFailure(login["json"]["errors"][0][1].ToString());
                });
            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _cookie = login["json"]["data"]["cookie"].ToString();
                    _modhash = login["json"]["data"]["modhash"].ToString();
                    LoginSuccess();
                });
            }
        }
    }
}