using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectOrganizer.Common;
using ProjectOrganizer.Web.BLL;
using ProjectOrganizer.MasterPages;
using ProjectOrganizer.Repository;
using ProjectOrganizer.Repository.EntityManager;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace ProjectOrganizer.Web.Car
{
    public partial class CAR_Create : System.Web.UI.Page
    {
        #region public properties

        string currentDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
        string carType = string.Empty;
        string emptyText = string.Empty;
        clsCarSessionVariables objsessionVariables = new clsCarSessionVariables();

        #endregion

        #region Page and control events

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 6-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This event will do all functionality required on page load.Bind data to controls.Handle postback.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["CarSession"] != null)
                {
                    objsessionVariables = (clsCarSessionVariables)Session["CarSession"];

                    if (!IsPostBack)
                    {
                        BindIssuedByDropDown();
                        BindFromDropDown();
                        BindToDropDown();
                        BindProjectGrid();
                        BindIssuedToDropDown();


                        string[] importArray = Session["importArray"] as string[];

                        // set header text accroding to car type
                        switch (objsessionVariables.carType)
                        {
                            case "C":
                                lblHeader.Text = "CAR";
                                break;
                            case "S":
                                lblHeader.Text = "SCAR";
                                break;
                            case "M":
                                lblHeader.Text = "MAR";
                                break;
                            case "P":
                                lblHeader.Text = "PAR";
                                break;
                        }

                        // set form as  Create or Edit or View.
                        switch (objsessionVariables.mode)
                        {
                            case "Create":
                                PageLoadValues();
                                DisableControls(Page, true);
                                ShowControlsForActiveType(objsessionVariables.carType);
                                DisableTextAreas(false);
                                btnSaveData.Enabled = true;
                                btnOpenForEdit.Enabled = false;
                                btnPreview.Enabled = true;
                                btnPrintPdf.Enabled = true;
                                btnVoidResc.Enabled = true;
                                btnNotify.Enabled = true;
                                btnClear.Enabled = true;

                                if (objsessionVariables.carType == clsActiveTypes.MAR)
                                {
                                    txtDatePresident.Text = string.Empty;
                                    ValidatorState(false);
                                }
                                else
                                    ValidatorState(true);

                                break;

                            case "Edit":
                                if (objsessionVariables.forImportAnswer == true)
                                    GetDataForEditAndView(objsessionVariables.carNumber, importArray);
                                else
                                    GetDataForEditAndView(objsessionVariables.carNumber);

                                DisableControls(Page, true);
                                DisableTextAreas(false);
                                btnSaveData.Enabled = true;
                                btnOpenForEdit.Enabled = false;
                                btnPreview.Enabled = true;
                                btnPrintPdf.Enabled = true;
                                btnVoidResc.Enabled = true;
                                btnNotify.Enabled = true;
                                btnClear.Enabled = true;
                                break;

                            case "View":
                                GetDataForEditAndView(objsessionVariables.carNumber);
                                DisableControls(Page, false);
                                DisableTextAreas(true);
                                btnSaveData.Enabled = false;
                                btnOpenForEdit.Enabled = true;
                                btnPreview.Enabled = true;
                                btnPrintPdf.Enabled = true;
                                btnVoidResc.Enabled = false;
                                btnNotify.Enabled = true;
                                btnClear.Enabled = false;
                                break;
                        }
                    }
                    else
                    {
                        if (ViewState["lstRefs"] == null)
                        {
                            // create datatabe object used for bind reference listbox
                            DataTable dtRefs = new DataTable();
                            dtRefs.Columns.AddRange(new DataColumn[2] { new DataColumn("refId", typeof(int)), new DataColumn("References", typeof(string)) });
                            dtRefs.Columns["refId"].AutoIncrement = true;
                            dtRefs.Columns["refId"].AutoIncrementSeed = 1;
                            dtRefs.Columns["refId"].AutoIncrementStep = 1;
                            ViewState["lstRefs"] = dtRefs;
                        }
                    }
                }
                else
                {
                    // if car type not available
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('please try again.there is some issue with car type selection')", true);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteErrorLog("CAR", "CAR_Create.aspx", "Page_Load", ex.Message);
            }

        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 12-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will save new data in Car_Project.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddnewProject_Click(object sender, EventArgs e)
        {
            CAR_Project objCarProject = new CAR_Project
            {
                ProjectID = txtAddProjectId.Text,
                ShipName = txtAddShipName.Text,
                HullNumber = txtAddHullNumber.Text,
                ContractPONumber = txtAddContract.Text
            };

            ECRFEntities context = new ECRFEntities();
            try
            {
                context.AddToCAR_Project(objCarProject);
                context.SaveChanges();
                BindProjectGrid();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Create.aspx", "btnAddnewProject_Click", ex.Message);
            }
            ModalPopupExtender.Show();
            clearControlsOfProjectPopUpExtender();
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 12-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will copy refrences from popup extender's add refrenece listbox to create form reference listboox .  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            lstboxRefs.Items.Clear();
            int j = 0;
            try
            {
                for (j = 0; j <= lstboxAddRefs.Items.Count - 1; j++)
                {
                    lstboxRefs.Items.Add(lstboxAddRefs.Items[j]);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Create.aspx", "btnDone_Click", ex.Message);
            }

            // clear items from AddListBox
            lstboxAddRefs.Items.Clear();
            mpeAddreferences.Hide();
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 12-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will allow user to add new reference to reference listbox on create form .  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddRefrences_Click(object sender, EventArgs e)
        {
            if (ViewState["lstRefs"] != null)
            {
                try
                {
                    DataTable dtRefs = (DataTable)ViewState["lstRefs"];
                    dtRefs.Rows.Add();
                    dtRefs.Rows[dtRefs.Rows.Count - 1]["References"] = txtAddRefs.Text.Trim();
                    txtAddRefs.Text = emptyText;
                    ViewState["lstRefs"] = dtRefs;
                    lstboxAddRefs.DataSource = dtRefs;
                    lstboxAddRefs.DataTextField = "References";
                    lstboxAddRefs.DataValueField = "refId";
                    lstboxAddRefs.DataBind();
                    mpeAddreferences.Show();
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("CAR", "CAR_Create.aspx", "btnAddRefrences_Click", ex.Message);
                }
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 12-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will delete reference from reference listbox of popup extender.   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteRefs_Click(object sender, EventArgs e)
        {
            if (ViewState["lstRefs"] != null)
            {
                try
                {
                    DataTable dtRefs = (DataTable)ViewState["lstRefs"];
                    foreach (ListItem item in lstboxAddRefs.Items)
                    {
                        if (item.Selected)
                        {
                            DataRow[] rows = dtRefs.Select("refId = " + item.Value);
                            dtRefs.Rows.Remove(rows[0]);
                        }
                    }
                    dtRefs.AcceptChanges();
                    ViewState["lstRefs"] = dtRefs;
                    lstboxAddRefs.DataSource = dtRefs;
                    lstboxAddRefs.DataTextField = "References";
                    lstboxAddRefs.DataValueField = "refId";
                    lstboxAddRefs.DataBind();
                    mpeAddreferences.Show();
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("CAR", "CAR_Create.aspx", "btnDeleteRefs_Click", ex.Message);
                }
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 15-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method write name of assignee on basis of Division in assignee textbox. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlIssuedTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int divId = Convert.ToInt32(ddlIssuedTo.SelectedValue);
                if (divId != 0)
                {
                    var assignee = clsCAR.GetAssignee(divId);
                    if (assignee.Count() > 0)
                    {
                        string assigneeeName = clsCAR.GetAssignee(divId).FirstOrDefault().Assignment;                       
                        if (assigneeeName != null)
                            txtAssignedTo.Text = assigneeeName;
                        else
                            txtAssignedTo.Text = emptyText;
                    }
                    else
                        txtAssignedTo.Text = emptyText;
                }
                else
                    txtAssignedTo.Text = emptyText;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Create.aspx", "ddlIssuedTo_SelectedIndexChanged", ex.Message);
            }

        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 24-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will save new car record to database.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveData_Click(object sender, EventArgs e)
        {
           // ECRFEntities context=new ECRFEntities();
            CAR objCar = new CAR();
            bool blValue = ValidateRadioButton();
            DateTime? dtDateIssued = null;
            DateTime? dtDatePressNotification = null;
            DateTime? dtDateDue = null;
            DateTime? dtDatePlannedComp1 = null;
            DateTime? dtDatePlannedComp2 = null;
            DateTime? dtDateActionPlanned = null;
            DateTime? dtDateApprove = null;
            DateTime? dtDateClose = null;
            List<CAR_Reference> lstCarReference = new List<CAR_Reference>();
            string activeType = objsessionVariables.carType;

            if (!string.IsNullOrEmpty(txtDateIssued.Text))
                dtDateIssued = DateTime.ParseExact(txtDateIssued.Text, "MM-dd-yyyy", null);
            else
                dtDateIssued = null;
            objCar.DateIssued = dtDateIssued;

            if (!blValue)
            {
                // message for check radio button
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('please select options below for satisfactory,accepted,effective and close car')", true);
                return;
            }
            else
            {
                try
                {
                    objCar.Type = activeType;
                    objCar.CarNumber = Convert.ToInt32(txtCarNumber.Text);

                    if (ddlIssuedBy.SelectedIndex > 0)
                        objCar.FK_DBKeyIssuedBy = (Convert.ToInt32(ddlIssuedBy.SelectedValue));
                    else
                        objCar.FK_DBKeyIssuedBy = null;

                    if (!string.IsNullOrEmpty(txtPlannedCompDate1.Text))
                    {
                        dtDatePlannedComp2 = DateTime.ParseExact(txtPlannedCompDate2.Text, "MM-dd-yyyy", null);
                        objCar.DatePlannedComplete = dtDatePlannedComp2;
                    }
                    else if (!string.IsNullOrEmpty(txtPlannedCompDate1.Text))
                    {
                        dtDatePlannedComp1 = DateTime.ParseExact(txtPlannedCompDate1.Text, "MM-dd-yyyy", null);
                        objCar.DatePlannedComplete = dtDatePlannedComp1;
                    }
                    else
                        objCar.DatePlannedComplete = null;

                    if (ddlIssuedTo.SelectedIndex > 0)
                        objCar.FK_DBKeyIssuedTo = (Convert.ToInt32(ddlIssuedTo.SelectedValue));
                    else
                        objCar.FK_DBKeyIssuedTo = null;

                    //find dbkey of assigne To.                     
                    int? intDbkeyAssignee = GetAssigneeDbKey(objCar.FK_DBKeyIssuedTo, txtAssignedTo.Text);

                    if (intDbkeyAssignee != 0)
                        objCar.FK_DBKeyDefaultAssigneeIssuedTo = intDbkeyAssignee;
                    else
                        objCar.FK_DBKeyDefaultAssigneeIssuedTo = null;

                    if (!string.IsNullOrEmpty(txtResponseDue.Text))
                        dtDateDue = DateTime.ParseExact(txtResponseDue.Text, "MM-dd-yyyy", null);
                    else
                        dtDateDue = null;
                    objCar.DateDue = dtDateDue;

                    if (rdbtnEffectiveYes.Checked)
                        objCar.Effective = true;
                    else
                        objCar.Effective = false;

                    objCar.RootCause = txtareaRootCause.InnerText;

                    objCar.PreventativeAction = txtareaPrsentativeAction.InnerText;

                    objCar.CorrectiveAction = txtareaCorrectiveAction.InnerText;

                    if (!string.IsNullOrEmpty(txtDateActionPlannedSubmitted.Text))
                        dtDateActionPlanned = DateTime.ParseExact(txtDateActionPlannedSubmitted.Text, "MM-dd-yyyy", null);
                    else
                        dtDateActionPlanned = null;
                    objCar.DatePlannedSubmitted = dtDateActionPlanned;

                    if (!string.IsNullOrEmpty(txtApproveDate.Text))
                        dtDateApprove = DateTime.ParseExact(txtApproveDate.Text, "MM-dd-yyyy", null);
                    else
                        dtDateApprove = null;

                    objCar.EffIReviewDate = dtDateApprove;
                    objCar.SubmittedBy = txtsubmittedBy.Text;
                    objCar.EffReviewBy = txtAppBy.Text;
                    objCar.Comments = txtComments.Text;
                    objCar.Evidence = txtEvidence.Text;
                    objCar.ClosedOutBy = txtCloseBy.Text;

                    if (!string.IsNullOrEmpty(txtCloseDate.Text))
                        dtDateClose = DateTime.ParseExact(txtCloseDate.Text, "MM-dd-yyyy", null);
                    else
                        dtDateClose = null;

                    objCar.DateClosed = dtDateClose;

                    if (rdbtnMajor.Checked)
                        objCar.Severity = true;
                    else
                        objCar.Severity = false;

                    if (rdbtnSatisfactoryYes.Checked)
                        objCar.Satisfactory = true;
                    else
                        objCar.Satisfactory = false;

                    if (rdbtnCloseCarYes.Checked)
                        objCar.CloseCar = true;
                    else
                        objCar.CloseCar = false;

                    if (rdbtnAcceptedYes.Checked)
                        objCar.Accepted = true;
                    else
                        objCar.Accepted = false;

                    if (ddlFrom.SelectedIndex > 0)
                        objCar.FK_DBKeyFrom = Convert.ToInt32(ddlFrom.SelectedValue);
                    else
                        objCar.FK_DBKeyFrom = null;

                    if (ddlTo.SelectedIndex > 0)
                        objCar.FK_DBKeyTo = Convert.ToInt32(ddlTo.SelectedValue);
                    else
                        objCar.FK_DBKeyTo = null;

                    if (ViewState["ProjectDbKey"] != null)
                        objCar.FK_DBKeyProject = Convert.ToInt32(ViewState["ProjectDbKey"]);
                    else
                        objCar.FK_DBKeyProject = null;

                    if (!string.IsNullOrEmpty(txtDatePresident.Text))
                        dtDatePressNotification = DateTime.ParseExact(txtDatePresident.Text, "MM-dd-yyyy", null);
                    else
                        dtDatePressNotification = null;
                    if (rdbtnPerfMeasure.Checked)
                        objCar.PerfMeasures = true;
                    else
                        objCar.PerfMeasures = false;

                    if (rdbtnResourceReq.Checked)
                        objCar.ResourceReq = true;
                    else
                        objCar.ResourceReq = false;

                    if (rdbtnKeyBussProcess.Checked)
                        objCar.KeyBusProcess = true;
                    else
                        objCar.KeyBusProcess = false;

                    if (rdbtnCorpObjective.Checked)
                        objCar.CorpObj = true;
                    else
                        objCar.CorpObj = false;


                    if (rdbtnOther.Checked)
                        objCar.Other = txtareaOther.InnerText;
                    else
                        objCar.Other = string.Empty;

                    objCar.PresNotification = dtDatePressNotification;
                    objCar.PreparedBy = txtPrepredBy.Text;
                    objCar.SystemAffected = txtsysAffected.Text;
                    objCar.Drawing = txtDwgNo.Text;
                    objCar.ItemAffected = txtItemAffected.Text;
                    objCar.ItemNumber = txtItemNumber.Text;
                    objCar.ActionID = txtCustomerCarNo.Text;
                    objCar.Discrepancy = txtareaDeficiency.InnerText;
                    objCar.Status = ConfigurationManager.AppSettings["ValidStatus"].ToString();

                    if (objsessionVariables.mode == ConfigurationManager.AppSettings["CreateMode"].ToString())
                    {
                        //save car to database
                        if (!IsDuplicateCar(objCar.CarNumber, objCar.Type, objCar.DateIssued.Value.Year))
                        {
                            clsCAR.SaveCar(objCar);


                            //save references to database
                            int carDbKey = GetCarDbKey(activeType, objCar.DateIssued.Value.Year, objCar.CarNumber);

                            string strRefrenes = string.Empty;

                            foreach (ListItem li in lstboxRefs.Items)
                            {
                                CAR_Reference objCarReference = new CAR_Reference();
                                objCarReference.FK_DBKeyCAR = carDbKey;
                                objCarReference.CarNumber = objCar.CarNumber;
                                objCarReference.CarType = objCar.Type;
                                objCarReference.Year = objCar.DateIssued.Value.Year.ToString();
                                objCarReference.Reference = li.ToString();
                                lstCarReference.Add(objCarReference);                                                                
                            }                            

                            clsCAR.SaveReferences(lstCarReference);

                            DisableControls(Page, false);
                            DisableTextAreas(true);
                            btnSaveData.Enabled = false;
                            btnOpenForEdit.Enabled = true;
                            btnPreview.Enabled = false;
                            btnPrintPdf.Enabled = false;
                            btnVoidResc.Enabled = false;
                            btnNotify.Enabled = false;
                            btnClear.Enabled = false;
                            objsessionVariables.mode = ConfigurationManager.AppSettings["ViewMode"].ToString();
                        }
                        else
                        {
                            // duplicate message.
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('duplicate car number')", true);
                            return;
                        }
                    }

                    else if (objsessionVariables.mode == ConfigurationManager.AppSettings["EditMode"].ToString())
                    {
                        if (objsessionVariables.carNumber != 0)
                            objCar.DBKey = objsessionVariables.carNumber;
                        else
                            objCar.DBKey = GetCarDbKey(activeType, objCar.DateIssued.Value.Year, objCar.CarNumber);

                        objCar.FK_DBKeyProject = (int?)ViewState["ProjectDBKey"];

                        clsCAR.UpdateCar(objCar);

                        string strRefrenes = string.Empty;

                        foreach (ListItem li in lstboxRefs.Items)
                        {
                            CAR_Reference objCarReference = new CAR_Reference();
                            objCarReference.FK_DBKeyCAR = objCar.DBKey;
                            objCarReference.CarNumber = objCar.CarNumber;
                            objCarReference.CarType = objCar.Type;
                            objCarReference.Year = objCar.DateIssued.Value.Year.ToString();
                            objCarReference.Reference = li.ToString();
                            lstCarReference.Add(objCarReference);
                                                    }

                        List<CAR_Reference> carRereferencesToDelete = clsCAR.ReferencesToDelete(objCar.DBKey);
                        clsCAR.DeleteReferences(carRereferencesToDelete);
                        clsCAR.SaveReferences(lstCarReference);
                        DisableControls(Page, false);
                        DisableTextAreas(true);
                        btnSaveData.Enabled = false;
                        btnOpenForEdit.Enabled = true;
                        btnPreview.Enabled = false;
                        btnPrintPdf.Enabled = false;
                        btnVoidResc.Enabled = false;
                        btnNotify.Enabled = false;
                        btnClear.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteErrorLog("CAR", "CAR_Create.aspx", "btnSaveData_Click", ex.Message);
                }
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 26-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will open form in edit mode.data is available for save and other functionality on page.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOpenForEdit_Click(object sender, EventArgs e)
        {
            DisableControls(Page, true);
            DisableTextAreas(false);
            ShowControlsForActiveType(objsessionVariables.carType);

            if (objsessionVariables.carType == clsActiveTypes.MAR)
                ValidatorState(false);
            else
                ValidatorState(true);

            btnSaveData.Enabled = true;
            btnOpenForEdit.Enabled = true;
            btnPreview.Enabled = true;
            btnPrintPdf.Enabled = true;
            btnVoidResc.Enabled = true;
            btnNotify.Enabled = true;
            btnClear.Enabled = true;
            objsessionVariables.mode = ConfigurationManager.AppSettings["EditMode"].ToString();
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 26-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will show next record on gridview page index changing.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvProject.PageIndex = e.NewPageIndex;
            BindProjectGrid();
            ModalPopupExtender.Show();
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 11-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will fill value in textbox from Project popup. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = grvProject.SelectedIndex;
                ViewState["ProjectDbKey"] = grvProject.DataKeys[index];
                txtProjectId.Text = grvProject.SelectedRow.Cells[2].Text;
                txtHull.Text = grvProject.SelectedRow.Cells[3].Text;
                txtShip.Text = grvProject.SelectedRow.Cells[4].Text;
                txtContPo.Text = grvProject.SelectedRow.Cells[5].Text;
                ModalPopupExtender.Hide();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Create.aspx", "grvProject_SelectedIndexChanged", ex.Message);

            }
        }
      
        //protected void lnkbtnCloseCar_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("CAR_Main.aspx", false);
        //}

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 29-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will clear and reset all controls of form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearOrResetControls(Page);

            //clear all text areas
            txtareaCorrectiveAction.InnerText = string.Empty;
            txtareaDeficiency.InnerText = string.Empty;
            txtareaOther.InnerText = string.Empty;
            txtareaPrsentativeAction.InnerText = string.Empty;
            txtareaRootCause.InnerText = string.Empty;
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 29-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method copy refereces from reference listbox to add referenece listbox. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddDel_Click(object sender, EventArgs e)
        {
            lstboxAddRefs.Items.Clear();
            foreach (ListItem li in lstboxRefs.Items)
            {
                lstboxAddRefs.Items.Add(li);
            }
            mpeAddreferences.Show();
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 29-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will redirect user to car  main screen .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnCloseCar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CAR_Main.aspx", false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 10-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will bind IssuedBy dropdown from database.
        /// </summary>
        private void BindIssuedByDropDown()
        {
            try
            {
                var issuedBy = clsCAR.BindIssuedByDropDown();
                if (issuedBy.Count() > 0)
                {
                    ddlIssuedBy.DataSource = issuedBy;
                    ddlIssuedBy.DataTextField = "IssuedBy";
                    ddlIssuedBy.DataValueField = "DBKey";
                    ddlIssuedBy.DataBind();
                    ddlIssuedBy.Items.Insert(0, new ListItem("- - - Select - - -", "0"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 10-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will bind "From" dropdown from database.
        /// </summary>
        private void BindFromDropDown()
        {
            try
            {
                var varFrom = clsCAR.BindFromDropDown();
                if (varFrom.Count() > 0)
                {
                    ddlFrom.DataSource = varFrom;
                    ddlFrom.DataTextField = "FromName";
                    ddlFrom.DataValueField = "DBKey";
                    ddlFrom.DataBind();
                    ddlFrom.Items.Insert(0, new ListItem("- - - Select - - -", "0"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 10-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will bind "To" dropdown from database. 
        /// </summary>
        private void BindToDropDown()
        {
            try
            {
                var varTo = clsCAR.BindToDropDown();
                if (varTo.Count() > 0)
                {
                    ddlTo.DataSource = varTo;
                    ddlTo.DataTextField = "ToName";
                    ddlTo.DataValueField = "DBKey";
                    ddlTo.DataBind();
                    ddlTo.Items.Insert(0, new ListItem("- - - Select - - -", "0"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 10-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: this method will bind projectId,hull number,ship name and contract #/PO  to gridview..
        /// </summary>
        private void BindProjectGrid()
        {
            try
            {
                var project = clsCAR.Getjobs();
                if (project.Count() > 0)
                {
                    grvProject.DataSource = project;
                    grvProject.DataBind();
                }
                else
                   EmptyGrid(grvProject);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 11-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will get and  set all values required at page load
        /// </summary>
        private void PageLoadValues()
        {
            try
            {
                txtDateIssued.Text = currentDate;
                txtDatePresident.Text = currentDate;
                clsCarSessionVariables objsession = (clsCarSessionVariables)Session["CarSession"];
                carType = objsession.carType;
                int lastCarNumber = clsCAR.FindMaxCarNumber(carType);
                //set nex car number
                if (lastCarNumber != 0)
                    txtCarNumber.Text = (lastCarNumber + 1).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 11-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:this method will get data for Edit.Data show in editable mode.
        /// </summary>
        /// <param name="dbKey"></param>
        private void GetDataForEditAndView(int dbKey)
        {
            List<CAR> lstEditData = new List<CAR>();
            try
            {
                lstEditData = clsCAR.GetDataForEdit(dbKey);
                int divisionId = lstEditData.FirstOrDefault().CAR_DefaultAssigneeIssuedTo.DivisionId;
                if (lstEditData.Count() > 0)
                {
                    // fill all textboxes and texareas.
                    if (lstEditData.FirstOrDefault().CarNumber != 0)
                        txtCarNumber.Text = lstEditData.FirstOrDefault().CarNumber.ToString();

                    if (lstEditData.FirstOrDefault().DateIssued != null)
                        txtDateIssued.Text = lstEditData.FirstOrDefault().DateIssued.Value.Date.ToString("MM/dd/yyyy");

                    txtsysAffected.Text = lstEditData.FirstOrDefault().SystemAffected;
                    txtProjectId.Text = lstEditData.FirstOrDefault().CAR_Project.ProjectID;
                    txtHull.Text = lstEditData.FirstOrDefault().CAR_Project.HullNumber;
                    txtShip.Text = lstEditData.FirstOrDefault().CAR_Project.ShipName;
                    txtContPo.Text = lstEditData.FirstOrDefault().CAR_Project.ContractPONumber;
                    txtAssignedTo.Text = lstEditData.FirstOrDefault().CAR_DefaultAssigneeIssuedTo.Assignment;
                    txtDwgNo.Text = lstEditData.FirstOrDefault().Drawing;
                    txtItemAffected.Text = lstEditData.FirstOrDefault().ItemAffected;

                    if (lstEditData.FirstOrDefault().PresNotification != null)
                        txtDatePresident.Text = lstEditData.FirstOrDefault().PresNotification.Value.Date.ToString("MM/dd/yyyy");

                    txtItemNumber.Text = lstEditData.FirstOrDefault().ItemNumber;
                    txtPrepredBy.Text = lstEditData.FirstOrDefault().PreparedBy;

                    if (lstEditData.FirstOrDefault().DateDue != null)
                        txtResponseDue.Text = lstEditData.FirstOrDefault().DateDue.Value.Date.ToString("MM/dd/yyyy");

                    txtsubmittedBy.Text = lstEditData.FirstOrDefault().SubmittedBy;

                    if (lstEditData.FirstOrDefault().DatePlannedComplete != null)
                        txtPlannedCompDate1.Text = lstEditData.FirstOrDefault().DatePlannedComplete.Value.Date.ToString("MM/dd/yyyy");

                    if (lstEditData.FirstOrDefault().DatePlannedComplete != null)
                        txtPlannedCompDate2.Text = lstEditData.FirstOrDefault().DatePlannedComplete.Value.Date.ToString("MM/dd/yyyy");

                    if (lstEditData.FirstOrDefault().DatePlannedSubmitted != null)
                        txtDateActionPlannedSubmitted.Text = lstEditData.FirstOrDefault().DatePlannedSubmitted.Value.Date.ToString("MM/dd/yyyy");

                    txtareaDeficiency.InnerText = lstEditData.FirstOrDefault().Discrepancy;
                    txtareaRootCause.InnerText = lstEditData.FirstOrDefault().RootCause;
                    txtareaPrsentativeAction.InnerText = lstEditData.FirstOrDefault().PreventativeAction;
                    txtareaCorrectiveAction.InnerText = lstEditData.FirstOrDefault().CorrectiveAction;
                    txtAppBy.Text = lstEditData.FirstOrDefault().EffReviewBy;

                    if (lstEditData.FirstOrDefault().EffIReviewDate != null)
                        txtApproveDate.Text = lstEditData.FirstOrDefault().EffIReviewDate.Value.Date.ToString("MM/dd/yyyy");

                    txtCloseBy.Text = lstEditData.FirstOrDefault().ClosedOutBy;

                    if (lstEditData.FirstOrDefault().DateClosed != null)
                        txtCloseDate.Text = lstEditData.FirstOrDefault().DateClosed.Value.Date.ToString("MM/dd/yyyy");

                    txtComments.Text = lstEditData.FirstOrDefault().Comments;
                    txtEvidence.Text = lstEditData.FirstOrDefault().Evidence;

                    // fill all radio buttons
                    if (lstEditData.FirstOrDefault().Severity == true)
                        rdbtnMajor.Checked = true;
                    else
                        rdbtnMinor.Checked = true;

                    if (lstEditData.FirstOrDefault().Satisfactory == true)
                        rdbtnSatisfactoryYes.Checked = true;
                    else
                        rdbtnSatisfactoryNo.Checked = true;

                    if (lstEditData.FirstOrDefault().Accepted == true)
                        rdbtnAcceptedYes.Checked = true;
                    else
                        rdbtnAcceptedNo.Checked = true;

                    if (lstEditData.FirstOrDefault().Effective == true)
                        rdbtnEffectiveYes.Checked = true;
                    else
                        rdbtnEffectiveNo.Checked = true;

                    if (lstEditData.FirstOrDefault().CloseCar == true)
                        rdbtnCloseCarYes.Checked = true;
                    else
                        rdbtnCloseCarNo.Checked = true;

                    // fill all dropdowns
                    ddlIssuedBy.SelectedValue = lstEditData.FirstOrDefault().FK_DBKeyIssuedBy.ToString();
                    ddlFrom.SelectedValue = lstEditData.FirstOrDefault().FK_DBKeyFrom.ToString();
                    ddlTo.SelectedValue = lstEditData.FirstOrDefault().FK_DBKeyTo.ToString();
                    ddlIssuedTo.SelectedValue = lstEditData.FirstOrDefault().CAR_DefaultAssigneeIssuedTo.DivisionId.ToString();
                    ViewState["ProjectDBKey"] = lstEditData.FirstOrDefault().FK_DBKeyProject;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 11-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: overloaded method.This method will get data from database in case of import answer.
        /// </summary>
        /// <param name="dbKey"></param>
        /// <param name="importAnswer"></param>
        private void GetDataForEditAndView(int dbKey, string[] importAnswer)
        {
            List<CAR> lstEditData = new List<CAR>();
            try
            {
                CultureInfo format_dt = new CultureInfo("en-US");
                lstEditData = clsCAR.GetDataForEdit(dbKey);
                if (lstEditData.Count() > 0)
                {
                    // fill all textboxes and texareas.
                    if (lstEditData.FirstOrDefault().CarNumber != 0)
                        txtCarNumber.Text = lstEditData.FirstOrDefault().CarNumber.ToString();

                    if (lstEditData.FirstOrDefault().DateIssued != null)
                        txtDateIssued.Text = lstEditData.FirstOrDefault().DateIssued.Value.Date.ToString("MM/dd/yyyy");

                    txtsysAffected.Text = lstEditData.FirstOrDefault().SystemAffected;
                    txtProjectId.Text = lstEditData.FirstOrDefault().CAR_Project.ProjectID;
                    txtHull.Text = lstEditData.FirstOrDefault().CAR_Project.HullNumber;
                    txtShip.Text = lstEditData.FirstOrDefault().CAR_Project.ShipName;
                    txtContPo.Text = lstEditData.FirstOrDefault().CAR_Project.ContractPONumber;
                    txtAssignedTo.Text = lstEditData.FirstOrDefault().CAR_DefaultAssigneeIssuedTo.Assignment;
                    txtDwgNo.Text = lstEditData.FirstOrDefault().Drawing;
                    txtItemAffected.Text = lstEditData.FirstOrDefault().ItemAffected;

                    if (lstEditData.FirstOrDefault().PresNotification != null)
                        txtDatePresident.Text = lstEditData.FirstOrDefault().PresNotification.Value.Date.ToString("MM/dd/yyyy");

                    txtItemNumber.Text = lstEditData.FirstOrDefault().ItemNumber;
                    txtPrepredBy.Text = lstEditData.FirstOrDefault().PreparedBy;

                    if (lstEditData.FirstOrDefault().DateDue != null)
                        txtResponseDue.Text = lstEditData.FirstOrDefault().DateDue.Value.Date.ToString("MM/dd/yyyy");

                    if (importAnswer[5] != "0")
                        txtsubmittedBy.Text = importAnswer[5];

                    else
                        txtsubmittedBy.Text = emptyText;

                    if (importAnswer[3] != null)                        
                        txtPlannedCompDate1.Text = importAnswer[3];

                    if (importAnswer[4] != null)
                        txtPlannedCompDate2.Text = importAnswer[4];

                    if (lstEditData.FirstOrDefault().DatePlannedSubmitted != null)
                        txtDateActionPlannedSubmitted.Text = importAnswer[6];

                    txtareaDeficiency.InnerText = lstEditData.FirstOrDefault().Discrepancy;

                    if (importAnswer[0] != "0")
                        txtareaRootCause.InnerText = importAnswer[0];
                    else
                        txtareaRootCause.InnerText = emptyText;

                    if (importAnswer[1] != "0")
                        txtareaPrsentativeAction.InnerText = importAnswer[1];
                    else
                        txtareaPrsentativeAction.InnerText = emptyText;

                    if (importAnswer[2] != "0")
                        txtareaCorrectiveAction.InnerText = importAnswer[2];
                    else
                        txtareaCorrectiveAction.InnerText = emptyText;

                    txtAppBy.Text = lstEditData.FirstOrDefault().EffReviewBy;

                    if (lstEditData.FirstOrDefault().EffIReviewDate != null)
                        txtApproveDate.Text = lstEditData.FirstOrDefault().EffIReviewDate.Value.Date.ToString("MM/dd/yyyy");

                    txtCloseBy.Text = lstEditData.FirstOrDefault().ClosedOutBy;

                    if (lstEditData.FirstOrDefault().DateClosed != null)
                        txtCloseDate.Text = lstEditData.FirstOrDefault().DateClosed.Value.Date.ToString("MM/dd/yyyy");

                    txtComments.Text = lstEditData.FirstOrDefault().Comments;
                    txtEvidence.Text = lstEditData.FirstOrDefault().Evidence;

                    // fill all radio buttons
                    if (lstEditData.FirstOrDefault().Severity == true)
                        rdbtnMajor.Checked = true;
                    else
                        rdbtnMinor.Checked = true;

                    if (lstEditData.FirstOrDefault().Satisfactory == true)
                        rdbtnSatisfactoryYes.Checked = true;
                    else
                        rdbtnSatisfactoryNo.Checked = true;

                    if (lstEditData.FirstOrDefault().Accepted == true)
                        rdbtnAcceptedYes.Checked = true;
                    else
                        rdbtnAcceptedNo.Checked = true;

                    if (lstEditData.FirstOrDefault().Effective == true)
                        rdbtnEffectiveYes.Checked = true;
                    else
                        rdbtnEffectiveNo.Checked = true;

                    if (lstEditData.FirstOrDefault().CloseCar == true)
                        rdbtnCloseCarYes.Checked = true;
                    else
                        rdbtnCloseCarNo.Checked = true;

                    // fill all dropdowns
                    ddlIssuedBy.SelectedValue = lstEditData.FirstOrDefault().FK_DBKeyIssuedBy.ToString();
                    ddlFrom.SelectedValue = lstEditData.FirstOrDefault().FK_DBKeyFrom.ToString();
                    ddlTo.SelectedValue = lstEditData.FirstOrDefault().FK_DBKeyTo.ToString();
                    ddlIssuedTo.SelectedValue = lstEditData.FirstOrDefault().CAR_DefaultAssigneeIssuedTo.DivisionId.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 12-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will change control status.
        /// </summary>
        /// <param name="status"></param>      
        protected void DisableControls(Control parent, bool State)
        {
            try
            {
                foreach (Control c in parent.Controls)
                {
                    if (c is DropDownList)
                        ((DropDownList)(c)).Enabled = State;

                    else if (c is TextBox)
                        ((TextBox)(c)).Enabled = State;

                    else if (c is RadioButton)
                        ((RadioButton)(c)).Enabled = State;

                    //else if (c is Button)
                    //    ((Button)(c)).Enabled = State;

                    DisableControls(c, State);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 12-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will change textareas status. 
        /// </summary>
        /// <param name="status"></param>
        private void DisableTextAreas(bool status)
        {
            txtareaCorrectiveAction.Disabled = status;
            txtareaDeficiency.Disabled = status;
            txtareaPrsentativeAction.Disabled = status;
            txtareaRootCause.Disabled = status;
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 12-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This method will clear controls of Project modalpopup extender.
        /// </summary>
        void clearControlsOfProjectPopUpExtender()
        {
            txtAddProjectId.Text = "";
            txtAddShipName.Text = "";
            txtAddHullNumber.Text = "";
            txtAddContract.Text = "";
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 15-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will write no record found text in gridview when empty. 
        /// </summary>
        /// <param name="grv"></param>
        private void EmptyGrid(GridView grv)
        {
            DataTable dtProject = new DataTable();
            dtProject.Columns.Add("DBKey", typeof(int));
            dtProject.Columns.Add("ProjectID", typeof(string));
            dtProject.Columns.Add("ShipName", typeof(string));
            dtProject.Columns.Add("HullNumber", typeof(string));
            dtProject.Columns.Add("ContractPONumber", typeof(string));            

            dtProject.Rows.Add(dtProject.NewRow());
            grv.DataSource = dtProject;
            grv.DataBind();
            int columncount = grv.Rows[0].Cells.Count;
            grv.Rows[0].Cells.Clear();
            grv.Rows[0].Cells.Add(new TableCell());
            grv.Rows[0].Cells[0].ColumnSpan = columncount;
            grv.Rows[0].Cells[0].Text = "No Records Found";
            grv.Rows[0].Cells[0].ForeColor = Color.Red;
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 15-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will bind "IssuedTo" record to dropdown. 
        /// </summary>
        private void BindIssuedToDropDown()
        {
            try
            {
                var divisionName = clsCAR.GetDivisions();
                if (divisionName.Count() > 0)
                {
                    ddlIssuedTo.DataSource = divisionName;
                    ddlIssuedTo.DataTextField = "DivisionName";
                    ddlIssuedTo.DataValueField = "DivID";
                    ddlIssuedTo.DataBind();
                    ddlIssuedTo.Items.Insert(0, new ListItem("- - - Select - - -", "0"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 22-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will return boolean value.used to identifiy radio button is checked or not for save new car record. 
        /// </summary>
        /// <returns></returns>
        private bool ValidateRadioButton()
        {
            bool blFlagChecked = false;
            if (objsessionVariables.carType == clsActiveTypes.MAR)
            {
                if (rdbtnEffectiveYes.Checked || rdbtnEffectiveNo.Checked)
                    blFlagChecked = true;
                else
                    blFlagChecked = false;
            }
            else
            {
                if ((rdbtnSatisfactoryYes.Checked || rdbtnSatisfactoryNo.Checked) && (rdbtnAcceptedYes.Checked || rdbtnAcceptedNo.Checked) && (rdbtnEffectiveYes.Checked || rdbtnEffectiveNo.Checked) && (rdbtnCloseCarYes.Checked || rdbtnCloseCarNo.Checked))
                    blFlagChecked = true;
                else
                    blFlagChecked = false;
            }
            return blFlagChecked;
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 23-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will return dbkey from database for assigne name on the basis of division id and assignee name.
        /// </summary>
        /// <param name="divId"></param>
        /// <param name="AssigneeName"></param>
        /// <returns></returns>
        private int? GetAssigneeDbKey(int? divId, string AssigneeName)
        {
            ECRFEntities context = new ECRFEntities();
            int? intDbkeyAssignee = null;
            var dbkey = context.CAR_DefaultAssigneeIssuedTo.Where(t => t.DivisionId == divId && t.Assignment == AssigneeName);
            if (dbkey.Count() > 0)
                intDbkeyAssignee = dbkey.FirstOrDefault().DBKey;

            return intDbkeyAssignee;
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 25-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will allow controls to visible and enable according to car type
        /// </summary>
        /// <param name="strActiveType"></param>
        private void ShowControlsForActiveType(string strActiveType)
        {
            if (strActiveType == clsActiveTypes.CAR || strActiveType == clsActiveTypes.PAR || strActiveType == clsActiveTypes.SCAR)
            {
                rowMar1.Attributes["Style"] = "display: none";
                rowMar2.Attributes["Style"] = "display: none";
                rowDiscrepency.Attributes["Style"] = "display: normal";
                ddlFrom.Enabled = true;
                ddlTo.Enabled = true;
                txtProjectId.Enabled = true;
                txtHull.Enabled = true;
                txtShip.Enabled = true;
                txtContPo.Enabled = true;
                txtsysAffected.Enabled = true;
                txtItemAffected.Enabled = true;
                txtItemNumber.Enabled = true;
                txtPrepredBy.Enabled = true;
                lstboxRefs.Enabled = true;
                txtDatePresident.Enabled = true;
                txtDwgNo.Enabled = true;
                txtCustomerCarNo.Enabled = true;
                rdbtnSatisfactoryNo.Enabled = true;
                rdbtnSatisfactoryYes.Enabled = true;
                rdbtnAcceptedYes.Enabled = true;
                rdbtnAcceptedNo.Enabled = true;
                rdbtnCloseCarYes.Enabled = true;
                rdbtnCloseCarNo.Enabled = true;
                rdbtnAcceptedYes.Enabled = true;
                rdbtnAcceptedNo.Enabled = true;
                rdbtnMajor.Enabled = true;
                rdbtnMinor.Enabled = true;
            }
            else
            {
                rowMar1.Attributes["Style"] = "display: normal";
                rowMar2.Attributes["Style"] = "display: normal";
                rowDiscrepency.Attributes["Style"] = "display: none";
                ddlFrom.Enabled = false;
                ddlTo.Enabled = false;
                txtProjectId.Enabled = false;
                txtHull.Enabled = false;
                txtShip.Enabled = false;
                txtContPo.Enabled = false;
                txtsysAffected.Enabled = false;
                txtItemAffected.Enabled = false;
                txtItemNumber.Enabled = false;
                txtPrepredBy.Enabled = false;
                lstboxRefs.Enabled = false;
                txtDatePresident.Enabled = false;
                txtDwgNo.Enabled = false;
                txtCustomerCarNo.Enabled = false;
                rdbtnSatisfactoryNo.Enabled = false;
                rdbtnSatisfactoryYes.Enabled = false;
                rdbtnAcceptedYes.Enabled = false;
                rdbtnAcceptedNo.Enabled = false;
                rdbtnCloseCarYes.Enabled = false;
                rdbtnCloseCarNo.Enabled = false;
                rdbtnAcceptedYes.Enabled = false;
                rdbtnAcceptedNo.Enabled = false;
                rdbtnMajor.Enabled = false;
                rdbtnMinor.Enabled = false;
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 26-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will return dbkey(primary key) of car from database.
        /// </summary>
        /// <param name="strCarType"></param>
        /// <param name="year"></param>
        /// <param name="carNumber"></param>
        /// <returns></returns>
        private int GetCarDbKey(string strCarType, int year, int carNumber)
        {
            ECRFEntities context = new ECRFEntities();
            var dbkeyCar = context.CAR.Where(t => t.Type == strCarType && t.CarNumber == carNumber && t.DateIssued.Value.Year == year);
            int intCarDbKey = dbkeyCar.FirstOrDefault().DBKey;
            return intCarDbKey;
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 26-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will check for duplicate car number before saving car to database.
        /// </summary>
        /// <param name="carNumber"></param>
        /// <param name="carType"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private bool IsDuplicateCar(int carNumber, string carType, int year)
        {
            bool blFlagCarNumber = false;
            ECRFEntities context = new ECRFEntities();
            var car = context.CAR.Where(t => t.Type == carType && t.CarNumber == carNumber && t.DateIssued.Value.Year == year);

            if (car.Count() > 0)
                blFlagCarNumber = true;

            return blFlagCarNumber;

        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 29-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will enable and disable required field and other validation on page. 
        /// </summary>
        /// <param name="state"></param>
        private void ValidatorState(bool state)
        {
            rfvddlFrom.Enabled = state;
            rfvddlTo.Enabled = state;
            rfvProject.Enabled = state;
            rfvHull.Enabled = state;
            rfvShip.Enabled = state;
            rfvContPO.Enabled = state;
            rfvPreparedBy.Enabled = state;
            rfvSysAffected.Enabled = state;
            rfvDrwng.Enabled = state;
            rfvItemAffected.Enabled = state;
            rfvItemNumber.Enabled = state;
            rfvDatePress.Enabled = state;
            rfvItemAffected.Enabled = state;
            rfvItemAffected.Enabled = state;
            rfvDiscrep.Enabled = state;
            btnAddDel.Enabled = state;
        }
        
        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 29-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will clear all textboxes and reset all option controls like dropdownlist,radio button etc.
        /// </summary>
        /// <param name="parent"></param>
        protected void ClearOrResetControls(Control parent)
        {

            //Loop through all the control present on the web page/form        
            foreach (Control ctrl in parent.Controls)
            {
                //check for all the TextBox controls on the page and clear them
                if (ctrl is TextBox)
                {
                    if (((TextBox)(ctrl)).ID != "txtCarNumber")
                        ((TextBox)(ctrl)).Text = string.Empty;
                }

                //check for all the DropDownList controls on the page and reset it to the very first item e.g. "-- Select One --"
                else if (ctrl is DropDownList)
                    ((DropDownList)(ctrl)).SelectedIndex = 0;

                //check for all the CheckBox controls on the page and unchecked the selection
                else if (ctrl is CheckBox)
                    ((CheckBox)(ctrl)).Checked = false;

                //check for all the RadioButton controls on the page and unchecked the selection
                else if (ctrl is RadioButton)
                    ((RadioButton)(ctrl)).Checked = false;

                ClearOrResetControls(ctrl);
            }
        }
        #endregion

    }
}