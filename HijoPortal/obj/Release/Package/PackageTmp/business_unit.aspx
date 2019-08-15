<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="business_unit.aspx.cs" Inherits="HijoPortal.business_unit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function updateBUSSUList(s, e) {
            var endCode = EntityCodeBUSSUDirect.GetText();
            var buCode = BUCodeBUSSUDirect.GetText();

            if (endCode.length > 0 && buCode.length > 0) {
                grdBUSSUDirect.UpdateEdit();
            }
        }

        var postponedCallbackBUSSU = false;
        function BUSSUEntity_IndexChanged(s, e) {

            if (BUSSUCallBackPanelDirect.InCallback()) {
                postponedCallbackBUSSU = true;
            }
            else {
                BUSSUCallBackPanelDirect.PerformCallback();
            }
        }

        function BUSSU_EndCallback(s, e) {
            if (postponedCallbackBUSSU) {
                BUSSUCallBackPanelDirect.PerformCallback();
                postponedCallbackBUSSU = false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%" Height="100%">
        <PanelCollection>
            <dx:PanelContent>
                <div id="dvHeader" style="height: 30px;">
                    <h1>Business Unit & Shared Service Unit  Setup</h1>
                </div>
                <dx:ASPxSplitter ID="ExecHLSSplitter" runat="server" ClientInstanceName="ExecHLSSplitterrDirect" BackColor="#e6eff7" AllowResize="true" Border-BorderStyle="None" Width="100%" Height="100%">
                    <Panes>
                        <dx:SplitterPane Size="60%" ScrollBars="Auto">
                            <ContentCollection>
                                <dx:SplitterContentControl runat="server">
                                    <%--Business Unit--%>
                                    <h3 style="text-align: center; width: 100%; margin-top: 2px;">Business Unit / Shared Services Unit</h3>
                                    <div id="dvExecutiveSetup" runat="server" class="scroll-container">
                                        <div style="width: 100%;">
                                            <dx:ASPxGridView ID="grdBUSSU" runat="server"
                                                ClientInstanceName="grdBUSSUDirect"
                                                EnableTheming="True"
                                                KeyboardSupport="true" Style="margin: 0 auto;"
                                                Width="100%"
                                                EnableCallBacks="true"
                                                KeyFieldName="PK"
                                                Theme="Office2010Blue" 
                                                OnBeforeGetCallbackResult="grdBUSSU_BeforeGetCallbackResult" 
                                                OnInitNewRow="grdBUSSU_InitNewRow" 
                                                OnRowInserting="grdBUSSU_RowInserting" 
                                                OnStartRowEditing="grdBUSSU_StartRowEditing" 
                                                OnRowUpdating="grdBUSSU_RowUpdating" 
                                                OnRowDeleting="grdBUSSU_RowDeleting" >
                                                <SettingsBehavior AllowSort="true" SortMode="Value" />

                                                <%--FocusedRowChanged="OnGridFocusedRowChangedSCMProcOff"--%>

                                                <%--function (s, e) {grdSCMProcurementOffDetailsDirect.PerformCallback('AddNew');}"--%>
                                                <%--RowClick="OnGridFocusedRowChangedSCMProcOff"
                                                            EndCallback="OnGridFocusedRowChangedSCMProcOff_EndCallback"--%>

                                                <ClientSideEvents
                                                    RowClick="function (s, e) {grdCreatorDirect.PerformCallback('Creator');}"
                                                    EndCallback="function (s, e) {grdCreatorDirect.Refresh();}" />

                                                <Columns>
                                                    <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="true" ShowNewButtonInHeader="true" VisibleIndex="0" Width="50px" CellStyle-HorizontalAlign="Left"></dx:GridViewCommandColumn>
                                                    <dx:GridViewDataColumn FieldName="PK" Visible="false" VisibleIndex="1"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="EntityCode" Visible="false" VisibleIndex="4"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="EntityCodeDesc" Caption="Entity" VisibleIndex="5"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="BUDeptCode" Visible="false" VisibleIndex="6"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="BUDeptCodeDesc" Caption="BU / Department" VisibleIndex="7"></dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="LastModified" Caption="Last Modified" VisibleIndex="8">
                                                        <EditItemTemplate>
                                                            <dx:ASPxLabel ID="ASPxLastModifiedTextBox" runat="server" Width="100%" Text='<%#Eval("LastModified")%>' Theme="Office2010Blue"></dx:ASPxLabel>
                                                        </EditItemTemplate>
                                                    </dx:GridViewDataColumn>
                                                </Columns>


                                                <SettingsCommandButton>
                                                    <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px"></EditButton>
                                                    <DeleteButton ButtonType="Image" Image-Url="Images/Delete.ico" Image-Width="15px"></DeleteButton>
                                                    <NewButton ButtonType="Image" Image-Url="Images/Add.ico" Image-Width="15px"></NewButton>
                                                </SettingsCommandButton>

                                                <SettingsEditing Mode="EditFormAndDisplayRow"></SettingsEditing>
                                                <Templates>
                                                    <EditForm>
                                                        <div style="padding: 4px 3px 4px">
                                                            <dx:ASPxPageControl ID="BUSSUPageControl" runat="server" Width="100%" Theme="Office2010Blue">
                                                                <TabPages>
                                                                    <dx:TabPage Text="BU / SSU Details" Visible="true">
                                                                        <ContentCollection>
                                                                            <dx:ContentControl ID="tabBUSSUInfo" runat="server">
                                                                                <table style="padding: 10px;">
                                                                                    <tr>
                                                                                        <td style="width: 25%;">
                                                                                            <dx:ASPxLabel runat="server" Text="Entity" Theme="Office2010Blue" />
                                                                                        </td>
                                                                                        <td>:</td>
                                                                                        <td style="width: 70%;">
                                                                                            <dx:ASPxComboBox ID="EntityCode" runat="server" ClientInstanceName="EntityCodeBUSSUDirect" OnInit="EntityCode_Init" AutoResizeWithContainer="false" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue"
                                                                                                ValidationSettings-ErrorDisplayMode="None" ValidationSettings-RequiredField-IsRequired="true" Width="100%">
                                                                                                <ClientSideEvents SelectedIndexChanged="BUSSUEntity_IndexChanged" />
                                                                                            </dx:ASPxComboBox>
                                                                                        </td>
                                                                                        <td style="width: 5%;">
                                                                                            <div style="display: none;">
                                                                                                <dx:ASPxTextBox ID="EntityValue" ClientInstanceName="EntityValueClient" runat="server" Text='<%#Eval("EntityCode")%>' Theme="Office2010Blue" Style="display: none;" />
                                                                                            </div>
                                                                                        </td>
                                                                                        <td></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <dx:ASPxLabel runat="server" Text="BU / Department" Theme="Office2010Blue" />
                                                                                        </td>
                                                                                        <td>:</td>
                                                                                        <td style="padding: 3px 3px 3px;">
                                                                                            <dx:ASPxCallbackPanel ID="BUSSUCallBackPanel" ClientInstanceName="BUSSUCallBackPanelDirect" runat="server" OnCallback="BUSSUCallBackPanel_Callback">
                                                                                                <ClientSideEvents EndCallback="BUSSU_EndCallback" />
                                                                                                <PanelCollection>
                                                                                                    <dx:PanelContent>
                                                                                                        <dx:ASPxComboBox ID="BUCode" runat="server" ClientInstanceName="BUCodeBUSSUDirect" OnInit="BUCode_Init" AutoResizeWithContainer="false" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue"
                                                                                                            ValidationSettings-ErrorDisplayMode="None" ValidationSettings-RequiredField-IsRequired="false" Width="100%">
                                                                                                            <ClientSideEvents SelectedIndexChanged="" />
                                                                                                        </dx:ASPxComboBox>
                                                                                                    </dx:PanelContent>
                                                                                                </PanelCollection>
                                                                                            </dx:ASPxCallbackPanel>
                                                                                        </td>
                                                                                        <td></td>
                                                                                        <td></td>
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
                                                                <ClientSideEvents Click="updateBUSSUList" />
                                                            </dx:ASPxButton>
                                                            <dx:ASPxButton runat="server" Text="Cancel" Theme="Office2010Blue" AutoPostBack="false">
                                                                <ClientSideEvents Click="function(s,e){grdBUSSUDirect.CancelEdit();}" />
                                                            </dx:ASPxButton>
                                                        </div>
                                                    </EditForm>
                                                </Templates>

                                                <EditFormLayoutProperties>
                                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
                                                </EditFormLayoutProperties>
                                                <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" />
                                                <SettingsPopup>
                                                    <EditForm Width="900">
                                                        <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="850" />
                                                    </EditForm>
                                                </SettingsPopup>
                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                                    AllowSort="true" ProcessFocusedRowChangedOnServer="True" ProcessSelectionChangedOnServer="True" AllowDragDrop="false" ConfirmDelete="true" />
                                                <SettingsText ConfirmDelete="Delete This Procurement Officer?" />
                                                <Styles>
                                                    <SelectedRow Font-Bold="False" Font-Italic="False">
                                                    </SelectedRow>
                                                    <FocusedRow Font-Bold="False" Font-Italic="False">
                                                    </FocusedRow>
                                                </Styles>
                                            </dx:ASPxGridView>
                                        </div>
                                    </div>
                                </dx:SplitterContentControl>
                            </ContentCollection>
                        </dx:SplitterPane>
                        <dx:SplitterPane ScrollBars="Auto">
                            <Panes>
                                <dx:SplitterPane Size="50%" ScrollBars="Auto">
                                    <ContentCollection>
                                        <dx:SplitterContentControl runat="server">
                                            <h3 style="text-align: center; width: 100%; margin-top: 2px;">Creator</h3>
                                            <div id="Div1" runat="server" class="scroll-container">
                                                <div style="width: 100%;">
                                                    <dx:ASPxGridView ID="grdCreator" runat="server"
                                                        ClientInstanceName="grdCreatorDirect"
                                                        EnableTheming="True"
                                                        KeyboardSupport="true" Style="margin: 0 auto;"
                                                        Width="100%"
                                                        EnableCallBacks="true"
                                                        KeyFieldName="PK"
                                                        Theme="Office2010Blue">

                                                        <ClientSideEvents
                                                            CustomButtonClick=""
                                                            EndCallback="function (s,e) {grdCreatorDirect.InCallback();}" />

                                                        <SettingsBehavior AllowSort="true" SortMode="Value" />

                                                        <Columns>
                                                            <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="true" ShowNewButtonInHeader="false" VisibleIndex="0" Width="40px" CellStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left">
                                                                        <dx:ASPxButton ID="AddProcCat" ClientInstanceName="AddProcCatDirect" runat="server" Image-Url="Images/Add.ico" Image-Width="15px" Image-ToolTip="New Row" RenderMode="Link" AutoPostBack="false" HorizontalAlign="Center" VerticalAlign="Middle">
                                                                            <ClientSideEvents Click="function (s, e) {grdSCMProcurementOffDetailsDirect.PerformCallback('AddNew');}" />
                                                                        </dx:ASPxButton>
                                                                    </div>
                                                                </HeaderTemplate>
                                                            </dx:GridViewCommandColumn>
                                                            <dx:GridViewDataColumn FieldName="PK" Visible="false" VisibleIndex="1"></dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="MasterKey" Visible="false" VisibleIndex="2">
                                                                <EditItemTemplate>
                                                                    <%--<dx:ASPxTextBox ID="ASPxMasterKeyTextBox" runat="server" Width="100%" Text='<%#Eval("MasterKey")%>' Theme="Office2010Blue" Enabled="false"></dx:ASPxTextBox>--%>
                                                                    <dx:ASPxHiddenField ID="ASPxMasterKeyHiddenField" runat="server"></dx:ASPxHiddenField>
                                                                </EditItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ProcCat" Visible="false" VisibleIndex="3"></dx:GridViewDataColumn>
                                                            <dx:GridViewDataColumn FieldName="ProcCatDesc" Caption="Procurement Category" VisibleIndex="4" SortOrder="Ascending">
                                                                <EditItemTemplate>
                                                                    <dx:ASPxComboBox ID="ProcurementCat" runat="server" ClientInstanceName="ProcurementCatDirect" OnInit="ProcurementCat_Init" AutoResizeWithContainer="false" TextFormatString="{1}" ValueType="System.String" Theme="Office2010Blue"
                                                                        ValidationSettings-ErrorDisplayMode="None" ValidationSettings-RequiredField-IsRequired="true" Width="100%">
                                                                        <ClientSideEvents SelectedIndexChanged="" />
                                                                    </dx:ASPxComboBox>
                                                                </EditItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                        </Columns>
                                                        <SettingsCommandButton>
                                                            <%--<NewButton ButtonType="Image" Image-Url="Images/Add.ico" Image-Width="15px"></NewButton>--%>
                                                            <EditButton ButtonType="Image" Image-Url="Images/Edit.ico" Image-Width="15px"></EditButton>
                                                            <DeleteButton ButtonType="Image" Image-Url="Images/Delete.ico" Image-Width="15px"></DeleteButton>
                                                            <UpdateButton ButtonType="Image" Image-Url="Images/Save.ico" Image-Width="15px"></UpdateButton>
                                                            <CancelButton ButtonType="Image" Image-Url="Images/Undo.ico" Image-Width="15px"></CancelButton>
                                                        </SettingsCommandButton>

                                                        <SettingsEditing Mode="Inline"></SettingsEditing>

                                                        <EditFormLayoutProperties>
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
                                                        </EditFormLayoutProperties>
                                                        <Settings ShowHeaderFilterButton="true" ShowFilterBar="Auto" ShowFilterRow="true" />
                                                        <SettingsPopup>
                                                            <EditForm Width="900">
                                                                <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="850" />
                                                            </EditForm>
                                                        </SettingsPopup>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
                                                            AllowSort="true" ProcessFocusedRowChangedOnServer="True" ProcessSelectionChangedOnServer="True" AllowDragDrop="false" ConfirmDelete="true" />
                                                        <SettingsText ConfirmDelete="Delete This Procurement Category?" />
                                                        <Styles>
                                                            <SelectedRow Font-Bold="False" Font-Italic="False">
                                                            </SelectedRow>
                                                            <FocusedRow Font-Bold="False" Font-Italic="False">
                                                            </FocusedRow>
                                                        </Styles>
                                                    </dx:ASPxGridView>
                                                </div>
                                            </div>
                                        </dx:SplitterContentControl>
                                    </ContentCollection>
                                </dx:SplitterPane>
                                <dx:SplitterPane ScrollBars="Auto">
                                    <ContentCollection>
                                        <dx:SplitterContentControl runat="server">
                                            <h3 style="text-align: center; width: 100%; margin-top: 2px;">Lead</h3>
                                            <div id="Div2" runat="server" class="scroll-container">
                                                <div style="width: 100%;">
                                                </div>
                                            </div>
                                        </dx:SplitterContentControl>
                                    </ContentCollection>
                                </dx:SplitterPane>
                            </Panes>
                        </dx:SplitterPane>
                    </Panes>
                </dx:ASPxSplitter>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
</asp:Content>
