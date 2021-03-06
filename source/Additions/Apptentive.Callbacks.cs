﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

using Object = Java.Lang.Object;

namespace ApptentiveSDK.Android
{
    public partial class Apptentive
    {
        /// <summary>
        /// The SDK must connect to our server at least once to download initial configuration for Message Center. 
        /// Call this method to see whether or not Message Center can be displayed.
        /// This task is performed asynchronously.
        /// </summary>
        /// <param name="callback">Called after we check to see if Message Center can be displayed, but before it is displayed. Called with true if an Interaction will be displayed, else false.</param>
        public static void CanShowMessageCenter(Action<bool> callback)
        {
            InvokeMethod("CanShowMessageCenter", new Type[] { typeof(IBooleanCallback) }, new object[] { WrapCallback(callback) });
        }

        /// <summary>
        /// Opens the Apptentive Message Center UI Activity, and allows custom data to be sent with the next message the user sends. If the user sends multiple messages, this data will only be sent
        /// with the first message sent after this method is invoked. Additional invocations of this method with custom data will repeat this process.
        /// If Message Center is closed without a message being sent, the custom data is cleared. This task is performed asynchronously. Message Center
        /// configuration may not have been downloaded yet when this is called.
        /// </summary>
        /// <param name="context">The context from which to launch the Message Center. This should be an Activity, except in rare cases where you don't have access to one, in which case Apptentive Message Center will launch in a new task.</param>
        /// <param name="callback">Called after we check to see if Message Center can be displayed, but before it is displayed.Called with true if an Interaction will be displayed, else false.</param>
        /// <param name="customData">A Map of String keys to Object values. Objects may be Strings, Numbers, or Booleans.If any message is sent by the Person, this data is sent with it, and then cleared. If no message is sent, this data is discarded.</param>
        public static void ShowMessageCenter(Context context, Action<bool> callback, IDictionary<string, Object> customData = null)
        {
            InvokeMethod("ShowMessageCenter", new Type[] { typeof(Context), typeof(IBooleanCallback), typeof(IDictionary<string, Object>) }, new object[] { context, WrapCallback(callback), customData });
        }

        /// <summary>
        /// This method takes a unique event string, stores a record of that event having been visited, determines if there is an interaction that
        /// is able to run for this event, and then runs it. If more than one interaction can run, then the most appropriate interaction takes precedence. Only
        /// one interaction at most will run per invocation of this method. This task is performed
        /// asynchronously.
        /// </summary>
        /// <param name="context">The context from which to launch the Interaction. This should be an Activity, except in rare cases where you don't have access to one, in which case Apptentive Interactions will launch in a new task.</param>
        /// <param name="eventName">A unique String representing the line this method is called on.</param>
        /// <param name="callback">Called after we check to see if an Interaction should be displayed. Called with true if an Interaction will be displayed, else false.</param>
        /// <param name="customData">A Map of String keys to Object values. Objects may be Strings, Numbers, or Booleans. This data is sent to the server for tracking information in the context of the engaged Event.</param>
        public static void Engage(Context context, string eventName, Action<bool> callback, IDictionary<string, Object> customData = null)
        {
            InvokeMethod("Engage", new Type[] { typeof(Context), typeof(string), typeof(IBooleanCallback), typeof(IDictionary<string, Object>) }, new object[] { context, eventName, WrapCallback(callback), customData });
        }

        /// <summary>
        /// This method can be used to determine if a call to one of the Engage() methods using the same event name will result in the display of an  Interaction.
        /// This is useful if you need to know whether an Interaction will be displayed before you create a UI Button, etc.
        /// </summary>
        /// <param name="eventName">A unique String representing the line this method is called on.</param>
        /// <param name="callback">Called after we check to see if an Interaction can be displayed. Called with true if an Interaction will be displayed, else false.</param>
        public static void QueryCanShowInteraction(string eventName, Action<bool> callback)
        {
            InvokeMethod("QueryCanShowInteraction", new Type[] { typeof(string), typeof(IBooleanCallback) }, new object[] { eventName, WrapCallback(callback) });
        }

        public static void BuildPendingIntentFromPushNotification(Action<PendingIntent> callback, IDictionary<string, string> data)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            InvokeMethod("BuildPendingIntentFromPushNotification", new Type[] { typeof(IPendingIntentCallback), typeof(IDictionary<string, string>) }, new object[] { new PendingIntentCallbackWrapper(callback), data });
        }

        public static void BuildPendingIntentFromPushNotification(Action<PendingIntent> callback, Bundle bundle)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            InvokeMethod("BuildPendingIntentFromPushNotification", new Type[] { typeof(IPendingIntentCallback), typeof(Bundle) }, new object[] { new PendingIntentCallbackWrapper(callback), bundle });
        }

        public static void BuildPendingIntentFromPushNotification(Action<PendingIntent> callback, Intent intent)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            InvokeMethod("BuildPendingIntentFromPushNotification", new Type[] { typeof(IPendingIntentCallback), typeof(Intent) }, new object[] { new PendingIntentCallbackWrapper(callback), intent });
        }

        public static void Login(string token, Action<bool, string> callback)
        {
            InvokeMethod("Login", new Type[] { typeof(string), typeof(ILoginCallback) }, new object[] { token, callback != null ? new LoginCallbackWrapper(callback) : null });
        }

        private static void InvokeMethod(string methodName, Type[] types, object[] parameters)
        {
            try
            {
                var method = typeof(Apptentive).GetMethod(methodName, types);
                if (method == null)
                {
                    Console.WriteLine("Unable to invoke method '" + methodName + "' since it can't be resolved");
                    return;
                }

                method.Invoke(null, parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to invoke method '" + methodName + "': " + e.Message);
            }
        }

        static BooleanCallbackWrapper WrapCallback(Action<bool> callback)
        {
            return callback != null ? new BooleanCallbackWrapper(callback) : null;
        }

        public partial interface IBooleanCallback
        {
        }

        public partial interface IPendingIntentCallback
        {
        }

        public partial interface ILoginCallback
        {
        }

        internal class BooleanCallbackWrapper : Object, IBooleanCallback
        {
            readonly Action<bool> m_callback;

            public BooleanCallbackWrapper(Action<bool> callback)
            {
                if (callback == null)
                {
                    throw new ArgumentNullException("callback");
                }
                m_callback = callback;
            }

            public void OnFinish(bool result)
            {
                m_callback(result);
            }
        }

        internal class PendingIntentCallbackWrapper : Object, IPendingIntentCallback
        {
            readonly Action<PendingIntent> m_callback;

            public PendingIntentCallbackWrapper(Action<PendingIntent> callback)
            {
                if (callback == null)
                {
                    throw new ArgumentNullException("callback");
                }
                m_callback = callback;
            }

            public void OnPendingIntent(PendingIntent intent)
            {
                m_callback(intent);
            }
        }

        internal class LoginCallbackWrapper : Object, ILoginCallback
        {
            readonly Action<bool, string> m_callback;

            public LoginCallbackWrapper(Action<bool, string> callback)
            {
                if (callback == null)
                {
                    throw new ArgumentNullException("callback");
                }
                m_callback = callback;
            }

            public void OnLoginFail(string errorMessage)
            {
                m_callback(false, errorMessage);
            }

            public void OnLoginFinish()
            {
                m_callback(true, null);
            }
        }
    }
}
