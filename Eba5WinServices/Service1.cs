using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using Eba5Dal;

namespace Eba5WinServices
{
    public partial class Service1 : ServiceBase
    {

        dsUtils.dsLogFile log = new dsUtils.dsLogFile();
   
        private bool isDebug = false;
        private bool isRestartFlow = false;
        private bool isUseRequest = false;
        private bool isUseSmr = false;
        private bool isRecloseTickets = false;
        private bool isUpdateAssignedTo = false;
    
        public Service1()
        {
            
            InitializeComponent();
           
            if (System.Configuration.ConfigurationManager.AppSettings["ServiceTimerInterval"] != "") this.timer1.Interval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ServiceTimerInterval"]);
            isRestartFlow = (System.Configuration.ConfigurationManager.AppSettings["RestartFlowMode"] != null && System.Configuration.ConfigurationManager.AppSettings["RestartFlowMode"] == "true");
            isRecloseTickets = (System.Configuration.ConfigurationManager.AppSettings["RecloseTickets"] != null && System.Configuration.ConfigurationManager.AppSettings["RecloseTickets"] == "true");
            isDebug = (System.Configuration.ConfigurationManager.AppSettings["debugMode"] != null && System.Configuration.ConfigurationManager.AppSettings["debugMode"] == "true");
            isUseRequest = (System.Configuration.ConfigurationManager.AppSettings["UseRequest"] != null && System.Configuration.ConfigurationManager.AppSettings["UseRequest"] == "true");
            isUseSmr = (System.Configuration.ConfigurationManager.AppSettings["UseSmr"] != null && System.Configuration.ConfigurationManager.AppSettings["UseSmr"] == "true");
            isUpdateAssignedTo = (System.Configuration.ConfigurationManager.AppSettings["isUpdateAssignedTo"] != null && System.Configuration.ConfigurationManager.AppSettings["isUpdateAssignedTo"] == "true");
              
 
        }
     
        protected override void OnStart(string[] args)
        {

            System.Threading.Thread.Sleep(10000);
            log.WriteLine("WINSERVICE", "Service start .");     
            timer1_Elapsed(null, null);
        }
      
        protected override void OnStop()
        {
            Dispose();
            log.WriteLine("WINSERVICE", "Service stop.");
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {         
            try
            {                     
                timer1.Enabled = false;
                EbaTransfer();
              
               if (isRecloseTickets) dsRecloseTicket();
               if (isUpdateAssignedTo) dsUpdateDifferentAssignedTo();

            }
            catch (Exception ex)
            {
               log.WriteLine("ERROR", "timer1_Elapsed() -> " + ex.ToString());
            }
            finally
            {
                timer1.Enabled = true;
            }		
        }

        private void EbaTransfer()
        {
            try
            {                
                Eba5Dal.EbaAktar dt = new EbaAktar();
                if (StaticVar.SyhmsfBaslat)   dt.Trig_SYMSF("SYHMSF");
                if (StaticVar.SyhavaBaslat)   dt.Trig_SYHAVA("SYHAVA");
                if (StaticVar.TerkinBaslat)   dt.Trig_TERKIN("TERKIN");
                if (StaticVar.TiadeBaslat)    dt.Trig_TIADE("TIADE");
                if (StaticVar.ThsidBaslat)    dt.Trig_THSID("THSID");
                if (StaticVar.NetofBaslat)    dt.Trig_NETOF("NETOF");
                if (StaticVar.NetIDBaslat)    dt.Trig_NETID("NETID");
                if (StaticVar.MuftonBaslat)   dt.Trig_MUFTON("MUFTON");
                if (StaticVar.MuhmasBaslat)   dt.Trig_MUHMAS("MUHMAS");
                if (StaticVar.ImlavaBaslat)   dt.Trig_IMLAVA("IMLAVA");
                if (StaticVar.CLAHOFBaslat)   dt.Trig_CLAHOF("CLAHOF");
                if (StaticVar.CLAHFCNCBaslat) dt.Trig_CLAHFCNC("CLAHFCNC");
                if (StaticVar.FatsBaslat)     dt.Trig_FATS("FatsBaslat");
                if (StaticVar.FatsBaslat)     dt.receiveFatsToEba3();
                if (StaticVar.AcrBaslat)      dt.Trig_ACR("AC");
                if (StaticVar.AcrBaslat)      dt.receiveTicketFromEbaAll("AC");
                if (StaticVar.UydgdaBaslat)   dt.Trig_UYDGDA("UYDGDA");
                if (StaticVar.SmrBaslat)      dt.Trig_SMR("CR");
                if (StaticVar.TalepSosBaslat) dt.Trig_TALEPSOS("RQ");
                if (StaticVar.SapOdfrmBaslat) dt.Trig_SAP_ODRM("SapOdfrmBaslat");
                if (StaticVar.SPFRMBaslat)    dt.Trig_SPFRMBaslat("SPFRM");
                if (StaticVar.FMSCBaslat)     dt.Trig_FMSCBaslat("FMSC");
                if (StaticVar.TOSHOLDBaslat)  dt.Trig_TOSHOLDBaslat("TOSHOLD");
                if (StaticVar.IMSBaslat)      dt.Trig_IMSBaslat("IMS");
                if (StaticVar.KPSBaslat)      dt.Trig_KPSBaslat("KPS");
                if (StaticVar.ITHALATBaslat)  dt.Trig_ITHALATBaslat("ITHALAT");
                if (StaticVar.AtsTeminatBaslat) dt.Trig_AtsTeminatBaslat("TIOF");
                if (StaticVar.AtsLimitBaslat) dt.Trig_AtsLimitBaslat("ELIOF");
                if (StaticVar.AtsVadeBaslat) dt.Trig_AtsVadeBaslat("FVIOF");
                if (StaticVar.AtsHavaleBaslat) dt.Trig_AtsHavaleBaslat("HTIOF");
                if (StaticVar.AtsFaturaIptalBaslat) dt.Trig_AtsFaturaIptalBaslat("FIIOF");
                if (StaticVar.AtsStandartVadeBaslat) dt.Trig_AtsStandartVadeBaslat("SVIOF");
                if (StaticVar.FtruckFaturaBaslat) dt.Trig_FtruckFaturaBaslat("ECFRM");
                if (StaticVar.MusteriFaturaBaslat) dt.Trig_MusteriFaturaBaslat("MFIOF");
                if (StaticVar.BayiOnayBaslat) dt.Trig_BayiOnayBaslat("NCS_BO");
                if (StaticVar.RAWSYSBaslat) dt.Trig_RAWSYSBaslat("N_RAWSYS");
                if (StaticVar.RAWSYS2Baslat) dt.Trig_RAWSYS2Baslat("N_RAWSY2");
                if (StaticVar.IATSTahsilatBaslat) dt.Trig_IATSTahsilatBaslat("NCS_IATS");
                if (StaticVar.TopluFaturaVadeGuncellemeBaslat) dt.Trig_TopluVadeGuncellemeBaslat("TFVGIOF");
                if (StaticVar.IATSFaturaVadeGuncellemeBaslat) dt.Trig_IATSVadeGuncellemeBaslat("IATS_VD");                
                if (StaticVar.SPIDSVadeBaslat) dt.Trig_SPIDSVadeBaslat("SPIDSIOF");
                if (StaticVar.ATFBaslat) dt.Trig_ATFBaslat("ATF");
                if (StaticVar.ATSDBSFTOFBaslat) dt.Trig_ATSDBSFTOFBaslat("DBSFTOF");
                if (StaticVar.TGIOFBaslat) dt.Trig_TGIOFBaslat("TGIOF");
                if (StaticVar.IATSTOFBaslat) dt.Trig_IATSTOFBaslat("IATSTOF");
                if (StaticVar.GPSBaslat) dt.Trig_GSPBaslat("GPS");
                if (StaticVar.InvalidStep)    dt.Trig_InvalidStep();
                //if (StaticVar.EXCARBaslat)    dt.Trig_EXCARBaslat("EXCAR"); Süreç İptal
                if (StaticVar.LFMTKContinueProcess) dt.Trig_LFMTKContinueProcess("LFMTKCNT"); 
                if (StaticVar.LFMTKBaslat) dt.Trig_LFMTKBaslat("LFMTK");
                if (StaticVar.N_VADUZTBaslat) dt.Trig_N_VADUZBaslat("N_VADUZT");
                if (StaticVar.N_VADUZTContinueProcess) dt.Trig_N_VADUZContinueProcess("N_VADUZCNT");
                if (StaticVar.TPSOFBaslat) dt.Trig_TPSOFBaslat("TPSOF");
                if (StaticVar.DOFOOFBaslat) dt.Trig_DOFOOFBaslat("DOFOOF");
                if (StaticVar.MLOABaslat) dt.Trig_SPIDS_MLOABaslat("MLOA");
                if (StaticVar.FGOFBaslat) dt.Trig_SPIDS_FGOFBaslat("FGOF");
                if (StaticVar.SLOFBaslat) dt.Trig_SPIDS_SLOFBaslat("SLOF");
                if (StaticVar.CKGOFBaslat) dt.Trig_SPIDS_CKGOFBaslat("CKGOF");
                if (StaticVar.PBOFBaslat) dt.Trig_PBOFBaslat("PBOF");
                if (StaticVar.CAPEX_DOFBaslat) dt.Trig_CAPEX_DOFBaslat("DOTFRM");
                if (StaticVar.BUYBACKBaslat) dt.Trig_BUYBACKBaslat("BUYBACK");
                if (StaticVar.YPTYDOFBaslat) dt.Trig_SPIDS_YPTYDOFBaslat("YPTYDOF");
                if (StaticVar.YPKKOFBaslat) dt.Trig_SPIDS_YPKKOFBaslat("YPKKOF");
                if (StaticVar.YPKPTOFBaslat) dt.Trig_SPIDS_YPKPTOFBaslat("YPKPTOF");
                if (StaticVar.NCS_ITHPBaslat) dt.Trig_NCS_ITlHPBaslat("NCS_ITHP");
                if (StaticVar.NCS_KAMPBaslat) dt.Trig_SPIDS_KOF_Baslat("NCS_KAMP");
                if (StaticVar.YPKSDOFBaslat) dt.Trig_SPIDS_YPKSDOF_Baslat("YPKSDOF");
                if (StaticVar.YPKTOFBaslat) dt.Trig_SPIDS_YPKTOF_Baslat("YPKTOF");
                if (StaticVar.N_GGMGFBaslat) dt.Trig_N_GGMGFBaslat("N_GGMGF"); 
                if (StaticVar.N_ZIYRANBaslat) dt.Trig_N_ZIYRANBaslat("N_ZIYRAN");
                if (StaticVar.UYDGDAContinueProcess) dt.Trig_UYDGDAContinueProcess("UYDGDACNT");
                if (StaticVar.RMISBaslat) dt.Trig_RMIS_Baslat("RMIS");

                // onaylanan red olunan belgeleri alıp IATS sistemine update eder..
                dt.receiveFromEbaAll("TE");// terkin
                dt.receiveFromEbaAll("TIA"); // tiade
                dt.receiveFromEbaAll("NETOF");
                dt.receiveFromEbaAll("NETID");
                dt.receiveFromEbaAll("THSID");
                //dt.PARAMETER_USER_DELETE();

                if (isDebug) log.WriteLine("INFO", "[AC] Transfer stop.");
            }
            catch (Exception ex)
            {
                log.WriteLine("ERROR", "EbaTransfer() -> " + ex.ToString());
            }
        }

        private void dsRestartFlow(string ticketType)
        {
            try
            {
                Eba5Dal.EbaAktar dt = new EbaAktar();
              if (isDebug) log.WriteLine("INFO", "[" + ticketType + "] Empty flow restart.");
             
               
                 dt.akisiBaslamayanlariTekrarBaslat(ticketType);
                if (isDebug) log.WriteLine("INFO", "[" + ticketType + "] Empty flow stop.");
            }
            catch (Exception ex)
            {
               log.WriteLine("ERROR", "dsRestartFlow(" + ticketType + ") -> " + ex.ToString());
            }
        }

        private void dsRecloseTicket()
        {
            try
            {

                Eba5Dal.EbaAktar dt = new EbaAktar();
                if (isDebug) log.WriteLine("INFO", "[ALL] Close tickets start.");
                    dt.altIsleriKapanmisAcikKalanIsleriKapat();
                if (isDebug) log.WriteLine("INFO", "[ALL] Close tickets stop.");
            }
            catch (Exception ex)
            {
               log.WriteLine("ERROR", "dsRecloseTicket() -> " + ex.ToString());
            }
        }

        private void dsUpdateDifferentAssignedTo()
        {
            try
            {
                Eba5Dal.EbaAktar dt = new EbaAktar();
                if (isDebug) log.WriteLine("INFO", "[ALL] Update different assignedTo start.");
                dt.altIsiFarkliKisiyeAtanmisIslerinAtananKisisiniGuncelle();
                if (isDebug) log.WriteLine("INFO", "[ALL] Update different assignedTo stop.");
            }
            catch (Exception ex)
            {
                log.WriteLine("ERROR", "dsRecloseTicket() -> " + ex.ToString());
            }
        }

    }
}
