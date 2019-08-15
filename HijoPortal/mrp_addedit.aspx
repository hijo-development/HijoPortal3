﻿<%@ Page Title="MOP Add/Edit" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="mrp_addedit.aspx.cs" Inherits="HijoPortal.mrp_addedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="jquery/MRPAddEdit.js"></script>
    <style>
        .width_for_chkbx {
            width: 8%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:TextBox ID="TextBoxLoading" runat="server" Visible="true" Style="display: none;"></asp:TextBox>
    <ajaxToolkit:ModalPopupExtender runat="server"
        ID="ModalPopupExtenderLoading"
        BackgroundCssClass="modalBackground"
        PopupControlID="PanelLoading"
        TargetControlID="TextBoxLoading"
        CancelControlID="ButtonErrorOK1"
        ClientIDMode="Static">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelLoading" runat="server"
        CssClass="modalPopupLoading"
        Height="200px"
        Width="200px"
        align="center"
        Style="display: none;">
        <img src="images/Loading.gif" style="height: 200px; width: 200px;" />
        <asp:Button ID="ButtonErrorOK1" runat="server" CssClass="buttons" Width="30%" Text="OK" Style="display: none;" />
    </asp:Panel>


    <dx:ASPxPopupControl ID="PopUpDelete" ClientInstanceName="PopUpDelete" runat="server" CloseAction="CloseButton" Modal="true" PopupAnimationType="Fade" CloseAnimationType="Fade" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="true" Theme="Moderno" Width="100%">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td colspan="2" style="padding-right: 20px; padding-bottom: 20px;">
                            <dx:ASPxLabel runat="server" Text="Delete this row?" Theme="Moderno" Width="300"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <dx:ASPxButton ID="OK_DELETE" runat="server" Text="OK" Theme="Moderno" AutoPostBack="false">
                                <ClientSideEvents Click="OK_DELETE" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="CANCEL_DELETE" runat="server" Text="CANCEL" Theme="Moderno" AutoPostBack="false">
                                <ClientSideEvents Click="CANCEL_DELETE" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>

            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="MRPNotify" ClientInstanceName="Add_Edit_MRPNotify" runat="server" CloseAction="CloseButton" Modal="true" PopupAnimationType="Fade" CloseAnimationType="Fade" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="true" Theme="Moderno" Width="100%" ContentStyle-Paddings-Padding="20">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <dx:ASPxLabel ID="MRPNotificationMessage" ClientInstanceName="Add_Edit_MRPNotificationMessage" runat="server" Text="" ForeColor="Red" Theme="Moderno" Width="300"></dx:ASPxLabel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="PopupSubmit" ClientInstanceName="PopupSubmit" runat="server" CloseAction="CloseButton" Modal="true" PopupAnimationType="Fade" CloseAnimationType="Fade" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="true" Theme="Moderno" Width="400px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td colspan="2" style="padding-right: 20px; padding-bottom: 20px;">
                            <dx:ASPxLabel runat="server" Text="Are you sure you want to submit this document?" Theme="Moderno" Width="300"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <dx:ASPxButton ID="OK_SUBMIT" runat="server" Text="SUBMIT" Theme="Moderno" OnClick="Submit_Click" AutoPostBack="false">
                                <ClientSideEvents Click="function(s,e){
                                    PopupSubmit.Hide();
                                    $find('ModalPopupExtenderLoading').show();
                                    e.processOnServer = true;
                                    }" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="CANCEL_SUBMIT" runat="server" Text="CANCEL" Theme="Moderno" AutoPostBack="false">
                                <ClientSideEvents Click="function(s,e){PopupSubmit.Hide();}" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <%--<dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" Theme="Office2010Blue" HorizontalAlign="Center" VerticalAlign="Middle" ClientInstanceName="loadingPanel" Modal="true"></dx:ASPxLoadingPanel>--%>

    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%" Height="100%" ScrollBars="Auto">
        <PanelCollection>
            <dx:PanelContent>
                <div id="dvHeader">
                    <h1>M O P  Details</h1>


                    <%--<table class="mrp-add-form-table" style="width: 100%; padding: 25px; margin-bottom: 5px;" border="0">--%>
                    <table border="0">
                        <tr>
                            <td style="width: 12%">
                                <dx:ASPxLabel runat="server" Text="MRP Number" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>:</td>
                            <td colspan="4">
                                <dx:ASPxLabel ID="DocNum" runat="server" Text="" Theme="Office2010Blue" Style="font-size: medium; font-weight: bold; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;"></dx:ASPxLabel>
                            </td>
                            <td style="width: 40%; text-align: right;" rowspan="2">
                                <%--<span style="font-size: 30px; cursor: pointer" onclick="openNav()">&#9776;</span>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel runat="server" Text="MONTH" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>:</td>
                            <td style="width: 20%">
                                <dx:ASPxLabel ID="Month" runat="server" Text="ASPxLabel" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td style="width: 8%">
                                <dx:ASPxLabel runat="server" Text="DATE CREATED" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>:</td>
                            <td style="width: 20%">
                                <dx:ASPxLabel ID="DateCreated" runat="server" Text="" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel runat="server" Text="YEAR" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>:</td>
                            <td>
                                <dx:ASPxLabel ID="Year" runat="server" Text="" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxLabel runat="server" Text="ENTITY" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>:</td>
                            <td colspan="2">
                                <dx:ASPxLabel ID="EntityCode" runat="server" Text="" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                                <div style="display: none;">
                                    <dx:ASPxLabel ID="Entity" runat="server" Text="" ClientInstanceName="EntityCodeAddEditDirect" Theme="Office2010Blue"></dx:ASPxLabel>
                                    <dx:ASPxLabel ID="BU" runat="server" Text="" ClientInstanceName="BUCodeAddEditDirect" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                                </div>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel runat="server" Text="DEPARTMENT" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>:</td>
                            <td>
                                <dx:ASPxLabel ID="BUCode" runat="server" Text="" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxLabel runat="server" Text="STATUS" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>:</td>
                            <td>
                                <dx:ASPxLabel runat="server" ID="Status" Text="STATUS" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>
                                <%--<dx:ASPxHiddenField ID="statusKey" runat="server" ClientInstanceName="statusKeyDirect"></dx:ASPxHiddenField>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel runat="server" Text="CREATOR" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td>:</td>
                            <td>
                                <dx:ASPxLabel ID="Creator" runat="server" Text="" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <div style="display: none;">
                                    <dx:ASPxTextBox ID="WorkFlowLineTxt" ClientInstanceName="WorkFlowLineTxt" runat="server" Width="170px"></dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="StatusKeyTxt" ClientInstanceName="StatusKeyTxt" runat="server" Width="170px"></dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="WorkFlowLineStatusTxt" ClientInstanceName="WorkFlowLineStatusTxt" runat="server" Width="170px"></dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="CurrentWorkFlowTxt" ClientInstanceName="CurrentWorkFlowTxt" runat="server" Width="170px"></dx:ASPxTextBox>
                                    <%--<dx:ASPxLabel ID="WorkFlowLineLbl" ClientInstanceName="WorkFlowLineLblDirect" runat="server" Text=""></dx:ASPxLabel>
                            <dx:ASPxLabel ID="StatusKeyLbl" ClientInstanceName="StatusKeyLblDirect" runat="server" Text=""></dx:ASPxLabel>--%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" style="text-align: right">
                                <%--OnClick="Submit_Click"--%>
                                <%--<dx:ASPxButton ID="MRPList" runat="server" Text="List" AutoPostBack="false" Theme="Office2010Blue" OnClick="MRPList_Click"></dx:ASPxButton>
                        &nbsp--%>
                                <dx:ASPxButton ID="Preview" runat="server" Text="Preview" CausesValidation="false" AutoPostBack="false" Theme="Office2010Blue" OnClick="Preview_Click">
                                    <ClientSideEvents Click="function(s,e){
                                        $find('ModalPopupExtenderLoading').show();
                                        e.processOnServer = true;
                                        }" />
                                </dx:ASPxButton>
                                &nbsp
                        <dx:ASPxButton ID="Submit" runat="server" Text="Submit" AutoPostBack="false" Theme="Office2010Blue">
                            <%--<ClientSideEvents Click="function(s,e){PopupSubmit.SetHeaderText('Confirm'); PopupSubmit.Show();}" />--%>
                            <ClientSideEvents Click="mrp_addedit_submit" />
                        </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: inherit">
                    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" Width="100%" ActiveTabIndex="0" OnLoad="ASPxPageControl1_Load" EnableHierarchyRecreation="true" Theme="Office2010Blue">
                        <TabPages>
                            <dx:TabPage Text="MRP">
                                <ContentCollection>
                                    <dx:ContentControl>

                                        <dx:ASPxHiddenField ID="ASPxHiddenFieldDMWrkFlwLn" ClientInstanceName="ASPxHiddenFieldDMWrkFlwLnDirect" runat="server"></dx:ASPxHiddenField>
                                        <dx:ASPxHiddenField ID="ASPxHiddenFieldDMStatusKey" ClientInstanceName="ASPxHiddenFieldDMStatusKeyDirect" runat="server"></dx:ASPxHiddenField>

                                        <%--<div id="dvDetails" class="scroll">--%>
                                        <table style="width: 100%; padding: 25px;" border="0">
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxRoundPanel ID="DirectMaterialsRoundPanel" runat="server" ClientInstanceName="DMRoundPanel" HeaderText="DIRECT MATERIALS" Font-Bold="true" EnableAnimation="true" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                                                        <ClientSideEvents CollapsedChanging="DMRoundPanel_CollapsedChanging" />
                                                        <PanelCollection>
                                                            <dx:PanelContent>
                                                                <div style="width: inherit; overflow-x: auto;">
                                                                    <dx:ASPxGridView ID="DirectMaterialsGrid" runat="server" ClientInstanceName="DirectMaterialsGrid" OnHtmlEditFormCreated="DirectMaterialsGrid_HtmlEditFormCreated" EnableCallBacks="True" Width="100%" Theme="Office2010Blue"
                                                                        OnInitNewRow="DirectMaterialsGrid_InitNewRow"
                                                                        OnRowInserting="DirectMaterialsGrid_RowInserting"
                                                                        OnRowDeleting="DirectMaterialsGrid_RowDeleting"
                                                                        OnStartRowEditing="DirectMaterialsGrid_StartRowEditing"
                                                                        OnRowUpdating="DirectMaterialsGrid_RowUpdating"
                                                                        OnBeforeGetCallbackResult="DirectMaterialsGrid_BeforeGetCallbackResult"
                                                                        OnDataBound="DirectMaterialsGrid_DataBound"
                                                                        OnCancelRowEditing="DirectMaterialsGrid_CancelRowEditing">
                                                                        <ClientSideEvents RowClick="function(s,e){focused(s,e,'Materials');}" />
                                                                        <ClientSideEvents CustomButtonClick="DirectMaterialsGrid_CustomButtonClick" />
                                                                        <ClientSideEvents BeginCallback="function(s,e){loadingPanel.Show();}" />
                                                                        <ClientSideEvents EndCallback="function(s,e){loadingPanel.Hide();}" />
                                                                        <Columns>
                                                                            <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                                                <HeaderTemplate>
                                                                                    <div style="text-align: left">
                                                                                        <dx:ASPxButton ID="Add" runat="server" Image-Url="Images/Add.ico" Image-Width="15px" Image-ToolTip="New Row" RenderMode="Link" AutoPostBack="false" HorizontalAlign="Center" VerticalAlign="Middle">
                                                                                            <ClientSideEvents Click="DirectMaterialsGrid_Add" />
                                                                                        </dx:ASPxButton>
                                                                                    </div>
                                                                                </HeaderTemplate>
                                                                                <CustomButtons>
                                                                                    <dx:GridViewCommandColumnCustomButton ID="DMEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                                                        <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                                    <dx:GridViewCommandColumnCustomButton ID="DMDelete" Image-AlternateText="Delete" Image-Url="Images/Delete.ico" Image-ToolTip="Delete Row" Image-Width="15px">
                                                                                        <Image AlternateText="Delete" ToolTip="Delete Row" Width="15px" Url="Images/Delete.ico"></Image>
                                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                                </CustomButtons>
                                                                            </dx:GridViewCommandColumn>
                                                                            <dx:GridViewDataColumn FieldName="PK" Visible="false"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="ExpenseCode" Visible="false"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="ActivityCode" Caption="Activity"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="VALUE" Visible="false"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="ExpenseCodeName" Caption="Expense"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="ItemCode"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="ItemDescription" Caption="Description"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="ItemDescriptionAddl" Caption="Description 2"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="UOM"></dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="Qty">
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                            </dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="Cost" VisibleIndex="9" CellStyle-HorizontalAlign="Right">
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            </dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="TotalCost" VisibleIndex="10" CellStyle-HorizontalAlign="Right">
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                            </dx:GridViewDataColumn>
                                                                            <%--<dx:GridViewDataColumn FieldName="WrkLine" Visible="false" VisibleIndex="11"></dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="StatusKey" Visible="false" VisibleIndex="12"></dx:GridViewDataColumn>--%>
                                                                        </Columns>

                                                                        <SettingsCommandButton>
                                                                            <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px">
                                                                                <Image Width="15px" Url="Images/Edit.ico"></Image>
                                                                            </EditButton>
                                                                            <DeleteButton ButtonType="Image" Image-Url="Images/Delete.ico" Image-Width="15px">
                                                                                <Image Width="15px" Url="Images/Delete.ico"></Image>
                                                                            </DeleteButton>
                                                                            <NewButton ButtonType="Image" Image-Url="Images/Add.ico" Image-Width="15px">
                                                                                <Image Width="15px" Url="Images/Add.ico"></Image>
                                                                            </NewButton>
                                                                        </SettingsCommandButton>

                                                                        <TotalSummary>
                                                                            <dx:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" ShowInColumn="Qty" DisplayFormat="Total: {0:0,0.00}" />
                                                                            <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" ShowInColumn="TotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                                                        </TotalSummary>

                                                                        <SettingsEditing Mode="EditFormAndDisplayRow"></SettingsEditing>
                                                                        <Templates>
                                                                            <EditForm>
                                                                                <div style="padding: 4px 3px 4px">
                                                                                    <dx:ASPxPageControl ID="DirectPageControl" runat="server" Width="100%" Theme="Office2010Blue">
                                                                                        <TabPages>
                                                                                            <dx:TabPage Text="Direct Materials" Visible="true">
                                                                                                <ContentCollection>
                                                                                                    <dx:ContentControl runat="server">
                                                                                                        <dx:ASPxHiddenField ID="entityhidden" runat="server" ClientInstanceName="entityhidden"></dx:ASPxHiddenField>
                                                                                                        <table style="width: 100%; padding: 10px;" border="0">
                                                                                                            <tr>
                                                                                                                <td style="vertical-align: top; width: 55%;">
                                                                                                                    <table style="width: 100%;">
                                                                                                                        <tr>
                                                                                                                            <td style="width: 50%;">
                                                                                                                                <div id="OperatingUnit_label" runat="server">
                                                                                                                                    <dx:ASPxLabel runat="server" Text="Operating Unit" Theme="Office2010Blue" />
                                                                                                                                </div>
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <table style="width: 100%;">
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 7%;"></td>
                                                                                                                                        <td>
                                                                                                                                            <div id="OperatingUnit_combo" runat="server">
                                                                                                                                                <dx:ASPxComboBox ID="OperatingUnit" runat="server" ClientInstanceName="OperatingUnit" OnInit="OperatingUnit_Init" AutoResizeWithContainer="true" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                                    <ClientSideEvents SelectedIndexChanged="OperatingUnitDM_SelectedIndexChanged" />
                                                                                                                                                    <ValidationSettings ValidateOnLeave="False" EnableCustomValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter value">
                                                                                                                                                    </ValidationSettings>
                                                                                                                                                </dx:ASPxComboBox>
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="width: 50%;">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Activity" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td style="width: 50%;">
                                                                                                                                <table style="width: 100%;">
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 7%;">
                                                                                                                                            <dx:ASPxCheckBox ID="ActivityCodeChkbx" runat="server" ClientInstanceName="ActivityCodeChkbxDM" Checked="true" Theme="Office2010Blue">
                                                                                                                                                <ClientSideEvents CheckedChanged="ActivityCodeChkbx_CheckedChanged" />
                                                                                                                                            </dx:ASPxCheckBox>
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <dx:ASPxComboBox ID="ActivityCode" runat="server" ClientInstanceName="ActivityCodeDirect" OnInit="ActivityCode_Init" AutoResizeWithContainer="true" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                                <ClientSideEvents SelectedIndexChanged="ActivityCodeIndexChange" />
                                                                                                                                            </dx:ASPxComboBox>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="width: 50%;">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Expense" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td style="width: 50%;">
                                                                                                                                <table style="width: 100%;">
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 7%;">
                                                                                                                                            <dx:ASPxCheckBox ID="ExpenseChkbx" runat="server" ClientInstanceName="ExpenseChkbxDM" Checked="true" Theme="Office2010Blue">
                                                                                                                                                <ClientSideEvents CheckedChanged="ExpenseChkbx_CheckedChanged" />
                                                                                                                                            </dx:ASPxCheckBox>
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <dx:ASPxComboBox ID="ExpenseCode" runat="server" ClientInstanceName="ExpenseCodeDM" OnInit="ExpenseCode_Init" ValueType="System.String" Width="300px" Theme="Office2010Blue">
                                                                                                                                                <ClientSideEvents SelectedIndexChanged="ExpenseCode_SelectedIndexChanged" />
                                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                            </dx:ASPxComboBox>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        

                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxLabel runat="server" Text="Description" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <table style="width: 100%;">
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 7%;"></td>
                                                                                                                                        <td>
                                                                                                                                            <dx:ASPxTextBox ID="ItemDescription" runat="server" ClientInstanceName="ItemDescriptionDirect" ReadOnly="false" Text='<%#Eval("ItemDescription")%>' Theme="Office2010Blue" Width="300px">
                                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                                <ClientSideEvents KeyPress="ItemCodeDirect_KeyPress" />
                                                                                                                                            </dx:ASPxTextBox>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td></td>
                                                                                                                            <td style="width: 50%;">
                                                                                                                                <table style="width: 100%;" border="0">
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 7%;"></td>
                                                                                                                                        <td>
                                                                                                                                            <div style="overflow-x: auto; width: 400px;">
                                                                                                                                                <dx:ASPxListBox ID="listbox" ClientInstanceName="listbox" runat="server" ValueType="System.String" OnCallback="listbox_Callback" ValueField="ITEMID" ClientVisible="false" Width="100%" Theme="Office2010Blue">
                                                                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                                    <Columns>
                                                                                                                                                        <dx:ListBoxColumn FieldName="ITEMID" Caption="Item Code" Width="80px"></dx:ListBoxColumn>
                                                                                                                                                        <dx:ListBoxColumn FieldName="NAMEALIAS" Caption="Description"></dx:ListBoxColumn>
                                                                                                                                                        <dx:ListBoxColumn FieldName="UOM" Caption="UOM" Width="50px"></dx:ListBoxColumn>
                                                                                                                                                        <dx:ListBoxColumn FieldName="LastCost" Caption="Last Price" Width="80px"></dx:ListBoxColumn>
                                                                                                                                                    </Columns>
                                                                                                                                                    <ItemStyle Wrap="True" VerticalAlign="Middle" />
                                                                                                                                                    <ClientSideEvents SelectedIndexChanged="listbox_selected" />
                                                                                                                                                    <ClientSideEvents EndCallback="listbox_EndCallback" />
                                                                                                                                                </dx:ASPxListBox>
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>

                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxLabel runat="server" Text="Description 2" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <table style="width: 100%;">
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 7%;"></td>
                                                                                                                                        <td>
                                                                                                                                            <dx:ASPxTextBox ID="ItemDescriptionAddl" runat="server" Text='<%#Eval("ItemDescriptionAddl")%>' Theme="Office2010Blue" Width="300px">
                                                                                                                                            </dx:ASPxTextBox>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>

<tr>
                                                                                                                            <td style="width: 50%;">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Item Code" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td style="width: 50%;">
                                                                                                                                <table style="width: 100%;">
                                                                                                                                    <tr>
                                                                                                                                        <td style="width: 7%;"></td>
                                                                                                                                        <td>
                                                                                                                                            <dx:ASPxTextBox ID="ItemCode" ClientInstanceName="ItemCodeDirect" runat="server" Text='<%#Eval("ItemCode")%>' AutoResizeWithContainer="true" Theme="Office2010Blue" Width="300px" ReadOnly="true">
                                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                                <%--<ClientSideEvents KeyPress="ItemCodeDirect_KeyPress" />--%>
                                                                                                                                            </dx:ASPxTextBox>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>

                                                                                                                            </td>
                                                                                                                        </tr>


                                                                                                                        <tr>
                                                                                                                            <td></td>

                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                                <%--<td style="vertical-align: top; width: 30%;">
                                                                                                    <table style="width: 100%;">
                                                                                                        
                                                                                                    </table>
                                                                                                </td>--%>
                                                                                                                <td style="vertical-align: top; width: 45%;">
                                                                                                                    <table style="width: 100%;">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxLabel runat="server" Text="UOM" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxComboBox ID="UOM" runat="server" ClientInstanceName="UOMDirect" OnInit="UOM_Init" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                </dx:ASPxComboBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxLabel runat="server" Text="Qty" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxTextBox ID="Qty" runat="server" ClientInstanceName="QtyDirect" Text='<%#Eval("Qty")%>' Theme="Office2010Blue" HorizontalAlign="Right" Width="300px">
                                                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                    <ClientSideEvents ValueChanged="OnValueChangeQty" />
                                                                                                                                    <ClientSideEvents KeyUp="OnKeyUpQtyDirect" />
                                                                                                                                    <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                                </dx:ASPxTextBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxLabel runat="server" Text="Cost" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxTextBox ID="Cost" runat="server" ClientInstanceName="CostDirect" Text='<%#Eval("Cost")%>' Theme="Office2010Blue" HorizontalAlign="Right" Width="300px">
                                                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                    <ClientSideEvents ValueChanged="OnValueChange" />
                                                                                                                                    <ClientSideEvents KeyUp="OnKeyUpCostDirect" />
                                                                                                                                    <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                                </dx:ASPxTextBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxLabel runat="server" Text="Total Cost" Theme="Office2010Blue" />
                                                                                                                            </td>
                                                                                                                            <td>
                                                                                                                                <dx:ASPxTextBox ID="TotalCost" runat="server" ClientInstanceName="TotalCostDirect" Text='<%#Eval("TotalCost")%>' ReadOnly="true" Theme="Office2010Blue" HorizontalAlign="Right" Width="300px">
                                                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter a value in Cost and Qty field" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                </dx:ASPxTextBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </dx:ContentControl>
                                                                                                </ContentCollection>
                                                                                            </dx:TabPage>
                                                                                        </TabPages>
                                                                                    </dx:ASPxPageControl>
                                                                                </div>
                                                                                <div style="text-align: right; padding: 2px">
                                                                                    <dx:ASPxButton runat="server" Text="Save" Theme="Office2010Blue" CausesValidation="true" AutoPostBack="false">
                                                                                        <ClientSideEvents Click="updateDirectMat" />
                                                                                    </dx:ASPxButton>
                                                                                    <dx:ASPxButton runat="server" Text="Cancel" CausesValidation="false" Theme="Office2010Blue" AutoPostBack="false">
                                                                                        <ClientSideEvents Click="function(s,e){DirectMaterialsGrid.CancelEdit();}" />
                                                                                    </dx:ASPxButton>
                                                                                </div>
                                                                            </EditForm>
                                                                        </Templates>
                                                                        <SettingsLoadingPanel Mode="Disabled" />
                                                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                                        <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                                                            AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" ConfirmDelete="true" />
                                                                        <SettingsText ConfirmDelete="Delete This Item?" />
                                                                    </dx:ASPxGridView>

                                                                </div>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxRoundPanel ID="OpexRoundPanel" runat="server" ClientInstanceName="OpRoundPanel" HeaderText="OPEX" EnableAnimation="true" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                                                        <ClientSideEvents CollapsedChanging="OpRoundPanel_CollapsedChanging" />
                                                        <PanelCollection>
                                                            <dx:PanelContent>
                                                                <dx:ASPxGridView ID="OPEXGrid" runat="server" ClientInstanceName="OPEXGrid" Width="100%" Theme="Office2010Blue"
                                                                    OnInitNewRow="OPEXGrid_InitNewRow"
                                                                    OnRowInserting="OPEXGrid_RowInserting"
                                                                    OnRowDeleting="OPEXGrid_RowDeleting"
                                                                    OnStartRowEditing="OPEXGrid_StartRowEditing"
                                                                    OnRowUpdating="OPEXGrid_RowUpdating"
                                                                    OnBeforeGetCallbackResult="OPEXGrid_BeforeGetCallbackResult"
                                                                    OnDataBound="OPEXGrid_DataBound"
                                                                    OnCancelRowEditing="OPEXGrid_CancelRowEditing"
                                                                    OnHtmlEditFormCreated="OPEXGrid_HtmlEditFormCreated">
                                                                    <ClientSideEvents RowClick="function(s,e){focused(s,e,'OPEX');}" />
                                                                    <ClientSideEvents BeginCallback="OnBeginCallback" />
                                                                    <ClientSideEvents CustomButtonClick="OPEXGrid_CustomButtonClick" />
                                                                    <ClientSideEvents EndCallback="function(s,e){loadingPanel.Hide();}" />
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: left">
                                                                                    <dx:ASPxButton ID="Add" runat="server" Image-Url="Images/Add.ico" Image-Width="15px" Image-ToolTip="New Row" RenderMode="Link" AutoPostBack="false" HorizontalAlign="Center" VerticalAlign="Middle">
                                                                                        <ClientSideEvents Click="OPEXGrid_Add" />
                                                                                    </dx:ASPxButton>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <CustomButtons>
                                                                                <dx:GridViewCommandColumnCustomButton ID="OPEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                                                    <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                                                </dx:GridViewCommandColumnCustomButton>
                                                                                <dx:GridViewCommandColumnCustomButton ID="OPDelete" Image-AlternateText="Delete" Image-Url="Images/Delete.ico" Image-ToolTip="Delete Row" Image-Width="15px">
                                                                                    <Image AlternateText="Delete" ToolTip="Delete Row" Width="15px" Url="Images/Delete.ico"></Image>
                                                                                </dx:GridViewCommandColumnCustomButton>
                                                                            </CustomButtons>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataColumn FieldName="PK" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ExpenseCodeName" Caption="Expense"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="VALUE" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ProcCatCode" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ProcCatName" Caption="Procurement Category"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ItemCode"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Description"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="DescriptionAddl" Caption="Description 2"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="UOM"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Qty" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Cost" VisibleIndex="9" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="TotalCost" VisibleIndex="10" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ExpenseCode" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="isItem" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="isProdCategory" Visible="false"></dx:GridViewDataColumn>
                                                                    </Columns>

                                                                    <SettingsCommandButton>
                                                                        <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Edit.ico"></Image>
                                                                        </EditButton>
                                                                        <DeleteButton ButtonType="Image" Image-Url="Images/Delete.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Delete.ico"></Image>
                                                                        </DeleteButton>
                                                                        <NewButton ButtonType="Image" Image-Url="Images/Add.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Add.ico"></Image>
                                                                        </NewButton>
                                                                    </SettingsCommandButton>
                                                                    <SettingsLoadingPanel Mode="Disabled" />
                                                                    <SettingsEditing Mode="EditFormAndDisplayRow"></SettingsEditing>

                                                                    <TotalSummary>
                                                                        <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" ShowInColumn="TotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                                                        <dx:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" ShowInColumn="Qty" DisplayFormat="Total: {0:0,0.00}" />
                                                                    </TotalSummary>

                                                                    <Templates>
                                                                        <EditForm>
                                                                            <div style="padding: 4px 3px 4px; overflow-x: auto; width: inherit;">
                                                                                <dx:ASPxPageControl ID="OPEXPageControl" runat="server" Width="100%" Theme="Office2010Blue">
                                                                                    <TabPages>
                                                                                        <dx:TabPage Text="Operating Expense" Visible="true">
                                                                                            <ContentCollection>
                                                                                                <dx:ContentControl runat="server">
                                                                                                    <dx:ASPxHiddenField ID="entityhidden" runat="server" ClientInstanceName="entityhiddenOP"></dx:ASPxHiddenField>
                                                                                                    <table style="width: 100%" border="0">
                                                                                                        <tr>
                                                                                                            <td style="width: 56%; vertical-align: top;">
                                                                                                                <table style="width: 100%;">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <div id="OperatingUnit_label" runat="server">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Operating Unit" Theme="Office2010Blue" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table style="width: 100%;">
                                                                                                                                <tr>
                                                                                                                                    <td class="width_for_chkbx"></td>
                                                                                                                                    <td>
                                                                                                                                        <div id="OperatingUnit_combo" runat="server">
                                                                                                                                            <dx:ASPxComboBox ID="OperatingUnit" runat="server" ClientInstanceName="OperatingUnitOP" OnInit="OperatingUnit_Init" AutoResizeWithContainer="true" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                                <ClientSideEvents SelectedIndexChanged="OperatingUnitOP_SelectedIndexChanged" />
                                                                                                                                                <ValidationSettings ValidateOnLeave="False" EnableCustomValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter value">
                                                                                                                                                </ValidationSettings>
                                                                                                                                            </dx:ASPxComboBox>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Expense" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table style="width: 100%;">
                                                                                                                                <tr>
                                                                                                                                    <td class="width_for_chkbx"></td>
                                                                                                                                    <td>
                                                                                                                                        <dx:ASPxComboBox ID="ExpenseCode" runat="server" ClientInstanceName="ExpenseCodeOPEX" OnInit="ExpenseCode_Init" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                            <ClientSideEvents SelectedIndexChanged="ExpenseCodeIndexChangeOPEX" />
                                                                                                                                            <%--<ClientSideEvents Init="ExpenseCodeCombo_Init" />--%>
                                                                                                                                        </dx:ASPxComboBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>

                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <div id="CA_prodlabel_div" class="CA_prodlabel_divClass" runat="server">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Procurement Category" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                            </div>

                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <div id="CA_prodcombo_div" class="CA_prodcombo_divClass" runat="server">
                                                                                                                                <table style="width: 100%;">
                                                                                                                                    <tr>
                                                                                                                                        <td class="width_for_chkbx">
                                                                                                                                            <dx:ASPxCheckBox ID="ProdCatChkbx" runat="server" ClientInstanceName="ProdCatChkbxClient" Checked="true" Theme="Office2010Blue">
                                                                                                                                                <ClientSideEvents CheckedChanged="ProdCatChkbx_CheckedChanged" />
                                                                                                                                            </dx:ASPxCheckBox>
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <dx:ASPxCallbackPanel ID="ProcCatOPEXCallback" runat="server" ClientInstanceName="ProcCatOPEXCallbackClient" OnCallback="ProcCatOPEXCallback_Callback">
                                                                                                                                                <PanelCollection>
                                                                                                                                                    <dx:PanelContent>
                                                                                                                                                        <dx:ASPxComboBox ID="ProcCatOPEX" runat="server" ClientInstanceName="ProcCatOPEX" OnInit="ProcCatOPEX_Init" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                                            <ClientSideEvents SelectedIndexChanged="ProcCatOPEX_SelectedIndexChanged" />
                                                                                                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                                        </dx:ASPxComboBox>
                                                                                                                                                    </dx:PanelContent>
                                                                                                                                                </PanelCollection>
                                                                                                                                            </dx:ASPxCallbackPanel>
                                                                                                                                        </td>

                                                                                                                                    </tr>
                                                                                                                                </table>

                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    

                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Item Description" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table style="width: 100%;">
                                                                                                                                <tr>
                                                                                                                                    <td class="width_for_chkbx"></td>
                                                                                                                                    <td>
                                                                                                                                        <dx:ASPxTextBox ID="Description" runat="server" ClientInstanceName="DescriptionOPEX" Text='<%#Eval("Description")%>' Width="300px" Theme="Office2010Blue">
                                                                                                                                            <%--<ClientSideEvents Init="Description_OP_Init" />--%>
                                                                                                                                            <ClientSideEvents KeyPress="ItemCodeOPEX_KeyPress" />
                                                                                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                        </dx:ASPxTextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>

                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;"></td>
                                                                                                                        <td>
                                                                                                                            <table style="width: 100%;">
                                                                                                                                <tr>
                                                                                                                                    <td class="width_for_chkbx"></td>
                                                                                                                                    <td>
                                                                                                                                        <div style="overflow-x: auto; width: 400px;">
                                                                                                                                            <dx:ASPxListBox ID="listboxOPEX" ClientInstanceName="listboxOPEX" runat="server" ValueType="System.String"
                                                                                                                                                ValueField="ITEMID" OnCallback="listboxOPEX_Callback" ClientVisible="false" Theme="Office2010Blue" Width="450px">
                                                                                                                                                <Columns>
                                                                                                                                                    <dx:ListBoxColumn FieldName="ITEMID" Caption="Item Code" Width="80px"></dx:ListBoxColumn>
                                                                                                                                                    <dx:ListBoxColumn FieldName="NAMEALIAS" Caption="Description" ToolTip="Item Description"></dx:ListBoxColumn>
                                                                                                                                                    <dx:ListBoxColumn FieldName="UOM" Caption="UOM" Width="50px"></dx:ListBoxColumn>
                                                                                                                                                    <dx:ListBoxColumn FieldName="LastCost" Caption="Last Price" Width="80px"></dx:ListBoxColumn>
                                                                                                                                                </Columns>
                                                                                                                                                <ItemStyle Wrap="True" VerticalAlign="Middle" />
                                                                                                                                                <ClientSideEvents SelectedIndexChanged="listbox_selectedOPEX" />
                                                                                                                                                <ClientSideEvents EndCallback="listboxOPEX_EndCallback" />
                                                                                                                                            </dx:ASPxListBox>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>

                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Item Description 2" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table style="width: 100%;">
                                                                                                                                <tr>
                                                                                                                                    <td class="width_for_chkbx"></td>
                                                                                                                                    <td>
                                                                                                                                        <dx:ASPxTextBox ID="DescriptionAddl" runat="server" Text='<%#Eval("DescriptionAddl")%>' Width="300px" Theme="Office2010Blue">
                                                                                                                                        </dx:ASPxTextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>

                                                                                                                        </td>
                                                                                                                    </tr><tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <div id="div1" class="div1Class" runat="server">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Item Code" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <div id="div2" class="div2Class" runat="server">
                                                                                                                                <table style="width: 100%;">
                                                                                                                                    <tr>
                                                                                                                                        <td class="width_for_chkbx">
                                                                                                                                            <dx:ASPxCheckBox ID="ItemCodeChkbx" runat="server" ClientInstanceName="ItemCodeChkbxClient" Checked="true" Theme="Office2010Blue">
                                                                                                                                                <ClientSideEvents CheckedChanged="ItemCodeChkbx_CheckedChanged" />
                                                                                                                                            </dx:ASPxCheckBox>
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <dx:ASPxTextBox ID="ItemCode" runat="server" ClientInstanceName="ItemCodeOPEX" Text='<%#Eval("ItemCode")%>' Theme="Office2010Blue" Width="300px" ReadOnly="true">
                                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                                <%--<ClientSideEvents KeyPress="ItemCodeOPEX_KeyPress" />--%>
                                                                                                                                            </dx:ASPxTextBox>
                                                                                                                                        </td>

                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                            <td style="width: 44%; vertical-align: top;">
                                                                                                                <table style="width: 100%;">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="UOM" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxComboBox ID="UOM" runat="server" ClientInstanceName="UOMOPEX" OnInit="UOM_Init" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxComboBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Qty" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Qty" runat="server" ClientInstanceName="QtyOPEX" Text='<%#Eval("Qty")%>' HorizontalAlign="Right" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                <ClientSideEvents ValueChanged="OnValueChangeQty" />
                                                                                                                                <ClientSideEvents KeyUp="OnKeyUpQtyOpex" />
                                                                                                                                <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Cost" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Cost" runat="server" ClientInstanceName="CostOPEX" Text='<%#Eval("Cost")%>' HorizontalAlign="Right" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                <ClientSideEvents ValueChanged="OnValueChange" />
                                                                                                                                <ClientSideEvents KeyUp="OnKeyUpCostOpex" />
                                                                                                                                <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 20%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Total Cost" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="TotalCost" runat="server" ClientInstanceName="TotalCostOPEX" Text='<%#Eval("TotalCost")%>' ReadOnly="true" HorizontalAlign="Right" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter a value in Cost and Qty field" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>

                                                                                                </dx:ContentControl>
                                                                                            </ContentCollection>
                                                                                        </dx:TabPage>
                                                                                    </TabPages>
                                                                                </dx:ASPxPageControl>
                                                                            </div>
                                                                            <div style="text-align: right; padding: 2px">
                                                                                <dx:ASPxButton runat="server" Text="Save" Theme="Office2010Blue" AutoPostBack="false">
                                                                                    <ClientSideEvents Click="updateOpex" />
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton runat="server" Text="Cancel" CausesValidation="false" Theme="Office2010Blue" AutoPostBack="false">
                                                                                    <ClientSideEvents Click="function(s,e){OPEXGrid.CancelEdit();}" />
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </EditForm>
                                                                    </Templates>
                                                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                                    <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                                                        AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" ConfirmDelete="true" />
                                                                    <SettingsText ConfirmDelete="Delete This Item?" />
                                                                </dx:ASPxGridView>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxRoundPanel ID="ManpowerRoundPanel" runat="server" ClientInstanceName="ManRoundPanel" HeaderText="MANPOWER" EnableAnimation="true" ClientInstanceNam="ManpowerRoundPanel" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                                                        <ClientSideEvents CollapsedChanging="ManRoundPanel_CollapsedChanging" />
                                                        <PanelCollection>
                                                            <dx:PanelContent>
                                                                <dx:ASPxGridView ID="ManPowerGrid" runat="server" ClientInstanceName="ManPowerGrid" Width="100%" Theme="Office2010Blue"
                                                                    OnInitNewRow="ManPowerGrid_InitNewRow"
                                                                    OnRowInserting="ManPowerGrid_RowInserting"
                                                                    OnRowDeleting="ManPowerGrid_RowDeleting"
                                                                    OnStartRowEditing="ManPowerGrid_StartRowEditing"
                                                                    OnRowUpdating="ManPowerGrid_RowUpdating"
                                                                    OnBeforeGetCallbackResult="ManPowerGrid_BeforeGetCallbackResult"
                                                                    OnDataBound="ManPowerGrid_DataBound"
                                                                    OnCancelRowEditing="ManPowerGrid_CancelRowEditing"
                                                                    OnHtmlEditFormCreated="ManPowerGrid_HtmlEditFormCreated">
                                                                    <ClientSideEvents RowClick="function(s,e){focused(s,e,'Manpower');}" />
                                                                    <ClientSideEvents CustomButtonClick="ManPowerGrid_CustomButtonClick" />
                                                                    <ClientSideEvents BeginCallback="function(s,e){loadingPanel.Show();}" />
                                                                    <ClientSideEvents EndCallback="function(s,e){loadingPanel.Hide();}" />
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: left">
                                                                                    <dx:ASPxButton ID="Add" runat="server" Image-Url="Images/Add.ico" Image-Width="15px" Image-ToolTip="New Row" RenderMode="Link" AutoPostBack="false" HorizontalAlign="Center" VerticalAlign="Middle">
                                                                                        <ClientSideEvents Click="ManPowerGrid_Add" />
                                                                                    </dx:ASPxButton>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <CustomButtons>
                                                                                <dx:GridViewCommandColumnCustomButton ID="MANEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                                                    <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                                                </dx:GridViewCommandColumnCustomButton>
                                                                                <dx:GridViewCommandColumnCustomButton ID="MANDelete" Image-AlternateText="Delete" Image-Url="Images/Delete.ico" Image-ToolTip="Delete Row" Image-Width="15px">
                                                                                    <Image AlternateText="Delete" ToolTip="Delete Row" Width="15px" Url="Images/Delete.ico"></Image>
                                                                                </dx:GridViewCommandColumnCustomButton>
                                                                            </CustomButtons>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataColumn FieldName="PK" Visible="false" VisibleIndex="1"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false" VisibleIndex="2"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ActivityCode" Caption="Activity" VisibleIndex="3"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="VALUE" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit" VisibleIndex="4"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ManPowerTypeKey" Visible="false" VisibleIndex="5"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ManPowerTypeKeyName" Caption="Type" VisibleIndex="6"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Description" VisibleIndex="7"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="UOM" VisibleIndex="8"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Qty" Caption="Head Count" VisibleIndex="9" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Cost" VisibleIndex="10" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                        </dx:GridViewDataColumn>

                                                                        <dx:GridViewDataColumn FieldName="TotalCost" VisibleIndex="11" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                    </Columns>

                                                                    <SettingsCommandButton>
                                                                        <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Edit.ico"></Image>
                                                                        </EditButton>
                                                                        <DeleteButton ButtonType="Image" Image-Url="Images/Delete.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Delete.ico"></Image>
                                                                        </DeleteButton>
                                                                        <NewButton ButtonType="Image" Image-Url="Images/Add.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Add.ico"></Image>
                                                                        </NewButton>
                                                                    </SettingsCommandButton>
                                                                    <SettingsEditing Mode="EditFormAndDisplayRow"></SettingsEditing>
                                                                    <SettingsLoadingPanel Mode="Disabled" />

                                                                    <TotalSummary>
                                                                        <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" ShowInColumn="TotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                                                        <dx:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" ShowInColumn="Qty" DisplayFormat="Total: {0:0,0.00}" />
                                                                    </TotalSummary>

                                                                    <Templates>
                                                                        <EditForm>
                                                                            <div style="padding: 4px 3px 4px">
                                                                                <dx:ASPxPageControl ID="ManPowerPageControl" runat="server" Width="100%" Theme="Office2010Blue">
                                                                                    <TabPages>
                                                                                        <dx:TabPage Text="Manpower" Visible="true">
                                                                                            <ContentCollection>
                                                                                                <dx:ContentControl runat="server">
                                                                                                    <dx:ASPxHiddenField ID="entityhidden" runat="server" ClientInstanceName="entityhiddenMAN"></dx:ASPxHiddenField>
                                                                                                    <table style="width: 100%" border="0">
                                                                                                        <tr>
                                                                                                            <td style="vertical-align: top;">
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%;">
                                                                                                                            <div id="OperatingUnit_label" runat="server">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Operating Unit" Theme="Office2010Blue" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <div id="OperatingUnit_combo" runat="server">
                                                                                                                                <dx:ASPxComboBox ID="OperatingUnit" runat="server" ClientInstanceName="OperatingUnitMAN" OnInit="OperatingUnit_Init" AutoResizeWithContainer="true" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue">
                                                                                                                                    <ClientSideEvents SelectedIndexChanged="OperatingUnitMAN_SelectedIndexChanged" />
                                                                                                                                    <ValidationSettings ValidateOnLeave="False" EnableCustomValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter value">
                                                                                                                                    </ValidationSettings>
                                                                                                                                </dx:ASPxComboBox>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="Activity Code" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxComboBox ID="ActivityCode" runat="server" ClientInstanceName="ActivityCodeMAN" OnInit="ActivityCode_Init" AutoResizeWithContainer="true" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="false"></ValidationSettings>
                                                                                                                                <ClientSideEvents SelectedIndexChanged="ActivityCodeIndexChangeMAN" />
                                                                                                                            </dx:ASPxComboBox>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="UOM" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxComboBox ID="UOM" runat="server" ClientInstanceName="UOMMAN" OnInit="UOM_Init" Width="170px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxComboBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                            <td style="vertical-align: top;">
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Type" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxComboBox ID="ManPowerTypeKeyName" runat="server" ClientInstanceName="ManPowerTypeKeyNameMAN" Text='<%#Eval("ManPowerTypeKeyName")%>' OnInit="ManPowerTypeKey_Init" Width="170px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxComboBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 10%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Description" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Description" runat="server" ClientInstanceName="DescriptionMAN" Text='<%#Eval("Description")%>' Width="170px" Theme="Office2010Blue">
                                                                                                                                <%--<ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>--%>
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="Head Count" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Qty" runat="server" ClientInstanceName="QtyMAN" Text='<%#Eval("Qty")%>' HorizontalAlign="Right" Width="170px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                <ClientSideEvents ValueChanged="OnValueChangeQty" />
                                                                                                                                <ClientSideEvents KeyUp="OnKeyUpQtyMan" />
                                                                                                                                <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%;">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Cost" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Cost" runat="server" ClientInstanceName="CostMAN" Text='<%#Eval("Cost")%>' HorizontalAlign="Right" Width="170px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                <ClientSideEvents ValueChanged="OnValueChange" />
                                                                                                                                <ClientSideEvents KeyUp="OnKeyUpCostMan" />
                                                                                                                                <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="Total Cost" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="TotalCost" runat="server" ClientInstanceName="TotalCostMAN" Text='<%#Eval("TotalCost")%>' ReadOnly="true" HorizontalAlign="Right" Width="170px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter a value in Cost and Qty field" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </dx:ContentControl>
                                                                                            </ContentCollection>
                                                                                        </dx:TabPage>
                                                                                    </TabPages>
                                                                                </dx:ASPxPageControl>
                                                                            </div>
                                                                            <div style="text-align: right; padding: 2px">
                                                                                <dx:ASPxButton runat="server" Text="Save" Theme="Office2010Blue" AutoPostBack="false">
                                                                                    <ClientSideEvents Click="updateManpower" />
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton runat="server" Text="Cancel" CausesValidation="false" Theme="Office2010Blue" AutoPostBack="false">
                                                                                    <ClientSideEvents Click="function(s,e){ManPowerGrid.CancelEdit();}" />
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </EditForm>
                                                                    </Templates>
                                                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                                    <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                                                        AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" ConfirmDelete="true" />
                                                                    <SettingsText ConfirmDelete="Delete This Item?" />
                                                                </dx:ASPxGridView>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxRoundPanel ID="CapexRoundPanel" runat="server" ClientInstanceName="CaRoundPanel" HeaderText="CAPEX" EnableAnimation="true" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                                                        <ClientSideEvents CollapsedChanging="CaRoundPanel_CollapsedChanging" />
                                                        <PanelCollection>
                                                            <dx:PanelContent>
                                                                <dx:ASPxGridView ID="CAPEXGrid" runat="server" ClientInstanceName="CAPEXGrid" Width="100%" Theme="Office2010Blue"
                                                                    OnInitNewRow="CAPEXGrid_InitNewRow"
                                                                    OnRowInserting="CAPEXGrid_RowInserting"
                                                                    OnRowDeleting="CAPEXGrid_RowDeleting"
                                                                    OnStartRowEditing="CAPEXGrid_StartRowEditing"
                                                                    OnRowUpdating="CAPEXGrid_RowUpdating"
                                                                    OnBeforeGetCallbackResult="CAPEXGrid_BeforeGetCallbackResult"
                                                                    OnDataBound="CAPEXGrid_DataBound"
                                                                    OnCancelRowEditing="CAPEXGrid_CancelRowEditing"
                                                                    OnHtmlEditFormCreated="CAPEXGrid_HtmlEditFormCreated">
                                                                    <ClientSideEvents RowClick="function(s,e){focused(s,e,'CAPEX');}" />
                                                                    <ClientSideEvents CustomButtonClick="CAPEXGrid_CustomButtonClick" />
                                                                    <ClientSideEvents BeginCallback="function(s,e){loadingPanel.Show();}" />
                                                                    <ClientSideEvents EndCallback="function(s,e){loadingPanel.Hide();}" />
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: left">
                                                                                    <dx:ASPxButton ID="Add" runat="server" Image-Url="Images/Add.ico" Image-Width="15px" Image-ToolTip="New Row" RenderMode="Link" AutoPostBack="false" HorizontalAlign="Center" VerticalAlign="Middle">
                                                                                        <ClientSideEvents Click="CAPEXGrid_Add" />
                                                                                    </dx:ASPxButton>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <CustomButtons>
                                                                                <dx:GridViewCommandColumnCustomButton ID="CAEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                                                    <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                                                </dx:GridViewCommandColumnCustomButton>
                                                                                <dx:GridViewCommandColumnCustomButton ID="CADelete" Image-AlternateText="Delete" Image-Url="Images/Delete.ico" Image-ToolTip="Delete Row" Image-Width="15px">
                                                                                    <Image AlternateText="Delete" ToolTip="Delete Row" Width="15px" Url="Images/Delete.ico"></Image>
                                                                                </dx:GridViewCommandColumnCustomButton>
                                                                            </CustomButtons>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataColumn FieldName="PK" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="VALUE" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ProdCat" Caption="Product Category"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ProdCode" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Description" Width="450px"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="UOM"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Qty" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Cost" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                        </dx:GridViewDataColumn>

                                                                        <dx:GridViewDataColumn FieldName="TotalCost" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                    </Columns>

                                                                    <SettingsCommandButton>
                                                                        <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Edit.ico"></Image>
                                                                        </EditButton>
                                                                        <DeleteButton ButtonType="Image" Image-Url="Images/Delete.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Delete.ico"></Image>
                                                                        </DeleteButton>
                                                                        <NewButton ButtonType="Image" Image-Url="Images/Add.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Add.ico"></Image>
                                                                        </NewButton>
                                                                    </SettingsCommandButton>
                                                                    <SettingsEditing Mode="EditFormAndDisplayRow"></SettingsEditing>
                                                                    <SettingsLoadingPanel Mode="Disabled" />

                                                                    <TotalSummary>
                                                                        <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" ShowInColumn="TotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                                                        <dx:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" ShowInColumn="Qty" DisplayFormat="Total: {0:0,0.00}" />
                                                                    </TotalSummary>

                                                                    <Templates>
                                                                        <EditForm>
                                                                            <div style="padding: 4px 3px 4px">
                                                                                <dx:ASPxPageControl ID="CAPEXPageControl" runat="server" Width="100%" Theme="Office2010Blue">
                                                                                    <TabPages>
                                                                                        <dx:TabPage Text="Capital Expenditure" Visible="true">
                                                                                            <ContentCollection>
                                                                                                <dx:ContentControl runat="server">
                                                                                                    <dx:ASPxHiddenField ID="entityhidden" runat="server" ClientInstanceName="entityhiddenCA"></dx:ASPxHiddenField>
                                                                                                    <table style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="vertical-align: top">
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%;">
                                                                                                                            <div id="OperatingUnit_label" runat="server">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Operating Unit" Theme="Office2010Blue" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <div id="OperatingUnit_combo" runat="server">
                                                                                                                                <dx:ASPxComboBox ID="OperatingUnit" runat="server" ClientInstanceName="OperatingUnitCA" OnInit="OperatingUnit_Init" AutoResizeWithContainer="true" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                    <ClientSideEvents SelectedIndexChanged="OperatingUnitCA_SelectedIndexChanged" />
                                                                                                                                    <ValidationSettings ValidateOnLeave="False" EnableCustomValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter value">
                                                                                                                                    </ValidationSettings>
                                                                                                                                </dx:ASPxComboBox>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Product Category" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxComboBox ID="ProdCat" runat="server" ClientInstanceName="ProdCatCAPEX" OnInit="ProdCat_Init" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                <ClientSideEvents SelectedIndexChanged="ProdCat_SelectedIndexChanged" />
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please select value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxComboBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Description" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Description" runat="server" ClientInstanceName="DescriptionCAPEX" Text='<%#Eval("Description")%>' Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="UOM" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxComboBox ID="UOM" runat="server" ClientInstanceName="UOMCAPEX" OnInit="UOM_Init" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxComboBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="Qty" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Qty" runat="server" ClientInstanceName="QtyCAPEX" Text='<%#Eval("Qty")%>' HorizontalAlign="Right" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                <ClientSideEvents ValueChanged="OnValueChangeQty" />
                                                                                                                                <ClientSideEvents KeyUp="OnKeyUpQtyCapex" />
                                                                                                                                <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Cost" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Cost" runat="server" ClientInstanceName="CostCAPEX" Text='<%#Eval("Cost")%>' HorizontalAlign="Right" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                <ClientSideEvents ValueChanged="OnValueChange" />
                                                                                                                                <ClientSideEvents KeyUp="OnKeyUpCostCapex" />
                                                                                                                                <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="Total Cost" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="TotalCost" runat="server" ClientInstanceName="TotalCostCAPEX" Text='<%#Eval("TotalCost")%>' ReadOnly="true" HorizontalAlign="Right" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter a value in Cost and Qty field" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </dx:ContentControl>
                                                                                            </ContentCollection>
                                                                                        </dx:TabPage>
                                                                                    </TabPages>
                                                                                </dx:ASPxPageControl>
                                                                            </div>
                                                                            <div style="text-align: right; padding: 2px">
                                                                                <dx:ASPxButton runat="server" Text="Save" Theme="Office2010Blue" AutoPostBack="false">
                                                                                    <ClientSideEvents Click="updateCAPEX" />
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton runat="server" Text="Cancel" CausesValidation="false" Theme="Office2010Blue" AutoPostBack="false">
                                                                                    <ClientSideEvents Click="function(s,e){CAPEXGrid.CancelEdit();}" />
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </EditForm>
                                                                    </Templates>
                                                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                                    <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                                                        AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" ConfirmDelete="true" />
                                                                    <SettingsText ConfirmDelete="Delete This Item?" />
                                                                </dx:ASPxGridView>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5"></td>
                                            </tr>
                                        </table>
                                        <%--</div>--%>
                                    </dx:ContentControl>
                                </ContentCollection>

                            </dx:TabPage>
                            <dx:TabPage Text="Revenue and Assumptions">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <dx:ASPxRoundPanel ID="RevenueRoundPanel" runat="server" ClientInstanceName="RevRoundPanel" HeaderText="REVENUE & ASSUMPTIONS" EnableAnimation="true" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                                                        <ClientSideEvents CollapsedChanging="RevRoundPanel_CollapsedChanging" />
                                                        <PanelCollection>
                                                            <dx:PanelContent>
                                                                <dx:ASPxGridView ID="RevenueGrid" runat="server" ClientInstanceName="RevenueGrid" Width="100%" Theme="Office2010Blue"
                                                                    OnInitNewRow="RevenueGrid_InitNewRow"
                                                                    OnRowInserting="RevenueGrid_RowInserting"
                                                                    OnRowDeleting="RevenueGrid_RowDeleting"
                                                                    OnStartRowEditing="RevenueGrid_StartRowEditing"
                                                                    OnRowUpdating="RevenueGrid_RowUpdating"
                                                                    OnBeforeGetCallbackResult="RevenueGrid_BeforeGetCallbackResult"
                                                                    OnDataBound="RevenueGrid_DataBound"
                                                                    OnCancelRowEditing="RevenueGrid_CancelRowEditing">
                                                                    <ClientSideEvents RowClick="function(s,e){focused(s,e,'Revenue');}" />
                                                                    <ClientSideEvents CustomButtonClick="RevenueGrid_CustomButtonClick" />
                                                                    <ClientSideEvents BeginCallback="function(s,e){loadingPanel.Show();}" />
                                                                    <ClientSideEvents EndCallback="function(s,e){loadingPanel.Hide();}" />
                                                                    <Columns>
                                                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                                            <HeaderTemplate>
                                                                                <div style="text-align: left">
                                                                                    <dx:ASPxButton ID="Add" runat="server" Image-Url="Images/Add.ico" Image-Width="15px" Image-ToolTip="New Row" RenderMode="Link" AutoPostBack="false" HorizontalAlign="Center" VerticalAlign="Middle">
                                                                                        <ClientSideEvents Click="RevenueGrid_Add" />
                                                                                    </dx:ASPxButton>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <CustomButtons>
                                                                                <dx:GridViewCommandColumnCustomButton ID="REVEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                                                    <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                                                </dx:GridViewCommandColumnCustomButton>
                                                                                <dx:GridViewCommandColumnCustomButton ID="REVDelete" Image-AlternateText="Delete" Image-Url="Images/Delete.ico" Image-ToolTip="Delete Row" Image-Width="15px">
                                                                                    <Image AlternateText="Delete" ToolTip="Delete Row" Width="15px" Url="Images/Delete.ico"></Image>
                                                                                </dx:GridViewCommandColumnCustomButton>
                                                                            </CustomButtons>
                                                                        </dx:GridViewCommandColumn>
                                                                        <dx:GridViewDataColumn FieldName="PK" Visible="false" ></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false" ></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="VALUE" Visible="false"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit" ></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="ProductName" Caption="Product" Width="400px"> </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="FarmName" Visible="false" Caption="Location" ></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="UOM"></dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Volume"  CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="Prize" Caption="Price" CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                        <dx:GridViewDataColumn FieldName="TotalPrize" Caption="Total"  CellStyle-HorizontalAlign="Right">
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                                            <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                                        </dx:GridViewDataColumn>
                                                                    </Columns>

                                                                    <SettingsCommandButton>
                                                                        <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Edit.ico"></Image>
                                                                        </EditButton>
                                                                        <DeleteButton ButtonType="Image" Image-Url="Images/Delete.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Delete.ico"></Image>
                                                                        </DeleteButton>
                                                                        <NewButton ButtonType="Image" Image-Url="Images/Add.ico" Image-Width="15px">
                                                                            <Image Width="15px" Url="Images/Add.ico"></Image>
                                                                        </NewButton>
                                                                    </SettingsCommandButton>
                                                                    <SettingsEditing Mode="EditFormAndDisplayRow"></SettingsEditing>
                                                                    <SettingsLoadingPanel Mode="Disabled" />

                                                                    <TotalSummary>
                                                                        <dx:ASPxSummaryItem FieldName="Volume" SummaryType="Sum" ShowInColumn="Volume" DisplayFormat="Total: {0:0,0.00}" />
                                                                        <dx:ASPxSummaryItem FieldName="TotalPrize" SummaryType="Sum" ShowInColumn="TotalPrize" DisplayFormat="Total: {0:0,0.00}" />
                                                                    </TotalSummary>

                                                                    <Templates>
                                                                        <EditForm>
                                                                            <div style="padding: 4px 3px 4px">
                                                                                <dx:ASPxPageControl ID="RevenuePageControl" runat="server" Width="100%" Theme="Office2010Blue">
                                                                                    <TabPages>
                                                                                        <dx:TabPage Text="Revenue & Assumptions" Visible="true">
                                                                                            <ContentCollection>
                                                                                                <dx:ContentControl runat="server">
                                                                                                    <dx:ASPxHiddenField ID="entityhidden" runat="server" ClientInstanceName="entityhiddenREV"></dx:ASPxHiddenField>
                                                                                                    <table style="width: 100%;">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%;">
                                                                                                                            <div id="OperatingUnit_label" runat="server">
                                                                                                                                <dx:ASPxLabel runat="server" Text="Operating Unit" Theme="Office2010Blue" />
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <div id="OperatingUnit_combo" runat="server">
                                                                                                                                <dx:ASPxComboBox ID="OperatingUnit" runat="server" ClientInstanceName="OperatingUnitREV" OnInit="OperatingUnit_Init" AutoResizeWithContainer="true" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue" Width="300px">
                                                                                                                                    <ClientSideEvents SelectedIndexChanged="OperatingUnitREV_SelectedIndexChanged" />
                                                                                                                                    <ValidationSettings ValidateOnLeave="False" EnableCustomValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter value">
                                                                                                                                    </ValidationSettings>
                                                                                                                                </dx:ASPxComboBox>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Product Name" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="ProductName" runat="server" ClientInstanceName="ProductNameRev" Text='<%#Eval("ProductName")%>' Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                         <td style="width: 40%">
                                                                                                                            <dx:ASPxLabel runat="server" Text="UOM"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxComboBox ID="UOM" runat="server" ValueType="System.String" OnInit="UOM_Init" Width="300px"></dx:ASPxComboBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <%--<tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="Location" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="FarmName" runat="server" ClientInstanceName="FarmNameRev" Text='<%#Eval("FarmName")%>' Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="false"></ValidationSettings>
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>--%>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="Volume" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Volume" runat="server" ClientInstanceName="VolumeRev" Text='<%#Eval("Volume")%>' Width="300px" HorizontalAlign="Right" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                <ClientSideEvents ValueChanged="OnValueChangeQty" />
                                                                                                                                <ClientSideEvents KeyUp="OnKeyUpQtyRev" />
                                                                                                                                <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 40%">
                                                                                                                            <dx:ASPxLabel runat="server" Text="Price" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="Prize" runat="server" ClientInstanceName="PrizeRev" Text='<%#Eval("Prize")%>' HorizontalAlign="Right" Width="300px" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter value" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                                <ClientSideEvents ValueChanged="OnValueChange" />
                                                                                                                                <ClientSideEvents KeyUp="OnKeyUpCostRev" />
                                                                                                                                <ClientSideEvents KeyPress="FilterDigit" />
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxLabel runat="server" Text="Total" Theme="Office2010Blue"></dx:ASPxLabel>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <dx:ASPxTextBox ID="TotalPrize" runat="server" ClientInstanceName="TotalPrizeRev" Text='<%#Eval("TotalPrize")%>' Width="300px" HorizontalAlign="Right" Theme="Office2010Blue">
                                                                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-ErrorText="Please enter a value in Prize and Volume field" RequiredField-IsRequired="true"></ValidationSettings>
                                                                                                                            </dx:ASPxTextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </dx:ContentControl>
                                                                                            </ContentCollection>
                                                                                        </dx:TabPage>
                                                                                    </TabPages>
                                                                                </dx:ASPxPageControl>
                                                                            </div>
                                                                            <div style="text-align: right; padding: 2px">
                                                                                <dx:ASPxButton runat="server" Text="Save" Theme="Office2010Blue" AutoPostBack="false">
                                                                                    <ClientSideEvents Click="updateRevenue" />
                                                                                </dx:ASPxButton>
                                                                                <dx:ASPxButton runat="server" Text="Cancel" CausesValidation="false" Theme="Office2010Blue" AutoPostBack="false">
                                                                                    <ClientSideEvents Click="function(s,e){RevenueGrid.CancelEdit();}" />
                                                                                </dx:ASPxButton>
                                                                            </div>
                                                                        </EditForm>
                                                                    </Templates>
                                                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                                    <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                                                        AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" ConfirmDelete="true" />
                                                                    <SettingsText ConfirmDelete="Delete This Item?" />
                                                                </dx:ASPxGridView>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxRoundPanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                    </dx:ASPxPageControl>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>


</asp:Content>
