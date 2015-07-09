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
using System.Text;
using System.Collections;
using System.Data;
using System.ComponentModel;
namespace ProjectOrganizer.Web.Car
{
    public partial class CAR_Export : System.Web.UI.Page
    {
        #region Public Properties
        public string activeType = string.Empty;

        public int year = 0;
        public int customer = 0;
        DataTable dtCar = new DataTable();
        // public int reference = 0;
        string refrence = string.Empty;
        clsCarSessionVariables objSessionVaraiable = new clsCarSessionVariables();
        #endregion

        #region Page and controls event

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 30-06-2015 
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
                    objSessionVaraiable = (clsCarSessionVariables)Session["CarSession"];
                    activeType = objSessionVaraiable.carType;
                }

                if (!IsPostBack)
                {
                    for (int i = 0; i < chkListExport.Items.Count; i++)
                    {
                        chkListExport.Items[i].Selected = true;
                    }

                    divColumns.Visible = true;
                    divCountResult.Visible = false;
                    GetAllCarData(activeType);
                    GetReferences(activeType);
                    BindYears();
                    BindIssuedByDropDown();
                    BindReferences(objSessionVaraiable.carType);
                    txtStartingCar.Enabled = false;
                    txtEndingCar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Export.aspx", "Page_Load", ex.Message);
            }
           
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 30-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This event will count no of data to export.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCountQa_Click(object sender, EventArgs e)
        {
            activeType = objSessionVaraiable.carType;
            var lstCar = new List<CAR>();
            var lstReferences = (List<CAR_Reference>)ViewState["lstReference"];
            year = Convert.ToInt32(ddlYear.SelectedValue);
            customer = Convert.ToInt32(ddlCustomer.SelectedValue);
            string refrence = ddlRefrence.SelectedItem.Text;

            try
            {
                if (rdbCustomFilter.Checked)
                {
                    if (ddlYear.SelectedIndex == 0 && ddlCustomer.SelectedIndex == 0 && ddlRefrence.SelectedIndex == 0)
                    {
                        if (ViewState["AllCar"] != null)
                        {
                            lstCar = ((List<CAR>)ViewState["AllCar"]);
                            if (lstCar != null)
                            {
                                txtCountQaData.Text = lstCar.Count().ToString();
                                ShowCountDataGrid(lstCar);
                            }
                        }
                    }
                    else if (ddlYear.SelectedIndex > 0 && ddlCustomer.SelectedIndex > 0 && ddlRefrence.SelectedIndex > 0)
                    {
                        // for all three listboxes chcked
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.Type == activeType && t.DateIssued.Value.Year == year && t.FK_DBKeyIssuedBy == customer).ToList();
                        List<CAR> dataExport = lstCar.Where(x => lstReferences.Any(y => y.FK_DBKeyCAR == x.DBKey && y.Reference == refrence)).ToList();

                        if (dataExport != null)
                        {
                            txtCountQaData.Text = dataExport.Count().ToString();
                            ShowCountDataGrid(dataExport.ToList());
                        }
                    }
                    else if (ddlYear.SelectedIndex > 0 && ddlCustomer.SelectedIndex > 0)
                    {
                        // year and custonmer                   
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.Type == activeType && t.DateIssued.Value.Year == year && t.FK_DBKeyIssuedBy == customer).ToList();
                        if (lstCar != null)
                        {
                            txtCountQaData.Text = lstCar.Count().ToString();
                            ShowCountDataGrid(lstCar);
                        }
                    }
                    else if (ddlYear.SelectedIndex > 0 && ddlRefrence.SelectedIndex > 0)
                    {
                        // year and reference
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.Type == activeType && t.DateIssued.Value.Year == year).ToList();
                        var dataExport = lstCar.Where(x => lstReferences.Any(y => y.FK_DBKeyCAR == x.DBKey && y.Reference == refrence));

                        if (dataExport != null)
                        {
                            txtCountQaData.Text = dataExport.Count().ToString();
                            ShowCountDataGrid(dataExport.ToList());
                        }
                    }
                    else if (ddlCustomer.SelectedIndex > 0 && ddlRefrence.SelectedIndex > 0)
                    {
                        // customer and reference
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.Type == activeType && t.FK_DBKeyIssuedBy == customer).ToList();
                        var dataExport = lstCar.Where(x => lstReferences.Any(y => y.FK_DBKeyCAR == x.DBKey && y.Reference == refrence));

                        if (dataExport != null)
                        {
                            txtCountQaData.Text = dataExport.Count().ToString();
                            ShowCountDataGrid(dataExport.ToList());
                        }
                    }
                    else if (ddlYear.SelectedIndex > 0)
                    {
                        //for only year selected
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.Type == activeType && t.DateIssued.Value.Year == year).ToList();
                        if (lstCar != null)
                        {
                            txtCountQaData.Text = lstCar.Count().ToString();
                            ShowCountDataGrid(lstCar);
                        }
                    }
                    else if (ddlCustomer.SelectedIndex > 0)
                    {
                        //for only customer selected
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.FK_DBKeyIssuedBy == customer).ToList();
                        if (lstCar != null)
                        {
                            txtCountQaData.Text = lstCar.Count().ToString();
                            ShowCountDataGrid(lstCar);
                        }
                    }
                    else if (ddlRefrence.SelectedIndex > 0)
                    {
                        //for only reference slected.
                        lstCar = ((List<CAR>)ViewState["AllCar"]);
                        var dataExport = lstCar.Where(x => lstReferences.Any(y => y.FK_DBKeyCAR == x.DBKey && y.Reference == refrence));

                        if (dataExport != null)
                        {
                            txtCountQaData.Text = dataExport.Count().ToString();
                            ShowCountDataGrid(dataExport.ToList());
                        }
                    }
                }
                else if (rdbCarSelectionFilter.Checked)
                {
                    int startingCarNumber = 0;
                    int endingCarNumber = 0;

                    if (!string.IsNullOrEmpty(txtStartingCar.Text) && !string.IsNullOrEmpty(txtEndingCar.Text))
                    {
                        startingCarNumber = Convert.ToInt32(txtStartingCar.Text);
                        endingCarNumber = Convert.ToInt32(txtEndingCar.Text);
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.CarNumber >= startingCarNumber && t.CarNumber <= endingCarNumber).ToList();                          
                    }
                    else if (string.IsNullOrEmpty(txtStartingCar.Text) && string.IsNullOrEmpty(txtEndingCar.Text))
                    {
                        lstCar = ((List<CAR>)ViewState["AllCar"]);                        
                    }
                    else if (!string.IsNullOrEmpty(txtStartingCar.Text) && string.IsNullOrEmpty(txtEndingCar.Text))
                    {
                        startingCarNumber = Convert.ToInt32(txtStartingCar.Text);
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.CarNumber >= startingCarNumber).ToList();                       
                    }
                    else if (string.IsNullOrEmpty(txtEndingCar.Text) && !string.IsNullOrEmpty(txtEndingCar.Text))
                    {
                        endingCarNumber = Convert.ToInt32(txtStartingCar.Text);
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.CarNumber <= endingCarNumber).ToList();
                    }

                    //show count in textbox and columns to export in gridview
                    if (lstCar != null)
                    {
                        txtCountQaData.Text = lstCar.Count().ToString();
                        ShowCountDataGrid(lstCar);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Export.aspx", "btnCountQa_Click", ex.Message);
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 2-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This event will export data to excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            activeType = objSessionVaraiable.carType;
            var lstCar = new List<CAR>();
            var lstReferences = (List<CAR_Reference>)ViewState["lstReference"];
            try
            {
                if (rdbCustomFilter.Checked)
                {
                    year = Convert.ToInt32(ddlYear.SelectedValue);
                    customer = Convert.ToInt32(ddlCustomer.SelectedValue);
                    refrence = ddlRefrence.SelectedItem.Text;
                    if (ddlYear.SelectedIndex == 0 && ddlCustomer.SelectedIndex == 0 && ddlRefrence.SelectedIndex == 0)
                    {
                        var lstAll = new List<CAR>();

                        if (ViewState["AllCar"] != null)
                        {
                            lstAll = ((List<CAR>)ViewState["AllCar"]);
                            ExportToExcel(lstAll);
                        }
                    }
                    else if (ddlYear.SelectedIndex > 0 && ddlCustomer.SelectedIndex > 0 && ddlRefrence.SelectedIndex > 0)
                    {
                        // for all three listboxes chcked 
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.DateIssued.Value.Year == year && t.FK_DBKeyIssuedBy == customer).ToList();
                        List<CAR> dataExport = lstCar.Where(x => lstReferences.Any(y => y.FK_DBKeyCAR == x.DBKey && y.Reference == refrence)).ToList();
                        ExportToExcel(dataExport);

                    }
                    else if (ddlYear.SelectedIndex > 0 && ddlCustomer.SelectedIndex > 0)
                    {
                        // year and custonmer
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.DateIssued.Value.Year == year && t.FK_DBKeyIssuedBy == customer).ToList();
                        ExportToExcel(lstCar);
                    }
                    else if (ddlYear.SelectedIndex > 0 && ddlRefrence.SelectedIndex > 0)
                    {
                        // year and reference
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.DateIssued.Value.Year == year).ToList();
                        List<CAR> dataExport = lstCar.Where(x => lstReferences.Any(y => y.FK_DBKeyCAR == x.DBKey && y.Reference == refrence)).ToList();
                        ExportToExcel(dataExport);
                    }
                    else if (ddlCustomer.SelectedIndex > 0 && ddlRefrence.SelectedIndex > 0)
                    {
                        // customer and reference
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.FK_DBKeyIssuedBy == customer).ToList();
                        List<CAR> dataExport = lstCar.Where(x => lstReferences.Any(y => y.FK_DBKeyCAR == x.DBKey && y.Reference == refrence)).ToList();
                        ExportToExcel(dataExport);
                    }
                    else if (ddlYear.SelectedIndex > 0)
                    {
                        //for only year selected
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.DateIssued.Value.Year == year).ToList();
                        ExportToExcel(lstCar);
                    }
                    else if (ddlCustomer.SelectedIndex > 0)
                    {
                        //for only customer selected
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.FK_DBKeyIssuedBy == customer).ToList();
                        ExportToExcel(lstCar);
                    }
                    else if (ddlRefrence.SelectedIndex > 0)
                    {
                        //for only reference slected.
                        lstCar = ((List<CAR>)ViewState["AllCar"]);
                        List<CAR> dataExport = lstCar.Where(x => lstReferences.Any(y => y.FK_DBKeyCAR == x.DBKey && y.Reference == refrence)).ToList();
                        ExportToExcel(dataExport);
                    }
                }

                else if (rdbCarSelectionFilter.Checked)
                {
                    int startingCarNumber = 0;
                    int endingCarNumber = 0;
                    if (!string.IsNullOrEmpty(txtStartingCar.Text) && !string.IsNullOrEmpty(txtEndingCar.Text))
                    {
                        startingCarNumber = Convert.ToInt32(txtStartingCar.Text);
                        endingCarNumber = Convert.ToInt32(txtEndingCar.Text);
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.CarNumber >= startingCarNumber && t.CarNumber <= endingCarNumber).ToList();
                        ExportToExcel(lstCar);
                    }
                    else if (string.IsNullOrEmpty(txtStartingCar.Text) && string.IsNullOrEmpty(txtEndingCar.Text))
                    {
                        lstCar = ((List<CAR>)ViewState["AllCar"]);
                        ExportToExcel(lstCar);

                    }
                    else if (!string.IsNullOrEmpty(txtStartingCar.Text) && string.IsNullOrEmpty(txtEndingCar.Text))
                    {
                        startingCarNumber = Convert.ToInt32(txtStartingCar.Text);
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.CarNumber >= startingCarNumber).ToList();
                        ExportToExcel(lstCar);
                    }
                    else if (string.IsNullOrEmpty(txtEndingCar.Text) && !string.IsNullOrEmpty(txtEndingCar.Text))
                    {
                        endingCarNumber = Convert.ToInt32(txtStartingCar.Text);
                        lstCar = ((List<CAR>)ViewState["AllCar"]).Where(t => t.CarNumber <= endingCarNumber).ToList();
                        ExportToExcel(lstCar);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Export.aspx", "btnExport_Click", ex.Message);
            }
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 07-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This event will set controls for car based selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbCarSelectionFilter_CheckedChanged(object sender, EventArgs e)
        {
            ddlYear.Enabled = false;
            ddlRefrence.Enabled = false;
            ddlCustomer.Enabled = false;
            txtStartingCar.Enabled = true;
            txtEndingCar.Enabled = true;
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 07-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: This event will set controls for custom based selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbCustomFilter_CheckedChanged(object sender, EventArgs e)
        {
            ddlYear.Enabled = true;
            ddlRefrence.Enabled = true;
            ddlCustomer.Enabled = true;
            txtStartingCar.Enabled = false;
            txtEndingCar.Enabled = false;
        }

        /// <summary>
        /// Created Date: 8-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This event will used to formatting all values in gridview row.   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvCountResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DateTime? dtIssueDate = DataBinder.Eval(e.Row.DataItem, "DateIssued") as DateTime?;
                    if (dtIssueDate != null)
                        e.Row.Cells[3].Text = dtIssueDate.Value.ToString("d");

                    DateTime? dtPressNotification = DataBinder.Eval(e.Row.DataItem, "PresNotification") as DateTime?;
                    if (dtPressNotification != null)
                        e.Row.Cells[11].Text = dtPressNotification.Value.ToString("d");

                    DateTime? dtDueDate = DataBinder.Eval(e.Row.DataItem, "DateDue") as DateTime?;
                    if (dtDueDate != null)
                        e.Row.Cells[13].Text = dtDueDate.Value.ToString("d");

                    DateTime? dtPlannedCompleteDate = DataBinder.Eval(e.Row.DataItem, "DatePlannedComplete") as DateTime?;
                    if (dtPlannedCompleteDate != null)
                        e.Row.Cells[22].Text = dtPlannedCompleteDate.Value.ToString("d");

                    DateTime? dtPlannedSubmittedDate = DataBinder.Eval(e.Row.DataItem, "DatePlannedSubmitted") as DateTime?;
                    if (dtPlannedSubmittedDate != null)
                        e.Row.Cells[23].Text = dtPlannedSubmittedDate.Value.ToString("d");

                    DateTime? dtEffIReviewDate = DataBinder.Eval(e.Row.DataItem, "EffIReviewDate") as DateTime?;
                    if (dtEffIReviewDate != null)
                        e.Row.Cells[30].Text = dtEffIReviewDate.Value.ToString("d");

                    DateTime? dtClosedDate = DataBinder.Eval(e.Row.DataItem, "DateClosed") as DateTime?;
                    if (dtClosedDate != null)
                        e.Row.Cells[35].Text = dtClosedDate.Value.ToString("d");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Export.aspx", "grvCountResult_RowDataBound", ex.Message);
            }            
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 08-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will redirect user to car  main screen .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgBtnCloseCar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Redirect("CAR_Main.aspx", false);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog("CAR", "CAR_Export.aspx", "imgBtnCloseCar_Click", ex.Message);
            }
            
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 30-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective: Bind year dropdown with last four years from current year.
        /// </summary>
        private void BindYears()
        {

            int year = DateTime.Now.Year - 4;
            try
            {
                for (int y = year; y <= DateTime.Now.Year; y++)
                {
                    ddlYear.Items.Add(new ListItem(y.ToString()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ddlYear.Items.Insert(0, new ListItem("- - - Select - - -", "0"));
        }

        /// <summary>
        /// Created By: HytechPro
        /// Created Date: 30-06-2015 
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
                    ddlCustomer.DataSource = issuedBy;
                    ddlCustomer.DataTextField = "IssuedBy";
                    ddlCustomer.DataValueField = "DBKey";
                    ddlCustomer.DataBind();
                    ddlCustomer.Items.Insert(0, new ListItem("- - - Select - - -", "0"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created Date: 30-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will bind reference dropdown from database. 
        /// </summary>
        /// <param name="carType"></param>
        private void BindReferences(string carType)
        {
            try
            {
                var references = clsCAR.GetAllRefernces(carType);
                if (references.Count() > 0)
                {
                    var lstReferences = references.Select(x => new { x.Reference, x.DBKey }).Distinct();
                    if (lstReferences.Count() > 0)
                    {
                        ddlRefrence.DataSource = lstReferences;
                        ddlRefrence.DataTextField = "Reference";
                        ddlRefrence.DataValueField = "DBKey";
                        ddlRefrence.DataBind();
                        ddlRefrence.Items.Insert(0, new ListItem("- - - Select - - -", "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created Date: 30-06-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will give values of checkboxlist selected item seperated with comma. 
        /// </summary>
        /// <returns></returns>
        private string GetCommaSeperatedValues()
        {
            string[] chkListItems;
            ArrayList chkListSelections = new ArrayList();
            try
            {
                foreach (ListItem item in chkListExport.Items)
                {
                    if (item.Selected)
                    {
                        chkListSelections.Add(item.Value);
                    }
                }

                chkListItems = (string[])chkListSelections.ToArray(typeof(string));
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return string.Join(",", chkListItems);
        }

        /// <summary>
        /// Created Date: 1-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will give all car record for selected car type. 
        /// </summary>
        /// <param name="carType"></param>
        /// <returns></returns>
        private List<CAR> GetAllCarData(string carType)
        {
            var allCarData = new List<CAR>();
            try
            {
                allCarData = clsCAR.GetCarData(carType);
                ViewState["AllCar"] = allCarData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return allCarData;
        }

        /// <summary>
        /// Created Date: 1-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will give all references from database. 
        /// </summary>
        /// <param name="carType"></param>
        /// <returns></returns>
        private List<CAR_Reference> GetReferences(string carType)
        {
            var listReferences = new List<CAR_Reference>();
            try
            {
                listReferences = clsCAR.GetAllRefernces(carType);
                ViewState["lstReference"] = listReferences;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listReferences;
        }

        /// <summary>
        /// Created Date: 1-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method convert list to datatable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table;
            try
            {
                PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
                table = new DataTable();
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return table;
        }

        /// <summary>
        /// Created Date: 2-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will export data to excel.  
        /// </summary>
        /// <param name="dataExport"></param>
        private void ExportToExcel(List<CAR> dataExport)
        {
            ECRFEntities context = new ECRFEntities();
            divCountResult.Visible = false;
            divColumns.Visible = true;
            try
            {
                var lstProject = clsCAR.GetCarProjectDetail();
                var lstDivId = dataExport.Where(x => (x.CAR_DefaultAssigneeIssuedTo.DivisionId != null)).Select(x => new { x.CAR_DefaultAssigneeIssuedTo.DivisionId, x.DBKey }).Distinct();
                var carToExport = (from objcar in dataExport
                                   join objDefaultAssignee in context.CAR_DefaultAssigneeIssuedTo on objcar.FK_DBKeyDefaultAssigneeIssuedTo equals objDefaultAssignee.DBKey
                                   join objDivision in context.DIVISIONS on objcar.CAR_DefaultAssigneeIssuedTo.DivisionId equals objDivision.DivID
                                   join objproject in context.CAR_Project on objcar.FK_DBKeyProject equals objproject.DBKey
                                   join objFrom in context.CAR_From on objcar.FK_DBKeyFrom equals objFrom.DBKey
                                   join objIssued in context.CAR_IssuedBy on objcar.FK_DBKeyIssuedBy equals objIssued.DBKey
                                   join objTo in context.CAR_To on objcar.FK_DBKeyTo equals objTo.DBKey

                                   select new clsCarCustom
                                   {
                                       Type = objcar.Type,
                                       Severity = objcar.Severity,
                                       DateIssued = objcar.DateIssued,
                                       CarNumber = objcar.CarNumber,
                                       IssuedBy = objIssued.IssuedBy,
                                       FromName = objFrom.FromName,
                                       DivisionName = objDivision.DivisionName,
                                       ProjectID = objproject.ProjectID,
                                       ShipName = objproject.ShipName,
                                       HullNumber = objproject.HullNumber,
                                       SystemAffected = objcar.SystemAffected,
                                       ItemAffected = objcar.ItemAffected,
                                       ActionID = objcar.ActionID,
                                       PreparedBy = objcar.PreparedBy,
                                       ItemNumber = objcar.ItemNumber,
                                       ContractPoNumber = objproject.ContractPONumber,
                                       Drawing = objcar.Drawing,
                                       Assignment = objDefaultAssignee.Assignment,
                                       PresNotification = objcar.PresNotification,
                                       Discrepancy = objcar.Discrepancy,
                                       DateDue = objcar.DateDue,
                                       ToName = objTo.ToName,
                                       PerfMeasures = objcar.PerfMeasures,
                                       CorpObj = objcar.CorpObj,
                                       ResourceReq = objcar.ResourceReq,
                                       KeyBusProcess = objcar.KeyBusProcess,
                                       Other = objcar.Other,
                                       RootCause = objcar.RootCause,
                                       CorrectiveAction = objcar.CorrectiveAction,
                                       PreventativeAction = objcar.PreventativeAction,
                                       DatePlannedComplete = objcar.DatePlannedComplete,
                                       DatePlannedSubmitted = objcar.DatePlannedSubmitted,
                                       SubmittedBy = objcar.SubmittedBy,
                                       DateSubmitted = objcar.DateSubmitted,
                                       Satisfactory = objcar.Satisfactory,
                                       Accepted = objcar.Accepted,
                                       Comments = objcar.Comments,
                                       EffReviewBy = objcar.EffReviewBy,
                                       EffIReviewDate = objcar.EffIReviewDate,
                                       Effective = objcar.Effective,
                                       Evidence = objcar.Evidence,
                                       CloseCar = objcar.CloseCar,
                                       ClosedOutBy = objcar.ClosedOutBy,
                                       DateClosed = objcar.DateClosed,
                                       Status = objcar.Status,
                                   }).ToList();

                if (carToExport != null)
                {
                    txtCountQaData.Text = carToExport.Count().ToString();
                    dtCar = ConvertToDataTable(carToExport);

                    foreach (ListItem item in chkListExport.Items)
                    {
                        if (!item.Selected)
                        {
                            string columName = item.Value;

                            if (!string.IsNullOrEmpty(columName))
                                dtCar.Columns.Remove(columName);
                        }
                    }

                    DeleteExtraColumns(dtCar);
                    dtCar.AcceptChanges();

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/ms-excel";
                    HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                    HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "exportCar.xls"));
                    HttpContext.Current.Response.Charset = "utf-8";
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                    //sets font
                    HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                    HttpContext.Current.Response.Write("<BR><BR><BR>");
                    HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='2' cellPadding='2' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                    string str = "\t";

                    foreach (DataColumn dtcol in dtCar.Columns)
                    {
                        HttpContext.Current.Response.Write("<Td>");
                        HttpContext.Current.Response.Write("<B>");
                        Response.Write(str + dtcol.ColumnName);
                        HttpContext.Current.Response.Write("</B>");
                        HttpContext.Current.Response.Write("</Td>");
                    }
                    HttpContext.Current.Response.Write("</TR>");
                    foreach (DataRow dr in dtCar.Rows)
                    {
                        string strCarNumber = string.Empty;
                        string strCarType = string.Empty;
                        string strCarYear = string.Empty;
                        int flag = 0;
                        HttpContext.Current.Response.Write("<TR>");
                        for (int j = 0; j < dtCar.Columns.Count; j++)
                        {
                            HttpContext.Current.Response.Write("<Td>");
                            if (dtCar.Columns[j].ColumnName == "Severity")
                            {
                                if (dr[j].ToString() == "true")
                                    Response.Write(str + "Major");
                                else
                                    Response.Write(str + "Minor");
                            }
                            else if ((dtCar.Columns[j].ColumnName == "Type") || (dtCar.Columns[j].ColumnName == "CarNumber") || (dtCar.Columns[j].ColumnName == "DateIssued"))
                            {
                                if (dtCar.Columns[j].ColumnName == "Type")
                                {
                                    strCarType = Convert.ToString(dr[j]);
                                    Response.Write(str + Convert.ToString(dr[j]));
                                    flag = flag + 1;
                                }
                                if (dtCar.Columns[j].ColumnName == "CarNumber")
                                {
                                    strCarNumber = Convert.ToString(dr[j]);
                                    flag = flag + 1;
                                }
                                if (dtCar.Columns[j].ColumnName == "DateIssued")
                                {
                                    strCarYear = Convert.ToString(dr[j]);
                                    strCarYear = strCarYear.Substring(8, 2);
                                    flag = flag + 1;
                                    Response.Write(str + Convert.ToString(dr[j]));
                                }
                            }
                            else
                            {
                                Response.Write(str + Convert.ToString(dr[j]));
                            }
                            if (flag == 3 && (dtCar.Columns[j].ColumnName == "CarNumber"))
                                Response.Write(str + strCarType + "-" + strCarYear + "-" + strCarNumber);

                            HttpContext.Current.Response.Write("</Td>");
                        }
                    }
                    HttpContext.Current.Response.Write("</TR>");
                    HttpContext.Current.Response.Write("</Table>");
                    HttpContext.Current.Response.Write("</font>");
                    divColumns.Visible = true;
                    divCountResult.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// Created Date: 2-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will give delete all extra column from datatable which are not used to export in excel.
        /// </summary>
        /// <param name="dtCar"></param>
        private void DeleteExtraColumns(DataTable dtCar)
        {
            try
            {
                if (dtCar.Columns["DBKey"].ColumnName == "DBKey")
                    dtCar.Columns.Remove("DBKey");
                if (dtCar.Columns["DivID"].ColumnName == "DivID")
                    dtCar.Columns.Remove("DivID");
                if (dtCar.Columns["projectDBkey"].ColumnName == "projectDBkey")
                    dtCar.Columns.Remove("projectDBkey");
                if (dtCar.Columns["FK_DBKeyIssuedBy"].ColumnName == "FK_DBKeyIssuedBy")
                    dtCar.Columns.Remove("FK_DBKeyIssuedBy");
                if (dtCar.Columns["FK_DBKeyFrom"].ColumnName == "FK_DBKeyFrom")
                    dtCar.Columns.Remove("FK_DBKeyFrom");
                if (dtCar.Columns["FK_DBKeyIssuedTo"].ColumnName == "FK_DBKeyIssuedTo")
                    dtCar.Columns.Remove("FK_DBKeyIssuedTo");
                if (dtCar.Columns["FK_DBKeyProject"].ColumnName == "FK_DBKeyProject")
                    dtCar.Columns.Remove("FK_DBKeyProject");
                if (dtCar.Columns["FK_DBKeyDefaultAssigneeIssuedTo"].ColumnName == "FK_DBKeyDefaultAssigneeIssuedTo")
                    dtCar.Columns.Remove("FK_DBKeyDefaultAssigneeIssuedTo");
                if (dtCar.Columns["FK_DBKeyTo"].ColumnName == "FK_DBKeyTo")
                    dtCar.Columns.Remove("FK_DBKeyTo");
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        /// <summary>
        /// Created Date: 8-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will give delete all extra column from datatable which are not used to show in countGrid in export.
        /// </summary>
        /// <param name="dtCar"></param>
        private void DeleteExtraColumnsForCount(DataTable dtCar)
        {
            try
            {
                if (dtCar.Columns["DBKey"].ColumnName == "DBKey")
                    dtCar.Columns.Remove("DBKey");
                if (dtCar.Columns["FK_DBKeyIssuedBy"].ColumnName == "FK_DBKeyIssuedBy")
                    dtCar.Columns.Remove("FK_DBKeyIssuedBy");
                if (dtCar.Columns["FK_DBKeyFrom"].ColumnName == "FK_DBKeyFrom")
                    dtCar.Columns.Remove("FK_DBKeyFrom");
                if (dtCar.Columns["FK_DBKeyIssuedTo"].ColumnName == "FK_DBKeyIssuedTo")
                    dtCar.Columns.Remove("FK_DBKeyIssuedTo");
                if (dtCar.Columns["FK_DBKeyProject"].ColumnName == "FK_DBKeyProject")
                    dtCar.Columns.Remove("FK_DBKeyProject");
                if (dtCar.Columns["FK_DBKeyDefaultAssigneeIssuedTo"].ColumnName == "FK_DBKeyDefaultAssigneeIssuedTo")
                    dtCar.Columns.Remove("FK_DBKeyDefaultAssigneeIssuedTo");
                if (dtCar.Columns["FK_DBKeyTo"].ColumnName == "FK_DBKeyTo")
                    dtCar.Columns.Remove("FK_DBKeyTo");
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        /// <summary>
        /// Created Date: 8-07-2015 
        /// Modified By: 
        /// Modified Date: 
        /// Objective:This method will remove all columns from datatable which are not checked for export and bind datasource to gridview.
        /// </summary>
        /// <param name="lstCountData"></param>
        private void ShowCountDataGrid(List<CAR> lstCountData)
        {
            DataTable dtcount = new DataTable();
            divCountResult.Visible = true;
            divColumns.Visible = false;
            try
            {
                if (lstCountData != null)
                {
                    var countData = lstCountData;
                    dtcount = ConvertToDataTable(lstCountData);
                    foreach (ListItem li in chkListExport.Items)
                    {
                        if (!li.Selected)
                        {
                            string columName = li.Value;

                            if (!string.IsNullOrEmpty(columName))
                                dtcount.Columns.Remove(columName);
                        }
                    }
                    DeleteExtraColumnsForCount(dtcount);
                    dtcount.AcceptChanges();
                    if (dtcount.Rows != null)
                    {
                        grvCountResult.DataSource = dtcount;
                        grvCountResult.DataBind();
                    }                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        private void GitHubClinetCommit()
        {
            int commitId;
            string commitName;
        }
         private void WebGitCommit()
        {
            int commitId;
            string commitName;
        }
        #endregion       
    }
}
