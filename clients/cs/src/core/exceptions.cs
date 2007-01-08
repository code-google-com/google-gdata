/* Copyright (c) 2006 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/
#region Using directives

#define USE_TRACING
#define USE_LOGGING

using System;
using System.Xml; 
using System.Net;
using System.Diagnostics;
#if WindowsCE
#else 
using System.Runtime.Serialization;
#endif
using System.Security.Permissions;
using System.IO;
using System.Text; 



#endregion


//////////////////////////////////////////////////////////////////////
// <summary>custom exceptions</summary> 
//////////////////////////////////////////////////////////////////////
namespace Google.GData.Client
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>standard exception class to be used inside the query object
    /// </summary> 
    //////////////////////////////////////////////////////////////////////
#if WindowsCE
#else 
    [Serializable]
#endif
    public class LoggedException : Exception
    {

        //////////////////////////////////////////////////////////////////////
        /// <summary>default constructor so that FxCop does not complain</summary> 
        //////////////////////////////////////////////////////////////////////
        public LoggedException()
        {
            
        }
        /////////////////////////////////////////////////////////////////////////////


        //////////////////////////////////////////////////////////////////////
        /// <summary>standard overloaded constructor</summary> 
        /// <param name="msg">msg for the exception</param>
        //////////////////////////////////////////////////////////////////////
        public LoggedException(string msg) : base(msg)
        {
            LoggedException.EnsureLogging();
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////
        /// <summary>standard overloaded constructor</summary> 
        /// <param name="msg">msg for the exception</param>
        /// <param name="exception">inner exception</param>
        //////////////////////////////////////////////////////////////////////
        public LoggedException(string msg, Exception exception) : base(msg,exception)
        {
            LoggedException.EnsureLogging();
        }
        /////////////////////////////////////////////////////////////////////////////

#if WindowsCE
#else 
        /// <summary>here to please FxCop and maybe for future use</summary> 
        protected LoggedException(SerializationInfo info,  StreamingContext context) : base(info, context)
        {
        }
#endif
        //////////////////////////////////////////////////////////////////////
        /// <summary>protected void EnsureLogging()</summary> 
        //////////////////////////////////////////////////////////////////////
        [Conditional("USE_LOGGING")] protected static void EnsureLogging()
        {
         }
        /////////////////////////////////////////////////////////////////////////////

    }
    /////////////////////////////////////////////////////////////////////////////





    //////////////////////////////////////////////////////////////////////
    /// <summary>standard exception class to be used inside the query object
    /// </summary> 
    //////////////////////////////////////////////////////////////////////
#if WindowsCE
#else 
    [Serializable]
#endif
    public class ClientQueryException : LoggedException
    {
        //////////////////////////////////////////////////////////////////////
        /// <summary>default constructor so that FxCop does not complain</summary> 
        //////////////////////////////////////////////////////////////////////
        public ClientQueryException()
        {

        }
        /////////////////////////////////////////////////////////////////////////////


        //////////////////////////////////////////////////////////////////////
        /// <summary>standard overloaded constructor</summary> 
        /// <param name="msg">msg for the exception</param>
        //////////////////////////////////////////////////////////////////////
        public ClientQueryException(string msg) : base(msg)
        {
        }
        /////////////////////////////////////////////////////////////////////////////

        /// <summary>here to please FxCop and for future use</summary> 
        public ClientQueryException(string msg, Exception innerException) : base(msg, innerException)
        {
        }

#if WindowsCE
#else 
        /// <summary>here to please FxCop and maybe for future use</summary> 
        protected ClientQueryException(SerializationInfo info,  StreamingContext context) : base(info, context)
        {
        }
#endif
    }
    /////////////////////////////////////////////////////////////////////////////


    //////////////////////////////////////////////////////////////////////
    /// <summary>standard exception class to be used inside the feed object
    /// </summary> 
    //////////////////////////////////////////////////////////////////////
#if WindowsCE
#else 
    [Serializable]
#endif
    public class ClientFeedException : LoggedException
    {

        //////////////////////////////////////////////////////////////////////
        /// <summary>default constructor so that FxCop does not complain</summary> 
        //////////////////////////////////////////////////////////////////////
        public ClientFeedException()
        {

        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////
        /// <summary>standard overloaded constructor</summary> 
        /// <param name="msg">msg for the exception</param>
        //////////////////////////////////////////////////////////////////////
        public ClientFeedException(string msg) : base(msg)
        {
        }
        /////////////////////////////////////////////////////////////////////////////

        /// <summary>here to please FxCop and for future use</summary> 
        public ClientFeedException(string msg, Exception innerException) : base(msg, innerException)
        {
        }
        /////////////////////////////////////////////////////////////////////////////
#if WindowsCE
#else 
        /// <summary>here to please FxCop and maybe for future use</summary> 
        protected ClientFeedException(SerializationInfo info,  StreamingContext context) : base(info, context)
        {
        }
#endif
    }
    /////////////////////////////////////////////////////////////////////////////


    //////////////////////////////////////////////////////////////////////
    /// <summary>standard exception class to be used inside GDataRequest
    /// </summary> 
    //////////////////////////////////////////////////////////////////////
#if WindowsCE
#else 
    [Serializable]
#endif
    public class GDataRequestException : LoggedException
    {

        /// <summary>holds the webresponse object</summary> 
        protected WebResponse webResponse;

        //////////////////////////////////////////////////////////////////////
        /// <summary>default constructor so that FxCop does not complain</summary> 
        //////////////////////////////////////////////////////////////////////
        public GDataRequestException()
        {

        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////
        /// <summary>Read only accessor for response</summary> 
        //////////////////////////////////////////////////////////////////////
        public WebResponse Response
        {
            get {return this.webResponse;}
        }

        /// <summary>
        /// this uses the webresponse object to get at the
        /// stream send back from the server and return the 
        /// error message. 
        /// </summary>
        public string ResponseString
        {
            get 
            {
                string responseText = null; 
        
                if (this.webResponse != null)
                {
                    // Obtain a 'Stream' object associated with the response object.
                    Stream receiver = this.webResponse.GetResponseStream();
                    if (receiver != null)
                    {
                        // Pipe the stream to a higher level stream reader with the default encoding format. 
                        // which is UTF8
                        // 
                        StreamReader readStream = new StreamReader(receiver); 
                        
                        // Read 256 charcters at a time.    
                        char []buffer = new char[256]; 
                        StringBuilder builder = new StringBuilder(1024); 
                        int count = readStream.Read( buffer, 0, 256 );
                        while (count > 0) 
                        {
                            // Dump the 256 characters on a string and display the string onto the console.
                            builder.Append(buffer); 
                            count = readStream.Read(buffer, 0, 256);
                        }
                        
                        // Release the resources of stream object.
                        readStream.Close();
                        receiver.Close(); 

                        responseText = builder.ToString(); 
                    }
                    
                }

                return responseText; 
            }
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>public GDataRequestException(WebException e)</summary> 
        /// <param name="msg"> the exception message as a string</param>
        /// <param name="exception"> the inner exception</param>
        //////////////////////////////////////////////////////////////////////
        public GDataRequestException(string msg, Exception exception) : base(msg, exception)
        {
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////
        /// <summary>public GDataRequestException(WebException e)</summary> 
        /// <param name="msg"> the exception message as a string</param>
        //////////////////////////////////////////////////////////////////////
        public GDataRequestException(string msg) : base(msg)
        {
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////
        /// <summary>public GDataRequestException(WebException e)</summary> 
        /// <param name="msg"> the exception message as a string</param>
        /// <param name="exception"> the inner exception</param>
        //////////////////////////////////////////////////////////////////////
        public GDataRequestException(string msg, WebException exception) : base(msg, exception)
        {
            if (exception != null)
            {
                this.webResponse = exception.Response;    
            }
            
        }
        /////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////
        /// <summary>public GDataRequestException(WebException e)</summary> 
        /// <param name="msg"> the exception message as a string</param>
        /// <param name="response"> the webresponse object that caused the exception</param>
        //////////////////////////////////////////////////////////////////////
        public GDataRequestException(string msg, WebResponse response) : base(msg)
        {
            this.webResponse = response;
        }
        /////////////////////////////////////////////////////////////////////////////
#if WindowsCE
#else 
        /// <summary>here to please FxCop and maybe for future use</summary> 
        protected GDataRequestException(SerializationInfo info,  StreamingContext context) : base(info, context)
        {
        }


        /// <summary>overridden to make FxCop happy and future use</summary> 
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData( SerializationInfo info,  StreamingContext context ) 
        {
            base.GetObjectData( info, context );
        }
#endif
    }
    /////////////////////////////////////////////////////////////////////////////
    


    //////////////////////////////////////////////////////////////////////
    /// <summary>exception class thrown when we encounter an access denied
    /// (HttpSTatusCode.Forbidden) when accessing a server
    /// </summary> 
    //////////////////////////////////////////////////////////////////////    
    public class GDataForbiddenException : GDataRequestException
    {
        //////////////////////////////////////////////////////////////////////
        /// <summary>constructs a forbidden exception</summary> 
        /// <param name="msg"> the exception message as a string</param>
        /// <param name="response"> the webresponse object that caused the exception</param>
        //////////////////////////////////////////////////////////////////////
        public GDataForbiddenException(string msg, WebResponse response) : base(msg)
        {
            this.webResponse = response;
        }

    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>exception class thrown when we encounter a redirect
    /// (302 and 307) when accessing a server
    /// </summary> 
    //////////////////////////////////////////////////////////////////////    
    public class GDataRedirectException : GDataRequestException
    {
        private string redirectLocation; 
        //////////////////////////////////////////////////////////////////////
        /// <summary>constructs a redirect execption</summary> 
        /// <param name="msg"> the exception message as a string</param>
        /// <param name="response"> the webresponse object that caused the exception</param>
        //////////////////////////////////////////////////////////////////////
        public GDataRedirectException(string msg, WebResponse response) : base(msg)
        {
            this.webResponse = response;
            if (response != null && response.Headers != null)
            {
                this.redirectLocation = response.Headers["Location"]; 
            }
            
        }


        /// <summary>
        /// returns the location header of the webresponse object
        /// which should be the location we should redirect to
        /// </summary>
        public string Location 
        {
            get 
            {
                return this.redirectLocation != null ? this.redirectLocation : "";
            }
        }

    }

} /////////////////////////////////////////////////////////////////////////////