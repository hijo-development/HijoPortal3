﻿<%@ Page Title="MOP Inventory Analyst (Edit)" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="mrp_inventanalyst.aspx.cs" Inherits="HijoPortal.mrp_inventanalyst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="jquery/MRPInventAnalyst.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dx:ASPxPopupControl ID="MRPNotify" ClientInstanceName="Add_Edit_MRPNotify_InventAnal" runat="server" Modal="true" CloseAction="CloseButton" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" Theme="Moderno" ContentStyle-Paddings-Padding="20">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <dx:ASPxLabel ID="MRPNotificationMessage" ClientInstanceName="Add_Edit_MRPNotificationMessage_InventAnal" runat="server" Text="" ForeColor="Red" Theme="Moderno"></dx:ASPxLabel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="PopupSubmit" ClientInstanceName="PopupSubmit" runat="server" Modal="true" Width="400px" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" Theme="Moderno">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td colspan="2" style="padding-right: 20px; padding-bottom: 20px;">
                            <dx:ASPxLabel runat="server" Text="Are you sure you want to submit this document?" Theme="Moderno"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <dx:ASPxButton ID="OK_SUBMIT" runat="server" Text="SUBMIT" Theme="Moderno" OnClick="Submit_Click" AutoPostBack="false">
                                <%--<ClientSideEvents Click="OK_DELETE" />--%>
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

    <dx:ASPxPopupControl ID="MRPAccessRights" ClientInstanceName="MRPAccessRights" runat="server" Width="100%" Modal="true" ShowCloseButton ="false" PopupAnimationType="Fade" CloseAnimationType="Fade"  PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="true" Theme="Moderno">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td style="padding-right: 20px; padding-bottom: 20px;">
                            <dx:ASPxLabel ID="MRPAccessRightsMsg" ClientInstanceName="MRPAccessRightsMsg" runat="server" Text="" Theme="Moderno" ForeColor="Red" Width="300"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr style ="height:10px;">
                        <td></td>
                    </tr>
                    <tr>
                        <td style="padding-right: 20px; padding-bottom: 20px; text-align:right;">
                            <dx:ASPxButton ID="RightsOK" runat="server" Text="OK" OnClick="RightsOK_Click" Theme="Moderno" AutoPostBack="false">
                                <ClientSideEvents Click="function(s,e){
                                    MRPAccessRights.Hide();
                                    $find('ModalPopupExtenderLoading').show();
                                    e.processOnServer = true;
                                    }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <div id="dvHeader">
        <h1 id="mrpHead" runat="server"></h1>
        <%--<h1>M O P  Details (Inventory Analyst)</h1>--%>
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
                    <span style="font-size: 30px; cursor: pointer" onclick="openNav()">&#9776;</span>
                </td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxLabel runat="server" Text="MONTH" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td>:</td>
                <td style="width: 20%">
                    <dx:ASPxLabel ID="Month" runat="server" Text="" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
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
                        <dx:ASPxLabel ID="Entity" runat="server" Text="" ClientInstanceName="EntityCodeInventDirect" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                        <dx:ASPxLabel ID="BU" runat="server" Text="" ClientInstanceName="BUCodeInventDirect" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
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
                    <dx:ASPxLabel runat="server" ID="Status" Text="" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <dx:ASPxLabel runat="server" Text="CREATOR" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td>:</td>
                <td>
                    <dx:ASPxLabel ID="Creator" runat="server" Text="" CssClass="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                </td>
                <td>
                    <div style="display: none;">
                        <dx:ASPxTextBox ID="WorkFlowLineTxt" ClientInstanceName="WorkFlowLineTxtInventAnal" runat="server" Width="170px"></dx:ASPxTextBox>
                        <dx:ASPxTextBox ID="StatusKeyTxt" ClientInstanceName="StatusKeyTxtInventAnal" runat="server" Width="170px"></dx:ASPxTextBox>
                        <dx:ASPxLabel ID="WorkFlowLineLbl" ClientInstanceName="WorkFlowLineLblDirect" runat="server" Text=""></dx:ASPxLabel>
                        <dx:ASPxLabel ID="StatusKeyLbl" ClientInstanceName="StatusKeyLblDirect" runat="server" Text=""></dx:ASPxLabel>
                    </div>
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="7" style="text-align: right">
                    <%--OnClick="Submit_Click"--%>
                    <dx:ASPxButton ID="Submit" runat="server" Text="Submit" AutoPostBack="false" Theme="Office2010Blue">
                        <ClientSideEvents Click="function(s,e){PopupSubmit.SetHeaderText('Confirm'); PopupSubmit.Show();}" />
                    </dx:ASPxButton>
                    &nbsp
                            <%--<dx:ASPxButton ID="MRPList" runat="server" Text="MOP LIST" AutoPostBack="false" Theme="Office2010Blue" OnClick="MRPList_Click"></dx:ASPxButton>
                        &nbsp--%>
                    <dx:ASPxButton ID="Preview" runat="server" Text="Preview" AutoPostBack="false" Theme="Office2010Blue" OnClick="Preview_Click"></dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>

    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" Width="100%" ActiveTabIndex="0" EnableHierarchyRecreation="true" Theme="Office2010Blue">
        <TabPages>
            <dx:TabPage Text="MRP">
                <ContentCollection>
                    <dx:ContentControl>

                        <dx:ASPxHiddenField ID="ASPxHiddenFieldDMWrkFlwLnInventAnal" ClientInstanceName="ASPxHiddenFieldDMWrkFlwLnInventAnalDirect" runat="server"></dx:ASPxHiddenField>
                        <dx:ASPxHiddenField ID="ASPxHiddenFieldDMStatusKeyInventAnal" ClientInstanceName="ASPxHiddenFieldDMStatusKeyInventAnalDirect" runat="server"></dx:ASPxHiddenField>

                        <dx:ASPxRoundPanel ID="DirectMaterialsRoundPanel" ClientInstanceName="DMGridRoundPanel" runat="server" HeaderText="DIRECT MATERIALS" Font-Bold="true" EnableAnimation="true" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                            <ClientSideEvents CollapsedChanging="DMGridRoundPanel_CollapsedChanging" />
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxGridView ID="DMGrid" runat="server" ClientInstanceName="DMGrid" EnableCallBacks="True" Width="100%" Theme="Office2010Blue"
                                        OnStartRowEditing="DMGrid_StartRowEditing"
                                        OnRowUpdating="DMGrid_RowUpdating"
                                        OnBeforeGetCallbackResult="DMGrid_BeforeGetCallbackResult"
                                        OnDataBound="DMGrid_DataBound"
                                        OnCancelRowEditing="DMGrid_CancelRowEditing">
                                        <%--<ClientSideEvents RowClick="function(s,e){focused(s,e,'Materials');}" />--%>
                                        <ClientSideEvents RowClick="function(s,e){focusedInventAnal(s,e,'Materials');}" />
                                        <ClientSideEvents CustomButtonClick="DMGrid_CustomButtonClick" />

                                        <Columns>
                                            <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="DMGridEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                        <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataColumn FieldName="PK" Visible="false" VisibleIndex="1"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false" VisibleIndex="2"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ActivityCode" Caption="Activity" VisibleIndex="3">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("ActivityCode")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit" VisibleIndex="4">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("RevDesc")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ItemCode" VisibleIndex="5">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("ItemCode")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ItemDescription" VisibleIndex="6">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("ItemDescription")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="UOM" VisibleIndex="7">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("UOM")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Qty" Caption="Requested Qty" VisibleIndex="8">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel ID="inv_qty" runat="server" ClientInstanceName="inv_qty_dm" Text='<%#Eval("Qty")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Cost" VisibleIndex="9">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("Cost")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TotalCost" VisibleIndex="10">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("TotalCost")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedQty" Caption="Recommended Qty For Purchase" CellStyle-BackColor="#66ffcc" HeaderStyle-BackColor="#66ffcc" VisibleIndex="11">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittedQty" ClientInstanceName="InvEdittedQty" runat="server" Text='<%#Eval("EdittedQty") %>' Width="100%" Theme="Office2010Blue" HorizontalAlign="Right">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                                                        <ClientSideEvents ValueChanged="OnValueChangeQty" KeyPress="FilterDigit" KeyUp="OnKeyUpQtytInvDirect" />
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedCost" Caption="Cost/Unit" VisibleIndex="12">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittedCost" ClientInstanceName="InvEdittedCost" runat="server" Text='<%#Eval("EdittedCost") %>' Width="100%" Theme="Office2010Blue" HorizontalAlign="Right">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                                                        <ClientSideEvents ValueChanged="OnValueChange" KeyPress="FilterDigit" KeyUp="OnKeyUpCosttInvDirect" />
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittiedTotalCost" Caption="Total Cost" VisibleIndex="13">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittiedTotalCost" ClientInstanceName="InvEdittiedTotalCost" runat="server" Text='<%#Eval("EdittiedTotalCost") %>' Width="100%" Border-BorderColor="Transparent" Theme="Office2010Blue">
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                        </Columns>

                                        <SettingsCommandButton>
                                            <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px"></EditButton>
                                            <UpdateButton ButtonType="Image" Image-Url="images/Save.ico" Image-Width="15px"></UpdateButton>
                                            <CancelButton ButtonType="Image" Image-Url="images/Undo.ico" Image-Width="15px"></CancelButton>
                                        </SettingsCommandButton>
                                        <SettingsEditing Mode="Inline"></SettingsEditing>

                                        <TotalSummary>
                                            <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" ShowInColumn="TotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" ShowInColumn="Qty" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="EdittiedTotalCost" SummaryType="Sum" ShowInColumn="EdittiedTotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="EdittedQty" SummaryType="Sum" ShowInColumn="EdittedQty" DisplayFormat="Total: {0:0,0.00}" />
                                        </TotalSummary>

                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                        <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                            AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" />
                                    </dx:ASPxGridView>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <dx:ASPxRoundPanel ID="OpexRoundPanel" runat="server" ClientInstanceName="OPGridRoundPanel" HeaderText="OPEX" Font-Bold="true" EnableAnimation="true" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                            <ClientSideEvents CollapsedChanging="OPGridRoundPanel_CollapsedChanging" />
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxGridView ID="OpGrid" runat="server" ClientInstanceName="OpGrid" EnableCallBacks="True" Width="100%" Theme="Office2010Blue"
                                        OnStartRowEditing="OpGrid_StartRowEditing"
                                        OnRowUpdating="OpGrid_RowUpdating"
                                        OnBeforeGetCallbackResult="OpGrid_BeforeGetCallbackResult"
                                        OnDataBound="OpGrid_DataBound"
                                        OnCancelRowEditing="OpGrid_CancelRowEditing">
                                        <%--<ClientSideEvents RowClick="function(s,e){focused(s,e,'OPEX');}" />--%>
                                        <ClientSideEvents RowClick="function(s,e){focusedInventAnal(s,e,'OPEX');}" />
                                        <ClientSideEvents CustomButtonClick="OpGrid_CustomButtonClick" />

                                        <Columns>
                                            <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="OPGridEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                        <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataColumn FieldName="PK" Visible="false" VisibleIndex="1"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false" VisibleIndex="2"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ExpenseCodeName" Caption="Activity" VisibleIndex="3">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("ExpenseCodeName")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit" VisibleIndex="4">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("RevDesc")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ItemCode" VisibleIndex="5">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("ItemCode")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Description" VisibleIndex="6">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("Description")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="UOM" VisibleIndex="7">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("UOM")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Qty" Caption="Requested Qty" VisibleIndex="8">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel ID="inv_qty" runat="server" ClientInstanceName="inv_qty_op" Text='<%#Eval("Qty")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Cost" VisibleIndex="9">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("Cost")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TotalCost" VisibleIndex="10">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("TotalCost")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedQty" Caption="Recommended Qty For Purchase" CellStyle-BackColor="#66ffcc" HeaderStyle-BackColor="#66ffcc" VisibleIndex="11">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittedQtyOp" ClientInstanceName="InvEdittedQtyOp" runat="server" Text='<%#Eval("EdittedQty") %>' Width="100%" Theme="Office2010Blue" HorizontalAlign="Right">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                                                        <ClientSideEvents ValueChanged="OnValueChangeQty" KeyPress="FilterDigit" KeyUp="OnKeyUpQtytInvOpex" />
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedCost" Caption="Cost/Unit" VisibleIndex="12">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittedCostOp" ClientInstanceName="InvEdittedCostOp" runat="server" Text='<%#Eval("EdittedCost") %>' Width="100%" Theme="Office2010Blue" HorizontalAlign="Right">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                                                        <ClientSideEvents ValueChanged="OnValueChange" KeyPress="FilterDigit" KeyUp="OnKeyUpCosttInvOpex" />
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedTotalCost" Caption="Total Cost" VisibleIndex="13">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittiedTotalCostOp" ClientInstanceName="InvEdittiedTotalCostOp" runat="server" Text='<%#Eval("EdittedTotalCost") %>' Width="100%" Border-BorderColor="Transparent" Theme="Office2010Blue">
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                        </Columns>

                                        <SettingsCommandButton>
                                            <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px"></EditButton>
                                            <UpdateButton ButtonType="Image" Image-Url="images/Save.ico" Image-Width="15px"></UpdateButton>
                                            <CancelButton ButtonType="Image" Image-Url="images/Undo.ico" Image-Width="15px"></CancelButton>
                                        </SettingsCommandButton>
                                        <SettingsEditing Mode="Inline"></SettingsEditing>

                                        <TotalSummary>
                                            <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" ShowInColumn="TotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" ShowInColumn="Qty" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="EdittedTotalCost" SummaryType="Sum" ShowInColumn="EdittedTotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="EdittedQty" SummaryType="Sum" ShowInColumn="EdittedQty" DisplayFormat="Total: {0:0,0.00}" />
                                        </TotalSummary>

                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                        <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                            AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" />
                                    </dx:ASPxGridView>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <dx:ASPxRoundPanel ID="ManpowerRoundPanel" runat="server" ClientInstanceName="MANGridRoundPanel" Font-Bold="true" EnableAnimation="true" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                            <ClientSideEvents CollapsedChanging="MANGridRoundPanel_CollapsedChanging" />
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxGridView ID="ManPoGrid" runat="server" ClientInstanceName="ManPoGrid" EnableCallBacks="True" Width="100%" Theme="Office2010Blue"
                                        OnStartRowEditing="ManPoGrid_StartRowEditing"
                                        OnRowUpdating="ManPoGrid_RowUpdating"
                                        OnBeforeGetCallbackResult="ManPoGrid_BeforeGetCallbackResult"
                                        OnDataBound="ManPoGrid_DataBound"
                                        OnCancelRowEditing="ManPoGrid_CancelRowEditing">
                                        <%--<ClientSideEvents RowClick="function(s,e){focused(s,e,'Manpower');}" />--%>
                                        <ClientSideEvents RowClick="function(s,e){focusedInventAnal(s,e,'Manpower');}" />
                                        <ClientSideEvents CustomButtonClick="ManPoGrid_CustomButtonClick" />

                                        <Columns>
                                            <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="MANGridEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                        <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataColumn FieldName="PK" Visible="false" VisibleIndex="1"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false" VisibleIndex="2"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ActivityCode" Caption="Activity" VisibleIndex="3">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("ActivityCode")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit" VisibleIndex="4">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("RevDesc")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ManPowerTypeKeyName" Caption="Type" VisibleIndex="5">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("ManPowerTypeKeyName")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Description" VisibleIndex="6">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("Description")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="UOM" VisibleIndex="7">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("UOM")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Qty" Caption="Requested Qty" VisibleIndex="8">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel ID="inv_qty" runat="server" ClientInstanceName="inv_qty_man" Text='<%#Eval("Qty")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Cost" VisibleIndex="9">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("Cost")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TotalCost" VisibleIndex="10">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("TotalCost")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedQty" Caption="Recommended Qty For Purchase" CellStyle-BackColor="#66ffcc" HeaderStyle-BackColor="#66ffcc" VisibleIndex="11">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittedQtyManPo" ClientInstanceName="InvEdittedQtyManPo" runat="server" Text='<%#Eval("EdittedQty") %>' Width="100%" Theme="Office2010Blue" HorizontalAlign="Right">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                                                        <ClientSideEvents ValueChanged="OnValueChangeQty" KeyPress="FilterDigit" KeyUp="OnKeyUpQtytInvManPower" />
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedCost" Caption="Cost/Unit" VisibleIndex="12">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittedCostManPo" ClientInstanceName="InvEdittedCostManPo" runat="server" Text='<%#Eval("EdittedCost") %>' Width="100%" Theme="Office2010Blue" HorizontalAlign="Right">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                                                        <ClientSideEvents ValueChanged="OnValueChange" KeyPress="FilterDigit" KeyUp="OnKeyUpCosttInvManPower" />
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittiedTotalCost" Caption="Total Cost" VisibleIndex="13">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittiedTotalCostManPo" ClientInstanceName="InvEdittiedTotalCostManPo" runat="server" Text='<%#Eval("EdittiedTotalCost") %>' Width="100%" Border-BorderColor="Transparent" Theme="Office2010Blue">
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                        </Columns>

                                        <SettingsCommandButton>
                                            <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px"></EditButton>
                                            <UpdateButton ButtonType="Image" Image-Url="images/Save.ico" Image-Width="15px"></UpdateButton>
                                            <CancelButton ButtonType="Image" Image-Url="images/Undo.ico" Image-Width="15px"></CancelButton>
                                        </SettingsCommandButton>
                                        <SettingsEditing Mode="Inline"></SettingsEditing>

                                        <TotalSummary>
                                            <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" ShowInColumn="TotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" ShowInColumn="Qty" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="EdittiedTotalCost" SummaryType="Sum" ShowInColumn="EdittiedTotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="EdittedQty" SummaryType="Sum" ShowInColumn="EdittedQty" DisplayFormat="Total: {0:0,0.00}" />
                                        </TotalSummary>

                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                        <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                            AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" />
                                    </dx:ASPxGridView>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <dx:ASPxRoundPanel ID="CapexRoundPanel" runat="server" ClientInstanceName="CAGridRoundPanel" Font-Bold="true" EnableAnimation="true" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%" Theme="Office2010Blue">
                            <ClientSideEvents CollapsedChanging="CAGridRoundPanel_CollapsedChanging" />
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxGridView ID="CapGrid" runat="server" ClientInstanceName="CapGrid" EnableCallBacks="True" Width="100%" Theme="Office2010Blue"
                                        OnStartRowEditing="CapGrid_StartRowEditing"
                                        OnRowUpdating="CapGrid_RowUpdating"
                                        OnBeforeGetCallbackResult="CapGrid_BeforeGetCallbackResult"
                                        OnDataBound="CapGrid_DataBound"
                                        OnCancelRowEditing="CapGrid_CancelRowEditing">
                                        <%--<ClientSideEvents RowClick="function(s,e){focused(s,e,'CAPEX');}" />--%>
                                        <ClientSideEvents RowClick="function(s,e){focusedInventAnal(s,e,'CAPEX');}" />
                                        <ClientSideEvents CustomButtonClick="CapGrid_CustomButtonClick" />

                                        <Columns>
                                            <dx:GridViewCommandColumn VisibleIndex="0" ButtonRenderMode="Image">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="CAGridEdit" Image-AlternateText="Edit" Image-Url="Images/Edit.ico" Image-ToolTip="Edit Row" Image-Width="15px">
                                                        <Image AlternateText="Edit" ToolTip="Edit Row" Width="15px" Url="Images/Edit.ico"></Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataColumn FieldName="PK" Visible="false"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="HeaderDocNum" Visible="false"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ProdDesc" Caption="Product Category">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("ProdDesc")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Description">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("Description")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="RevDesc" Caption="Operating Unit">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("RevDesc")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="UOM">
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("UOM")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Qty" Caption="Requested Qty">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel ID="inv_qty" runat="server" ClientInstanceName="inv_qty_ca" Text='<%#Eval("Qty")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Cost">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("Cost")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TotalCost">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxLabel runat="server" Text='<%#Eval("TotalCost")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedQty" Caption="Recommended Qty For Purchase" CellStyle-BackColor="#66ffcc" HeaderStyle-BackColor="#66ffcc">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittedQtyCapex" ClientInstanceName="InvEdittedQtyCapex" runat="server" Text='<%#Eval("EdittedQty") %>' Width="100%" Theme="Office2010Blue" HorizontalAlign="Right">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                                                        <ClientSideEvents ValueChanged="OnValueChangeQty" KeyPress="FilterDigit" KeyUp="OnKeyUpQtytInvCapex" />
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittedCost" Caption="Cost/Unit">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittedCostCapex" ClientInstanceName="InvEdittedCostCapex" runat="server" Text='<%#Eval("EdittedCost") %>' Width="100%" Theme="Office2010Blue" HorizontalAlign="Right">
                                                        <ValidationSettings RequiredField-IsRequired="true" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                                                        <ClientSideEvents ValueChanged="OnValueChange" KeyPress="FilterDigit" KeyUp="OnKeyUpCosttInvCapex" />
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EdittiedTotalCost" Caption="Total Cost">
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                                <FooterCellStyle HorizontalAlign="Right" Font-Bold="true"></FooterCellStyle>
                                                <EditItemTemplate>
                                                    <dx:ASPxTextBox ID="InvEdittiedTotalCostCapex" ClientInstanceName="InvEdittiedTotalCostCapex" runat="server" Text='<%#Eval("EdittiedTotalCost") %>' Width="100%" Border-BorderColor="Transparent" Theme="Office2010Blue">
                                                    </dx:ASPxTextBox>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                        </Columns>

                                        <SettingsCommandButton>
                                            <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px"></EditButton>
                                            <UpdateButton ButtonType="Image" Image-Url="images/Save.ico" Image-Width="15px"></UpdateButton>
                                            <CancelButton ButtonType="Image" Image-Url="images/Undo.ico" Image-Width="15px"></CancelButton>
                                        </SettingsCommandButton>
                                        <SettingsEditing Mode="Inline"></SettingsEditing>

                                        <TotalSummary>
                                            <dx:ASPxSummaryItem FieldName="TotalCost" SummaryType="Sum" ShowInColumn="TotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" ShowInColumn="Qty" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="EdittiedTotalCost" SummaryType="Sum" ShowInColumn="EdittiedTotalCost" DisplayFormat="Total: {0:0,0.00}" />
                                            <dx:ASPxSummaryItem FieldName="EdittedQty" SummaryType="Sum" ShowInColumn="EdittedQty" DisplayFormat="Total: {0:0,0.00}" />
                                        </TotalSummary>

                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                        <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" ShowFooter="true" />
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                            AllowSort="true" ProcessFocusedRowChangedOnServer="False" ProcessSelectionChangedOnServer="False" AllowDragDrop="false" />
                                    </dx:ASPxGridView>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>
</asp:Content>
