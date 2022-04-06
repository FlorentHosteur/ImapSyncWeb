using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Tamir.SharpSsh.jsch;

namespace IMAPSyncWeb.Code
{
    public static class SSHHelpers
    {

        public static void ConnectSSH()
        {
            try
            {
                //Create a new JSch instance
                JSch jsch = new JSch();

                //Prompt for username and server host

                String host = "root@217.16.0.199";

                String user = host.Substring(0, host.IndexOf('@'));
                host = host.Substring(host.IndexOf('@') + 1);

                //Create a new SSH session
                Session session = jsch.getSession(user, host, 22);

                // username and password will be given via UserInfo interface.
                UserInfo ui = new MyUserInfo();
                session.setUserInfo(ui);

                //Connect to remote SSH server
                session.connect();

                //Open a new Shell channel on the SSH session
                Channel channel = session.openChannel("shell");

                //Redirect standard I/O to the SSH channel
                MemoryStream streamcommand = new MemoryStream();
                StreamWriter cmd = new StreamWriter(streamcommand);
                cmd.AutoFlush = true;
                
                channel.setInputStream(streamcommand);

                
                var fileStream = File.Create(HttpContext.Current.Server.MapPath(@"~\Jobs\") + "test.txt");
               
                channel.setOutputStream(fileStream);

                


                //Connect the channel
                channel.connect();


                //Wait till channel is closed
                while (!channel.isClosed())
                {
                    cmd.Write("ls" + Environment.NewLine);
                    
                    System.Threading.Thread.Sleep(500);
                    cmd.Write("exit" + Environment.NewLine);
                    
                }

                //Disconnect from remote server
                channel.disconnect();
                session.disconnect();

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }

        }

    }

    public class MyUserInfo : UserInfo
    {
        /// <summary>
        /// Holds the user password
        /// </summary>
        private String passwd = "Gth31su8";

        /// <summary>
        /// Returns the user password
        /// </summary>
        public String getPassword() { return passwd; }

        /// <summary>
        /// Prompt the user for a Yes/No input
        /// </summary>
        public bool promptYesNo(String str)
        {
            return true;
        }

        /// <summary>
        /// Returns the user passphrase (passwd for the private key file)
        /// </summary>
        public String getPassphrase() { return null; }

        /// <summary>
        /// Prompt the user for a passphrase (passwd for the private key file)
        /// </summary>
        public bool promptPassphrase(String message) { return true; }

        /// <summary>
        /// Prompt the user for a password
        /// </summary>\
        public bool promptPassword(String message)
        {
            return true;
        }

        /// <summary>
        /// Shows a message to the user
        /// </summary>
        public void showMessage(String message)
        {
            
        }
    }
}