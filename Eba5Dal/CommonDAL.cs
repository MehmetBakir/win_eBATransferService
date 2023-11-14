using System;
using System.Data;
using System.Data.OracleClient;
using FordOtosan;

namespace Eba5Dal
{

    public class CommonDAL
    {

        private OracleHelper dal;
        dsUtils.dsLogFile log = new dsUtils.dsLogFile();

        string Schema = "EBAWORKFLOW5";
        string Server = "E3000";

        public CommonDAL()
        {
            dal = new OracleHelper(Server, Schema);
        }

        public void GetDbConnectionControl()
        {
            try
            {

                log.WriteLine("DBConfigFile", System.Configuration.ConfigurationManager.AppSettings["DBConfigFile"].ToString());

                CrXIUtility.DBUtils DbUlt = new CrXIUtility.DBUtils();
                string[] Keys = DbUlt.GetConInf(Schema, Server);

                if (Keys.Length > 0)
                    log.WriteLine("GetDbConnectionControl", "True");
                else
                    log.WriteLine("GetDbConnectionControl", "False");


            }
            catch (Exception ex)
            {
                log.WriteLine("GetDbConnectionControl", ex.ToString());
            }
        }


        #region Nacsoft

        public DataTable Get_SYMSF_TrigRecords()
        {
            DataTable dt = new DataTable();

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SYHMSF.ONAYDAKILER";

            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void UPDATE_SYHMSF3_Statu(string GID)
        {
            DataTable dt = new DataTable();

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SYHMSF.UPDATE_SYHMSF3";

            oCmd.Parameters.Add(new OracleParameter("GID", OracleType.VarChar));
            oCmd.Parameters["GID"].Value = GID;

            dal.ExecuteNonQuery(oCmd);

        }

        public void UPDATE_SYHMSF_INIT(string GID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SYHMSF.UPDATE_SYHMSF_INIT";
            oCmd.Parameters.Add(new OracleParameter("GID", OracleType.VarChar));
            oCmd.Parameters["GID"].Value = GID;
            dal.ExecuteNonQuery(oCmd);
        }

        public void UPDATE_SYHAVA3_INIT(string GID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SYHAVA.UPDATE_SYHAVA3_INIT";
            oCmd.Parameters.Add(new OracleParameter("GID", OracleType.VarChar));
            oCmd.Parameters["GID"].Value = GID;
            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_SYHAVA_TrigRecords()
        {
            DataTable dt = new DataTable();

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SYHAVA.ONAYDAKILER";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void UPDATE_SYHAVA3_Statu(string GID)
        {
            DataTable dt = new DataTable();

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SYHAVA.UPDATE_SYHAVA3";

            oCmd.Parameters.Add(new OracleParameter("GID", OracleType.VarChar));
            oCmd.Parameters["GID"].Value = GID;

            dal.ExecuteNonQuery(oCmd);

        }

        public DataTable Get_TERKIN_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EWS_TERKIN.P_TETIKLENECEK_FORM";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public DataTable Get_TIADE_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EWS_TIADE.P_TETIKLENECEK_FORM";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public DataTable Get_THSID_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EWS_THSID.P_TETIKLENECEK_FORM";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public DataTable Get_NETOF_TrigRecords()
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EWS_NETOF.P_TETIKLENECEK_FORM";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public DataTable Get_NETID_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EWS_NETID.P_TETIKLENECEK_FORM";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void InsertServiceErrorLog(string txtISID, string txtESID, string txtHATA, string txtSurec, string CHC_OK)
        {
            DataTable dt = new DataTable();

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EWS_LOG.P_InsertLog";

            oCmd.Parameters.Add(new OracleParameter("I_TXTISID", OracleType.VarChar));
            oCmd.Parameters["I_TXTISID"].Value = txtISID;

            oCmd.Parameters.Add(new OracleParameter("I_TXTESID", OracleType.VarChar));
            oCmd.Parameters["I_TXTESID"].Value = txtESID;

            oCmd.Parameters.Add(new OracleParameter("I_TXTHATA", OracleType.VarChar));
            oCmd.Parameters["I_TXTHATA"].Value = txtHATA;

            oCmd.Parameters.Add(new OracleParameter("I_TXTSUREC", OracleType.VarChar));
            oCmd.Parameters["I_TXTSUREC"].Value = txtSurec;

            oCmd.Parameters.Add(new OracleParameter("I_CHC_OK", OracleType.VarChar));
            oCmd.Parameters["I_CHC_OK"].Value = CHC_OK;

            dal.ExecuteNonQuery(oCmd);

        }

        public void Update_IATS_Processes_Approve_FROM_EBA(string eBAType, string eBAID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IATS_APPROVE.UPDATE_IATS_APPROVE_FROM_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TYPE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TYPE"].Value = eBAType;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = eBAID;

            dal.ExecuteNonQuery(oCmd);

        }

        public DataTable GET_IATS_APPROVE_FROM_EBA(string eBAType)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IATS_APPROVE.GET_IATS_APPROVE_FROM_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TYPE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TYPE"].Value = eBAType;

            oCmd.Parameters.Add(new OracleParameter("P_CURSOR", OracleType.Cursor));
            oCmd.Parameters["P_CURSOR"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void IATS_SEND_EBA_FOR_APPROVAL(string eBAType, string eBAID, string P_IATS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IATS_APPROVE.IATS_SEND_EBA_FOR_APPROVAL";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TYPE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TYPE"].Value = eBAType;

            oCmd.Parameters.Add(new OracleParameter("P_IATS_ID", OracleType.VarChar));
            oCmd.Parameters["P_IATS_ID"].Value = P_IATS_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = eBAID;

            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_CLAHOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_CLAHOF.P_SELECT";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void CLAHOF_P_UPDATE(string P_EBA_ID, string P_EBA_ERROR, string P_FORM_NO, string P_USER_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_CLAHOF.P_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ERROR", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ERROR"].Value = P_EBA_ERROR;

            oCmd.Parameters.Add(new OracleParameter("P_FORM_NO", OracleType.VarChar));
            oCmd.Parameters["P_FORM_NO"].Value = P_FORM_NO;

            oCmd.Parameters.Add(new OracleParameter("P_USER_ID", OracleType.VarChar));
            oCmd.Parameters["P_USER_ID"].Value = P_USER_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_CLAHFCNC_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_CLAHFCNC.P_SELECT";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void CLAHFCNC_P_UPDATE(string P_EBA_ID, string P_EBA_ERROR, string P_FORM_NO, string P_USER_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_CLAHFCNC.P_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ERROR", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ERROR"].Value = P_EBA_ERROR;

            oCmd.Parameters.Add(new OracleParameter("P_FORM_NO", OracleType.VarChar));
            oCmd.Parameters["P_FORM_NO"].Value = P_FORM_NO;

            oCmd.Parameters.Add(new OracleParameter("P_USER_ID", OracleType.VarChar));
            oCmd.Parameters["P_USER_ID"].Value = P_USER_ID;

            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_MUFTON_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_MUFTON.P_SELECT";

            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void MUFTON_P_MUHMHB_SEND_EBA_FOR_APPROVAL(int PID, string SQ_INV)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_MUFTON.P_MUHMHB_SEND_EBA_FOR_APPROVAL";

            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = PID;

            oCmd.Parameters.Add(new OracleParameter("P_SQ_INV", OracleType.VarChar));
            oCmd.Parameters["P_SQ_INV"].Value = SQ_INV;

            dal.ExecuteNonQuery(oCmd);
        }

        public void MUFTON_P_MUHMHB_SEND_EBA_FOR_ROLLBACK(int P_GID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_MUFTON.P_MUHMHB_SEND_EBA_FOR_ROLLBACK";

            oCmd.Parameters.Add(new OracleParameter("P_GID", OracleType.VarChar));
            oCmd.Parameters["P_GID"].Value = P_GID;

            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_MUHMAS_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_MUHMAS.P_SELECT";

            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void UPDATE_MUHMAS(string GID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_MUHMAS.P_UPDATE_MUHMAS3";

            oCmd.Parameters.Add(new OracleParameter("GID", OracleType.VarChar));
            oCmd.Parameters["GID"].Value = GID;

            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_IMLAVA_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IMLAVA.P_SELECT";

            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void UPDATE_IMLAVA3(string GID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IMLAVA.P_UPDATE_IMLAVA3";

            oCmd.Parameters.Add(new OracleParameter("GID", OracleType.VarChar));
            oCmd.Parameters["GID"].Value = GID;

            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_UYDGDA_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_UYDGDA.P_SELECT";

            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public DataTable Get_SMR_TrigRecords(string ticketType)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SMR_SERVICE.PRC_GET_TICKETS_FOR_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_CD_TICKET_TYPE", OracleType.VarChar));
            oCmd.Parameters["P_CD_TICKET_TYPE"].Value = ticketType;
            oCmd.Parameters.Add(new OracleParameter("P_RC_DATA", OracleType.Cursor));
            oCmd.Parameters["P_RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public DataTable Get_SPFRM_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPFRM.P_GET_SP_TO_EBA";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void Get_SPFRM_UPDATE(string P_CD_GSDB, string P_CD_TYPE)
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPFRM.P_SET_SP_STATU";

            oCmd.Parameters.Add(new OracleParameter("P_CD_GSDB", OracleType.VarChar));
            oCmd.Parameters["P_CD_GSDB"].Value = P_CD_GSDB;

            oCmd.Parameters.Add(new OracleParameter("P_CD_TYPE", OracleType.VarChar));
            oCmd.Parameters["P_CD_TYPE"].Value = P_CD_TYPE;

            dal.ExecuteNonQuery(oCmd);

        }

        public void Get_EXCAR_UPDATE(string P_SQ_INV)
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EXCAR.P_SET_EXCAR_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SQ_INV", OracleType.VarChar));
            oCmd.Parameters["P_SQ_INV"].Value = P_SQ_INV;

            dal.ExecuteNonQuery(oCmd);

        }

        public DataTable Get_FMSC_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_FMSC.P_GET_SP_TO_EBA";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void Get_FMSC_UPDATE(string P_CD_TYPE, string P_ID, string P_EBA_ID)
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_FMSC.P_SET_SP_STATU";

            oCmd.Parameters.Add(new OracleParameter("P_TYPE", OracleType.VarChar));
            oCmd.Parameters["P_TYPE"].Value = P_CD_TYPE;

            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = P_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            dal.ExecuteNonQuery(oCmd);

        }

        public DataTable Get_TOSHOLD_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_TOSHOLD.V_HOLD_WAITINGEBAFLOW";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void Get_TOSHOLD_UPDATE(string p_sq_hold_rule_id, string p_cd_eba_status)
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_TOSHOLD.p_set_eba_status";

            oCmd.Parameters.Add(new OracleParameter("p_sq_hold_rule_id", OracleType.VarChar));
            oCmd.Parameters["p_sq_hold_rule_id"].Value = p_sq_hold_rule_id;

            oCmd.Parameters.Add(new OracleParameter("p_cd_eba_status", OracleType.VarChar));
            oCmd.Parameters["p_cd_eba_status"].Value = p_cd_eba_status;


            dal.ExecuteNonQuery(oCmd);

        }


        public DataTable Get_IMS_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IMS.P_IMS_REQUEST_LIST";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void Get_IMS_UPDATE(string P_ID, string P_EBA_ID, string P_CD_MODIFIER_EBA)
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IMS.P_UPD_REQUEST_EBA_ID";

            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = P_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_CD_MODIFIER_EBA", OracleType.VarChar));
            oCmd.Parameters["P_CD_MODIFIER_EBA"].Value = P_CD_MODIFIER_EBA;


            dal.ExecuteNonQuery(oCmd);

        }

        public DataTable Get_KPS_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_KPS.P_KPS_LIST";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void Get_KPS_UPDATE(string P_CD_FLOW_TYPE, string P_ID_EBA_PROCESS, string P_ID_KPS_HDR)
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_KPS.PRC_EBA_START";

            oCmd.Parameters.Add(new OracleParameter("P_CD_FLOW_TYPE", OracleType.VarChar));
            oCmd.Parameters["P_CD_FLOW_TYPE"].Value = P_CD_FLOW_TYPE;

            oCmd.Parameters.Add(new OracleParameter("P_ID_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_ID_EBA_PROCESS"].Value = P_ID_EBA_PROCESS;

            oCmd.Parameters.Add(new OracleParameter("P_ID_KPS_HDR", OracleType.VarChar));
            oCmd.Parameters["P_ID_KPS_HDR"].Value = P_ID_KPS_HDR;


            dal.ExecuteNonQuery(oCmd);

        }


        public DataTable Get_ITHALAT_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ITHALAT.P_ITHALAT_LIST";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public void Get_ITHALAT_UPDATE(string p_eba_no, string p_grup_id, string p_statu)
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ITHALAT.PRC_EBA_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("p_eba_no", OracleType.VarChar));
            oCmd.Parameters["p_eba_no"].Value = p_eba_no;

            oCmd.Parameters.Add(new OracleParameter("p_grup_id", OracleType.VarChar));
            oCmd.Parameters["p_grup_id"].Value = p_grup_id;

            oCmd.Parameters.Add(new OracleParameter("p_statu", OracleType.VarChar));
            oCmd.Parameters["p_statu"].Value = p_statu;


            dal.ExecuteNonQuery(oCmd);

        }

        public void UPDATE_SMR3(string ticked_id, string ticketType, string pid)
        {
            string globalid = GLOBALID(pid);
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SMR_SERVICE.PRC_SET_EBA_ID";
            oCmd.Parameters.Add(new OracleParameter("P_ID_TICKET", OracleType.VarChar));
            oCmd.Parameters["P_ID_TICKET"].Value = ticked_id;
            oCmd.Parameters.Add(new OracleParameter("P_CD_TICKET_TYPE", OracleType.VarChar));
            oCmd.Parameters["P_CD_TICKET_TYPE"].Value = ticketType;
            oCmd.Parameters.Add(new OracleParameter("P_ID_EBA", OracleType.VarChar));
            oCmd.Parameters["P_ID_EBA"].Value = globalid;

            dal.ExecuteNonQuery(oCmd);
        }

        public string GLOBALID(string pid)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SMR_SERVICE.P_GID";
            oCmd.Parameters.Add(new OracleParameter("PID", OracleType.VarChar));
            oCmd.Parameters["PID"].Value = pid;
            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;
            DataTable dt = dal.ExecuteDataTable(oCmd);
            return dt.Rows[0]["GID"].ToString();

        }

        public void UPDATE_UYDGDA3(string GID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_UYDGDA.P_UPDATE3";

            oCmd.Parameters.Add(new OracleParameter("FID", OracleType.VarChar));
            oCmd.Parameters["FID"].Value = GID;

            dal.ExecuteNonQuery(oCmd);
        }

        public void MUFTON_P_InserteBA3()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_MUFTON.P_INSERTS_EBA3";
            dal.ExecuteNonQuery(oCmd);

        }

        #endregion

        #region Eba3 To Eba5 Geçiş 2015

        #region Fats (SBKYHF)

        public DataTable getFatsTrigRecords()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SBKYHF.P_EBA3_READ";

            cmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            cmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(cmd);
        }

        public void fatsUpdate(string fatsID)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SBKYHF.P_EBA3_UPDATE";
            cmd.Parameters.Add(new OracleParameter("P_FATSID", OracleType.VarChar));
            cmd.Parameters["P_FATSID"].Value = fatsID;
            cmd.Parameters.Add(new OracleParameter("STATU", OracleType.VarChar));
            cmd.Parameters["STATU"].Value = "B";
            dal.ExecuteNonQuery(cmd);
        }

        public void receiveFatsUpdate()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SBKYHF.P_RECEIVE_TO_EBA3";
            dal.ExecuteNonQuery(oCmd);
        }

        #endregion;

        #endregion

        #region ServiceErrorControl

        ///<summary>
        ///Son 24 saat içerisinde log kayıtları üzerinde hata kaydını kontrol eder.
        ///</summary> 
        public bool GetServisErrorControl(string ticket, string type)
        {
            bool control = true;

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EWS_LOG.P_CONTROL";
                cmd.Parameters.Add(new OracleParameter("P_TICKET", OracleType.VarChar));
                cmd.Parameters["P_TICKET"].Value = ticket;
                cmd.Parameters.Add(new OracleParameter("P_TYPE", OracleType.VarChar));
                cmd.Parameters["P_TYPE"].Value = type;
                cmd.Parameters.Add(new OracleParameter("P_COUNT", OracleType.VarChar, 10));
                cmd.Parameters["P_COUNT"].Direction = ParameterDirection.Output;

                //dal.ExecuteNonQuery(cmd);
                int satir = dal.ExecuteNonQuery(cmd);              

                log.WriteLine("DEBUG","P_COUNT :"+ Convert.ToInt32(cmd.Parameters["P_COUNT"].Value.ToString()));
                log.WriteLine("DEBUG", "satir :" + satir); 
                log.WriteLine("DEBUG", "P_TICKET :" + Convert.ToInt32(cmd.Parameters["P_TICKET"].Value.ToString()));
                log.WriteLine("DEBUG", "P_TYPE :" + (cmd.Parameters["P_TYPE"].Value.ToString()));

                if (Convert.ToInt32(cmd.Parameters["P_COUNT"].Value.ToString()) > 0)
                    control = false;


            }
            catch (Exception Ex)
            {
                control = true;
            }

            //log.WriteLine("DEBUG", "control :" + control.ToString());
            return control;

        }

        public DataTable GetFlowDocuments(string ProcessID)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format("SELECT PROCESSID, FILEPROFILEID  FROM EBAWORKFLOW5.FLOWDOCUMENTS WHERE PROCESSID = {0}", ProcessID);

            return dal.ExecuteDataTable(cmd);
        }

        public DataTable GetInvalidStep()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_EWS_LOG.P_INVALID_CONTROL";

            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        #endregion

        #region Eskiler

        public DataTable SelectNewTicketsForEba(string ticketType)
        {

            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE.GET_NEW_TICKET_FOR_EBA_LIST";

            oCmd.Parameters.Add(new OracleParameter("P_TICKET_TYPE", OracleType.VarChar, 2));
            oCmd.Parameters["P_TICKET_TYPE"].Value = ticketType;

            oCmd.Parameters.Add(new OracleParameter("P_RefCursor", OracleType.Cursor));
            oCmd.Parameters["P_RefCursor"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);

        }

        public string InsertNewEbaForm(string idTicket, int P_id)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE.INSERT_NEW_EBA_FORM";

            oCmd.Parameters.Add(new OracleParameter("P_TICKET_ID", OracleType.Number, 10));
            oCmd.Parameters["P_TICKET_ID"].Value = Convert.ToInt32(idTicket);

            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.Number, 10));
            oCmd.Parameters["P_ID"].Value = Convert.ToInt32(P_id);


            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar, 10));
            oCmd.Parameters["P_EBA_ID"].Direction = ParameterDirection.Output;

            dal.ExecuteNonQuery(oCmd);

            return oCmd.Parameters["P_EBA_ID"].Value.ToString();

        }

        public DataTable GetFlowNotStartList(string ticketType)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE.GET_FLOW_NOT_START_LIST";

            oCmd.Parameters.Add(new OracleParameter("P_TICKET_TYPE", OracleType.VarChar, 2));
            oCmd.Parameters["P_TICKET_TYPE"].Value = ticketType;

            oCmd.Parameters.Add(new OracleParameter("P_RefCursor", OracleType.Cursor));
            oCmd.Parameters["P_RefCursor"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        private DataTable SelectTicketsApproveFromEbaAll(string ticketType)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE.GET_TICKETS_APPROVE_FROM_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_TICKET_TYPE", OracleType.VarChar, 2));
            oCmd.Parameters["P_TICKET_TYPE"].Value = ticketType;

            oCmd.Parameters.Add(new OracleParameter("P_RefCursor", OracleType.Cursor));
            oCmd.Parameters["P_RefCursor"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        private DataTable SelectTicketsApproveFromEbaDA()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE_DA.GET_TICKETS_APPROVE_FROM_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_RefCursor", OracleType.Cursor));
            oCmd.Parameters["P_RefCursor"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public DataTable SelectTicketsApproveFromEba(string ticketType)
        {

            return SelectTicketsApproveFromEbaAll(ticketType);

        }

        private int UpdateTicketApproveFromEbaAll(string ticketType, string idEba)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE.UPDATE_TICKET_APPROVE_FROM_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_TICKET_TYPE", OracleType.VarChar, 2));
            oCmd.Parameters["P_TICKET_TYPE"].Value = ticketType;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar, 10));
            oCmd.Parameters["P_EBA_ID"].Value = idEba;

            return dal.ExecuteNonQuery(oCmd);
        }

        private int UpdateTicketApproveFromEbaDA(string ticketType, string idEba)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE_DA.UPDATE_TICKET_APPROVE_FROM_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_DA_EBA_ID", OracleType.VarChar, 10));
            oCmd.Parameters["P_DA_EBA_ID"].Value = idEba;

            return dal.ExecuteNonQuery(oCmd);
        }

        public int UpdateTicketApproveFromEba(string ticketType, string idEba)
        {

            return UpdateTicketApproveFromEbaAll(ticketType, idEba);
        }

        public void CloseDefectiveTickets()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE.CLOSE_DEFECTIVE_TICKETS";

            dal.ExecuteNonQuery(oCmd);
        }

        public void UpdateDifferentAssignedTo()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA_APPROVE.UPDATE_DIFFERENT_ASSIGNED_TO";

            dal.ExecuteNonQuery(oCmd);
        }

        public int RecoveryTicketsFromEba(string ticketType, string idEba)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE.RECOVERY_TICKETS_FROM_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_TICKET_TYPE", OracleType.VarChar, 2));
            oCmd.Parameters["P_TICKET_TYPE"].Value = ticketType;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar, 10));
            oCmd.Parameters["P_EBA_ID"].Value = idEba;

            return dal.ExecuteNonQuery(oCmd);
        }

        public DataTable GetFlowNotStartListDA()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE_DA.GET_FLOW_NOT_START_LIST";

            oCmd.Parameters.Add(new OracleParameter("P_RefCursor", OracleType.Cursor));
            oCmd.Parameters["P_RefCursor"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public int RecoveryTicketsFromEbaDA(string idEbaDA)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "HELPDESK.PCK_EBA5_APPROVE_DA.RECOVERY_TICKETS_FROM_EBA";

            oCmd.Parameters.Add(new OracleParameter("P_DA_EBA_ID", OracleType.VarChar, 10));
            oCmd.Parameters["P_DA_EBA_ID"].Value = idEbaDA;

            dal.TransactionBegin();
            try
            {
                int ii = dal.ExecuteNonQuery(oCmd);
                dal.TransactionCommit();
                return ii;
            }
            catch (Exception ex)
            {
                dal.TransactionRollback();
                throw ex;
            }
        }

        #endregion

        public DataTable Get_SAPODFRM_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SAP.PRC_GET_ODFRM";

            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public DataTable Get_SAPODFRM_PERSONEL_TrigRecords(string Id)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SAP.PRC_GET_ODFRM_PERSONEL";

            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = Id;

            oCmd.Parameters.Add(new OracleParameter("RC_DATA", OracleType.Cursor));
            oCmd.Parameters["RC_DATA"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Get_SAPODFRM_SAP_Send(string Id, string Update)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SAP.PRC_SET_ODFRM_SAP_SEND";

            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = Id;

            oCmd.Parameters.Add(new OracleParameter("P_UPDATE", OracleType.VarChar));
            oCmd.Parameters["P_UPDATE"].Value = Update;

            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_ATF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATF.PRC_GET_ARACTALEP_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATF.PRC_ARACTALEP_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATF_STATU_Update(string P_UNIQUE_ID, string P_EBA_NO, string P_CD_STATUS)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATF.PRC_ARACTALEP_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_NO", OracleType.VarChar));
            oCmd.Parameters["P_EBA_NO"].Value = P_EBA_NO;

            oCmd.Parameters.Add(new OracleParameter("P_CD_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_CD_STATUS"].Value = P_CD_STATUS;

            dal.ExecuteNonQuery(oCmd);
        }

        #region ATS Sistemi Formları

        public DataTable Get_ATSTEMINAT_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TEMINAT.PRC_GET_TEMINAT_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATSTEMINAT_FORM_Update(string P_SEQ_TEMINAT_HAREKET, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TEMINAT.PRC_GET_TEMINAT_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SEQ_TEMINAT_HAREKET", OracleType.VarChar));
            oCmd.Parameters["P_SEQ_TEMINAT_HAREKET"].Value = P_SEQ_TEMINAT_HAREKET;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;

            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATSTEMINAT_STATU_Update(string P_CD_EBA_STATUS, string P_SEQ_TEMINAT, string P_MODIFFIER, string P_KAYIT_TIPI, string P_ID_EBA_PROCESS)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TEMINAT.PRC_TEMINAT_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_CD_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_CD_EBA_STATUS"].Value = P_CD_EBA_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_SEQ_TEMINAT", OracleType.VarChar));
            oCmd.Parameters["P_SEQ_TEMINAT"].Value = P_SEQ_TEMINAT;

            oCmd.Parameters.Add(new OracleParameter("P_MODIFFIER", OracleType.VarChar));
            oCmd.Parameters["P_MODIFFIER"].Value = P_MODIFFIER;

            oCmd.Parameters.Add(new OracleParameter("P_KAYIT_TIPI", OracleType.VarChar));
            oCmd.Parameters["P_KAYIT_TIPI"].Value = P_KAYIT_TIPI;

            oCmd.Parameters.Add(new OracleParameter("P_ID_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_ID_EBA_PROCESS"].Value = P_ID_EBA_PROCESS;

            dal.ExecuteNonQuery(oCmd);
        }


        public DataTable Get_ATSLIMIT_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_ELIOF.PRC_GET_EKLIMIT_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATSLIMIT_FORM_Update(string SQ_LIMIT, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_ELIOF.PRC_EKLIMIT_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SQ_LIMIT_HAREKET", OracleType.VarChar));
            oCmd.Parameters["P_SQ_LIMIT_HAREKET"].Value = SQ_LIMIT;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;

            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATSLIMIT_STATU_Update(string P_EBA_STATUS, string P_SQ_LIMIT, string P_MODIFFIER, string P_CKAYIT_TIP, string P_ID_EBA_PROCESS)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_ELIOF.PRC_EKLIMIT_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATUS"].Value = P_EBA_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_SQ_LIMIT", OracleType.VarChar));
            oCmd.Parameters["P_SQ_LIMIT"].Value = P_SQ_LIMIT;

            oCmd.Parameters.Add(new OracleParameter("P_MODIFFIER", OracleType.VarChar));
            oCmd.Parameters["P_MODIFFIER"].Value = P_MODIFFIER;

            oCmd.Parameters.Add(new OracleParameter("P_CKAYIT_TIP", OracleType.VarChar));
            oCmd.Parameters["P_CKAYIT_TIP"].Value = P_CKAYIT_TIP;

            oCmd.Parameters.Add(new OracleParameter("P_ID_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_ID_EBA_PROCESS"].Value = P_ID_EBA_PROCESS;

            dal.ExecuteNonQuery(oCmd);
        }


        public DataTable Get_ATSFATURAVADE_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_FVIOF.PRC_GET_FATURAVADE_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATSFATURAVADE_FORM_Update(string P_INV_NO, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_FVIOF.PRC_FATURAVADE_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_INV_NO", OracleType.VarChar));
            oCmd.Parameters["P_INV_NO"].Value = P_INV_NO;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATSFATURAVADE_STATU_Update(string INV_NO, string ID_EBA_PROCESS, string EBA_STATUS, string EBA_USERID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_FVIOF.PRC_FATURAVADE_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_CD_INV_NO", OracleType.VarChar));
            oCmd.Parameters["P_CD_INV_NO"].Value = INV_NO;

            oCmd.Parameters.Add(new OracleParameter("P_ID_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_ID_EBA_PROCESS"].Value = ID_EBA_PROCESS;

            oCmd.Parameters.Add(new OracleParameter("P_CD_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_CD_EBA_STATUS"].Value = EBA_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_USERID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_USERID"].Value = EBA_USERID;

            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_ATSHAVALE_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_HTIOF.PRC_GET_HTIOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATSHAVALE_FORM_Update(string P_REF, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_HTIOF.PRC_GET_HTIOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_REF", OracleType.VarChar));
            oCmd.Parameters["P_REF"].Value = P_REF;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATSHAVALE_STATU_Update(string P_REF, string P_CD_EBA_STATUS, string P_MODIFFIER)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_HTIOF.PRC_HTIOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_REF", OracleType.VarChar));
            oCmd.Parameters["P_REF"].Value = P_REF;

            oCmd.Parameters.Add(new OracleParameter("P_CD_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_CD_EBA_STATUS"].Value = P_CD_EBA_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_MODIFFIER", OracleType.VarChar));
            oCmd.Parameters["P_MODIFFIER"].Value = P_MODIFFIER;

            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_ATSFATURAIPTAL_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_FIIOF.PRC_GET_FIIOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATSFATURAIPTAL_FORM_Update(string P_INV_NO, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_FIIOF.PRC_GET_FIIOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_INV_NO", OracleType.VarChar));
            oCmd.Parameters["P_INV_NO"].Value = P_INV_NO;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATSFATURAIPTAL_STATU_Update(string P_INV_NO, string P_PROCESSID, string P_EBA_STATUS, string P_MODIFFIER)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_FIIOF.PRC_FIIOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_INV_NO", OracleType.VarChar));
            oCmd.Parameters["P_INV_NO"].Value = P_INV_NO;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESSID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATUS"].Value = P_EBA_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_MODIFFIER", OracleType.VarChar));
            oCmd.Parameters["P_MODIFFIER"].Value = P_MODIFFIER;


            dal.ExecuteNonQuery(oCmd);
        }



        public DataTable Get_ATS_STANDARTVADE_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_SVIOF.PRC_GET_STANDARTVADE_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATS_STANDARTVADE_FORM_Update(string P_ID_TERM, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_SVIOF.PRC_STANDARTVADE_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_ID_TERM", OracleType.VarChar));
            oCmd.Parameters["P_ID_TERM"].Value = P_ID_TERM;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATS_STANDARTVADE_STATU_Update(string P_IDTERM, string P_STATUS, string P_EBANO)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_SVIOF.PRC_STANDARTVADE_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_IDTERM", OracleType.VarChar));
            oCmd.Parameters["P_IDTERM"].Value = P_IDTERM;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBANO", OracleType.VarChar));
            oCmd.Parameters["P_EBANO"].Value = P_EBANO;

            dal.ExecuteNonQuery(oCmd);
        }


        public DataTable Get_FTRUCKFATURA_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ECFRM.PRC_GET_FORM";

            oCmd.Parameters.Add(new OracleParameter("P_CURSOR", OracleType.Cursor));
            oCmd.Parameters["P_CURSOR"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }


        public void Set_FtruckFatura_FORM_Update(string P_FORM_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ECFRM.PRC_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_FORM_ID", OracleType.Number));
            oCmd.Parameters["P_FORM_ID"].Value = P_FORM_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;

            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_FTRUCKFATURA_STATU_Update(string FORM_ID, string P_STATUS)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ECFRM.PRC_SET_FORM_STATU";

            oCmd.Parameters.Add(new OracleParameter("P_CD_FORM_ID", OracleType.Number));
            oCmd.Parameters["P_CD_FORM_ID"].Value = FORM_ID;

            oCmd.Parameters.Add(new OracleParameter("P_CD_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_CD_STATUS"].Value = P_STATUS;

            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_ATS_MUSTERIFATURA_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_MFIOF.PRC_GET_MUSTERIFATURA_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATS_MUSTERIFATURA_FORM_Update(string P_SQ_MUSTERI, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_MFIOF.PRC_MUSTERIFATURA_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SQ_MUSTERI", OracleType.VarChar));
            oCmd.Parameters["P_SQ_MUSTERI"].Value = P_SQ_MUSTERI;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATS_MUSTERIFATURA_STATU_Update(string P_SQ_MUSTERI, string P_EBA_STATU, string P_EBA_NO)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_MFIOF.PRC_MUSTERIFATURA_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SQ_MUSTERI", OracleType.VarChar));
            oCmd.Parameters["P_SQ_MUSTERI"].Value = P_SQ_MUSTERI;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATU", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATU"].Value = P_EBA_STATU;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_NO", OracleType.VarChar));
            oCmd.Parameters["P_EBA_NO"].Value = P_EBA_NO;

            dal.ExecuteNonQuery(oCmd);
        }
        public DataTable Get_IATS_VADEGUNCELLEME_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_IATS_EXPIRY.PRC_GET_FATURAVADE_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public DataTable Get_ATS_TOPLUVADEGUNCELLEME_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TFVGIOF.PRC_GET_TOPLUFATURAVADE_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATS_TOPLUVADEGUNCELLEME_FORM_Update(string P_GROUP_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TFVGIOF.PRC_TOPLUFATURAVADE_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_GROUP_ID", OracleType.VarChar));
            oCmd.Parameters["P_GROUP_ID"].Value = P_GROUP_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATS_TOPLUVADEGUNCELLEME_STATU_Update(string P_GROUP_ID, string P_EBA_NO, string P_EBA_STATU, string P_EBA_USER)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TFVGIOF.PRC_TOPLUFATURAVADE_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_GROUP_ID", OracleType.VarChar));
            oCmd.Parameters["P_GROUP_ID"].Value = P_GROUP_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_NO", OracleType.VarChar));
            oCmd.Parameters["P_EBA_NO"].Value = P_EBA_NO;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATU", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATU"].Value = P_EBA_STATU;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_USER", OracleType.VarChar));
            oCmd.Parameters["P_EBA_USER"].Value = P_EBA_USER;


            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_ATS_SPIDSVADE_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_SPIDSIOF.PRC_GET_SPIDSVADE_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATS_SPIDSVADE_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_SPIDSIOF.PRC_SPIDSVADE_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATS_SPIDSVADE_STATU_Update(string P_CD_EBA_ID, string P_CD_UNIQUE_ID, string P_EBA_STATU)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_SPIDSIOF.PRC_SPIDSVADE_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_CD_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_CD_EBA_ID"].Value = P_CD_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_CD_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_CD_UNIQUE_ID"].Value = P_CD_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_CD_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_CD_STATUS"].Value = P_EBA_STATU;

            dal.ExecuteNonQuery(oCmd);
        }



        public DataTable Get_ATS_TGIOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TGIOF.PRC_GET_TAHSILAT_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_ATS_TGIOF_FORM_Update(string P_ID_NO, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TGIOF.PRC_TAHSILAT_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_ID_NO", OracleType.VarChar));
            oCmd.Parameters["P_ID_NO"].Value = P_ID_NO;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATS_TGIOF_STATU_Update(int P_ID_NO, string P_EBA_STATUS, string P_PROCESSID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_TGIOF.PRC_TAHSILAT_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_ID_NO", OracleType.Number));
            oCmd.Parameters["P_ID_NO"].Value = P_ID_NO;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATUS"].Value = P_EBA_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESSID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESSID"].Value = P_PROCESSID;

            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_IATS_TOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IATSTOF.PRC_GET_IATS_TEMINAT_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_IATS_TOF_FORM_Update(string P_SQ_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IATSTOF.PRC_IATS_TEMINAT_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SQ_ID", OracleType.VarChar));
            oCmd.Parameters["P_SQ_ID"].Value = P_SQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_IATS_TOF_STATU_Update(string P_ID_NO, string P_EBA_STATUS, string P_PROCESSID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_IATSTOF.PRC_IATS_TEMINAT_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_ID_NO;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATUS"].Value = P_EBA_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_REQUEST_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_REQUEST_ID"].Value = P_PROCESSID;

            dal.ExecuteNonQuery(oCmd);
        }
        public DataTable Get_RAWSYS_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_RAWSYS.PRC_GET_RAWSYS_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }
        public DataTable Get_RAWSYS2_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_RAWSYS.PRC_GET_RAWSYS_START2";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }
        public DataTable Get_IATSTahsilat_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_IATS.PRC_GET_RAWSYS_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }
        public DataTable Get_ATS_NCS_BO_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_NCS_BO.PRC_GET_BAYIONAY_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }



        public void Set_ATS_N_RAWSYS_FORM_Update(string P_SQ_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_RAWSYS.PRC_EBA_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("SQ_ID", OracleType.VarChar));
            oCmd.Parameters["SQ_ID"].Value = P_SQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_PROCESS"].Value = P_PROCESS_ID;

            dal.ExecuteNonQuery(oCmd);
        }
        public void Set_IATS_Tahsilat_FORM_Update(string P_SQ_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_IATS.PRC_EBA_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("SQ_EBA_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["SQ_EBA_PROCESS_ID"].Value = P_SQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_PROCESS"].Value = P_PROCESS_ID;

            dal.ExecuteNonQuery(oCmd);
        }
        public void Set_ATS_N_RAWSYS2_FORM_Update(string P_SQ_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_RAWSYS.PRC_EBA_FORM_UPDATE2";

            oCmd.Parameters.Add(new OracleParameter("SQ_ID", OracleType.VarChar));
            oCmd.Parameters["SQ_ID"].Value = P_SQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_PROCESS"].Value = P_PROCESS_ID;

            dal.ExecuteNonQuery(oCmd);
        }
        public void Set_ATS_NCS_BO_FORM_Update(string P_SQ_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_NCS_BO.PRC_EBA_ATS_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_APPROVAL_ID", OracleType.VarChar));
            oCmd.Parameters["P_APPROVAL_ID"].Value = P_SQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_PROCESS"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }


        public DataTable Get_N_VADUZ_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_N_VADUZT.PRC_GET_LIFO_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public DataTable Get_N_VADUZTContinueProcess_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_N_VADUZT.PRC_GET_LIFO_IPTAL";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }


        public DataTable Get_N_VADUZTREQID_TrigRecords(string LIFO_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_LFMTK.PRC_GET_REQID";

            oCmd.Parameters.Add(new OracleParameter("P_LIFO_ID", OracleType.VarChar));
            oCmd.Parameters["P_LIFO_ID"].Value = LIFO_ID;
            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }



        public DataTable Get_ATS_DBSFTOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_DBSFTOF.PRC_GET_DBSFTOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }
        public void Set_ATS_DBSFTOF_FORM_Update(string P_SQ_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_DBSFTOF.PRC_DBSFTOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SQ_ATS_BULK", OracleType.VarChar));
            oCmd.Parameters["P_SQ_ATS_BULK"].Value = P_SQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_PROCESS"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_ATS_DBSFTOF_STATU_Update(string P_ID_NO, string P_EBA_STATUS, string P_PROCESSID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_ATS_DBSFTOF.PRC_DBSFTOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SQ_ATS_BULK", OracleType.VarChar));
            oCmd.Parameters["P_SQ_ATS_BULK"].Value = P_ID_NO;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATUS"].Value = P_EBA_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_PROCESS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_PROCESS"].Value = P_PROCESSID;

            dal.ExecuteNonQuery(oCmd);
        }

        #endregion




        public DataTable Get_LFMTK_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_LFMTK.PRC_GET_LIFO_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        //todo: 

        public DataTable Get_TFSOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_TFSOF.TETIKLENECEK_SURECLER";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public DataTable Get_LFMTKContinueProcess_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_LFMTK.PRC_GET_LIFO_IPTAL";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }
        public DataTable Get_LFMTKREQID_TrigRecords(string LIFO_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_LFMTK.PRC_GET_REQID";

            oCmd.Parameters.Add(new OracleParameter("P_LIFO_ID", OracleType.VarChar));
            oCmd.Parameters["P_LIFO_ID"].Value = LIFO_ID;

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }


        public DataTable Get_GPS_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_GPS.P_GPS_LIST";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_GPS_FORM_Update(int P_PROCESS_ID, int P_ID_PROCESS_TEMP)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_GPS.UPDATE_FORM";

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.Number));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;

            oCmd.Parameters.Add(new OracleParameter("P_ID_PROCESS_TEMP", OracleType.Number));
            oCmd.Parameters["P_ID_PROCESS_TEMP"].Value = P_ID_PROCESS_TEMP;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_GPS_STATU_Update(int P_KPS_HDR_ID, int P_ID_EBA, string P_STATUS, int P_ID_PROCESS_TEMP)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_GPS.PRC_HDR_APPROVE";

            oCmd.Parameters.Add(new OracleParameter("P_KPS_HDR_ID", OracleType.Number));
            oCmd.Parameters["P_KPS_HDR_ID"].Value = P_KPS_HDR_ID;

            oCmd.Parameters.Add(new OracleParameter("P_ID_EBA", OracleType.Number));
            oCmd.Parameters["P_ID_EBA"].Value = P_ID_EBA;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_ID_PROCESS_TEMP", OracleType.Number));
            oCmd.Parameters["P_ID_PROCESS_TEMP"].Value = P_ID_PROCESS_TEMP;

            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_DOFOOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_DOFOOF.PRC_GET_IDA_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_DOFOOF_FORM_Update(int P_PROCESS_ID, string P_IDRECONDEBTHEADER)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_DOFOOF.PRC_IDA_FORM_UPDATE";


            oCmd.Parameters.Add(new OracleParameter("P_IDRECONDEBTHEADER", OracleType.VarChar));
            oCmd.Parameters["P_IDRECONDEBTHEADER"].Value = P_IDRECONDEBTHEADER;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.Number));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_DOFOOF_STATU_Update(string P_ID_RECON_DEBT_HEADER, string P_EBA_ID, string P_STATUS)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_DOFOOF.PRC_HDR_APPROVE";

            oCmd.Parameters.Add(new OracleParameter("P_ID_RECON_DEBT_HEADER", OracleType.VarChar));
            oCmd.Parameters["P_ID_RECON_DEBT_HEADER"].Value = P_ID_RECON_DEBT_HEADER;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_REJECT_REASON", OracleType.VarChar));
            oCmd.Parameters["P_REJECT_REASON"].Value = "";

            oCmd.Parameters.Add("P_REC", OracleType.Cursor).Direction = ParameterDirection.Output;


            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_SPIDS_MLOA_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_MLOA.PRC_GET_SPIDS_MLOA_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_MLOA_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_MLOA.PRC_SPIDS_MLOA_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_MLOA_STATU_Update(int P_UNIQ_ID, string P_STATUS, int P_EBA_ID, string P_EBA_TITLE)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_MLOA.PRC_SPIDS_MLOA_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.Number));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.Number));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TITLE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TITLE"].Value = P_EBA_TITLE;


            dal.ExecuteNonQuery(oCmd);
        }



        public DataTable Get_SPIDS_FGOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_FGOF.PRC_GET_SPIDS_FGOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_FGOF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_FGOF.PRC_SPIDS_FGOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_FGOF_STATU_Update(int P_UNIQ_ID, string P_STATUS, int P_EBA_ID, string P_EBA_TITLE)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_FGOF.PRC_SPIDS_FGOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.Number));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.Number));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TITLE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TITLE"].Value = P_EBA_TITLE;


            dal.ExecuteNonQuery(oCmd);
        }



        public DataTable Get_SPIDS_SLOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_SLOF.PRC_GET_SPIDS_SLOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_SLOF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_SLOF.PRC_SPIDS_SLOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_SLOF_STATU_Update(int P_UNIQ_ID, string P_STATUS, int P_EBA_ID, string P_EBA_TITLE)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_SLOF.PRC_SPIDS_SLOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.Number));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.Number));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TITLE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TITLE"].Value = P_EBA_TITLE;


            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_SPIDS_CKGOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_CKGOF.PRC_GET_SPIDS_CKGOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_CKGOF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_CKGOF.PRC_SPIDS_CKGOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_CKGOF_STATU_Update(int P_UNIQ_ID, string P_STATUS, int P_EBA_ID, string P_EBA_TITLE)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_CKGOF.PRC_SPIDS_CKGOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.Number));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.Number));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TITLE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TITLE"].Value = P_EBA_TITLE;


            dal.ExecuteNonQuery(oCmd);
        }



        public DataTable Get_PBOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_PBOF.PRC_GET_PBOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_PBOF_FORM_Update(string P_TICKET_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_PBOF.PRC_PBOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_TICKET_ID", OracleType.VarChar));
            oCmd.Parameters["P_TICKET_ID"].Value = P_TICKET_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_PBOF_STATU_Update(string P_TICKET_ID, string P_DECISION_MMB_CODE, string P_DESCRIPTION_OF_PROBLEM, string P_EBA_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_PBOF.PRC_PBOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_TICKET_ID", OracleType.VarChar));
            oCmd.Parameters["P_TICKET_ID"].Value = P_TICKET_ID;

            oCmd.Parameters.Add(new OracleParameter("P_DECISION_MMB_CODE", OracleType.VarChar));
            oCmd.Parameters["P_DECISION_MMB_CODE"].Value = P_DECISION_MMB_CODE;

            oCmd.Parameters.Add(new OracleParameter("P_DESCRIPTION_OF_PROBLEM", OracleType.VarChar));
            oCmd.Parameters["P_DESCRIPTION_OF_PROBLEM"].Value = P_DESCRIPTION_OF_PROBLEM;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;


            dal.ExecuteNonQuery(oCmd);
        }





        public DataTable Get_CAPEX_DOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_CAPEX_DOF.PRC_GET_CAPEX_DOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }


        public DataTable Get_EBA_ID(string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_CAPEX_DOF.PRC_GET_EBA_ID";

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_CAPEX_DOF_FORM_Update(string P_ID, int P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_CAPEX_DOF.PRC_CAPEX_DOF_FORM_UPDATE";


            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = P_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.Number));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_CAPEX_DOF_STATU_Update(string P_ID, string P_EBA_ID, string P_STATUS, string P_REJECT)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_CAPEX_DOF.PRC_CAPEX_DOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = P_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATUS"].Value = P_STATUS;


            oCmd.Parameters.Add(new OracleParameter("p_eba_reject", OracleType.VarChar));
            oCmd.Parameters["p_eba_reject"].Value = P_REJECT;


            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_BUYBACK_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_BUYBACK.PRC_GET_BUYBACK_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_BUYBACK_FORM_Update(string P_ID, int P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_BUYBACK.PRC_BUYBACK_FORM_UPDATE";


            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = P_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.Number));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_BUYBACK_STATU_Update(string P_ID, string P_EBA_ID, string P_STATUS)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_BUYBACK.PRC_BUYBACK_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_ID", OracleType.VarChar));
            oCmd.Parameters["P_ID"].Value = P_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_EBA_STATUS"].Value = P_STATUS;

            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_SPIDS_YPTYDOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPTYDOF.PRC_GET_SPIDS_YPTYDOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_YPTYDOF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPTYDOF.PRC_SPIDS_YPTYDOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_YPTYDOF_STATU_Update(int P_UNIQ_ID, string P_STATUS, int P_EBA_ID, string P_EBA_TITLE)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPTYDOF.PRC_SPIDS_YPTYDOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.Number));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.Number));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TITLE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TITLE"].Value = P_EBA_TITLE;


            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_SPIDS_YPKKOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKKOF.PRC_GET_SPIDS_YPKKOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_YPKKOF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKKOF.PRC_SPIDS_YPKKOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_YPKKOF_STATU_Update(int P_UNIQ_ID, string P_STATUS, int P_EBA_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKKOF.PRC_SPIDS_YPKKOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.Number));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.Number));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;


            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_SPIDS_YPKPTOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_YPKPTOF.PRC_GET_SPIDS_YPKPTOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_YPKPTOF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_YPKPTOF.PRC_SPIDS_YPKPTOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_YPKPTOF_STATU_Update(string P_USER, string P_STATUS, string P_UNIQ_ID, string P_EBA_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_YPKPTOF.PRC_SPIDS_YPKPTOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_USER", OracleType.VarChar));
            oCmd.Parameters["P_USER"].Value = P_USER;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;


            dal.ExecuteNonQuery(oCmd);
        }



        public void PARAMETER_USER_DELETE()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PRC_PARAMETER_USER_DELETE";

            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_NCS_ITHP_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_ITHP.PRC_GET_NCS_ITHP_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_NCS_ITHP_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_NCS_ITHP.PRC_NCS_ITHP_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SOS_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }


        public DataTable Get_SPIDS_KOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_KOF.PRC_GET_SPIDS_KOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_KOF_FORM_Update(int P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_KOF.PRC_SPIDS_KOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.Number, 2000));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_KOF_STATU_Update(string P_USER, string P_STATUS, string P_UNIQ_ID, string P_EBA_ID, string P_SW_CHECK)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_KOF.PRC_SPIDS_KOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_USER", OracleType.VarChar));
            oCmd.Parameters["P_USER"].Value = P_USER;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_SW_CHCK", OracleType.VarChar));
            oCmd.Parameters["P_SW_CHCK"].Value = P_SW_CHECK;



            dal.ExecuteNonQuery(oCmd);
        }


        public DataTable Get_SPIDS_YPKSDOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKSDOF.PRC_GET_SPIDS_YPKSDOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_YPKSDOF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKSDOF.PRC_SPIDS_YPKSDOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_YPKSDOF_STATU_Update(string P_EBA_ID, string P_STATUS, int P_UNIQ_ID, string P_EBA_TITLE)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKSDOF.PRC_SPIDS_YPKSDOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.Number, 2000));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_TITLE", OracleType.VarChar));
            oCmd.Parameters["P_EBA_TITLE"].Value = P_EBA_TITLE;


            dal.ExecuteNonQuery(oCmd);
        }




        public DataTable Get_SPIDS_YPKTOF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKTOF.PRC_GET_SPIDS_YPKTOF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_SPIDS_YPKTOF_FORM_Update(string P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKTOF.PRC_SPIDS_YPKTOF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_SPIDS_YPKTOF_STATU_Update(string P_USER, string P_STATUS, string P_UNIQ_ID, string P_EBA_ID, string P_SW_CHECK)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKTOF.PRC_SPIDS_YPKTOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_USER", OracleType.VarChar));
            oCmd.Parameters["P_USER"].Value = P_USER;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATUS;

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_SW_CHCK", OracleType.VarChar));
            oCmd.Parameters["P_SW_CHCK"].Value = P_SW_CHECK;



            dal.ExecuteNonQuery(oCmd);
        }

        public DataTable Get_N_GGMGF_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_N_GGMGF.PRC_GET_N_GGMGF_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_N_GGMGF_FORM_Update(string P_SQ_ID, string P_FORM_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_N_GGMGF.PRC_N_GGMGF_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_SQ_ID", OracleType.VarChar));
            oCmd.Parameters["P_SQ_ID"].Value = P_SQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_FORM_ID", OracleType.VarChar));
            oCmd.Parameters["P_FORM_ID"].Value = P_FORM_ID;

            dal.ExecuteNonQuery(oCmd);

        }

        public DataTable Get_N_ZIYRAN_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_N_ZIYRAN.PRC_GET_N_ZIYRAN_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_N_ZIYRAN_FORM_Update(string P_UNIQUE_ID, string P_FORM_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_N_ZIYRAN.PRC_EBA_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.VarChar));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_FORM_ID", OracleType.VarChar));
            oCmd.Parameters["P_FORM_ID"].Value = P_FORM_ID;

            dal.ExecuteNonQuery(oCmd);


        }



        public DataTable Get_UYDGDAContinueProcess_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_UYDGDA.PRC_GET_OTOMATIK_ONAY";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }
        public DataTable Get_UYDGDAREQID_TrigRecords(string EBA_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_UYDGDA.PRC_GET_REQUESTID";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public DataTable Get_UYDGDAFORMKONTROL_TrigRecords(string EBA_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_UYDGDA.PRC_GET_FORM_DURUM_KONTROLU";

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }



        public DataTable Get_RMIS_TrigRecords()
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_RMIS.PRC_GET_RMIS_START";

            oCmd.Parameters.Add(new OracleParameter("RC_SELECT", OracleType.Cursor));
            oCmd.Parameters["RC_SELECT"].Direction = ParameterDirection.Output;

            return dal.ExecuteDataTable(oCmd);
        }

        public void Set_RMIS_FORM_Update(int P_UNIQUE_ID, string P_PROCESS_ID)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_RMIS.PRC_RMIS_FORM_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQUE_ID", OracleType.Number));
            oCmd.Parameters["P_UNIQUE_ID"].Value = P_UNIQUE_ID;

            oCmd.Parameters.Add(new OracleParameter("P_PROCESS_ID", OracleType.VarChar));
            oCmd.Parameters["P_PROCESS_ID"].Value = P_PROCESS_ID;


            dal.ExecuteNonQuery(oCmd);
        }

        public void Set_RMIS_STATU_Update(int P_UNIQ_ID, string P_EBA_ID, string P_STATU, string P_USER)
        {
            OracleCommand oCmd = new OracleCommand();
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "EBAWORKFLOW5.PCK_EBA_SPIDS_YPKTOF.PRC_SPIDS_YPKTOF_STATU_UPDATE";

            oCmd.Parameters.Add(new OracleParameter("P_UNIQ_ID", OracleType.Number));
            oCmd.Parameters["P_UNIQ_ID"].Value = P_UNIQ_ID;

            oCmd.Parameters.Add(new OracleParameter("P_EBA_ID", OracleType.VarChar));
            oCmd.Parameters["P_EBA_ID"].Value = P_EBA_ID;

            oCmd.Parameters.Add(new OracleParameter("P_STATUS", OracleType.VarChar));
            oCmd.Parameters["P_STATUS"].Value = P_STATU;

            oCmd.Parameters.Add(new OracleParameter("P_USER", OracleType.VarChar));
            oCmd.Parameters["P_USER"].Value = P_USER;

           

            dal.ExecuteNonQuery(oCmd);
        }


    }
}
