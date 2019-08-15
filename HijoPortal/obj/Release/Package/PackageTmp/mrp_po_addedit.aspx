﻿<%@ Page Title="Purchase Order" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="mrp_po_addedit.aspx.cs" Inherits="HijoPortal.mrp_po_addedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/POAddedit.css" rel="stylesheet" />
    <script type="text/javascript" src="jquery/POAddedit.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" Theme="Office2010Blue"></dx:ASPxLoadingPanel>

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


    <dx:ASPxPopupControl ID="DeletePopup" runat="server" ClientInstanceName="DeletePopupClient" Modal="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Theme="Moderno" Width="400px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table class="innertable">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Are you sure you want to delete?" Theme="Moderno"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <dx:ASPxButton ID="OK" runat="server" Text="OK" AutoPostBack="false" Theme="Moderno">
                                <ClientSideEvents Click="DeleteItem" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="CancelPopUp" runat="server" Text="CANCEL" AutoPostBack="false" Theme="Moderno">
                                <ClientSideEvents Click="function(s,e){DeletePopupClient.Hide();}" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="PONotify" ClientInstanceName="POAddEdit_MRPNotify" runat="server" Modal="true" CloseAction="CloseButton" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" Theme="Moderno" ContentStyle-Paddings-Padding="20" Width="400px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <dx:ASPxLabel ID="PONotifyLbl" ClientInstanceName="POAddEdit_MRPNotificationMessage" runat="server" Text="" ForeColor="Red" Theme="Moderno"></dx:ASPxLabel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="PopupSubmit" ClientInstanceName="POAddEditPopupSubmit" runat="server" Modal="true" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" Theme="Moderno" Width="400px">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td colspan="2" style="padding-right: 20px; padding-bottom: 20px;">
                            <dx:ASPxLabel runat="server" Text="Are you sure you want to submit this Purchase Order document to AX?" Theme="Moderno"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <dx:ASPxButton ID="OK_SUBMIT" runat="server" Text="SUBMIT" OnClick="Submit_Click" Theme="Moderno" AutoPostBack="false">
                                <ClientSideEvents Click="function(s,e){
                                    POAddEditPopupSubmit.Hide();
                                    $find('ModalPopupExtenderLoading').show();
                                    e.processOnServer = true;
                                    }" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="CANCEL_SUBMIT" runat="server" Text="CANCEL" Theme="Moderno" AutoPostBack="false">
                                <ClientSideEvents Click="function(s,e){POAddEditPopupSubmit.Hide();}" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <div id="dvHeader" style="height: 30px;">
        <h1>Purchase Order</h1>
    </div>
    <div>
        <table class="table_detail" border="0">
            <tr style="height: 23px;">
                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="PO Number" Theme="Office2010Blue"></dx:ASPxLabel>
                    <div style="display: none;">
                        <dx:ASPxTextBox ID="txtStatus" ClientInstanceName="txtStatus" runat="server" Width="100%"></dx:ASPxTextBox>
                    </div>
                </td>
                <td class="table_po_semi">:</td>
                <td class="table_po_td_data">
                    <table class="innertable" border="0">
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="PONumberLbl" runat="server" Text="" Theme="Office2010Blue" Width="100%" Font-Bold="true"></dx:ASPxLabel>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td>
                    <dx:ASPxLabel runat="server" Text="Status" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td>:</td>
                <td>
                    <table class="innertable" border="0">
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="StatusLbl" runat="server" Text="" Theme="Office2010Blue" Width="100%"></dx:ASPxLabel>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="Supplier" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi">:</td>
                <td class="table_po_td_data">
                    <table class="innertable" border="0">
                        <tr>
                            <td>
                                <dx:ASPxComboBox ID="VendorCombo" runat="server" Width="100%" ValueType="System.String" Theme="Office2010Blue">
                                    <ClientSideEvents SelectedIndexChanged="VendorCombo_SelectedIndexChanged" />
                                    <ValidationSettings ErrorImage-Width="10px" ErrorDisplayMode="ImageWithTooltip" RequiredField-IsRequired="true"></ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="VendorLbl" runat="server" ClientInstanceName="VendorLblClient" CssClass="innertable_width" Text="" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                </td>

                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="Site" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi">:</td>
                <td class="table_po_td_data">
                    <table class="innertable" border="0">
                        <tr>
                            <td>
                                <dx:ASPxComboBox ID="SiteCombo" runat="server" Width="100%" ValueType="System.String" Theme="Office2010Blue">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-IsRequired="true"></ValidationSettings>
                                    <ClientSideEvents SelectedIndexChanged="SiteCombo_SelectedIndexChanged" />
                                </dx:ASPxComboBox>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="SiteLbl" runat="server" ClientInstanceName="SiteLblClient" CssClass="innertable_width" Text="" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                </td>

                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="Expected Delivery" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi">:</td>
                <td class="table_po_td_data" style="width: 10%;">
                    <dx:ASPxDateEdit ID="ExpDel" runat="server" AllowUserInput="false" CssClass="innertable_width" Width="100%" Theme="Office2010Blue">
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-IsRequired="true"></ValidationSettings>
                        <%--<ClientSideEvents GotFocus="function(s,e){s.ShowDropDown(); }" />--%>
                    </dx:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="Terms" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi">:</td>
                <td class="table_po_td_data">
                    <dx:ASPxCallbackPanel ID="TermsCallback" runat="server" ClientInstanceName="TermsCallbackClient" OnCallback="TermsCallback_Callback" CssClass="innertable_width">
                        <PanelCollection>
                            <dx:PanelContent>
                                <table class="innertable">
                                    <tr>
                                        <td>
                                            <dx:ASPxComboBox ID="TermsCombo" runat="server" Width="100%" ValueType="System.String" Theme="Office2010Blue">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-IsRequired="true"></ValidationSettings>
                                                <ClientSideEvents SelectedIndexChanged="TermsCombo_SelectedIndexChanged" />
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="TermsLbl" runat="server" ClientInstanceName="TermsLblClient" CssClass="innertable_width" Text="" Theme="Office2010Blue"></dx:ASPxLabel>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                </td>

                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="Warehouse" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi">:</td>
                <td class="table_po_td_data">
                    <table class="innertable">
                        <tr>
                            <td>
                                <dx:ASPxCallbackPanel ID="WarehouseCallback" runat="server" ClientInstanceName="WarehouseCallbackClient" OnCallback="WarehouseCallback_Callback">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <dx:ASPxComboBox ID="WarehouseCombo" runat="server" Width="100%" ValueType="System.String" Theme="Office2010Blue">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-IsRequired="true"></ValidationSettings>
                                                <ClientSideEvents SelectedIndexChanged="WarehouseCombo_SelectedIndexChanged" />
                                            </dx:ASPxComboBox>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="WarehouseLbl" runat="server" ClientInstanceName="WarehouseLblClient" CssClass="innertable_width" Text="" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>

                </td>

                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="MOP Reference" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi">:</td>
                <td style="width: 10%; vertical-align: top;">
                    <dx:ASPxTextBox ID="MOPReference" runat="server" Width="170px" ReadOnly="true" BackColor="Transparent" Border-BorderColor="Transparent" Theme="Office2010Blue"></dx:ASPxTextBox>
                    <%--<dx:ASPxListBox ID="MOPRef" runat="server" ValueType="System.String" Theme="Office2010Blue"></dx:ASPxListBox>--%>
                </td>
            </tr>
            <tr>
                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="Currecny" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi">:</td>
                <td class="table_po_td_data">
                    <table class="innertable">
                        <tr>
                            <td>
                                <dx:ASPxCallbackPanel ID="CurrencyCallback" runat="server" ClientInstanceName="CurrencyCallbackClient" OnCallback="CurrencyCallback_Callback">
                                    <ClientSideEvents EndCallback="CurrencyCallback_EndCallback" />
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <dx:ASPxComboBox ID="CurrencyCombo" runat="server" ClientInstanceName="CurrencyComboClient" Width="100%" ValueType="System.String" Theme="Office2010Blue">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-IsRequired="true"></ValidationSettings>
                                                <ClientSideEvents SelectedIndexChanged="CurrencyCombo_SelectedIndexChanged" />
                                            </dx:ASPxComboBox>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="CurrencyLbl" runat="server" ClientInstanceName="CurrencyLblClient" CssClass="innertable_width" Text="" Theme="Office2010Blue"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>

                </td>

                <td class="table_po_td_label">
                    <dx:ASPxLabel runat="server" Text="Location" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi">:</td>
                <td class="table_po_td_data">
                    <table class="innertable">
                        <tr>
                            <td>
                                <dx:ASPxCallbackPanel ID="LocationCallback" runat="server" ClientInstanceName="LocationCallbackClient" OnCallback="LocationCallback_Callback">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <dx:ASPxComboBox ID="LocationCombo" runat="server" Width="100%" ValueType="System.String" Theme="Office2010Blue">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-IsRequired="true"></ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td class="table_po_td_label" style="vertical-align: middle;">
                    <dx:ASPxLabel runat="server" Text="Remarks" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td class="table_po_semi" style="vertical-align: middle;">:</td>
                <td class="table_po_td_data" colspan="7" style="padding-top: 5px;">
                    <dx:ASPxTextBox ID="Remarks" runat="server" Width="100%" Theme="Office2010Blue">
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" RequiredField-IsRequired="true"></ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="grid_style">
        <dx:ASPxGridView ID="POAddEditGrid" runat="server" ClientInstanceName="POAddEditGridClient" Width="100%" Theme="Office2010Blue"
            OnStartRowEditing="POAddEditGrid_StartRowEditing"
            OnRowUpdating="POAddEditGrid_RowUpdating"
            OnRowDeleting="POAddEditGrid_RowDeleting"
            OnBeforeGetCallbackResult="POAddEditGrid_BeforeGetCallbackResult"
            OnCancelRowEditing="POAddEditGrid_CancelRowEditing"
            OnDataBound="POAddEditGrid_DataBound">
            <ClientSideEvents CustomButtonClick="POAddEditGrid_CustomButtonClick" />
            <ClientSideEvents EndCallback="POAddEditGrid_EndCallback" />
            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image" Width="40">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                    <CustomButtons>
                        <dx:GridViewCommandColumnCustomButton ID="Edit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px"></dx:GridViewCommandColumnCustomButton>
                        <dx:GridViewCommandColumnCustomButton ID="Delete" Image-Url="images/Delete.ico" Image-Width="15px"></dx:GridViewCommandColumnCustomButton>
                        <dx:GridViewCommandColumnCustomButton ID="Update" Image-Url="images/Save.ico" Image-Width="15px" Visibility="EditableRow"></dx:GridViewCommandColumnCustomButton>
                        <dx:GridViewCommandColumnCustomButton ID="Cancel" Image-Url="images/Undo.ico" Image-Width="15px" Visibility="EditableRow"></dx:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dx:GridViewCommandColumn>
                <dx:GridViewDataColumn FieldName="PK" Visible="false"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="ItemPK" Visible="false"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Identifier" Visible="false"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="ProdCat" Visible="false"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="ItemCode">
                    <EditItemTemplate>
                        <dx:ASPxLabel runat="server" Text='<%#Eval("ItemCode") %>' Theme="Office2010Blue"></dx:ASPxLabel>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="CapexCIP" Caption="Capex ID">
                    <EditItemTemplate>
                        <dx:ASPxLabel runat="server" Text='<%#Eval("CapexCIP") %>' Theme="Office2010Blue"></dx:ASPxLabel>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Description" Width="300px">
                    <EditItemTemplate>
                        <dx:ASPxLabel runat="server" Text='<%#Eval("Description") %>' Theme="Office2010Blue"></dx:ASPxLabel>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="RequestedQty">
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <EditCellStyle HorizontalAlign="Right"></EditCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxLabel ID="RequestedQty" runat="server" ClientInstanceName="ReqQtyClient" Text='<%#Eval("RequestedQty") %>' Theme="Office2010Blue"></dx:ASPxLabel>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Cost">
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <EditCellStyle HorizontalAlign="Right"></EditCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxLabel runat="server" Text='<%#Eval("Cost") %>' Theme="Office2010Blue"></dx:ASPxLabel>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="TotalCost">
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <EditCellStyle HorizontalAlign="Right"></EditCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxLabel runat="server" Text='<%#Eval("TotalCost") %>' Theme="Office2010Blue"></dx:ASPxLabel>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="POUOM" Caption="PO UOM">
                    <HeaderStyle HorizontalAlign="Left" />
                    <CellStyle HorizontalAlign="Left"></CellStyle>
                    <EditCellStyle HorizontalAlign="Left"></EditCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxComboBox ID="POUOM" runat="server" ClientInstanceName="POUOMClient" ValueType="System.String" Width="100px" OnInit="POUOM_Init" Theme="Office2010Blue">
                            <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                        </dx:ASPxComboBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="POQty">
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <EditCellStyle HorizontalAlign="Right"></EditCellStyle>
                    <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxTextBox ID="POQty" runat="server" ClientInstanceName="POQtyClient" Text='<%#Eval("POQty") %>' HorizontalAlign="Right" Width="60px" Theme="Office2010Blue">
                            <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                            <ClientSideEvents KeyUp="POQty_KeyUp" />
                            <ClientSideEvents ValueChanged="OnValueChangeQty" />
                            <ClientSideEvents KeyPress="FilterDigit" />
                        </dx:ASPxTextBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="POCost">
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <EditCellStyle HorizontalAlign="Right"></EditCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxTextBox ID="POCost" runat="server" ClientInstanceName="POCostClient" Text='<%#Eval("POCost") %>' HorizontalAlign="Right" Width="80px" Theme="Office2010Blue">
                            <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                            <ClientSideEvents KeyUp="POCost_KeyUp" />
                            <ClientSideEvents ValueChanged="OnValueChange" />
                            <ClientSideEvents KeyPress="FilterDigit" />
                        </dx:ASPxTextBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="TotalPOCost">
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <EditCellStyle HorizontalAlign="Right"></EditCellStyle>
                    <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxTextBox ID="TotalPOCost" runat="server" ClientInstanceName="TotalPOCostClient" Text='<%#Eval("TotalPOCost") %>' HorizontalAlign="Right" ReadOnly="true" Border-BorderColor="Transparent" Width="100px" Theme="Office2010Blue">
                            <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                        </dx:ASPxTextBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="wVAT" Caption="w/ VAT">
                    <HeaderStyle HorizontalAlign="Center" />
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <DataItemTemplate>
                        <dx:ASPxCheckBox runat="server" Checked='<%#Eval("wVAT") %>' Theme="Office2010Blue" ReadOnly="true"></dx:ASPxCheckBox>
                    </DataItemTemplate>
                    <EditItemTemplate>
                        <dx:ASPxCheckBox ID="CheckwVAT" runat="server" ClientInstanceName="CheckwVATClient" Checked='<%#Eval("wVAT") %>' Theme="Office2010Blue">
                            <ClientSideEvents CheckedChanged="POCheck_Changed" />
                        </dx:ASPxCheckBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="POCostwVAT" Caption="PO Cost w/ VAT">
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <EditCellStyle HorizontalAlign="Right"></EditCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxTextBox ID="POCostwVAT" ClientInstanceName="POCostwVATClient" Text='<%#Eval("POCostwVAT") %>' runat="server" Width="80px" HorizontalAlign="Right" Border-BorderColor="Transparent" Theme="Office2010Blue" ReadOnly="true">
                            <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                            <%--<ClientSideEvents ValueChanged="POCostwVAT_Changed" />--%>
                            <%--<ClientSideEvents TextChanged ="POCostwVAT_Changed" />--%>
                        </dx:ASPxTextBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="TotalPOCostwVAT" Caption="Total Cost w/ VAT">
                    <HeaderStyle HorizontalAlign="Right" />
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <EditCellStyle HorizontalAlign="Right"></EditCellStyle>
                    <EditItemTemplate>
                        <dx:ASPxTextBox ID="TotalPOCostwVAT" ClientInstanceName="TotalPOCostwVATClient" Text='<%#Eval("TotalPOCostwVAT") %>' runat="server" Width="100px" HorizontalAlign="Right" Border-BorderColor="Transparent" Theme="Office2010Blue" ReadOnly="true">
                            <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                        </dx:ASPxTextBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="TaxGroup">
                    <EditItemTemplate>
                        <dx:ASPxComboBox ID="TaxGroup" runat="server" ClientInstanceName="TaxGroupClient" OnInit="TaxGroup_Init" Width="100px" ValueType="System.String" Theme="Office2010Blue">
                            <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                        </dx:ASPxComboBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="TaxItemGroup">
                    <EditItemTemplate>
                        <dx:ASPxComboBox ID="TaxItemGroup" runat="server" ClientInstanceName="TaxItemGroupClient" OnInit="TaxItemGroup_Init" Width="100px" ValueType="System.String" Theme="Office2010Blue">
                            <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                        </dx:ASPxComboBox>
                    </EditItemTemplate>
                </dx:GridViewDataColumn>
            </Columns>
            <Styles>
                <Cell Wrap="true"></Cell>
                <InlineEditCell Wrap="true"></InlineEditCell>
            </Styles>
            <SettingsEditing Mode="Inline"></SettingsEditing>
            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                AllowSort="true" ProcessFocusedRowChangedOnServer="True" ProcessSelectionChangedOnServer="True" AllowDragDrop="false" />
            <Settings ShowFooter="true" />

            <TotalSummary>
                <dx:ASPxSummaryItem FieldName="POQty" SummaryType="Sum" ShowInColumn="POQty" DisplayFormat="Total: {0:0,0.00}" />
                <dx:ASPxSummaryItem FieldName="TotalPOCost" SummaryType="Sum" ShowInColumn="TotalPOCost" DisplayFormat="Total: {0:0,0.00}" />
            </TotalSummary>

            <%--<SettingsCommandButton>
                    <EditButton Image-Url="images/Edit.ico">
                        <Image Width="15px"></Image>
                    </EditButton>
                    <DeleteButton Image-Url="images/Delete.ico">
                        <Image Width="15px"></Image>
                    </DeleteButton>
                    <UpdateButton Image-Url="images/Save.ico">
                        <Image Width="15px"></Image>
                    </UpdateButton>
                    <CancelButton Image-Url="images/Undo.ico">
                        <Image Width="15px"></Image>
                    </CancelButton>
                </SettingsCommandButton>--%>
        </dx:ASPxGridView>
    </div>
    <div>
        <table class="innertable" style="margin-top: 10px;">
            <tr>
                <td style="text-align: right;">
                    <dx:ASPxButton ID="Save" runat="server" ClientInstanceName="SaveClient" OnClick="Save_Click" AutoPostBack="false" Text="SAVE" Theme="Office2010Blue"></dx:ASPxButton>
                    <dx:ASPxButton ID="Submit" runat="server" ClientInstanceName="SubmitClient" AutoPostBack="false" Text="Submit to AX" Theme="Office2010Blue">
                        <ClientSideEvents Click="POAddEditSubmitBtn_Click" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
