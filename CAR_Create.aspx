<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="CAR_Create.aspx.cs" Inherits="ProjectOrganizer.Web.Car.CAR_Create" %>

<%-- ccdd start--%>
<%--<%@ Register TagPrefix="uc" TagName="WebUserControl" Src="~/Car/CustomDropdown.ascx" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %--%>
<%-- ccdd end--%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- ccdd start--%>
    <%--    <link href="../css/CAR/EditableDropDown/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../js/Car/EditableDropDown/jquery-1.6.4.min.js"></script>
    <script src="../js/Car/EditableDropDown/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../js/Car/EditableDropDown/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../js/Car/EditableDropDown/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../js/Car/EditableDropDown/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../js/Car/EditableDropDown/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../js/Car/EditableDropDown/jquery.ui.combobox.js" type="text/javascript"></script>--%>
    <%-- ccdd end--%>

    <script type="text/javascript" src="../js/Car/jquery.min.js"></script>
    <link href='http://fonts.googleapis.com/css?family=Roboto' rel='stylesheet' type='text/css' />
    <link href="../css/CAR/Create/style.css" type="text/css" rel="stylesheet" />
    <link href="../css/CAR/Create/jquery.selectbox-0.2.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../js/car/jquery.selectbox-0.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('select').selectBox();
            $('select').selectBox('settings', {
                'mobile': 'true',
                'menuTransition': 'fade'
            });

            $('#btnAddDel').click(function () {
                $('#lstboxRefs option').each(function (i) {
                    $('#lstboxRefs option').appendTo('#lstboxAddRefs');
                });
            });
          
        });

        function GetDropDownCss() {
            $('select').selectBox();
            $('select').selectBox('settings', {
                'mobile': 'true',
                'menuTransition': 'fade'
            });
        }


    </script>
    <script type="text/javascript">

        function ClearProjectPopup() {
            $get('<%=txtAddProjectId.ClientID%>').value = "";
            $get('<%=txtAddShipName.ClientID%>').value = "";
            $get('<%=txtAddHullNumber.ClientID%>').value = "";
            $get('<%=txtAddContract.ClientID%>').value = "";
        }

       
    </script>
    <style type="text/css">
        .Background {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .Popup {
            background-color: #FFFFFF;
            border-width: 1px;
            width: 465px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updpnlcontrols" runat="server" UpdateMode="Conditional" cssclass="createFormpar-content">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(GetDropDownCss);
            </script>

            <div class="createFormpar">
                <div class="header">
                    <asp:Label ID="lblHeader" runat="server"></asp:Label>
                    <a>
                        <asp:ImageButton ID="imgBtnCloseCar" runat="server" ImageUrl="../images/Car_Images/Create/cancel.png" OnClick="imgBtnCloseCar_Click" />
                    </a>

                    <%--<a href="#">
                        <img src="../images/Car_Images/Create/cancel.png" alt="" onclick="ImageClick()"/>
                        
                    </a>--%>
                </div>

                <div class="createFormpar-content">
                    <div class="create">
                        <h3>Create</h3>


                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td width="80px">
                                    <label>CAR Number:</label></td>
                                <td>
                                    <asp:TextBox ID="txtCarNumber" runat="server" ReadOnly="True"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCarNumber" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtCarNumber" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                                <td width="100px">
                                    <label>Severity :</label></td>
                                <td>
                                    <asp:RadioButton ID="rdbtnMajor" runat="server" Text="Major" GroupName="severity" />
                                    <asp:RadioButton ID="rdbtnMinor" runat="server" Text="Minor" GroupName="severity" AutoPostBack="True" Checked="True" />
                                </td>
                                <td width="100px">Date Isued :</td>
                                <td>
                                    <asp:TextBox ID="txtDateIssued" runat="server" CssClass="datepic"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clndrdateissued" runat="server" TargetControlID="txtDateIssued" Format="MM-dd-yyyy"></ajax:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvDateIssued" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtDateIssued" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Issued By :</label></td>
                                <td>
                                    <%--<editable:EditableDropDownList ID="ddlIssuedBy" runat="server" ClientIDMode="Static" Sorted="true"></editable:EditableDropDownList>--%>

                                    <asp:DropDownList ID="ddlIssuedBy" runat="server" CssClass="selectBox">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvddlIssuedBy" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="ddlIssuedBy" SetFocusOnError="true" ValidationGroup="saveCar" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <label>From :</label></td>
                                <td>
                                    <asp:DropDownList ID="ddlFrom" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvddlFrom" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="ddlFrom" SetFocusOnError="true" ValidationGroup="saveCar" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                                <td>To :</td>
                                <td>
                                    <asp:DropDownList ID="ddlTo" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvddlTo" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="ddlTo" SetFocusOnError="true" ValidationGroup="saveCar" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblproject" runat="server">Project ID :</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProjectId" CssClass="dotted" placeholder="Click here to select" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvProject" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtProjectId" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <label>Ref(s) :</label></td>
                                <td class="listBox">
                                    <asp:ListBox ID="lstboxRefs" runat="server" Rows="1" SelectionMode="Multiple" Style="width: 99px; display: inline-block; float: left;"></asp:ListBox>
                                    <asp:Button ID="btnAddDel" runat="server" Text="Add/Del" CssClass="add-del" OnClick="btnAddDel_Click" />
                                </td>
                                <td>Sys Affected :</td>
                                <td>
                                    <asp:TextBox ID="txtsysAffected" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSysAffected" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtsysAffected" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Hull :</label></td>
                                <td>
                                    <asp:TextBox ID="txtHull" runat="server" CssClass="dotted" placeholder="Click here to select"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvHull" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtHull" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <label>Issued To :</label></td>
                                <td>
                                    <asp:DropDownList ID="ddlIssuedTo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlIssuedTo_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvIssuedTo" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="ddlIssuedTo" SetFocusOnError="true" ValidationGroup="saveCar" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                                <td>DWG # :</td>
                                <td>
                                    <asp:TextBox ID="txtDwgNo" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDrwng" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtDwgNo" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Ship :</label></td>
                                <td>
                                    <asp:TextBox ID="txtShip" runat="server" CssClass="dotted" placeholder="Click here to select"></asp:TextBox></td>
                                <asp:RequiredFieldValidator ID="rfvShip" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtShip" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                <td>
                                    <label>Assigned To :</label></td>
                                <td>
                                    <asp:TextBox ID="txtAssignedTo" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAssignedTo" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtAssignedTo" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>

                                </td>
                                <td>Item Affected :</td>
                                <td>
                                    <asp:TextBox ID="txtItemAffected" runat="server"></asp:TextBox></td>
                                <asp:RequiredFieldValidator ID="rfvItemAffected" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtItemAffected" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                            </tr>
                            <tr>
                                <td>
                                    <label>Cont #/P.O. :</label></td>
                                <td>
                                    <asp:TextBox ID="txtContPo" runat="server" CssClass="dotted" placeholder="Click here to select"></asp:TextBox></td>
                                <asp:RequiredFieldValidator ID="rfvContPO" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtContPo" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                <td>
                                    <label>Date President:</label></td>
                                <td>
                                    <asp:TextBox ID="txtDatePresident" CssClass="datepic" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clndrDatePresident" runat="server" TargetControlID="txtDatePresident" Format="MM-dd-yyyy"></ajax:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvDatePress" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtDatePresident" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                                <td>Item Number :</td>
                                <td>
                                    <asp:TextBox ID="txtItemNumber" runat="server"></asp:TextBox></td>
                                <asp:RequiredFieldValidator ID="rfvItemNumber" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtItemNumber" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                            </tr>
                            <tr>
                                <td>
                                    <label>Perpared By :</label></td>
                                <td>
                                    <asp:TextBox ID="txtPrepredBy" runat="server"></asp:TextBox></td>
                                <asp:RequiredFieldValidator ID="rfvPreparedBy" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtPrepredBy" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                <td>
                                    <%-- <label>Deficiency :</label>--%><label>Response :</label></td>
                                <%-- <td colspan="3">
                                    <textarea rows="4" id="txtareaDeficiency" runat="server"></textarea>
                                </td>--%>
                                <td>
                                    <asp:TextBox ID="txtResponseDue" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clndrDateResponse" runat="server" TargetControlID="txtResponseDue" Format="MM-dd-yyyy"></ajax:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvResponse" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtResponseDue" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <label>Customer CAR Number :</label></td>
                                <td>
                                    <asp:TextBox ID="txtCustomerCarNo" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr id="rowDiscrepency" runat="server">
                                <td>
                                    <%--<label>Response :</label>--%>
                                    <label>Deficiency :</label>
                                </td>
                                <td colspan="5">
                                    <%-- <asp:TextBox ID="txtResponseDue" runat="server"></asp:TextBox>--%>
                                    <textarea rows="2" id="txtareaDeficiency" runat="server"></textarea>
                                    <asp:RequiredFieldValidator ID="rfvDiscrep" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtareaDeficiency" SetFocusOnError="true" ValidationGroup="saveCar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="rowMar1" runat="server" style="display: none">
                                <td colspan="2">
                                    <asp:RadioButton ID="rdbtnPerfMeasure" runat="server" Text="New/Change Performance Measure" GroupName="Mar" />
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdbtnCorpObjective" runat="server" Text="New/Change Corporate Objective" GroupName="Mar" />
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdbtnResourceReq" runat="server" Text="New/Change Resource Requirement" GroupName="Mar" />
                                </td>
                            </tr>
                            <tr id="rowMar2" runat="server" style="display: none">
                                <td colspan="2">
                                    <asp:RadioButton ID="rdbtnKeyBussProcess" runat="server" Text="New/Change Key Bussiness Process" GroupName="Mar" />
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdbtnOther" runat="server" Text="Other(Annotate below)" GroupName="Mar" />
                                </td>
                                <td colspan="2">
                                    <textarea rows="2" id="txtareaOther" runat="server"></textarea>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!--create end-->
                    <!--par-response start-->
                    <div class="par-response">
                        <h3>PAR Response</h3>
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td width="32%">
                                    <label>Root Cause</label></td>
                                <td>(<label>Corrective Action - Immediate Response (to correct
             problem, minimize risk or achieve short term benefits)</label></td>
                                <td>Preventative Action - Long term Response: (to pervent
            occurence/recurrence of an issue/achieve long term benefits)</td>
                            </tr>
                            <tr>
                                <td>
                                    <textarea id="txtareaRootCause" runat="server" rows="3"></textarea></td>
                                <td>
                                    <textarea id="txtareaCorrectiveAction" runat="server" rows="3"></textarea></td>
                                <td>
                                    <textarea id="txtareaPrsentativeAction" runat="server" rows="3"></textarea></td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td>
                                    <label>Planned Completion Date :</label></td>
                                <td>
                                    <asp:TextBox ID="txtPlannedCompDate1" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clndrPlannedCompDate1" runat="server" TargetControlID="txtPlannedCompDate1" Format="MM-dd-yyyy"></ajax:CalendarExtender>
                                </td>
                                <td>
                                    <label>Planned Completion Date :</label></td>
                                <td>
                                    <asp:TextBox ID="txtPlannedCompDate2" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clndrPlannedCompDate2" runat="server" TargetControlID="txtPlannedCompDate2" Format="MM-dd-yyyy"></ajax:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Submitted by :</label></td>
                                <td>
                                    <asp:TextBox ID="txtsubmittedBy" runat="server"></asp:TextBox></td>
                                <td>
                                    <label>Date Action Plan Submitted :</label></td>
                                <td>
                                    <asp:TextBox ID="txtDateActionPlannedSubmitted" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clndrDateActionPlanned" runat="server" TargetControlID="txtDateActionPlannedSubmitted" Format="MM-dd-yyyy"></ajax:CalendarExtender>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <!--par-response end-->
                    <!--evalution start-->
                    <div class="evaluation" id="divEvaluation" runat="server">
                        <h3>Evaluation of Action Taken</h3>
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td>
                                    <label><b>Satisfactory?</b></label><asp:RadioButton ID="rdbtnSatisfactoryYes" runat="server" Text="Yes" GroupName="satisfactory" />
                                    <asp:RadioButton ID="rdbtnSatisfactoryNo" runat="server" Text="No" GroupName="satisfactory" />&nbsp;&nbsp;
                                    <label><b>Accepted?</b></label><asp:RadioButton ID="rdbtnAcceptedYes" runat="server" Text="Yes" GroupName="accepted" />
                                    <asp:RadioButton ID="rdbtnAcceptedNo" runat="server" Text="No" GroupName="accepted" /></td>
                                <td>App. by :</td>
                                <td>
                                    <asp:TextBox ID="txtAppBy" runat="server"></asp:TextBox></td>
                                <td>Date :</td>
                                <td>
                                    <asp:TextBox ID="txtApproveDate" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clndrApproveDate" runat="server" TargetControlID="txtApproveDate" Format="MM-dd-yyyy"></ajax:CalendarExtender>
                                </td>
                                <td>Comments :</td>
                                <td>
                                    <asp:TextBox ID="txtComments" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <label><b>Effective?</b></label><asp:RadioButton ID="rdbtnEffectiveYes" runat="server" Text="Yes" GroupName="effective" />
                                    <asp:RadioButton ID="rdbtnEffectiveNo" runat="server" Text="No" GroupName="effective" />&nbsp;&nbsp;&nbsp;&nbsp;
                                    <label><b>Close CAR?</b></label><asp:RadioButton ID="rdbtnCloseCarYes" runat="server" Text="Yes" GroupName="closecar" />
                                    <asp:RadioButton ID="rdbtnCloseCarNo" runat="server" Text="No" GroupName="closecar" /></td>
                                <td>Close by :</td>
                                <td>
                                    <asp:TextBox ID="txtCloseBy" runat="server"></asp:TextBox></td>
                                <td>Date :</td>
                                <td>
                                    <asp:TextBox ID="txtCloseDate" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clndrCloseDate" runat="server" TargetControlID="txtCloseDate" Format="MM-dd-yyyy"></ajax:CalendarExtender>
                                </td>
                                <td>Evidence :</td>
                                <td>
                                    <asp:TextBox ID="txtEvidence" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                        <div>
                            <asp:Button class="inputtext" ID="btnOpenForView" runat="server" Text="Open for View" />
                            <asp:Button class="inputtext" ID="btnOpenForEdit" runat="server" Text="Open for Edits" OnClick="btnOpenForEdit_Click" />
                            <asp:Button class="inputtext" ID="btnSaveCarFile" runat="server" Text="Save CAR file" />
                            <asp:Button class="inputtext" ID="btnPreview" runat="server" Text="Preview" />
                            <asp:Button class="inputtext" ID="btnSaveData" runat="server" Text="Save Data" OnClick="btnSaveData_Click" ValidationGroup="saveCar" />
                            <asp:Button class="inputtext" ID="btnPrintPdf" runat="server" Text="Print PDF" />
                            <asp:Button class="inputtext" ID="btnVoidResc" runat="server" Text="Void/Resc." />
                            <asp:Button class="inputtext" ID="btnNotify" runat="server" Text="Notify" />
                            <asp:Button class="inputtext" ID="btnClear" runat="server" Text="Clear" OnClientClick="resetAllControls();" OnClick="btnClear_Click" />
                        </div>
                    </div>
                    <!--evalution end-->
                </div>
            </div>
            <!-- region mpe Project start-->
            <div>
                <ajax:ModalPopupExtender ID="ModalPopupExtender" runat="server" PopupControlID="pnlProject" TargetControlID="txtProjectId" CancelControlID="anchorCancel" OnCancelScript="ClearProjectPopup()">
                </ajax:ModalPopupExtender>
                <asp:Panel ID="pnlProject" runat="server" align="center" CssClass="Popup" style="display:none">
                    <div class="division">
                        <div class="header">
                            Project <a href="#" id="anchorCancel" runat="server">
                                <img src="../images/Car_Images/Create/cancel.png" alt="" /></a>
                        </div>
                        <div class="division-con">
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <th colspan="4">
                                        <asp:GridView ID="grvProject" runat="server" DataKeyNames="DBKey" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" AutoGenerateSelectButton="True" OnSelectedIndexChanged="grvProject_SelectedIndexChanged" OnPageIndexChanging="grvProject_PageIndexChanging" PageSize="5">
                                            <Columns>
                                                <asp:BoundField HeaderText="ID" DataField="DBKey" Visible="False" />
                                                <asp:BoundField HeaderText="Project Id" DataField="ProjectID" />
                                                <asp:BoundField HeaderText="Ship Name" DataField="ShipName" />
                                                <asp:BoundField HeaderText="Hull Number" DataField="HullNumber" />
                                                <asp:BoundField HeaderText="Contract #/PO" DataField="ContractPONumber" />
                                            </Columns>
                                        </asp:GridView>
                                    </th>
                                </tr>
                                <tr>
                                    <th width="30%" style="text-align: center; padding: 5px 0px 5px 10px;">
                                        <asp:TextBox ID="txtAddProjectId" runat="server" Width="50px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvProjectId" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtAddProjectId" SetFocusOnError="True" ValidationGroup="AddProject"></asp:RequiredFieldValidator>
                                    </th>
                                    <th width="20%" style="text-align: left; padding: 5px 0px 5px 0px;">
                                        <asp:TextBox ID="txtAddShipName" runat="server" Width="50px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAddShipName" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtAddShipName" SetFocusOnError="True" ValidationGroup="AddProject"></asp:RequiredFieldValidator>
                                    </th>
                                    <th width="20%" style="text-align: left; padding: 5px 0px 5px 0px;">
                                        <asp:TextBox ID="txtAddHullNumber" runat="server" Width="50px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAddhullNumber" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtAddHullNumber" SetFocusOnError="True" ValidationGroup="AddProject"></asp:RequiredFieldValidator>
                                    </th>
                                    <th width="30%" style="text-align: left; padding: 5px 0px 5px 0px;">
                                        <asp:TextBox ID="txtAddContract" runat="server" Width="50px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAddContpo" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtAddContract" SetFocusOnError="True" ValidationGroup="AddProject"></asp:RequiredFieldValidator>
                                    </th>
                                </tr>
                                <tr>
                                    <th colspan="4" style="text-align: center;">
                                        <asp:Button ID="btnAddnewProject" CssClass="inputtext" runat="server" Text="Add New/Exit" OnClick="btnAddnewProject_Click" ValidationGroup="AddProject" />
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
                <ajax:ModalPopupExtender ID="mpeHull" runat="server" PopupControlID="pnlProject" TargetControlID="txtHull" CancelControlID="anchorCancel" OnCancelScript="ClearProjectPopup()">
                </ajax:ModalPopupExtender>
                <ajax:ModalPopupExtender ID="mpeShip" runat="server" PopupControlID="pnlProject" TargetControlID="txtShip" CancelControlID="anchorCancel" OnCancelScript="ClearProjectPopup()">
                </ajax:ModalPopupExtender>
                <ajax:ModalPopupExtender ID="mpeContPo" runat="server" PopupControlID="pnlProject" TargetControlID="txtContPo" CancelControlID="anchorCancel" OnCancelScript="ClearProjectPopup()">
                </ajax:ModalPopupExtender>
            </div>
            <!--End region mpe Project -->

            <!-- region popup add reference start -->
            <div>
                <ajax:ModalPopupExtender ID="mpeAddreferences" runat="server" PopupControlID="pnlAddReferences" TargetControlID="btnhiddenReferences" CancelControlID="anchorAddReferenceCancel">
                </ajax:ModalPopupExtender>
                 <asp:Button ID="btnhiddenReferences" runat="server" Text="Button" Style="display: none" />
                <asp:Panel ID="pnlAddReferences" runat="server" align="center" Style="display: none" CssClass="Popup">
                    <div class="division">
                        <div class="header">
                            Add References <a href="#" id="anchorAddReferenceCancel" runat="server">
                                <img src="../images/Car_Images/Create/cancel.png" alt="" /></a>
                        </div>
                        <div class="division-con">
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <th>
                                        <asp:TextBox ID="txtAddRefs" runat="server"></asp:TextBox>
                                    </th>
                                    <th>
                                        <asp:Button ID="btnAddRefrences" runat="server" Text=">>" OnClick="btnAddRefrences_Click" /><br />
                                        <br />
                                        <asp:Button ID="btnDeleteRefs" runat="server" Text="<<" OnClick="btnDeleteRefs_Click" />
                                    </th>
                                    <th>
                                        <asp:ListBox ID="lstboxAddRefs" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </th>
                                </tr>
                                <tr style="text-align: center">
                                    <th colspan="3" style="text-align: center;">
                                        <asp:Button ID="btnDone" CssClass="inputtext" runat="server" Text="Done" OnClick="btnDone_Click" />
                                    </th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <!-- end region popup add reference -->
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlIssuedTo" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="grvProject" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnDone" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnAddRefrences" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnDeleteRefs" EventName="Click" />

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

