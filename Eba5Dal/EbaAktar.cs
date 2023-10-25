using eBAPI.Connection;
using eBAPI.Workflow;
using eBASystemAPI;
using System;
using System.Collections.Generic;
using System.Data;

namespace Eba5Dal
{
    public enum enmEbaConnType { eachTicket = 0, blockTicket = 1 }; 

    public class EbaAktar
    {
        private bool isDebug = false;
        dsUtils.dsLogFile log = new dsUtils.dsLogFile();

        public EbaAktar()
        {
            isDebug = (System.Configuration.ConfigurationManager.AppSettings["debugMode"] != null && System.Configuration.ConfigurationManager.AppSettings["debugMode"] == "true");
        }

        /// <summary>
        /// IATS süreçleri update.
        /// -Terkin, Tiade, NETOF,NETID, THSID süreçlerinde 20(onaylandı) ve 21(reddeildi) statularında
        /// bulunan süreçler çekilip, IATS sistemine eBA belge No update ediliyor.
        /// </summary>
        /// <param name="ebaType"></param>

        public void receiveFromEbaAll(string ebaType)
        {
            CommonDAL cdal = new CommonDAL();
            DataTable dt = cdal.GET_IATS_APPROVE_FROM_EBA(ebaType);
            log.WriteLine("DEBUG", "[" + ebaType + "]receiveFromEbaAll row count: " + dt.Rows.Count);
            foreach (DataRow dr in dt.Rows)
            {
                cdal.Update_IATS_Processes_Approve_FROM_EBA(ebaType, dr["BELGENO"].ToString());
                log.WriteLine("DEBUG", "[" + ebaType + "]receiveFromEbaAll BELGENO: " + dr["BELGENO"].ToString());
            }
        }

        #region Nacsoft
        public void Trig_CLAHOF(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            //dal.CLAHOF_P_InserteBA3();
            dal = new CommonDAL();
            DataTable dt = dal.Get_CLAHOF_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_CLAHOF row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    string userr = "";
                    try
                    {
                        con = new eBAConnection();
                        con.Server = StaticVar.ServerName;
                        con.UserID = StaticVar.UserName;
                        con.Password = StaticVar.PassWord;
                        con.CommandTimeout = 500;
                        con.Open();

                        log.WriteLine("DEBUG", "Impersonate User : " + dr["CD_CREATOR"].ToString());
                        userr = dr["CD_CREATOR"].ToString();
                        con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);

                        WorkflowManager mgr = con.WorkflowManager;
                        WorkflowProcess proccess = mgr.CreateProcess("CLAHOF");

                        proccess.Parameters.Add("TG_HOF_NO", dr["HOF_NO"].ToString());
                        proccess.Parameters.Update();
                        proccess.Start();

                        int pid = proccess.ProcessId;
                        PrID = pid.ToString();
                        log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                        // dal.UPDATE_CLAHOF(dr["ID"].ToString());
                        dal.CLAHOF_P_UPDATE(pid.ToString(), "", dr["HOF_NO"].ToString(), "EBA5SERVIS");

                        log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                    }
                    catch (Exception ex)
                    {
                        string sErr = "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["HOF_NO"].ToString() + "  |  hata: " + ex.ToString();

                        dal.CLAHOF_P_UPDATE("", "ERROR" + sErr, dr["HOF_NO"].ToString(), "EBA5SERVIS");
                        log.WriteLine("ERROR", sErr);
                        dal.InsertServiceErrorLog(dr["HOF_NO"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());
                    }
                    finally
                    {
                        con.Close();

                    }
                }


            }
        }

        public void Trig_CLAHFCNC(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            //dal.CLAHFCNC_P_InserteBA3();
            dal = new CommonDAL();
            DataTable dt = dal.Get_CLAHFCNC_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_CLAHFCNC row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    string userr = "";
                    try
                    {
                        con = new eBAConnection();
                        con.Server = StaticVar.ServerName;
                        con.UserID = StaticVar.UserName;
                        con.Password = StaticVar.PassWord;
                        con.CommandTimeout = 500;
                        con.Open();

                        log.WriteLine("DEBUG", "Impersonate User : " + dr["CD_CREATOR"].ToString());
                        userr = dr["CD_CREATOR"].ToString();
                        con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);

                        WorkflowManager mgr = con.WorkflowManager;
                        WorkflowProcess proccess = mgr.CreateProcess("CLAHFCNC");

                        proccess.Parameters.Add("TG_KLEYM_NO", dr["CLAIM_NO"].ToString());
                        proccess.Parameters.Update();
                        proccess.Start();

                        int pid = proccess.ProcessId;
                        PrID = pid.ToString();
                        log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                        // dal.UPDATE_CLAHFCNC(dr["ID"].ToString());
                        dal.CLAHFCNC_P_UPDATE(pid.ToString(), "", dr["CLAIM_NO"].ToString(), "EBA5SERVIS");

                        log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                    }
                    catch (Exception ex)
                    {
                        string sErr = "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["CLAIM_NO"].ToString() + "  |  hata: " + ex.ToString();

                        dal.CLAHFCNC_P_UPDATE("", "ERROR" + sErr, dr["CLAIM_NO"].ToString(), "EBA5SERVIS");
                        log.WriteLine("ERROR", sErr);
                        dal.InsertServiceErrorLog(dr["CLAIM_NO"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

        public void Trig_SYMSF(string ticketType)
        {

            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SYMSF_TrigRecords();

            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_SYMSF row count: " + dt.Rows.Count);

            if (dt.Rows.Count > 0)
            {

                eBAConnection con = new eBAConnection();

                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["MASRAFNO"].ToString(), "SYHMSF") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", "Impersonate User : " + dr["RUSER"].ToString());
                            con.Impersonate(dr["RUSER"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("SYHMSF");

                            proccess.Parameters.Add("TG_MASRAFNO", dr["MASRAFNO"].ToString());
                            proccess.Parameters.Add("TG_SYHTIP", dr["TIP"].ToString());
                            proccess.Parameters.Add("TG_AVANSALM", dr["AVANSALM"].ToString());
                            proccess.Parameters.Add("TG_PLANGERC", dr["PLANGERC"].ToString());

                            proccess.Parameters.Add("TG_SYGUNSAY", dr["SYGUNSAY"].ToString());
                            proccess.Parameters.Add("TG_LIMITASI", dr["LIMITASI"].ToString());
                            proccess.Parameters.Add("TG_MASRAFTP", dr["MASRAFTP"].ToString());
                            proccess.Parameters.Add("TG_GID", dr["ID"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                        }
                    }
                    catch (Exception ex)
                    {
                        dal.UPDATE_SYHMSF_INIT(dr["ID"].ToString());
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> MASRAF NO: " + dr["MASRAFNO"].ToString() + "GID: " + dr["ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["MASRAFNO"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["RUSER"].ToString());
                    }
                    finally { con.Close(); }
                }
            }
        }

        public void Trig_SYHAVA(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SYHAVA_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_SYHAVA row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {

                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_ADVAN"].ToString(), "SYHAVA") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            log.WriteLine("DEBUG", "Impersonate User : " + dr["RUSER"].ToString());
                            con.Impersonate(dr["RUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("SYHAVA");

                            proccess.Parameters.Add("TG_P_SQ_ADVANCE", dr["SQ_ADVAN"].ToString());
                            proccess.Parameters.Add("TG_TIP", dr["TIP"].ToString());
                            proccess.Parameters.Add("TG_FormAcanSicil", dr["RUSER"].ToString());
                            proccess.Parameters.Add("TG_UZUNON", dr["UZUNON"].ToString());
                            proccess.Parameters.Add("TG_GID", dr["ID"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                        }
                    }
                    catch (Exception ex)
                    {
                        dal.UPDATE_SYHAVA3_INIT(dr["ID"].ToString());
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> SQ_ADVAN: " + dr["SQ_ADVAN"].ToString() + "GID: " + dr["ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_ADVAN"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["RUSER"].ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

        public void Trig_TERKIN(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_TERKIN_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_TERKIN row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["SISTEMNO"].ToString(), "TERKIN") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("TERKIN");

                            proccess.Parameters.Add("TG_SISTEMNO", dr["SISTEMNO"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "] süreç başladı. PID: " + PrID);
                            dal.IATS_SEND_EBA_FOR_APPROVAL("TE", PrID, dr["SISTEMNO"].ToString());
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] trig -> foreach -> sistem no: " + dr["SISTEMNO"].ToString() + "  |  ticket: " + "\n -> " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SISTEMNO"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());
                    }
                    finally { con.Close(); }
                }
            }
            log.WriteLine("DEBUG", "[" + ticketType + "] süreçler tetiklendi. ");
        }

        public void Trig_TIADE(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_TIADE_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_TIADE row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();

                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["SISTEMNO"].ToString(), "TIADE") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("TIADE");

                            proccess.Parameters.Add("TG_SISTEMNO", dr["SISTEMNO"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.IATS_SEND_EBA_FOR_APPROVAL("TIA", PrID, dr["SISTEMNO"].ToString());
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["SISTEMNO"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SISTEMNO"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());
                    }
                    finally { con.Close(); }

                }


            }
        }

        public void Trig_THSID(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_THSID_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_THSID row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["SISTEMNO"].ToString(), "THSID") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("THSID");

                            proccess.Parameters.Add("TG_SISTEMNO", dr["SISTEMNO"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.IATS_SEND_EBA_FOR_APPROVAL("THSID", PrID, dr["SISTEMNO"].ToString());
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["SISTEMNO"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["MASRAFNO"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());
                    }
                    finally { con.Close(); }

                }
            }
        }

        public void Trig_NETOF(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_NETOF_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_NETOF row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["SISTEMNO"].ToString(), "NETOF") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("NETOF");

                            proccess.Parameters.Add("TG_SISTEMNO", dr["SISTEMNO"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.IATS_SEND_EBA_FOR_APPROVAL("NETOF", PrID, dr["SISTEMNO"].ToString());
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["SISTEMNO"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SISTEMNO"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());
                    }
                    finally { con.Close(); }

                }

            }
        }

        public void Trig_NETID(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_NETID_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_NETID row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {

                        if (dal.GetServisErrorControl(dr["SISTEMNO"].ToString(), "NETID") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("NETID");

                            proccess.Parameters.Add("TG_SISTEMNO", dr["SISTEMNO"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.IATS_SEND_EBA_FOR_APPROVAL("NETID", PrID, dr["SISTEMNO"].ToString());
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["SISTEMNO"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SISTEMNO"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());
                    }
                    finally { con.Close(); }
                }


            }
        }

        public void Trig_MUFTON(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            dal.MUFTON_P_InserteBA3();
            dal = new CommonDAL();
            DataTable dt = dal.Get_MUFTON_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_MUFTON row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    string userr = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID"].ToString(), "MUFTON") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", "Impersonate User : " + dr["RUSER"].ToString());
                            userr = dr["RUSER"].ToString();
                            con.Impersonate(dr["RUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("MUFTON");
                            
                            proccess.Parameters.Add("TG_GID", dr["ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.MUFTON_P_MUHMHB_SEND_EBA_FOR_APPROVAL(pid, dr["SQ_INV"].ToString());

                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        dal.MUFTON_P_MUHMHB_SEND_EBA_FOR_ROLLBACK(Convert.ToInt32(PrID == "" ? dr["ID"].ToString() : PrID));
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["RUSER"].ToString());
                    }
                    finally
                    {
                        con.Close();

                    }
                }


            }
        }



        public void Trig_MUHMAS(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_MUHMAS_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_MUHMAS row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID"].ToString(), "MUHMAS") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", "Impersonate User : " + dr["RUSER"].ToString());
                            con.Impersonate(dr["RUSER"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("MUHMAS");

                            proccess.Parameters.Add("GID", dr["ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.UPDATE_MUHMAS(dr["ID"].ToString());
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                        }

                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["RUSER"].ToString());
                    }
                    finally { con.Close(); }
                }


            }
        }

        public void Trig_IMLAVA(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_IMLAVA_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_IMLAVA row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();

                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID"].ToString(), "IMLAVA") == true)
                        {
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", "Impersonate User : " + dr["RUSER"].ToString());
                            con.Impersonate(dr["RUSER"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("IMLAVA");

                            proccess.Parameters.Add("TG_GID", dr["ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();

                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.UPDATE_IMLAVA3(dr["ID"].ToString());
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["RUSER"].ToString());
                    }
                    finally { con.Close(); }
                }


            }
        }

        #endregion

        #region Eba3 To Eba5 Geçiş 2015

        #region FATS (SBKYHF)

        public void Trig_FATS(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.getFatsTrigRecords();
            log.WriteLine("DEBUG", string.Format("[{0}]Trig_FATS eba3 to eba5 row count: {1}", ticketType, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = string.Empty;
                    try
                    {
                        if (dal.GetServisErrorControl(dr["FATSID"].ToString(), ticketType) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", string.Format("Impersonate User : {0}", dr["RUSER"].ToString()));
                            con.Impersonate(dr["RUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("SBKYHF");

                            proccess.Parameters.Add("TG_FATSID", dr["FATSID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();
                            log.WriteLine("DEBUG", string.Format("[{0}]süreç FATS başladı pid: {1}", ticketType, PrID));
                            dal.fatsUpdate(dr["FATSID"].ToString());
                            log.WriteLine("DEBUG", string.Format("[{0}]süreç FATS statu update basarılı. Fats id : {1}", ticketType, dr["FATSID"].ToString()));
                            #endregion
                        }
                    }
                    catch (Exception Ex)
                    {
                        log.WriteLine("ERROR", string.Format("[{0}] Trig -> foreach -> FATS ID : {1} | Hata : {2}", ticketType, dr["FATSID"].ToString(), Ex.ToString()));
                        dal.InsertServiceErrorLog(dr["FATSID"].ToString(), PrID, Ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["RUSER"].ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

        }

        public void receiveFatsToEba3()
        {
            log.WriteLine("DEBUG", "[ALL] receiveFatsToEba3() begin.");
            CommonDAL dal = new CommonDAL();
            dal.receiveFatsUpdate();
            log.WriteLine("DEBUG", "[ALL] receiveFatsToEba3() end.");
        }

        #endregion

        #region ACR (AC)

        public void Trig_ACR(string ticketType)
        {

            CommonDAL dal = new CommonDAL();
            DataTable dtAcr = dal.SelectNewTicketsForEba(ticketType);
            log.WriteLine("DEBUG", string.Format("[{0}]Trig_ACR HELPDESK to EBA5 row count: {1}", ticketType, dtAcr.Rows.Count));

            if (dtAcr.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dtAcr.Rows)
                {

                    string idEba, PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["TICKET_ID"].ToString(), ticketType) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", "Impersonate User : " + dr["USER_ID"].ToString());
                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("ACR");

                            proccess.Parameters.Add("V_PUST", dr["USER_ID"].ToString());
                            proccess.Parameters.Update();

                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            if (isDebug) log.WriteLine("DEBUG", "[" + dr["TICKET_TYPE"].ToString() + "] Trig_ACR -> foreach(DataRow dr in dtAcr.Rows)");
                            idEba = dal.InsertNewEbaForm(dr["TICKET_ID"].ToString(), pid);
                            if (isDebug) log.WriteLine("DEBUG", "[" + dr["TICKET_TYPE"].ToString() + "] Trig_ACR -> InsertNewEbaForm -> EbaId: " + idEba + ")"); log.WriteLine("INFO", "[" + dr["TICKET_TYPE"].ToString() + "] > SEND to EBA -> user: " + dr["USER_ID"].ToString().PadRight(10) + "  |  ticket: " + dr["TICKET_ID"].ToString().PadRight(10) + "  |  eba: " + idEba);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] receiveAcrFromEbaAll -> foreach(DataRow dr in dtEba.Rows) -> user: " + dr["USER_ID"].ToString() + "  |  ticket: " + dr["TICKET_ID"] + "\n -> " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["TICKET_TYPE"].ToString(), dr["TICKET_ID"].ToString(), ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());
                    }

                }

                con.Close();
            }

        }

        #region lburgu : dsRestartFlow fonksiyonu tarafından tetikleniyor. Akış başlamayan formları tekrar tetiklemek amaçlı. Teknik akış testi yapılmadı.
        public void akisiBaslamayanlariTekrarBaslat(string ticketType)
        {
            if (isDebug) log.WriteLine("DEBUG", "[" + ticketType + "] akisiBaslamayanlariTekrarBaslat('" + ticketType + "') begin.");
            CommonDAL dal = new CommonDAL();
            DataTable dtEba = dal.GetFlowNotStartList(ticketType);
            if (isDebug) log.WriteLine("DEBUG", "[" + ticketType + "] dtEba.Rows.Count: " + dtEba.Rows.Count.ToString());
            if (dtEba.Rows.Count > 0)
            {

                eBAConnection con = new eBAConnection();
                con.Server = StaticVar.ServerName;
                con.UserID = StaticVar.UserName;
                con.Password = StaticVar.PassWord;
                con.Open();
                foreach (DataRow dr in dtEba.Rows)
                {

                    try
                    {

                        if (isDebug) log.WriteLine("DEBUG", "[" + ticketType + "]  startFlow(" + dr["ID"].ToString() + ", " + dr["RUSER"].ToString() + ")");
                        // startFlow(ticketType, dr["ID"].ToString(), dr["RUSER"].ToString());

                        con.Impersonate(StaticVar.UserName);
                        WorkflowManager mgr = con.WorkflowManager;
                        WorkflowProcess proccess = mgr.CreateProcess("ACR");
                        proccess.Start();
                        int pid = proccess.ProcessId;
                        //idEba = dal.InsertNewEbaForm(dr["TICKET_ID"].ToString(), pid);                   
                        log.WriteLine("INFO", "[" + ticketType + "] > Restart Flow   -> user: " + dr["RUSER"].ToString() + "  |  eba: " + dr["ID"].ToString());
                    }
                    catch (Exception ex)
                    {
                        //throw Ex;
                        log.WriteLine("ERROR", "[" + ticketType + "] akisiBaslamayanlariTekrarBaslat('" + ticketType + "') -> " + ex.ToString());
                        // if (ex.ToString().IndexOf(" -> startFlow ") > -1 // && isEbaRecoveryErrorExist(ex.ToString()))
                        {
                            try
                            {
                                dal.RecoveryTicketsFromEba(ticketType, dr["ID"].ToString());
                                log.WriteLine("RECOVERY", "[" + ticketType + "] > Restart Flow   -> RecoveryTicketsFromEba(" + ticketType + "," + dr["ID"].ToString() + ") -> user: " + dr["RUSER"].ToString());
                            }
                            catch (Exception ex2)
                            {
                                log.WriteLine("ERROR", "[" + ticketType + "] > Restart Flow   -> RecoveryTicketsFromEba(" + ticketType + "," + dr["ID"].ToString() + ") -> user: " + dr["RUSER"].ToString() + "\n -> " + ex2.ToString());
                            }
                        }
                    }

                }

            }
            if (isDebug) log.WriteLine("DEBUG", "[" + ticketType + "] akisiBaslamayanlariTekrarBaslat('" + ticketType + "') end.");
        }

        public void altIsleriKapanmisAcikKalanIsleriKapat()
        {
            if (isDebug) log.WriteLine("DEBUG", "[ALL] altIsleriKapanmisAcikKalanIsleriKapat() begin.");
            CommonDAL dal = new CommonDAL();
            dal.CloseDefectiveTickets();
            if (isDebug) log.WriteLine("DEBUG", "[ALL] altIsleriKapanmisAcikKalanIsleriKapat() end.");
        }

        public void altIsiFarkliKisiyeAtanmisIslerinAtananKisisiniGuncelle()
        {
            if (isDebug) log.WriteLine("DEBUG", "[ALL] altIsiFarkliKisiyeAtanmisIslerinAtananKisisiniGuncelle() begin.");
            CommonDAL dal = new CommonDAL();
            dal.UpdateDifferentAssignedTo();
            if (isDebug) log.WriteLine("DEBUG", "[ALL] altIsiFarkliKisiyeAtanmisIslerinAtananKisisiniGuncelle() end.");
        }
        #endregion

        #endregion

        #region SMR (CR)
        public void Trig_SMR(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SMR_TrigRecords(ticketType);
            log.WriteLine("DEBUG", string.Format("[{0}]Trig_SMR HELPDESK to EBA5 row count: {1}", ticketType, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["TICKET_IDF"].ToString(), ticketType) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", "Impersonate User : " + dr["USER_ID"].ToString());
                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("SMR");

                            proccess.Parameters.Add("TG_TICKETID", dr["TICKET_IDF"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.UPDATE_SMR3(dr["TICKET_IDF"].ToString(), ticketType, PrID);
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["TICKET_IDF"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["TICKET_IDF"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());
                    }
                    finally { con.Close(); }
                }

            }

        }
        #endregion

        #region UYDGDA
        public void Trig_UYDGDA(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_UYDGDA_TrigRecords();
            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_UYDGDA row count: " + dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID"].ToString(), ticketType) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", "Impersonate User : " + dr["RUSER"].ToString());
                            con.Impersonate(dr["RUSER"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("UYDGDA");

                            proccess.Parameters.Add("TG_GID", dr["ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.UPDATE_UYDGDA3(dr["ID"].ToString());
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");
                            #endregion
                        }

                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["RUSER"].ToString());
                    }
                    finally { con.Close(); }
                }


            }
        }
        #endregion

        #region TALEPSOS

        public void Trig_TALEPSOS(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SMR_TrigRecords(ticketType);
            log.WriteLine("DEBUG", string.Format("[{0}]Trig_TALEPSOS HELPDESK to EBA5 row count: {1}", ticketType, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["TICKET_IDF"].ToString(), ticketType) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            log.WriteLine("DEBUG", "Impersonate User : " + dr["USER_ID"].ToString());
                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("TALEPSOS");

                            proccess.Parameters.Add("TG_TICKETID", dr["TICKET_IDF"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();


                            WorkflowProcess test = mgr.GetProcess(12121);


                            int pid = proccess.ProcessId;
                            PrID = pid.ToString();
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı pid: " + PrID);
                            dal.UPDATE_SMR3(dr["TICKET_IDF"].ToString(), ticketType, PrID);
                            log.WriteLine("DEBUG", "[" + ticketType + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["TICKET_IDF"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["TICKET_IDF"].ToString(), PrID, ex.ToString(), ticketType, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        #endregion

        public void receiveTicketFromEbaAll(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dtEba = dal.SelectTicketsApproveFromEba(ticketType);
            if (dtEba.Rows.Count > 0)
            {
                if (isDebug) log.WriteLine("DEBUG", "receiveAcrFromEbaAll -> [" + ticketType + "] dtEba.Rows.Count>0");
                foreach (DataRow dr in dtEba.Rows)
                {
                    try
                    {
                        string idEba = dr["ID"].ToString();
                        if (isDebug) log.WriteLine("DEBUG", "[" + ticketType + "] receiveAcrFromEbaAll -> UpdateAcrApproveFromEba(EbaId: " + idEba + ")");
                        int iCount = dal.UpdateTicketApproveFromEba(ticketType, idEba);
                        string sStatus = (dr["SW_APPROVED"].ToString() == "1") ? "APPROVED" : "REJECTED ";
                        log.WriteLine("INFO", "[" + ticketType + "] > " + sStatus + " from EBA    -> user: " + dr["EBAUSR"].ToString().PadRight(10) + "  |  ticket: " + dr["TICKETID"].ToString().PadRight(10) + "  |  eba: " + idEba);
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] receiveAcrFromEbaAll -> foreach(DataRow dr in dtEba.Rows) -> user: " + dr["EBAUSR"].ToString() + "  |  ticket: " + dr["TICKETID"] + "\n -> " + ex.ToString());
                    }
                }
            }
            dal = null;
        }

        #endregion

        #region Excetion Yönetimi

        private void GetDeleteProcess(string ProcessID, string UserID)
        {
            try
            {
                if (!string.IsNullOrEmpty(ProcessID) && !string.IsNullOrEmpty(UserID))
                {
                    CommonDAL dal = new CommonDAL();
                    DataTable dt = dal.GetFlowDocuments(ProcessID);

                    if (dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0]["PROCESSID"].ToString()) && !string.IsNullOrEmpty(dt.Rows[0]["FILEPROFILEID"].ToString()))
                        {
                            #region ProcessDelete
                            eBAConnection con = new eBAConnection();
                            try
                            {
                                int dtProcessID = Convert.ToInt32(dt.Rows[0]["PROCESSID"].ToString());
                                int dtDocumentID = Convert.ToInt32(dt.Rows[0]["FILEPROFILEID"].ToString());


                                con.Server = StaticVar.ServerName;
                                con.UserID = StaticVar.UserName;
                                con.Password = StaticVar.PassWord;
                                con.CommandTimeout = 500;
                                con.Open();

                                con.Impersonate(UserID, ImpersonationStatusType.Hidden);
                                WorkflowManager mgr = con.WorkflowManager;

                                mgr.DeleteProcess(dtProcessID);
                                mgr.DeleteDocument(dtDocumentID);

                            }
                            catch (Exception Ex)
                            {
                                log.WriteLine("Error : Document Delete Process", string.Format("{0} UserID başlattığı {1} ProcessID bilgisine sahip kayıt iptal edilemedi. Hata Detayları : {3}", UserID, ProcessID, Ex.Message.ToString()));
                            }
                            finally { con.Close(); }
                            #endregion
                        }

                    }

                }
            }
            catch (Exception Ex)
            {
                log.WriteLine("Error : Document Delete Process Read", string.Format("{0} UserID başlattığı {1} ProcessID bilgisine sahip kayıt iptal edilemedi. Hata Detayları : {3}", UserID, ProcessID, Ex.Message.ToString()));
            }
        }

        public void Trig_InvalidStep()
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.GetInvalidStep();

            if (dt.Rows.Count > 0)
            {
                try
                {
                    log.WriteLine("INVALID", string.Format("Invalid step row count: {0}", dt.Rows.Count));

                    eBAConnection con = new eBAConnection();
                    con = new eBAConnection();
                    con.Server = StaticVar.ServerName;
                    con.UserID = StaticVar.UserName;
                    con.Password = StaticVar.PassWord;
                    con.CommandTimeout = 500;
                    con.Open();

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["STEP"].ToString()) > 0)
                        {
                            try
                            {
                                Utils.RollbackFlow(con, Convert.ToInt32(dr["ID"].ToString()), Convert.ToInt32(dr["STEP"].ToString()));
                                log.WriteLine("DEBUG", string.Format("{0} Akış bir adım geri alındı. (PROCESSID: {1} , FLOWSTEP: {2})", DateTime.Now.ToString(), dr["ID"].ToString().ToString(), dr["STEP"].ToString().ToString()));
                            }
                            catch (Exception Ex)
                            {
                                log.WriteLine("DEBUG", string.Format("{0} Akış geri alınamadı. (PROCESSID: {1} , FLOWSTEP: {2})", DateTime.Now.ToString(), dr["ID"].ToString().ToString(), dr["STEP"].ToString().ToString()));
                                dal.InsertServiceErrorLog(dr["ID"].ToString(), dr["STEP"].ToString(), Ex.ToString(), "INVALID", "0");
                            }
                        }
                        /*Akış başlatanda invalid durumu olduğunda geçerli olur.
                        else
                        {
                           GetDeleteProcess(dr["ID"].ToString(), StaticVar.UserName);
                        }*/
                    }


                }
                catch (Exception Ex)
                {

                    log.WriteLine("INVALID", string.Format("Invalid step, Hata : {0}", Ex.Message.ToString()));
                }
            }
        }

        #endregion

        #region SAP


        #region EXCAR
        public void Trig_EXCARBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPFRM_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}] Trig_EXCARBaslat EXCAR to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_INV"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(StaticVar.DefaultUser, ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_SQ_INV", dr["SQ_INV"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Get_EXCAR_UPDATE(dr["SQ_INV"].ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> EXCAR  Form No: " + dr["SQ_INV"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_INV"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, StaticVar.DefaultUser);

                    }
                    finally { con.Close(); }

                }
            }
        }
        #endregion


        public void Trig_SAP_ODRM(string ticketType)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SAPODFRM_TrigRecords();

            log.WriteLine("DEBUG", "[" + ticketType + "]Trig_SAP_ODRM row count: " + dt.Rows.Count);

            if (dt.Rows.Count > 0)
            {

                SapServiceReference.ZWSHPA_EBA_ENTGRSYNClient client = new SapServiceReference.ZWSHPA_EBA_ENTGRSYNClient("eba_soap12");
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        #region Detail Send
                        DataTable personel = dal.Get_SAPODFRM_PERSONEL_TrigRecords(dr["ID"].ToString());

                        if (personel.Rows.Count > 0)
                        {
                            var arguments = new SapServiceReference.Z_HPA_EBA_ENTEGRASYON();
                            List<SapServiceReference.ZSTHPA_EBA_ENTG> entgList = new List<SapServiceReference.ZSTHPA_EBA_ENTG>();

                            foreach (DataRow drp in personel.Rows)
                            {
                                var arg = new SapServiceReference.ZSTHPA_EBA_ENTG()
                                {
                                    ONERI_ID = int.Parse(dr["ID"].ToString()),
                                    ONERI_AD = dr["OneriBaslik"].ToString(),
                                    ONERI_TEXT = dr["OneriAciklama"].ToString(),
                                    PARASAL_KAZANC = decimal.Parse(dr["ParasalKazanc"].ToString()),
                                    GIRIS_TARIH = dr["OneriTarihi"].ToString(),
                                    KABUL_TARIH = dr["OneriKabulTarihi"].ToString(),
                                    PERNR = drp["PERNR"].ToString(),
                                    EK_ALAN1 = dr["BelgeNo"].ToString()
                                };

                                entgList.Add(arg);

                            }

                            arguments.IT_TABLE = entgList.ToArray();
                            var result = client.Z_HPA_EBA_ENTEGRASYON(arguments);

                            if (result.EV_TRNSFR_OK == "X")
                                //SAP gönderim başarılı.
                                dal.Get_SAPODFRM_SAP_Send(dr["ID"].ToString(), "B");
                            else
                                //SAP gönderim başarısız.
                                dal.Get_SAPODFRM_SAP_Send(dr["ID"].ToString(), "H");

                        }
                        else
                            //Personel bilgisi bulunamadı
                            dal.Get_SAPODFRM_SAP_Send(dr["ID"].ToString(), "H");

                        #endregion        

                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + ticketType + "] Trig -> foreach -> sistem no: " + dr["ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID"].ToString(), dr["ID"].ToString(), ex.ToString(), ticketType, "0");
                        //SAP gönderim başarısız.
                        dal.Get_SAPODFRM_SAP_Send(dr["ID"].ToString(), "H");
                    }

                }
                client.Close();

            }



        }

        #endregion

        #region Supplier Portal Formları

        public void Trig_SPFRMBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPFRM_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Trig_SPFRMBaslat SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["CD_GSDB"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(StaticVar.DefaultUser, ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess("SPFRM");

                            proccess.Parameters.Add("V_CD_GSDB", dr["CD_GSDB"].ToString());
                            proccess.Parameters.Add("V_CD_TYPE", dr["CD_TYPE"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Get_SPFRM_UPDATE(dr["CD_GSDB"].ToString(), dr["CD_TYPE"].ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> Supplier Form No: " + dr["CD_GSDB"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["CD_GSDB"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, StaticVar.DefaultUser);

                    }
                    finally { con.Close(); }

                }
            }
        }

        #endregion

        #region FMSCBaslat
        public void Trig_FMSCBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_FMSC_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Trig_FMSCBaslat SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["CD_TTHID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);
                            string typex = "";
                            string idx = "";

                            if (dr["CD_TYPE"].ToString() == "AHYB") { typex = "1"; idx = dr["CD_METRICID"].ToString(); }
                            if (dr["CD_TYPE"].ToString() == "AHYO") { typex = "2"; idx = dr["CD_TTHID"].ToString(); }
                            if (dr["CD_TYPE"].ToString() == "BHYO") { typex = "3"; idx = dr["CD_TTHID"].ToString(); }
                            if (dr["CD_TYPE"].ToString() == "AHYR") { typex = "4"; idx = dr["CD_TTHID"].ToString(); }
                            if (dr["CD_TYPE"].ToString() == "BHYR") { typex = "5"; idx = dr["CD_TTHID"].ToString(); }
                            if (dr["CD_TYPE"].ToString() == "TRAC") { typex = "6"; idx = dr["CD_TTDID"].ToString(); }
                            if (dr["CD_TYPE"].ToString() == "AHDO") { typex = "7"; idx = dr["CD_TTHID"].ToString(); }

                            proccess.Parameters.Add("V_FORMTYPE", typex);
                            proccess.Parameters.Add("V_METRICID", dr["CD_METRICID"].ToString());
                            proccess.Parameters.Add("V_TTHID", dr["CD_TTHID"].ToString());
                            proccess.Parameters.Add("V_TTDID", dr["CD_TTDID"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Get_FMSC_UPDATE(dr["CD_TYPE"].ToString(), idx, PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> Supplier Form No: " + dr["CD_TTHID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["CD_TTHID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        #endregion
        #region TOSHOLDBaslat
        public void Trig_TOSHOLDBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_TOSHOLD_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Trig_TOSHOLDBaslat SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_HOLD_RULE_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CH_REQUESTED_BY"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);
                            string typex = "";
                            string p_cd_eba_status = "2";

                            proccess.Parameters.Add("V_ARACSAYISI", dr["ARAC_SAY"].ToString());
                            proccess.Parameters.Add("V_RULEID", dr["SQ_HOLD_RULE_ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Get_TOSHOLD_UPDATE(dr["SQ_HOLD_RULE_ID"].ToString(), p_cd_eba_status);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> TOSHOLD Form No: " + dr["SQ_HOLD_RULE_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_HOLD_RULE_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CH_REQUESTED_BY"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        #endregion
        #region IMSBaslat
        public void Trig_IMSBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_IMS_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Trig_IMSBaslat SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["TRX_USER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);
                            string typex = "";
                            string p_cd_eba_status = "1";

                            proccess.Parameters.Add("V_FORMTYPE", "OTOMATIK");
                            proccess.Parameters.Add("V_ID", dr["ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Get_IMS_UPDATE(dr["ID"].ToString(), PrID, p_cd_eba_status);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> IMS Form No: " + dr["ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["TRX_USER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        #endregion
        #region KPSBaslat
        public void Trig_KPSBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_KPS_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Trig_KPSBaslat SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID_KPS_HEADER"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);
                            string typex = "";
                            string p_cd_eba_status = "1";

                            proccess.Parameters.Add("V_AKISTIPI", dr["CD_FLOW_TYPE"].ToString());
                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_KPS_HDR_ID", dr["ID_KPS_HEADER"].ToString());
                            proccess.Parameters.Add("V_KPS_PAGE_NO", dr["CD_KPS_PAGE_NO"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Get_KPS_UPDATE(dr["CD_FLOW_TYPE"].ToString(), PrID, dr["ID_KPS_HEADER"].ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> KPS Form No: " + dr["ID_KPS_HEADER"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID_KPS_HEADER"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        #endregion
        #region ITHALATBaslat
        public void Trig_ITHALATBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ITHALAT_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Trig_ITHALATBaslat SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["EBA_GRUP_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_GRUP_ID", dr["EBA_GRUP_ID"].ToString());
                            proccess.Parameters.Add("V_AKISBASLATAN", dr["USER_ID"].ToString());
                            proccess.Parameters.Add("V_TEXT", dr["TEXT"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Get_ITHALAT_UPDATE(PrID, dr["EBA_GRUP_ID"].ToString(), "2");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> ITHALAT Form No: " + dr["EBA_GRUP_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["EBA_GRUP_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        #endregion
        #region ATS Sistemi Formları
        public void Trig_AtsTeminatBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATSTEMINAT_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Trig_AtsTeminatBaslat SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SEQ_TEMINAT"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_APPROVER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_APPROVER"].ToString());
                            proccess.Parameters.Add("V_SEQ_TEMINAT", dr["SEQ_TEMINAT"].ToString());
                            proccess.Parameters.Add("V_KAYIT_TIPI", dr["CD_KAYIT_TIP"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATSTEMINAT_FORM_Update(dr["SEQ_TEMINAT_HAREKET"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATSTEMINAT_STATU_Update("B", dr["SEQ_TEMINAT"].ToString(), dr["CD_APPROVER"].ToString(), dr["CD_KAYIT_TIP"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SEQ_TEMINAT"].ToString() + " SEQ_TEMINAT_HAREKET: " + dr["SEQ_TEMINAT_HAREKET"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SEQ_TEMINAT"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_APPROVER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_AtsLimitBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATSLIMIT_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATSLIMIT_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_LIMIT_HAREKET"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_APPROVER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_SQ_LIMIT_HAREKET", dr["SQ_LIMIT_HAREKET"].ToString());
                            proccess.Parameters.Add("V_KAYIT_TIPI", dr["CD_KAYIT_TIP"].ToString());
                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_APPROVER"].ToString());
                            proccess.Parameters.Add("V_TUTAR", dr["CS_YENI_YONETIM_LIMIT"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATSLIMIT_FORM_Update(dr["SQ_LIMIT_HAREKET"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATSLIMIT_STATU_Update("B", dr["SQ_LIMIT"].ToString(), dr["CD_APPROVER"].ToString(), dr["CD_KAYIT_TIP"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_LIMIT_HAREKET"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_LIMIT_HAREKET"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["cd_approver"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_AtsVadeBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATSFATURAVADE_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATSFATURAVADE_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["CD_INV_NO"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_USER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_USER"].ToString());
                            proccess.Parameters.Add("V_KAYIT_TIPI", dr["SW_DATE"].ToString());
                            proccess.Parameters.Add("V_CD_TYPE", dr["CD_TYPE"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATSFATURAVADE_FORM_Update(dr["CD_INV_NO"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATSFATURAVADE_STATU_Update(dr["CD_INV_NO"].ToString(), PrID, "B", dr["CD_USER"].ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["CD_INV_NO"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["CD_INV_NO"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_USER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_AtsHavaleBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATSHAVALE_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATSHAVALE_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["CD_REF"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_ISLEM_TIPI", dr["CD_EBA_TYPE"].ToString());
                            proccess.Parameters.Add("V_HESAP_TIPI", dr["CD_HESAP_TIPI"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATSHAVALE_FORM_Update(dr["CD_REF"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATSHAVALE_STATU_Update(dr["CD_REF"].ToString(), "B", dr["CD_CREATOR"].ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["CD_REF"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["CD_REF"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_AtsFaturaIptalBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATSFATURAIPTAL_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATSFATURAIPTAL_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["CD_INV_NO"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_USER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_USER"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATSFATURAIPTAL_FORM_Update(dr["CD_INV_NO"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATSFATURAIPTAL_STATU_Update(dr["CD_INV_NO"].ToString(), PrID, "B", dr["CD_USER"].ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["CD_INV_NO"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["CD_INV_NO"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_USER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_AtsStandartVadeBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATS_STANDARTVADE_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATS_STANDARTVADE_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID_TERM"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_PT", dr["CD_PT"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_STANDARTVADE_FORM_Update(dr["ID_TERM"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATS_STANDARTVADE_STATU_Update(dr["ID_TERM"].ToString(), "1", PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["ID_TERM"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID_TERM"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_FtruckFaturaBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_FTRUCKFATURA_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_FTRUCKFATURA_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["FORM_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["USER_ID"].ToString());
                            proccess.Parameters.Add("V_FORM_ID", dr["FORM_ID"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_FtruckFatura_FORM_Update(dr["FORM_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_FTRUCKFATURA_STATU_Update(dr["FORM_ID"].ToString(), "B");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["FORM_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["FORM_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_MusteriFaturaBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATS_MUSTERIFATURA_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATS_MUSTERIFATURA_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_MUSTERI_FATURALARI_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_USER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_SQ_MUSTERI", dr["SQ_MUSTERI_FATURALARI_EBA"].ToString());
                            proccess.Parameters.Add("V_FATURA_TIPI", dr["CD_FATURA_TIP"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_MUSTERIFATURA_FORM_Update(dr["SQ_MUSTERI_FATURALARI_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATS_MUSTERIFATURA_STATU_Update(dr["SQ_MUSTERI_FATURALARI_EBA"].ToString(), "B", PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_MUSTERI_FATURALARI_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_MUSTERI_FATURALARI_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_USER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_IATSTahsilatBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_IATSTahsilat_TrigRecords(); // başlayacak süreç listesi alınır
            log.WriteLine("DEBUG", string.Format("[{0}]Get_NCS_IATS_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_EBA_PROCESS_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);
                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("SQ_ID", dr["SQ_EBA_PROCESS_ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();
                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_IATS_Tahsilat_FORM_Update(dr["SQ_EBA_PROCESS_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_EBA_PROCESS_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_EBA_PROCESS_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }
                }
            }
        }
        public void Trig_RAWSYS2Baslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_RAWSYS2_TrigRecords(); // başlayacak süreç listesi alınır
            log.WriteLine("DEBUG", string.Format("[{0}]Get_N_RAWSYS2_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["EBA_USER"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);
                            proccess.Parameters.Add("V_AKISBASLATAN", dr["EBA_USER"].ToString());
                            proccess.Parameters.Add("SQ_ID", dr["SQ_ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();
                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_N_RAWSYS2_FORM_Update(dr["SQ_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["EBA_USER"].ToString());

                    }
                    finally { con.Close(); }
                }
            }
        }
        public void Trig_RAWSYSBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_RAWSYS_TrigRecords(); // başlayacak süreç listesi alınır
            log.WriteLine("DEBUG", string.Format("[{0}]Get_N_RAWSYS_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["EBA_USER"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);
                            proccess.Parameters.Add("V_AKISBASLATAN", dr["EBA_USER"].ToString());
                            proccess.Parameters.Add("SQ_ID", dr["SQ_ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();
                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_N_RAWSYS_FORM_Update(dr["SQ_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["EBA_USER"].ToString());

                    }
                    finally { con.Close(); }
                }
            }
        }
        public void Trig_BayiOnayBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATS_NCS_BO_TrigRecords(); // başlayacak süreç listesi alınır

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATS_NCS_BO_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));
            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["APPROVAL_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["PROCESS_USER_ID"].ToString(), ImpersonationStatusType.Hidden);


                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["PROCESS_USER_ID"].ToString());
                            proccess.Parameters.Add("V_APPROVAL_ID", dr["APPROVAL_ID"].ToString());
                            proccess.Parameters.Add("V_TABLE_ID", dr["TABLE_ID"].ToString());
                            proccess.Parameters.Add("V_UNIT_ID", dr["ORG_UNIT_ID"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_NCS_BO_FORM_Update(dr["APPROVAL_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            //dal.Set_ATS_DBSFTOF_STATU_Update(dr["APPROVAL_ID"].ToString(), "P", PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["APPROVAL_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["APPROVAL_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["PROCESS_USER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }

        }
        public void Trig_IATSVadeGuncellemeBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_IATS_VADEGUNCELLEME_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_IATS_VADEGUNCELLEME_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dr["SW_MULTI"].ToString() == "0")
                        {
                            FormName = "IATS_VD";
                        }
                        else
                        {
                            FormName = "IATS_TVD";
                        }
                        if (dal.GetServisErrorControl(dr["SQ_HEADER_NO"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_SQ_HEADER_NO", dr["SQ_HEADER_NO"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();
                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_HEADER_NO"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_HEADER_NO"].ToString(), PrID, ex.ToString(), FormName, "0");
                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());
                    }
                    finally { con.Close(); }
                }
            }
        }

        public void Trig_TopluVadeGuncellemeBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATS_TOPLUVADEGUNCELLEME_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATS_TOPLUVADEGUNCELLEME_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SEQ_INVOICES_TEMP"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_USER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_USER"].ToString());
                            proccess.Parameters.Add("V_GRUP_ID", dr["SEQ_INVOICES_TEMP"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_TOPLUVADEGUNCELLEME_FORM_Update(dr["SEQ_INVOICES_TEMP"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATS_TOPLUVADEGUNCELLEME_STATU_Update(dr["SEQ_INVOICES_TEMP"].ToString(), PrID, "B", dr["CD_USER"].ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SEQ_INVOICES_TEMP"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SEQ_INVOICES_TEMP"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_USER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_SPIDSVadeBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATS_SPIDSVADE_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATS_SPIDSVADE_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["UNIQUE_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["USER_ID"].ToString());
                            proccess.Parameters.Add("V_VADETYPE", dr["VADE_TYPE"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["UNIQUE_ID"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_SPIDSVADE_FORM_Update(dr["UNIQUE_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATS_SPIDSVADE_STATU_Update(PrID, dr["UNIQUE_ID"].ToString(), "B");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["UNIQUE_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["UNIQUE_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_ATFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["UNIQUE_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["USER_ID"].ToString());
                            proccess.Parameters.Add("V_TALEPTIPI", dr["VEH_TYPE"].ToString());
                            proccess.Parameters.Add("V_ARACSORUMLUSU", dr["CD_RESPONSIBLE"].ToString());
                            proccess.Parameters.Add("V_UNIQ_NO", dr["UNIQUE_ID"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATF_FORM_Update(dr["UNIQUE_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATF_STATU_Update(dr["UNIQUE_ID"].ToString(), PrID, "B");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["UNIQUE_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["UNIQUE_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_TGIOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATS_TGIOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATS_TGIOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_EBA_PROCESS_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);


                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["USER_ID"].ToString());
                            proccess.Parameters.Add("V_ID_NO", dr["SQ_EBA_PROCESS_ID"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_TGIOF_FORM_Update(dr["SQ_EBA_PROCESS_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATS_TGIOF_STATU_Update(Convert.ToInt32(dr["SQ_EBA_PROCESS_ID"].ToString()), "B", PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_EBA_PROCESS_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_EBA_PROCESS_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_IATSTOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_IATS_TOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_IATS_TOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["USER_ID"].ToString(), ImpersonationStatusType.Hidden);


                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["USER_ID"].ToString());
                            proccess.Parameters.Add("V_SQ_ID", dr["SQ_ID"].ToString());
                            proccess.Parameters.Add("V_TALEP_TIPI", dr["CH_GUARANTEE_DEF"].ToString());
                            proccess.Parameters.Add("V_BOLGE_KODU", dr["CD_COUNTRY"].ToString());
                            proccess.Parameters.Add("V_DML_TYPE", dr["CH_DML_TYPE"].ToString());
                            proccess.Parameters.Add("V_ARAC", dr["SW_ARAC"].ToString());
                            proccess.Parameters.Add("V_YEDEK_PARCA", dr["SW_YP"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_IATS_TOF_FORM_Update(dr["SQ_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_IATS_TOF_STATU_Update(dr["SQ_ID"].ToString(), "B", PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["USER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        public void Trig_ATSDBSFTOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_ATS_DBSFTOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_ATS_DBSFTOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SQ_ATS_BULK"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_USER"].ToString(), ImpersonationStatusType.Hidden);


                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_USER"].ToString());
                            proccess.Parameters.Add("V_SQ_ATS_BULK", dr["SQ_ATS_BULK"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_ATS_DBSFTOF_FORM_Update(dr["SQ_ATS_BULK"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_ATS_DBSFTOF_STATU_Update(dr["SQ_ATS_BULK"].ToString(), "B", PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_ATS_BULK"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_ATS_BULK"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_USER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }
        #endregion


        public void Trig_GSPBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_GPS_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_GPS_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID_PROCESS_TEMP"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);


                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_KPS_HDR_ID", dr["ID_KPS_HEADER"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_GPS_FORM_Update(Convert.ToInt32(PrID), Convert.ToInt32(dr["ID_PROCESS_TEMP"].ToString()));
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_GPS_STATU_Update(Convert.ToInt32(dr["ID_KPS_HEADER"].ToString()), Convert.ToInt32(PrID), "2", Convert.ToInt32(dr["ID_PROCESS_TEMP"].ToString()));
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["ID_PROCESS_TEMP"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID_PROCESS_TEMP"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        public void Trig_N_VADUZBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_N_VADUZ_TrigRecords();
            log.WriteLine("DEBUG", string.Format("[{0}]Get_N_VADUZ_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count)); if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID_VALOR"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName); 
                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_LIFOMATIK", dr["ID_VALOR"].ToString());
                            proccess.Parameters.Update();
                            proccess.Start();
                            PrID = proccess.ProcessId.ToString(); log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["ID_VALOR"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID_VALOR"].ToString(), PrID, ex.ToString(), FormName, "0");
                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());
                    }
                    finally { con.Close(); }
                }
            }
        }
        public void Trig_N_VADUZContinueProcess(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_N_VADUZTContinueProcess_TrigRecords();
            DataTable dtreq;
            log.WriteLine("DEBUG", string.Format("[{0}]Get_N_VADUZTContinueProcess_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count)); if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        dtreq = dal.Get_N_VADUZTREQID_TrigRecords(dr["ID_VALOR"].ToString()); if (dal.GetServisErrorControl(dr["ID_VALOR"].ToString(), FormName) == true && dtreq.Rows.Count > 0)
                        {
                            #region SurecDevamEttir                             string pauserName = "AkisDurdurucu1";                             con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            int resultId = 5;
                            int reqID = Convert.ToInt32(dtreq.Rows[0]["ID"].ToString());
                            con.Open(); WorkflowManager manager = con.WorkflowManager;
                            WorkflowProcess process = manager.GetProcess(Convert.ToInt32(dtreq.Rows[0]["PROCESSID"].ToString()));
                            if (!process.Finished)
                            {
                                process.Continue(reqID, resultId);
                            }
                            log.WriteLine("DEBUG", "[" + FormName + "]süreci continue process ile devam ettirildi. pid: " + PrID);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["ID_VALOR"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID_VALOR"].ToString(), PrID, ex.ToString(), FormName, "0");
                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());
                    }
                    finally { con.Close(); }
                }
            }
        }


        public void Trig_LFMTKBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_LFMTK_TrigRecords();
            log.WriteLine("DEBUG", string.Format("[{0}]Get_LIFOMATIK_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        if (dr["CD_PT"].ToString() == "H")
                        {
                            FormName = "LFMTK";
                        }
                        else
                        {
                            FormName = "LFMTKBNK";
                        }

                        if (dal.GetServisErrorControl(dr["ID_LIFOMATIK"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_LIFOMATIK", dr["ID_LIFOMATIK"].ToString());
                            log.WriteLine("DEBUG_proxy_4", "test proxy_4");
                            proccess.Parameters.Update();
                            proccess.Start();
                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["ID_LIFOMATIK"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID_LIFOMATIK"].ToString(), PrID, ex.ToString(), FormName, "0");
                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());
                    }
                    finally { con.Close(); }
                }
            }
        }


        public void Trig_LFMTKContinueProcess(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_LFMTKContinueProcess_TrigRecords();
            DataTable dtreq;
            log.WriteLine("DEBUG", string.Format("[{0}]Get_LIFOMATIK_PROCESS_CONTINUE_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {
                        dtreq = dal.Get_LFMTKREQID_TrigRecords(dr["ID_LIFOMATIK"].ToString());

                        if (dal.GetServisErrorControl(dr["ID_LIFOMATIK"].ToString(), FormName) == true && dtreq.Rows.Count > 0)
                        {
                            #region SurecDevamEttir

                            string pauserName = "AkisDurdurucu1";

                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            int resultId = 5;
                            int reqID = Convert.ToInt32(dtreq.Rows[0]["ID"].ToString());
                            con.Open();

                            WorkflowManager manager = con.WorkflowManager;
                            WorkflowProcess process = manager.GetProcess(Convert.ToInt32(dtreq.Rows[0]["PROCESSID"].ToString()));
                            if (!process.Finished)
                            {
                                process.Continue(reqID, resultId);
                            }



                            log.WriteLine("DEBUG", "[" + FormName + "]süreci continue process ile devam ettirildi. pid: " + PrID);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["ID_LIFOMATIK"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID_LIFOMATIK"].ToString(), PrID, ex.ToString(), FormName, "0");
                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());
                    }
                    finally { con.Close(); }
                }
            }
        }


        public void Trig_TPSOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_TFSOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_TPSOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));

            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";
                    try
                    {

                        if (dal.GetServisErrorControl(dr["FK_TIP_LEVEL_SQ_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                            con.Impersonate(dr["CH_USER"].ToString(), ImpersonationStatusType.Hidden);
                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("SQ_ID", dr["FK_TIP_LEVEL_SQ_ID"].ToString());
                            proccess.Parameters.Add("V_SQID", dr["FK_TIP_LEVEL_SQ_ID"].ToString());
                            proccess.Parameters.Add("P_CH_EBA_UNIQ_ID", dr["NM_FORM_TYPE"].ToString());

                            proccess.Parameters.Update();
                            proccess.Start();
                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["FK_TIP_LEVEL_SQ_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["FK_TIP_LEVEL_SQ_ID"].ToString(), PrID, ex.ToString(), FormName, "0");
                        GetDeleteProcess(PrID, dr["CH_USER"].ToString());
                    }
                    finally { con.Close(); }
                }
            }
        }

        public void Trig_DOFOOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_DOFOOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_DOFOOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["IDRECONDEBTHEADER"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CDCREATOR"].ToString(), ImpersonationStatusType.Hidden);


                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CDCREATOR"].ToString());
                            proccess.Parameters.Add("V_ID", dr["IDRECONDEBTHEADER"].ToString());
                            proccess.Parameters.Add("V_FATURA_ALT_TIPI", dr["INVOICE_SUB_GROUP"].ToString());
                            proccess.Parameters.Add("V_FATURA_TIPI", dr["INVOICE_GROUP"].ToString());
                            proccess.Parameters.Add("V_MUSTERI_KODU", dr["HESAP_KODU"].ToString());



                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_DOFOOF_FORM_Update(Convert.ToInt32(PrID), dr["IDRECONDEBTHEADER"].ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_DOFOOF_STATU_Update(dr["IDRECONDEBTHEADER"].ToString(), PrID, "B");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["IDRECONDEBTHEADER"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["IDRECONDEBTHEADER"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CDCREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        public void Trig_SPIDS_MLOABaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_MLOA_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_MLOA_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("FT_MUDUR", dr["CD_PRICETYPE"].ToString());
                            proccess.Parameters.Add("V_KM", dr["CD_TYPE"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_MLOA_FORM_Update(dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_MLOA_STATU_Update(Convert.ToInt32(dr["NO_EBA"].ToString()), "B", Convert.ToInt32(PrID), "-");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        public void Trig_SPIDS_FGOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_FGOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_FGOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("FT_MUDUR", dr["CD_PRICETYPE"].ToString());
                            proccess.Parameters.Add("V_KM", dr["CD_TYPE"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_FGOF_FORM_Update(dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_FGOF_STATU_Update(Convert.ToInt32(dr["NO_EBA"].ToString()), "B", Convert.ToInt32(PrID), "-");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        public void Trig_SPIDS_SLOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_SLOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_SLOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_SLOF_FORM_Update(dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_SLOF_STATU_Update(Convert.ToInt32(dr["NO_EBA"].ToString()), "B", Convert.ToInt32(PrID), "-");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }



        public void Trig_SPIDS_CKGOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_CKGOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_CKGOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("FT_MUDUR", dr["CD_PRICETYPE"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_CKGOF_FORM_Update(dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_CKGOF_STATU_Update(Convert.ToInt32(dr["NO_EBA"].ToString()), "B", Convert.ToInt32(PrID), "-");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }


        public void Trig_PBOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_PBOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_PBOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["TICKET_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["OWNER_ID"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["OWNER_ID"].ToString());
                            proccess.Parameters.Add("V_KATEGORI", dr["TICKET_CATEGORY_ID"].ToString());
                            proccess.Parameters.Add("V_ALT_DEPARTMAN", dr["SUB_DEPARTMENT_ID"].ToString());
                            proccess.Parameters.Add("V_TOTAL_COST_MMB", dr["TOTAL_COST_MMB"].ToString());
                            proccess.Parameters.Add("V_TICKET_ID", dr["TICKET_ID"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_PBOF_FORM_Update(dr["TICKET_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_PBOF_STATU_Update(dr["TICKET_ID"].ToString(), "B", "Basladi", PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["TICKET_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["TICKET_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["OWNER_ID"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }


        public void Trig_CAPEX_DOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_CAPEX_DOF_TrigRecords();


            log.WriteLine("DEBUG", string.Format("[{0}]Get_CAPEX_DOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["ID_PAYMENT"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);


                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_FORMTYPE", "CAPEX_DOTFRM");
                            proccess.Parameters.Add("V_CAPEX_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_CAPEX_ID", dr["ID_PAYMENT"].ToString());
                            proccess.Parameters.Add("V_CAPEX_TUTAR", dr["CS_INVOICE"].ToString());
                            proccess.Parameters.Add("V_CAPEX_PARA_BIRIMI", dr["CD_INVOICE_CURRENCY"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            DataTable dt1 = dal.Get_EBA_ID(PrID);

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_CAPEX_DOF_FORM_Update(dr["ID_PAYMENT"].ToString(), Convert.ToInt32(PrID));
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_CAPEX_DOF_STATU_Update(dr["ID_PAYMENT"].ToString(), dt1.Rows[0]["FILEPROFILEID"].ToString(), "20", "");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["ID_PAYMENT"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["ID_PAYMENT"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        public void Trig_BUYBACKBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_BUYBACK_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_BUYBACK_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["BUYBACK_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);


                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);


                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_ID", dr["BUYBACK_ID"].ToString());
                            proccess.Parameters.Add("V_ULKE_KODU", dr["CD_MARKET_CODE"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_BUYBACK_FORM_Update(dr["BUYBACK_ID"].ToString(), Convert.ToInt32(PrID));
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_BUYBACK_STATU_Update(dr["BUYBACK_ID"].ToString(), PrID, "B");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["BUYBACK_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["BUYBACK_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }



        public void Trig_SPIDS_YPTYDOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_YPTYDOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_YPTYDOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_YPTYDOF_FORM_Update(dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_YPTYDOF_STATU_Update(Convert.ToInt32(dr["NO_EBA"].ToString()), "B", Convert.ToInt32(PrID), "-");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        public void Trig_SPIDS_YPKKOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_YPKKOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_YPKKOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_YPKKOF_FORM_Update(dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_YPKKOF_STATU_Update(Convert.ToInt32(dr["NO_EBA"].ToString()), "B", Convert.ToInt32(PrID));
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }


        public void Trig_SPIDS_YPKPTOFBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_YPKPTOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_YPKPTOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_YPKPTOF_FORM_Update(dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_YPKPTOF_STATU_Update(dr["CD_INSERTEDUSER"].ToString(), "B", dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        public void PARAMETER_USER_DELETE()
        {

            log.WriteLine("DEBUG", string.Format("PARAMETER_USER_DELETE SP to EBA5 ", "INVALID USER DELETE"));

            try
            {

                CommonDAL dal = new CommonDAL();
                dal.PARAMETER_USER_DELETE();


                log.WriteLine("DEBUG", "[PARAMETER_USER_DELETE]süreç başladı pid: ");

                log.WriteLine("DEBUG", "[PARAMETER_USER_DELETE]süreç başladı form update başarılı.");


            }
            catch (Exception ex)
            {
                log.WriteLine("ERROR", " hata: " + ex.ToString());

            }


        }





        public void Trig_NCS_ITlHPBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_NCS_ITHP_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_NCS_ITHP_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["SOS_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["SOS_ID"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();

                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_NCS_ITHP_FORM_Update((dr["SOS_ID"].ToString()), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");
                          
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SOS_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SOS_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }


        public void Trig_SPIDS_KOF_Baslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_KOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_KOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_KOF_FORM_Update(Convert.ToInt32(dr["NO_EBA"].ToString()), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_KOF_STATU_Update(dr["CD_INSERTEDUSER"].ToString(), "B", dr["NO_EBA"].ToString(), PrID, "D");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

        public void Trig_SPIDS_YPKSDOF_Baslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_YPKSDOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_YPKSDOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["EBA_ID"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["EBA_ID"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_YPKSDOF_FORM_Update(dr["EBA_ID"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_YPKSDOF_STATU_Update(dr["EBA_ID"].ToString(), "B", Convert.ToInt32(PrID), "");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["EBA_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["EBA_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }




        public void Trig_SPIDS_YPKTOF_Baslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_SPIDS_YPKTOF_TrigRecords();

            log.WriteLine("DEBUG", string.Format("[{0}]Get_SPIDS_YPKTOF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                        if (dal.GetServisErrorControl(dr["NO_EBA"].ToString(), FormName) == true)
                        {
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_INSERTEDUSER"].ToString(), ImpersonationStatusType.Hidden);

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_INSERTEDUSER"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["NO_EBA"].ToString());


                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();


                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);
                            dal.Set_SPIDS_YPKTOF_FORM_Update(dr["NO_EBA"].ToString(), PrID);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            dal.Set_SPIDS_YPKTOF_STATU_Update(dr["CD_INSERTEDUSER"].ToString(), "B", dr["NO_EBA"].ToString(), PrID, "H");
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı statu update başarılı.");

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["NO_EBA"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["NO_EBA"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_INSERTEDUSER"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
        }

            public void Trig_N_GGMGFBaslat(string FormName)
            {
                CommonDAL dal = new CommonDAL();
                DataTable dt = dal.Get_N_GGMGF_TrigRecords();
                

            log.WriteLine("DEBUG", string.Format("[{0}]Get_N_GGMGF_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection();
                foreach (DataRow dr in dt.Rows)
                {
                    string PrID = "";

                    try
                    {
                       // log.WriteLine("DEBUG", string.Format("[{0}]Get_N_GGMGF_TrigRecords SP to EBA5 row count: {1},{2}", FormName, dt.Rows.Count, dr["SQ_ID"].ToString()));
                        if (dal.GetServisErrorControl(dr["SQ_ID"].ToString(), FormName) == true)
                        {
                           // log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak");
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();
                       
                                                 con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);
                                                 //log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak2");

                                                 WorkflowManager mng = con.WorkflowManager;
                                                 log.WriteLine("DEBUG", "[" + FormName + "]Form başlamaya girdi.");
                                                 WorkflowDocument doc = mng.CreateDocument("N_GGMGF", "FORM");
                                                 log.WriteLine("DEBUG", dr["SQ_ID"].ToString() + " başladı " + doc.DocumentId.ToString());
                                                 dal.Set_N_GGMGF_FORM_Update((dr["SQ_ID"].ToString()), doc.DocumentId.ToString());
                                                 log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                                                 WorkflowManager mgr = con.WorkflowManager;
                                                 WorkflowProcess proccess = mgr.CreateProcess(FormName);

                                                 proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                                                 proccess.Parameters.Add("V_UNIQID", dr["SQ_ID"].ToString());
                                                 proccess.Parameters.Add("V_FORMID", doc.DocumentId.ToString());

                                                 //log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak3");
                                                 proccess.Parameters.Update();
                                                 proccess.Start();

                                                 PrID = proccess.ProcessId.ToString();
                                                 log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);       

                            #endregion
                        }
                        else
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç bulunamadı");

                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["SQ_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["SQ_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
            else
                log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak");
            
            }

        public void Trig_N_ZIYRANBaslat(string FormName)
        {
            CommonDAL dal = new CommonDAL();
            DataTable dt = dal.Get_N_ZIYRAN_TrigRecords();


            log.WriteLine("DEBUG", string.Format("[{0}]Get_N_ZIYRAN_TrigRecords SP to EBA5 row count: {1}", FormName, dt.Rows.Count));


            if (dt.Rows.Count > 0)
            {
                eBAConnection con = new eBAConnection(); 
                foreach (DataRow dr in dt.Rows) 
                {
                    string PrID = "";

                    try
                    {
                        log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak27");

               
                        if (dal.GetServisErrorControl(dr["CD_UNIQUE_ID"].ToString(), FormName) == true)
                        {                        
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak");
                            #region Kayıt Başlat
                            con = new eBAConnection();
                            con.Server = StaticVar.ServerName;
                            con.UserID = StaticVar.UserName;
                            con.Password = StaticVar.PassWord;
                            con.CommandTimeout = 500;
                            con.Open();

                            con.Impersonate(dr["CD_CREATOR"].ToString(), ImpersonationStatusType.Hidden);
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak24");

                            WorkflowManager mng = con.WorkflowManager;
                            // log.WriteLine("DEBUG", "[" + FormName + "]Form başlamaya girdi.");
                            WorkflowDocument doc = mng.CreateDocument("N_ZIYRAN", "FORM");
                            // log.WriteLine("DEBUG", dr["CD_UNIQUE_ID"].ToString() + " başladı " + doc.DocumentId.ToString());

                            log.WriteLine("DEBUG", "[" + FormName + "]V_UNIQUID.Value: "+ dr["CD_UNIQUE_ID"].ToString());

                            dal.Set_N_ZIYRAN_FORM_Update((dr["CD_UNIQUE_ID"].ToString()), doc.DocumentId.ToString());
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı form update başarılı.");

                            WorkflowManager mgr = con.WorkflowManager;
                            WorkflowProcess proccess = mgr.CreateProcess(FormName);

                            proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                            proccess.Parameters.Add("V_UNIQID", dr["CD_UNIQUE_ID"].ToString());
                            proccess.Parameters.Add("V_FORMID", doc.DocumentId.ToString());
                            proccess.Parameters.Add("V_ZIYARET", dr["CD_APPROVER"].ToString());

                           // log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak3");
                            proccess.Parameters.Update();
                            proccess.Start();

                            PrID = proccess.ProcessId.ToString();
                            log.WriteLine("DEBUG", "[" + FormName + "]süreç başladı pid: " + PrID);  

                            /*     WorkflowManager mgr = con.WorkflowManager;
                                 WorkflowProcess proccess = mgr.CreateProcess(FormName);
                                 proccess.Parameters.Add("V_AKISBASLATAN", dr["CD_CREATOR"].ToString());
                                 proccess.Parameters.Add("V_UNIQID", dr["CD_UNIQUE_ID"].ToString());
                                 proccess.Parameters.Update();
                                 proccess.Start();
                                 PrID = proccess.ProcessId.ToString();  */

                            #endregion
                        }
                            else
                             log.WriteLine("DEBUG", "[" + FormName + "]süreç bulunamadı");

                    }
                    catch (Exception ex)
                    {
                        log.WriteLine("ERROR", "[" + FormName + "] Trig -> foreach -> " + FormName + " Form No: " + dr["CD_UNIQUE_ID"].ToString() + "  |  hata: " + ex.ToString());
                        dal.InsertServiceErrorLog(dr["CD_UNIQUE_ID"].ToString(), PrID, ex.ToString(), FormName, "0");

                        GetDeleteProcess(PrID, dr["CD_CREATOR"].ToString());

                    }
                    finally { con.Close(); }

                }
            }
            else
                log.WriteLine("DEBUG", "[" + FormName + "]süreç başlatılacak");

        }




    }
}



