<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="CAR_Export.aspx.cs" Inherits="ProjectOrganizer.Web.Car.CAR_Export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <link href='http://fonts.googleapis.com/css?family=Roboto' rel='stylesheet' type='text/css' />
    <link href="../css/CAR/Create/style.css" type="text/css" rel="stylesheet" />
    <link href="../css/CAR/Create/jquery.selectbox-0.2.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/Car/jquery.selectbox-0.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('select').selectBox();
            $('select').selectBox('settings', {
                'mobile': 'true',
                'menuTransition': 'fade'
            });

            $("#chkAll").on("click", function () {
                var all = $(this);
                $('input:checkbox').each(function () {
                    $(this).prop('checked', all.prop('checked'));
                    $('#chkBoxIncludeRefs').attr('checked', false);
                });
                
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="createFormpar">
        <div class="header">
            Export Data<a>
                <asp:ImageButton ID="imgBtnCloseCar" runat="server" ImageUrl="../images/Car_Images/Create/cancel.png" OnClick="imgBtnCloseCar_Click" />
            </a>
        </div>
        <div class="export-data clearfix">
            <p class="heading2">East Coast Repair and Fabrication Correction Action Request (CAR) Log</p>

            <div class="filters">
                <h3>Filters</h3>
                <div class="form">
                    <asp:RadioButton ID="rdbCustomFilter" runat="server" Text="Custom Filter" GroupName="filter" Checked="true" AutoPostBack="True" OnCheckedChanged="rdbCustomFilter_CheckedChanged" />
                    &nbsp;  &nbsp; &nbsp;<asp:RadioButton ID="rdbCarSelectionFilter" runat="server" Text="Car Selection Filter" GroupName="filter" AutoPostBack="True" OnCheckedChanged="rdbCarSelectionFilter_CheckedChanged" />
                    <div class="select-filters">
                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                    </div>
                    <div class="select-filters">
                        <asp:DropDownList ID="ddlCustomer" runat="server"></asp:DropDownList>
                    </div>
                    <div class="select-filters">
                        <asp:DropDownList ID="ddlRefrence" runat="server"></asp:DropDownList>
                    </div>
                    <p class="input-filters clearfix">
                        <label>Starting CAR#</label>
                        <asp:TextBox ID="txtStartingCar" runat="server"></asp:TextBox>
                    </p>
                    <p class="input-filters clearfix">
                        <label>Ending CAR#</label>
                        <asp:TextBox ID="txtEndingCar" runat="server"></asp:TextBox>
                    </p>
                    <p><em><strong>Double-Click to edit (must include first 5)</strong></em></p>
                    <div>
                        <asp:CheckBox ID="chkBoxIncludeRefs" runat="server" Text="Include References" ClientIDMode="Static" />
                       <%-- <input type="checkbox" id="chkBoxIncludeRefs"  name="Include References" value="Include References"/>--%>
                        <asp:Button ID="btnCountQa" runat="server" Text="Count Qualifying data" CssClass="gray" OnClick="btnCountQa_Click" />
                        <asp:TextBox ID="txtCountQaData" runat="server" Style="margin: 6px 0 0; width: 35px;"></asp:TextBox>
                    </div>
                    <div align="center">
                        <asp:Button ID="btnExport" runat="server" Style="padding: 10px 20px" Text="Export" CssClass="blue" OnClick="btnExport_Click" />
                        <asp:Button ID="btnExit" runat="server" Style="padding: 10px 20px" Text="Exit" CssClass="gray" />
                    </div>
                </div>
            </div>
            <!--filters end-->

            <div class="Export-File">
                <h3>Export File Format</h3>
                <%-- Division Export Colomn selection Start--%>
                <div class="form" id="divExportColumns">
                    <b><label><input type="checkbox" id="chkAll" name="Mark All" checked="checked"/>Check/Uncheck All</label></b>
                    <div id="divColumns" runat="server" style="border: 1px solid #cccccc; height: 396px; overflow: auto;">
                        
                        <asp:CheckBoxList ID="chkListExport" runat="server">
                            <asp:ListItem Text="Issued By" Value="IssuedBy"> </asp:ListItem>
                            <asp:ListItem Text="From" Value="FromName"></asp:ListItem>
                            <asp:ListItem Text="Issued To" Value="DivisionName"></asp:ListItem>
                            <asp:ListItem Text="Project Id" Value="ProjectID"></asp:ListItem>
                            <asp:ListItem Text="Ship Name" Value="ShipName"></asp:ListItem>
                            <asp:ListItem Text="Hull Number" Value="HullNumber"></asp:ListItem>
                            <asp:ListItem Text="System Affected" Value="SystemAffected"></asp:ListItem>
                            <asp:ListItem Text="Item Affected" Value="ItemAffected"></asp:ListItem>
                            <asp:ListItem Text="Action Id" Value="ActionID"></asp:ListItem>
                            <asp:ListItem Text="Prepared By" Value="PreparedBy"></asp:ListItem>
                            <asp:ListItem Text="Item Number" Value="ItemNumber"></asp:ListItem>
                            <asp:ListItem Text="Contract/Po" Value="ContractPoNumber"></asp:ListItem>
                            <asp:ListItem Text="Drawing" Value="Drawing"></asp:ListItem>
                            <asp:ListItem Text="Assigned To" Value="Assignment"></asp:ListItem>
                            <asp:ListItem Text="Pres. Notification" Value="PresNotification"></asp:ListItem>
                            <asp:ListItem Text="Discrepancy" Value="Discrepancy"></asp:ListItem>
                            <asp:ListItem Text="Date Due" Value="DateDue"></asp:ListItem>
                            <asp:ListItem Text="Pref Measure" Value="PerfMeasures"></asp:ListItem>
                            <asp:ListItem Text="To" Value="ToName"></asp:ListItem>
                            <asp:ListItem Text="Corp Objective" Value="CorpObj"></asp:ListItem>
                            <asp:ListItem Text="Resource Request" Value="ResourceReq"></asp:ListItem>
                            <asp:ListItem Text="Key Bus Process" Value="KeyBusProcess"></asp:ListItem>
                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            <asp:ListItem Text="Root Cause" Value="RootCause"></asp:ListItem>
                            <asp:ListItem Text="Coorective Action" Value="CorrectiveAction"></asp:ListItem>
                            <asp:ListItem Text="Preventative Action" Value="PreventativeAction"></asp:ListItem>
                            <asp:ListItem Text="Planned Completion Date" Value="DatePlannedComplete"></asp:ListItem>
                            <asp:ListItem Text="Planned Date Submitted" Value="DatePlannedSubmitted"></asp:ListItem>
                            <asp:ListItem Text="Submitted By" Value="SubmittedBy"></asp:ListItem>
                            <asp:ListItem Text="Date Submitted" Value="DateSubmitted"></asp:ListItem>
                            <asp:ListItem Text="SatisFactory" Value="Satisfactory"></asp:ListItem>
                            <asp:ListItem Text="Accepted" Value="Accepted"></asp:ListItem>
                            <asp:ListItem Text="Comments" Value="Comments"></asp:ListItem>
                            <asp:ListItem Text="Eff.Reveiew By" Value="EffReviewBy"></asp:ListItem>
                            <asp:ListItem Text="Eff.IReviewDate" Value="EffIReviewDate"></asp:ListItem>
                            <asp:ListItem Text="Effective" Value="Effective"></asp:ListItem>
                            <asp:ListItem Text="Evidence" Value="Evidence"></asp:ListItem>
                            <asp:ListItem Text="ClosedCar" Value="CloseCar"></asp:ListItem>
                            <asp:ListItem Text="Closed Out By" Value="ClosedOutBy"></asp:ListItem>
                            <asp:ListItem Text="Date Closed" Value="DateClosed"></asp:ListItem>
                            <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                    <div id="divCountResult" runat="server" visible="false" style="border: 1px solid #cccccc; height: 396px; overflow: auto;">
                        <asp:GridView ID="grvCountResult" runat="server" OnRowDataBound="grvCountResult_RowDataBound">
                        </asp:GridView>
                    </div>
                </div>
                <%-- Division Export Colomn selection end--%>               
            </div>
            <!--filters end-->
        </div>
        <!--export datea end-->
    </div>
</asp:Content>
