<%@ Page Title="Print Preview" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="hlsSOA_Print.aspx.cs" Inherits="HijoPortal.hlsSOA_Print" %>

<%@ Register Assembly="DevExpress.XtraReports.v17.2.Web, Version=17.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
     function BeforeRender(s, e) {
         e.reportPreview.showMultipagePreview(true);
         //e.reportPreview.showWholepagePreview(true);
        }
    function InitDocViewer(s, e) {
        //s.GetReportPreview().zoom(0.9);
        s.GetReportPreview().zoom(DevExpress.Report.Preview.ZoomAutoBy.PageWidth);
    }
</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%" Height="100%" ScrollBars="Auto">
        <PanelCollection>
            <dx:PanelContent>
                <%--<div id="dvHeader" style="height: 30px;">
                    <h1 id="HLSSOATitle" runat="server">Print Preview</h1>
                </div>--%>
                <%--ReportSourceId="HijoPortal.devexrep.XtraReportSOA"--%>
                <div>
                    <table style="width: 100%; margin-top: 0px;">
                        <tr>
                            <td>
                                <dx:ASPxWebDocumentViewer ID="ASPxWebDocumentViewer1" runat="server"
                                    DisableHttpHandlerValidation="False">
                                   <%-- <ClientSideEvents BeforeRender="BeforeRender"/>--%>
                                    <ClientSideEvents Init="InitDocViewer" />
                                </dx:ASPxWebDocumentViewer>
                            </td>
                        </tr>
                    </table>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
</asp:Content>
