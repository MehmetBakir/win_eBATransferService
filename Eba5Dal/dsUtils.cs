using System;
using System.IO;
using System.Web.Mail;
using System.Collections;
using System.Configuration;


namespace Eba5Dal
{
    public class dsUtils
    {

        public dsUtils()
        {
        }
        public class dsLogFile
        {
            string sLogFile;
            FileStream fsLog;
            dsMail mail = new dsMail();
            bool isSend2Mail = System.Configuration.ConfigurationManager.AppSettings["IsSendError2Mail"] == "1";
            
            public dsLogFile()
            {
                string sPath = AppDomain.CurrentDomain.BaseDirectory + @"\" + "Logs";
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }
                sPath = AppDomain.CurrentDomain.BaseDirectory;
                sLogFile = sPath + @"\Eba5WinServices.log";
            }

            public void  WriteLine(string sSubject, string sMessage)
            {
                ArchiveFile();

                fsLog = new FileStream(sLogFile, FileMode.Append, FileAccess.Write);
                StreamWriter swx = new StreamWriter(fsLog, System.Text.Encoding.GetEncoding(1254));
                try
                {
                    swx.WriteLine("<------------------------------------------------------------------------>");
                    swx.WriteLine(System.DateTime.Now + "  " + sSubject.PadRight(30, ' ') + "  >  " + sMessage);
                    swx.WriteLine("<------------------------------------------------------------------------>");
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
                finally
                {
                    swx.Flush();
                    swx.Close();
                    swx = null;
                    fsLog.Close();
                    fsLog = null;
                }

                if (isSend2Mail && sSubject.IndexOf("ERROR") > -1) 
                {
                    mail.sendMail("Error", sSubject + "  >  " + sMessage.Replace("\t", ""));
                }
                else if (isSend2Mail && sSubject.IndexOf("WARNING") > -1) 
                {
                    mail.sendMail("Warning", sSubject + "  >  " + sMessage.Replace("\t", ""));
                }
            }

            public void ArchiveFile()
            {
                FileInfo fi = new FileInfo(sLogFile);

                if (File.Exists(sLogFile) && fi.Length > 1048576)
                {
                    DateTime myDate = DateTime.Now;
                    string sArchiveFile = Path.GetDirectoryName(sLogFile) + @"\Logs\Ut_" + Path.GetFileNameWithoutExtension(sLogFile) + "_" + myDate.Year.ToString("0000") + myDate.Month.ToString("00") + myDate.Day.ToString("00") + "_" + myDate.Hour.ToString("00") + myDate.Minute.ToString("00") + myDate.Millisecond.ToString("000") + ".log";
                    File.Move(sLogFile, sArchiveFile);
                }
            }

            private void DeleteOldArchiveFiles()
            {
                DateTime myDate = DateTime.Today.AddMonths(-1);		//Bir ay önceki arşivler
                string sPath = Path.GetDirectoryName(sLogFile);
                string sArchiveFile = @"zArchive_" + myDate.Year.ToString("0000") + myDate.Month.ToString("00") + "*.log";
                DirectoryInfo dir = new DirectoryInfo(sPath);
                FileInfo[] logfiles = dir.GetFiles(sArchiveFile);
                foreach (FileInfo f in logfiles)
                {
                    int iDay = Convert.ToInt32(f.Name.Substring(f.Name.LastIndexOf("Ut_" + Path.GetFileNameWithoutExtension(sLogFile) + "_") + 14, 2));
                    if (iDay <= DateTime.Today.Day)
                    {
                        File.Delete(f.FullName);
                        //ss += "-"+ f.Name.Substring(f.Name.LastIndexOf("Archive_")+14,2);
                    }
                }
            }

        }

        #region dsTxtFile
        public class dsTxtFile
        {
            private string _fileName;

            public string FileName
            {
                get { return _fileName; }
                set { _fileName = value; }
            }

            public dsTxtFile(string fileName)
            {
                _fileName = fileName;
            }

            public string readFile()
            {
                string all = null;
                try
                {
                    if (!File.Exists(_fileName))
                    {
                        throw new Exception("File not found. (" + _fileName + ")");
                    }

                    using (StreamReader sr = new StreamReader(_fileName, System.Text.Encoding.GetEncoding(1254)))
                    {
                        all = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    dsLogFile log = new dsLogFile();
                    log.WriteLine("ERR-READFILE", ex.ToString());
                }
                return all;
            }

            public bool writeFile(string sMessage)
            {
                bool bResult = false;
                try
                {
                    FileStream fsTxt;
                    fsTxt = new FileStream(_fileName, FileMode.Create, FileAccess.Write);
                    StreamWriter swx = new StreamWriter(fsTxt, System.Text.Encoding.GetEncoding(1254));
                    swx.WriteLine(sMessage);
                    swx.Close();
                    bResult = true;
                }
                catch (Exception ex)
                {
                    dsLogFile log = new dsLogFile();
                    log.WriteLine("ERR-WRITEFILE", ex.ToString());
                }
                return bResult;
            }
        }
        #endregion

        #region dsMail
        public class dsMail
        {
            string _SmtpMail = ConfigurationSettings.AppSettings["SMTPServer"];
            string _toList = ConfigurationSettings.AppSettings["SendToErrorMailList"];
            string _toListExt = ConfigurationSettings.AppSettings["SendToErrorMailListExternal"];

            public dsMail()
            {
                SmtpMail.SmtpServer = _SmtpMail;
                if (_toList != null && _toList != "") _toList = _toList.Replace(",", "@ford.com.tr;") + "@ford.com.tr"; else _toList = "";
                if (_toListExt != null && _toListExt != "")
                {
                    _toListExt = _toListExt.Replace(",", ";");
                    if (_toList != "") _toList += ";";
                    _toList += _toListExt;
                }
            }

            #region getTemplateFile
            private string getTemplateFile(string fileName)
            {
                string sTemplate = "";
                string sPath = AppDomain.CurrentDomain.BaseDirectory + "mailTemplates";
                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                string sFile = sPath + @"\" + fileName + ".mst";
                if (!File.Exists(sFile))
                {
                    sTemplate = ""
                        + "\r\n<Message>"
                        + "\r\n\r\nApplication   : <Application>"
                        + "\r\nServerName    : <ServerName>"
                        + "\r\nLog Directory : <LogPath>"
                        + "\r\n\r\nLog dosyasını kontrol ediniz.";
                    dsTxtFile dst = new dsTxtFile(sFile);
                    dst.writeFile(sTemplate);
                    dst = null;
                }
                else
                {
                    dsTxtFile dst = new dsTxtFile(sFile);
                    sTemplate = dst.readFile();
                    dst = null;
                }
                if (sTemplate == null) sTemplate = "";
                return sTemplate;
            }
            #endregion

            public void sendMail(string sSubject, string sMessage)
            {
                string template;
                if (sSubject.IndexOf("Error") > -1)
                {
                    template = getTemplateFile("msgTempError");
                }
                else if (sSubject.IndexOf("Warning") > -1)
                {
                    template = getTemplateFile("msgTempWarning");
                }
                else if (sSubject.IndexOf("Status Info") > -1)
                {
                    template = getTemplateFile("msgTempStatusInfo");
                }
                else
                {
                    template = getTemplateFile("msgTempDefault");
                }

                if (_toList != null && _toList != "")
                {
                    template = template.Replace("<Message>", sMessage);
                    template = template.Replace("<Application>", "Eba5WinServices (Windows Service App)");
                    template = template.Replace("<ServerName>", Environment.MachineName);
                    template = template.Replace("<LogPath>", AppDomain.CurrentDomain.BaseDirectory + "logs");
                    foreach (string sKey in ConfigurationSettings.AppSettings.AllKeys)
                    {
                        template = template.Replace("<" + sKey + ">", ConfigurationSettings.AppSettings[sKey]);
                    }
                    MailMessage mail = new MailMessage();
                    mail.To = _toList;
                    mail.From = "Eba5WinServices@ford.com.tr";
                    mail.Subject = "Eba5WinServices " + sSubject + " - (EBA5 Data Transfer WinService)";
                    mail.Body = template;
                    //SmtpMail.SmtpServer = "websmtp1.ford.com.tr"; 
                    try
                    {
                        SmtpMail.Send(mail);
                    }
                    catch
                    {
                        //
                    }
                }
            }
        }
        #endregion

    }
}
    


