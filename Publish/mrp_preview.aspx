﻿<%@ Page Title="MOP Preview" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="mrp_preview.aspx.cs" Inherits="HijoPortal.mrp_preview" %>

<%@ Register Assembly="DevExpress.Web.v17.2, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dx:ASPxPopupControl ID="LogsPopup" runat="server" Modal="true" CloseAction="CloseButton"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" Theme="Office2010Blue">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxMemo ID="LogsMemo" runat="server" Height="71px" Width="250px" Theme="Office2010Blue">
                                <DisabledStyle ForeColor="Black"></DisabledStyle>
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; padding-top: 10px;">
                            <dx:ASPxButton ID="LogsBtn" runat="server" Text="Save" Theme="Office2010Blue"
                                OnClick="LogsBtn_Click" AutoPostBack="false">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <div id="dvContentWrapper" runat="server" class ="ContentWrapper">
        <div id="dvHeader" style="height: 150px; background-color: #ffffff; padding: 5px 5px 0px 0px; border-radius: 2px;">
            <h1>M R P  Preview</h1>
            <table style="width: 80%; margin: auto;">
                <tr>
                    <td style="width: 12%">
                        <dx:ASPxLabel runat="server" Text="MRP Number" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                    <td>:</td>
                    <td colspan="4">
                        <dx:ASPxLabel ID="DocNum" runat="server" Text="" Theme="Office2010Blue" Style="font-size: medium; font-weight: bold; font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;"></dx:ASPxLabel>
                    </td>
                    <td rowspan="3" style="width: 40%">

                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel runat="server" Text="Month" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                    <td>:</td>
                    <td style="width: 20%">
                        <dx:ASPxLabel ID="Month" runat="server" Text="ASPxLabel" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                    <td style="width: 8%">
                        <dx:ASPxLabel runat="server" Text="Entity" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                    <td>:</td>
                    <td style="width: 20%">
                        <dx:ASPxLabel ID="EntityCode" runat="server" Text="" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel runat="server" Text="Year" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                    <td>:</td>
                    <td>
                        <dx:ASPxLabel ID="Year" runat="server" Text="" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel runat="server" Text="Department" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                    <td>:</td>
                    <td>
                        <dx:ASPxLabel ID="BUCode" runat="server" Text="" Theme="Office2010Blue"></dx:ASPxLabel>
                    </td>
                </tr>
            </table>
        </div>
        <div style="background-color: #ffffff; padding: 0px 0px 10px 0px;">
            <table id="tblSummCost" runat="server" style="width: 80%; margin: auto; margin-bottom: 10px;" border="1">
                <%--<table id="tblSummCost" runat="server">--%>
                <tr>
                    <td style="background-color: mediumspringgreen; border-bottom-color: transparent; text-align: center; font-weight: bold;"
                        colspan="3">Summary of Cost and Expenses</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:ListView ID="PreviewListSummary" runat="server">
                            <LayoutTemplate>
                                <table class="table1" style="width: 100%" runat="server" border="1">
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="SummaryDesc" runat="server" />
                                    </td>
                                    <td style="text-align: right; padding-right: 5px;">
                                        <asp:Label ID="SummaryTotal" runat="server" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75%; border-right-width: 0px; padding-left: 5px;">Total</td>
                    <td id="TotalAmountSummary" runat="server" style="width: 15%; border-left-width: 0px; border-right-width: 0px; text-align: right; padding-right: 5px;"></td>
                    <td style="border-left-width: 0px;"></td>
                </tr>
            </table>

            <table runat="server" style="width: 80%; margin: auto; margin-bottom: 10px;" border="1">
                <tr>
                    <td style="background-color: mediumspringgreen; border-bottom-color: transparent; text-align: center; font-weight: bold;" colspan="3">DIRECT MATERIALS</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:ListView ID="MatListview" runat="server" OnItemCommand="MatListview_ItemCommand">
                            <LayoutTemplate>
                                <table class="table1" style="width: 100%" runat="server">
                                    <tr class="headerRow">
                                        <th style="width: 0px"></th>
                                        <th style="width: 40%;"></th>
                                        <th style="width: 10%;">UOM</th>
                                        <th style="width: 10%;">Qty</th>
                                        <th style="width: 15%;">Est. Cost/Unit</th>
                                        <th style="width: 15%;">Total</th>
                                        <th style="width: 10%;"></th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 0px">
                                        <asp:Label ID="MatID" runat="server"
                                            Text='<%# Eval("PK") %>' Visible="false" />
                                    </td>
                                    <td style="padding-left: 5px;">
                                        <asp:Label ID="MatDescription" runat="server"
                                            Text='<%# Eval("ItemDescription") %>' />
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="MatUOM" runat="server"
                                            Text='<%# Eval("UOM") %>' />
                                    </td>

                                    <td style="text-align: right;">
                                        <asp:Label ID="MatQty" runat="server"
                                            Text='<%# Eval("Qty") %>' />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="MatCost" runat="server"
                                            Text='<%# Eval("Cost") %>' />
                                    </td>
                                    <td style="text-align: right; padding-right: 5px;">
                                        <asp:Label ID="MatTotalCost" runat="server"
                                            Text='<%# Eval("TotalCost") %>' />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" CssClass="link-btn" runat="server" CommandName="Link" ImageUrl="~/Images/comment-black.png" Width="20px" Height="20px" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75%; border-right-width: 0px; padding-left: 5px; font-weight: bold">Total</td>
                    <td id="TAMat" runat="server" style="width: 15%; border-left-width: 0px; border-right-width: 0px; text-align: right; padding-right: 5px; font-weight: bold"></td>
                    <td style="border-left-width: 0px;"></td>
                </tr>
            </table>


            <table runat="server" style="width: 80%; margin: auto; margin-bottom: 10px;" border="1">
                <tr>
                    <td style="background-color: mediumspringgreen; border-bottom-color: transparent; text-align: center; font-weight: bold;" colspan="3">OPEX</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:ListView ID="OpexListiview" runat="server" OnItemCommand="OpexListiview_ItemCommand">
                            <LayoutTemplate>
                                <table class="table1" style="width: 100%" runat="server">
                                    <tr class="headerRow">
                                        <th style="width: 0px"></th>
                                        <th style="width: 40%;"></th>
                                        <th style="width: 10%;">UOM</th>
                                        <th style="width: 10%;">Qty</th>
                                        <th style="width: 15%;">Est. Cost/Unit</th>
                                        <th style="width: 15%;">Total</th>
                                        <th style="width: 10%;"></th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 0px">
                                        <asp:Label ID="OpexID" runat="server"
                                            Text='<%# Eval("PK") %>' Visible="false" />
                                    </td>
                                    <td style="padding-left: 5px;">
                                        <asp:Label ID="OpexDescription" runat="server"
                                            Text='<%# Eval("Description") %>' />
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="OpexUOM" runat="server"
                                            Text='<%# Eval("UOM") %>' />
                                    </td>

                                    <td style="text-align: right;">
                                        <asp:Label ID="OpexQty" runat="server"
                                            Text='<%# Eval("Qty") %>' />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="OpexCost" runat="server"
                                            Text='<%# Eval("Cost") %>' />
                                    </td>
                                    <td style="text-align: right; padding-right: 5px;">
                                        <asp:Label ID="OpexTotalCost" runat="server"
                                            Text='<%# Eval("TotalCost") %>' />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" CssClass="link-btn" runat="server" CommandName="Link" ImageUrl="~/Images/comment-black.png" Width="20px" Height="20px" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75%; border-right-width: 0px; padding-left: 5px; font-weight: bold">Total</td>
                    <td id="TAOpex" runat="server" style="width: 15%; border-left-width: 0px; border-right-width: 0px; text-align: right; padding-right: 5px; font-weight: bold"></td>
                    <td style="border-left-width: 0px;"></td>
                </tr>
            </table>

            <table runat="server" style="width: 80%; margin: auto; margin-bottom: 10px;" border="1">
                <tr>
                    <td style="background-color: mediumspringgreen; border-bottom-color: transparent; text-align: center; font-weight: bold;" colspan="3">MANPOWER</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:ListView ID="ManListview" runat="server" OnItemCommand="ManListview_ItemCommand">
                            <LayoutTemplate>
                                <table class="table1" style="width: 100%" runat="server">
                                    <tr class="headerRow">
                                        <th style="width: 0px"></th>
                                        <th style="width: 40%;"></th>
                                        <th style="width: 10%;">UOM</th>
                                        <th style="width: 10%;">Qty</th>
                                        <th style="width: 15%;">Est. Cost/Unit</th>
                                        <th style="width: 15%;">Total</th>
                                        <th style="width: 10%;"></th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 0px">
                                        <asp:Label ID="ManID" runat="server"
                                            Text='<%# Eval("PK") %>' Visible="false" />
                                    </td>
                                    <td style="padding-left: 5px;">
                                        <asp:Label ID="ManDescription" runat="server"
                                            Text='<%# Eval("Description") %>' />
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="ManUOM" runat="server"
                                            Text='<%# Eval("UOM") %>' />
                                    </td>

                                    <td style="text-align: right;">
                                        <asp:Label ID="ManQty" runat="server"
                                            Text='<%# Eval("Qty") %>' />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="ManCost" runat="server"
                                            Text='<%# Eval("Cost") %>' />
                                    </td>
                                    <td style="text-align: right; padding-right: 5px;">
                                        <asp:Label ID="ManTotalCost" runat="server"
                                            Text='<%# Eval("TotalCost") %>' />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" CssClass="link-btn" runat="server" CommandName="Link" ImageUrl="~/Images/comment-black.png" Width="20px" Height="20px" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75%; border-right-width: 0px; padding-left: 5px; font-weight: bold">Total</td>
                    <td id="TAManpower" runat="server" style="width: 15%; border-left-width: 0px; border-right-width: 0px; text-align: right; padding-right: 5px; font-weight: bold"></td>
                    <td style="border-left-width: 0px;"></td>
                </tr>
            </table>


            <table runat="server" style="width: 80%; margin: auto; margin-bottom: 10px;" border="1">
                <tr>
                    <td style="background-color: mediumspringgreen; border-bottom-color: transparent; text-align: center; font-weight: bold;" colspan="3">CAPEX</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:ListView ID="CapexListview" runat="server" OnItemCommand="CapexListview_ItemCommand">
                            <LayoutTemplate>
                                <table class="table1" style="width: 100%" runat="server">
                                    <tr class="headerRow">
                                        <th style="width: 0px"></th>
                                        <th style="width: 40%;"></th>
                                        <th style="width: 10%;">UOM</th>
                                        <th style="width: 10%;">Qty</th>
                                        <th style="width: 15%;">Est. Cost/Unit</th>
                                        <th style="width: 15%;">Total</th>
                                        <th style="width: 10%;"></th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 0px">
                                        <asp:Label ID="CapexID" runat="server"
                                            Text='<%# Eval("PK") %>' Visible="false" />
                                    </td>
                                    <td style="padding-left: 5px;">
                                        <asp:Label ID="CapexDescription" runat="server"
                                            Text='<%# Eval("Description") %>' />
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="CapexUOM" runat="server"
                                            Text='<%# Eval("UOM") %>' />
                                    </td>

                                    <td style="text-align: right;">
                                        <asp:Label ID="CapexQty" runat="server"
                                            Text='<%# Eval("Qty") %>' />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="CapexCost" runat="server"
                                            Text='<%# Eval("Cost") %>' />
                                    </td>
                                    <td style="text-align: right; padding-right: 5px;">
                                        <asp:Label ID="CapexTotalCost" runat="server"
                                            Text='<%# Eval("TotalCost") %>' />
                                    </td>
                                    <td>

                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" CommandName="Link" EnableViewState="false">LinkButton</asp:LinkButton>--%>
                                        <asp:ImageButton ID="ImageButton1" CssClass="link-btn" runat="server" CommandName="Link" ImageUrl="~/Images/comment-black.png" Width="20px" Height="20px" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75%; border-right-width: 0px; padding-left: 5px; font-weight: bold">Total</td>
                    <td id="TotalAmountTD" runat="server" style="width: 15%; border-left-width: 0px; border-right-width: 0px; text-align: right; padding-right: 5px; font-weight: bold"></td>
                    <td style="border-left-width: 0px;"></td>
                </tr>
            </table>

            <table runat="server" style="width: 80%; margin: auto;" border="1">
                <tr>
                    <td style="background-color: mediumspringgreen; border-bottom-color: transparent; text-align: center; font-weight: bold;" colspan="3">REVENUE ASSUMPTIONS</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:ListView ID="RevListview" runat="server" OnItemCommand="RevListview_ItemCommand">
                            <LayoutTemplate>
                                <table class="table1" style="width: 100%" runat="server">
                                    <tr class="headerRow">
                                        <th style="width: 0px"></th>
                                        <th style="width: 40%;"></th>
                                        <th style="width: 10%;">UOM</th>
                                        <th style="width: 10%;">Qty</th>
                                        <th style="width: 15%;">Est. Cost/Unit</th>
                                        <th style="width: 15%;">Total</th>
                                        <th style="width: 10%;"></th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 0px">
                                        <asp:Label ID="RevID" runat="server"
                                            Text='<%# Eval("PK") %>' Visible="false" />
                                    </td>
                                    <td style="padding-left: 5px;">
                                        <asp:Label ID="RevProduct" runat="server"
                                            Text='<%# Eval("ProductName") %>' />
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="RevFarm" runat="server"
                                            Text='<%# Eval("FarmName") %>' />
                                    </td>

                                    <td style="text-align: right;">
                                        <asp:Label ID="RevVolume" runat="server"
                                            Text='<%# Eval("Volume") %>' />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="RevPrize" runat="server"
                                            Text='<%# Eval("Prize") %>' />
                                    </td>
                                    <td style="text-align: right; padding-right: 5px;">
                                        <asp:Label ID="RevTotalPrize" runat="server"
                                            Text='<%# Eval("TotalPrize") %>' />
                                    </td>
                                    <td>

                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" CommandName="Link" EnableViewState="false">LinkButton</asp:LinkButton>--%>
                                        <asp:ImageButton ID="ImageButton1" CssClass="link-btn" runat="server" CommandName="Link" ImageUrl="~/Images/comment-black.png" Width="20px" Height="20px" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75%; border-right-width: 0px; padding-left: 5px; font-weight: bold">Total</td>
                    <td id="TARevenue" runat="server" style="width: 15%; border-left-width: 0px; border-right-width: 0px; text-align: right; padding-right: 5px; font-weight: bold"></td>
                    <td style="border-left-width: 0px;"></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
