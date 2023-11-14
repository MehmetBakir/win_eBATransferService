using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eba5Dal
{
    public static class StaticVar
    {
        public static bool SyhmsfBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SyhmsfBaslat"].ToString());
        public static bool SyhavaBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SyhavaBaslat"].ToString());
        public static bool MuftonBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["MuftonBaslat"].ToString());
        public static bool MuhmasBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["MuhmasBaslat"].ToString());
        public static bool NetofBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["NetofBaslat"].ToString());
        public static bool NetIDBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["NetIDBaslat"].ToString());
        public static bool TerkinBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TerkinBaslat"].ToString());
        public static bool TiadeBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TiadeBaslat"].ToString());
        public static bool ThsidBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ThsidBaslat"].ToString());
        public static bool ImlavaBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ImlavaBaslat"].ToString());
        public static bool CLAHOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CLAHOFBaslat"].ToString());
        public static bool CLAHFCNCBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CLAHFCNCBaslat"].ToString());
        public static bool FatsBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["FatsBaslat"].ToString());
        public static bool AcrBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AcrBaslat"].ToString());
        public static bool SmrBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SmrBaslat"].ToString());
        public static bool UydgdaBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UydgdaBaslat"].ToString());
        public static bool TalepSosBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TalepSos"].ToString());
        public static bool SapOdfrmBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SapOdfrmBaslat"].ToString());
        public static bool InvalidStep = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["INVALIDStep"].ToString());
        public static bool SPFRMBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SPFRMBaslat"].ToString());
        public static bool FMSCBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["FMSCBaslat"].ToString());
        public static bool TOSHOLDBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TOSHOLDBaslat"].ToString());
        public static bool IMSBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IMSBaslat"].ToString());
        public static bool KPSBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KPSBaslat"].ToString());
        public static bool ITHALATBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ITHALATBaslat"].ToString());
        public static bool AtsTeminatBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AtsTeminatBaslat"].ToString());
        public static bool AtsLimitBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AtsLimitBaslat"].ToString());
        public static bool AtsVadeBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AtsVadeBaslat"].ToString());
        public static bool AtsHavaleBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AtsHavaleBaslat"].ToString());
        public static bool AtsFaturaIptalBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AtsFaturaIptalBaslat"].ToString());
        public static bool AtsStandartVadeBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AtsStandartVadeBaslat"].ToString());
        public static bool FtruckFaturaBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["FtruckFaturaBaslat"].ToString());
        public static bool MusteriFaturaBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["MusteriFaturaBaslat"].ToString());
        public static bool BayiOnayBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["BayiOnayBaslat"].ToString());
        public static bool RAWSYSBaslat   = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RAWSYSBaslat"].ToString());
        public static bool RAWSYS2Baslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RAWSYS2Baslat"].ToString());
        public static bool IATSTahsilatBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IATSTahsilatBaslat"].ToString());
        public static bool TopluFaturaVadeGuncellemeBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TopluFaturaVadeGuncellemeBaslat"].ToString());
        public static bool IATSFaturaVadeGuncellemeBaslat  = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IATSFaturaVadeGuncellemeBaslat"].ToString());
        public static bool SPIDSVadeBaslat                  = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SPIDSVadeBaslat"].ToString());
        public static bool ATFBaslat                        = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ATFBaslat"].ToString());
        public static bool TGIOFBaslat                      = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TGIOFBaslat"].ToString());
        public static bool IATSTOFBaslat                    = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IATSTOFBaslat"].ToString());
        public static bool ATSDBSFTOFBaslat                 = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ATSDBSFTOFBaslat"].ToString());
        public static bool GPSBaslat                        = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["GPSBaslat"].ToString());
        public static bool LFMTKBaslat                      = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["LFMTKBaslat"].ToString());
        public static bool LFMTKContinueProcess             = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["LFMTKContinueProcess"].ToString());
        public static bool N_VADUZTBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["N_VADUZTBaslat"].ToString());
        public static bool N_VADUZTContinueProcess = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["N_VADUZTContinueProcess"].ToString());
        public static bool TPSOFBaslat                      = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TPSOFBaslat"].ToString());
        public static bool DOFOOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["DOFOOFBaslat"].ToString());
        public static bool MLOABaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["MLOABaslat"].ToString());
        public static bool FGOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["FGOFBaslat"].ToString());
        public static bool SLOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SLOFBaslat"].ToString());
        public static bool CKGOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CKGOFBaslat"].ToString());
        public static bool PBOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["PBOFBaslat"].ToString());
        public static bool CAPEX_DOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CAPEX_DOFBaslat"].ToString());
        public static bool BUYBACKBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["BUYBACKBaslat"].ToString());
        public static bool YPTYDOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["YPTYDOFBaslat"].ToString());
        public static bool YPKKOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["YPKKOFBaslat"].ToString());
        public static bool YPKPTOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["YPKPTOFBaslat"].ToString());
        public static bool NCS_ITHPBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["NCS_ITHPBaslat"].ToString());
        public static bool NCS_KAMPBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["NCS_KAMPBaslat"].ToString());
        public static bool YPKSDOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["YPKSDOFBaslat"].ToString());
        public static bool YPKTOFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["YPKTOFBaslat"].ToString());
        public static bool N_GGMGFBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["N_GGMGFBaslat"].ToString());
        public static bool N_ZIYRANBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["N_ZIYRANBaslat"].ToString());
        public static bool UYDGDAContinueProcess = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UYDGDAContinueProcess"].ToString());
        public static bool RMISBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RMISBaslat"].ToString());



        //public static bool EXCARBaslat = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EXCARBaslat"].ToString());

        public static string ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"].ToString();
        public static string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
        public static string PassWord = System.Configuration.ConfigurationManager.AppSettings["PassWord"].ToString();
        public static string DefaultUser = System.Configuration.ConfigurationManager.AppSettings["DefaultUser"].ToString();
    }
}
