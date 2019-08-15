﻿$(document).ready(function () {

    changeWidth.resizeWidth();
});

$(window).resize(function () {
    changeWidth.resizeWidth();
});

changeWidth = {
    resizeWidth: function () {
        var heightNoScrollBars = $(window).height();
        var widthNoScrollBars = $(window).width();
        //var x = Math.abs($('body').width() - document.documentElement.clientWidth);
        var fullwidthBrowser = $('body').width();
        //var fullheightBrowser = $('body').height();
        var leftPanel = (fullwidthBrowser * 0.15);
        var centerPanel = fullwidthBrowser * 0.70;
        var origCenterPanel = fullwidthBrowser * 0.85;
        var rightPanel = leftPanel;
        var leftCenter = rightPanel + origCenterPanel;
        var mrpwidth = fullwidthBrowser * 0.84;
        var mrpwidthWrapper = fullwidthBrowser * 0.85;

        //var contentHeight = 600;
        //$('#MRPPanel').width(mrpwidth);
        $('#MasterPanel').width(mrpwidth);
        $('#AddFormPanel').width(mrpwidth);
        $('#PanelLeft').width(leftPanel);

        var h = window.innerHeight
            || document.documentElement.clientHeight
            || document.body.clientHeight;

        var contentHeight = h - ($('#dvBanner').height() + $('#footer').height() + 10);
        var contentHeightInside = h - ($('#dvBanner').height() + $('#footer').height() + 35);
        //var mrpWrapperH = h - 130;
        var mrpWrapperH = h - 200;
        var mrpWrapperH_Details = h - 310;
        //var mrpWrapperH_Details = (h - $('#divHeaderMRP')) - 170;

        var HeaderH = $('#dvHeader').height();

        $('#dvChangePW').height(mrpWrapperH + 10);
        $('#MRP_Wrapper').height(mrpWrapperH);
        $('#MRP_Wrapper').width(mrpwidthWrapper);
        $('#MRP_Wrapper_Details').height(mrpWrapperH_Details);

        var DetailH = mrpWrapperH - (HeaderH + 15);
        //var DetailH = 600;

        $('#dvDetails').height(DetailH);

        //MainSplitterClient.Setheight(contentHeight);

        $('#dvSplitter').height(contentHeight);

        $('#dvMOPWorkflow').height(contentHeight - (HeaderH + 30));

        $('#dvSCMSetup').height(contentHeight - (HeaderH + 30));
        $('#dvFinanceSetup').height(contentHeight - (HeaderH + 30));
        $('#dvExecutiveSetup').height(contentHeight - (HeaderH + 30));
        $('#dvWorkflowSetup').height(contentHeight - (HeaderH + 30));

        //$('#dvContentWrapper').height(contentHeightInside);

        //console.log("Center Height: " + centerPanelHeight + " form Height: " + formHeight + ":::: " + h1);

        //console.log("Body Height: " + h);
        //console.log("Header Height: " + $('#dvBanner').height());
        //console.log("Footer Height: " + $('#footer').height());
        //console.log("Content Height: " + contentHeight);
    }
}

function devSplitterResize(s, e) {
    var h = window.innerHeight
        || document.documentElement.clientHeight
        || document.body.clientHeight;

    var contentHeight = h - ($('#dvBanner').height() + $('#footer').height() + 10);

    $('#MainSplitterClient').height(contentHeight);

    //MainSplitterClient.height(contentHeight);

    //console.log("Body Height: " + h);
    //console.log("Header Height: " + $('#dvBanner').height());
    //console.log("Footer Height: " + $('#footer').height());
    //console.log("Content Height: " + contentHeight);
}

function SplitterContentResize(s, e) {
    var fullwidthBrowser = $('body').width();
    var navWidth = $('#containMenu').width();
    var sidePanelwidth = $('#SideMenu').width();

    var contentWidth = fullwidthBrowser - (navWidth + sidePanelwidth);

    $('#MRP_Wrapper').width(contentWidth);

}

function openNav() {
    document.getElementById("mySidenav").style.width = "350px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}

//REUSABLE FUNCTION START HERE....
function FilterDigit(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    //KEY (TAB) keycode: 0
    //KEY (0 to 9) keycode: 48-57
    //Key (DEL)    keycode: 8
    //Key (.)    keycode: 46
    var textboxval = s.GetText().split(".");
    var length = textboxval.length;
    var text = s.GetText().substring(s.GetText().indexOf(".") + 1, s.GetText().length);
    //console.log(text);
    if (key == 46) {
        if (length > 1) {
            ASPxClientUtils.PreventEvent(e.htmlEvent);
        }
    } else {

    }

    if ((key >= 48 && key <= 57) || key == 8 || key == 46 || key == 0) {
        return true;
    } else {
        ASPxClientUtils.PreventEvent(e.htmlEvent);
    }
}

function OnValueChangeQty(s, e) {
    s.SetText(CorrectValue(s.GetText(), 2));
}

function OnValueChange(s, e) {
    s.SetText(CorrectValue(s.GetText(), 1));
}

function CorrectValue(str, type) {
    switch (type) {
        case 1:
            return accounting.formatMoney(str);
        case 2:
            return accounting.formatNumber(str);
    }
}
//END OF REUSABLE FUNCTION

// MasterMRp
function CustomButtonClick(s, e) {
    //console.log("custom button click" + e.buttonID);
    var button = e.buttonID;
    if (button == "Delete") {
        var result = confirm("Delete this row?");
        if (result)
            e.processOnServer = true;
    } else if (button == "Edit") {
        e.processOnServer = true;
    } else if (button == "Preview") {
        e.processOnServer = true;
    } else if (button == "Submit") {
        e.processOnServer = true;
    }
}

function MainTableEndCallback(s, e) {

    //MainTable.InCallback();

    var hidden_val = MRPHiddenVal.Get('hidden_value');
    if (hidden_val == "InvalidCreator") {
        MRPNotificationMessage.SetText("You are not authorized to access this item");
        MRPNotify.SetHeaderText("Alert");
        MRPNotify.Show();
        MRPHiddenVal.Set('hidden_value', ' ');
    }
}


// MRPAddForm Script
function OperatingUnitDM(s, e) {
    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function OperatingUnitOP(s, e) {
    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function OperatingUnitCA(s, e) {
    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function OperatingUnitMAN(s, e) {
    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function OperatingUnitREV(s, e) {
    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function DirectMaterialsGrid_CustomButtonClick(s, e) {
    var button = e.buttonID;
    if (button == "Edit") {
        if (OPEXGrid.IsEditing() || OPEXGrid.IsNewRowEditing())
            OPEXGrid.CancelEdit();

        if (ManPowerGrid.IsEditing() || ManPowerGrid.IsNewRowEditing())
            ManPowerGrid.CancelEdit();

        if (CAPEXGrid.IsEditing() || CAPEXGrid.IsNewRowEditing())
            CAPEXGrid.CancelEdit();

        if (RevenueGrid.IsEditing() || RevenueGrid.IsNewRowEditing())
            RevenueGrid.CancelEdit();

        s.StartEditRow(e.visibleIndex);
    } else if (button == "Delete"){
        s.DeleteRow(s.GetFocusedRowIndex());
    }
}

function DirectMaterialsGrid_Add(s, e) {
    if (OPEXGrid.IsEditing() || OPEXGrid.IsNewRowEditing())
        OPEXGrid.CancelEdit();

    if (ManPowerGrid.IsEditing() || ManPowerGrid.IsNewRowEditing())
        ManPowerGrid.CancelEdit();

    if (CAPEXGrid.IsEditing() || CAPEXGrid.IsNewRowEditing())
        CAPEXGrid.CancelEdit();

    if (RevenueGrid.IsEditing() || RevenueGrid.IsNewRowEditing())
        RevenueGrid.CancelEdit();

    DirectMaterialsGrid.AddNewRow();
}

function updateDirectMat(s, e) {
    var entityval = entityhidden.Get('hidden_value');
    var bool = true;
    console.log(entityval);
    if (entityval == "display") {
        if (OperatingUnit.GetText().length == 0) {
            OperatingUnit.SetIsValid(false);
            bool = false;
        } else {
            OperatingUnit.SetIsValid(true);
            bool = true;
        }
    }

    var actCode = ActivityCodeDirect.GetText();
    var itemCode = ItemCodeDirect.GetText();
    var itemDesc = ItemDescriptionDirect.GetText();
    var uom = UOMDirect.GetText();
    var cost = CostDirect.GetText();
    var qty = QtyDirect.GetText();
    var totalcost = TotalCostDirect.GetText();

    if (actCode.length > 0 && itemCode.length > 0 && itemDesc.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool) {
        DirectMaterialsGrid.UpdateEdit();
    }
}
function updateOpex(s, e) {

    var entityval = entityhiddenOP.Get('hidden_value');
    var bool = true;
    if (entityval == "display") {
        if (OperatingUnitOP.GetText().length == 0) {
            OperatingUnitOP.SetIsValid(false);
            bool = false;
        } else {
            OperatingUnitOP.SetIsValid(true);
            bool = true;
        }
    }

    var expense = ExpenseCodeOPEX.GetText();
    var itemCode = ItemCodeOPEX.GetText();
    var itemDesc = DescriptionOPEX.GetText();
    var uom = UOMOPEX.GetText();
    var cost = CostOPEX.GetText();
    var qty = QtyOPEX.GetText();
    var totalcost = TotalCostOPEX.GetText();

    if (expense.length > 0 && itemDesc.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool) {
        OPEXGrid.UpdateEdit();
    }
}

function updateManpower(s, e) {

    var entityval = entityhiddenMAN.Get('hidden_value');
    var bool = true;
    if (entityval == "display") {
        if (OperatingUnitMAN.GetText().length == 0) {
            OperatingUnitMAN.SetIsValid(false);
            bool = false;
        } else {
            OperatingUnitMAN.SetIsValid(true);
            bool = true;
        }
    }

    var activity = ActivityCodeMAN.GetText();
    var type = ManPowerTypeKeyNameMAN.GetText();
    var itemDesc = DescriptionMAN.GetText();
    var uom = UOMMAN.GetText();
    var cost = CostMAN.GetText();
    var qty = QtyMAN.GetText();
    var totalcost = TotalCostMAN.GetText();

    if (activity.length > 0 && type.length > 0 && itemDesc.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool) {
        ManPowerGrid.UpdateEdit();
    }
}

function updateCAPEX(s, e) {
    var entityval = entityhiddenCA.Get('hidden_value');
    var bool = true;
    if (entityval == "display") {
        if (OperatingUnitCA.GetText().length == 0) {
            OperatingUnitCA.SetIsValid(false);
            bool = false;
        } else {
            OperatingUnitCA.SetIsValid(true);
            bool = true;
        }
    }

    var itemDesc = DescriptionCAPEX.GetText();
    var uom = UOMCAPEX.GetText();
    var cost = CostCAPEX.GetText();
    var qty = QtyCAPEX.GetText();
    var totalcost = TotalCostCAPEX.GetText();

    if (itemDesc.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool) {
        CAPEXGrid.UpdateEdit();
    }
}

function updateRevenue(s, e) {
    var entityval = entityhiddenREV.Get('hidden_value');
    var bool = true;
    if (entityval == "display") {
        if (OperatingUnitREV.GetText().length == 0) {
            OperatingUnitREV.SetIsValid(false);
            bool = false;
        } else {
            OperatingUnitREV.SetIsValid(true);
            bool = true;
        }
    }

    var product = ProductNameRev.GetText();
    var farm = FarmNameRev.GetText();
    var prize = PrizeRev.GetText();
    var volume = VolumeRev.GetText();
    var totalprize = TotalPrizeRev.GetText();

    if (product.length > 0 && farm.length > 0 && prize.length > 0 && volume.length > 0 && totalprize.length > 0 && bool) {
        RevenueGrid.UpdateEdit();
    }
}


function OnKeyUpCostDirect(s, e) {//OnChange
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(QtyDirect.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalCostDirect.SetText(accounting.formatMoney(total));
            TotalCostDirect.SetIsValid(true);
        }
    } else {
        TotalCostDirect.SetText("");
        TotalCostDirect.SetIsValid(false);
    }
}

function OnKeyUpCostOpex(s, e) {//OnChange
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(QtyOPEX.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalCostOPEX.SetText(accounting.formatMoney(total));
            TotalCostOPEX.SetIsValid(true);
        }
    } else {
        TotalCostOPEX.SetText("");
        TotalCostOPEX.SetIsValid(false);
    }
}
function OnKeyUpCostMan(s, e) {//OnChange
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(QtyMAN.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalCostMAN.SetText(accounting.formatMoney(total));
            TotalCostMAN.SetIsValid(true);
        }
    } else {
        TotalCostMAN.SetText("");
        TotalCostMAN.SetIsValid(false);
    }
} function OnKeyUpCostCapex(s, e) {//OnChange
    //var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(QtyCAPEX.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalCostCAPEX.SetText(accounting.formatMoney(total));
            TotalCostCAPEX.SetIsValid(true);
        }
    } else {
        TotalCostCAPEX.SetText("");
        TotalCostCAPEX.SetIsValid(false);
    }
}

function OnKeyUpCostRev(s, e) {
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(VolumeRev.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalPrizeRev.SetText(accounting.formatMoney(total));
            TotalPrizeRev.SetIsValid(true);
        }
    } else {
        TotalPrizeRev.SetText("");
        TotalPrizeRev.SetIsValid(false);
    }
}


function OnKeyUpQtyDirect(s, e) {//OnChange
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(s.GetText()).toFixed(2);
    var cost = parseFloat(accounting.unformat(CostDirect.GetText()));
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalCostDirect.SetText(accounting.formatMoney(total));
            TotalCostDirect.SetIsValid(true);
        }
    } else {
        TotalCostDirect.SetText("");
        TotalCostDirect.SetIsValid(false);
    }
}

function OnKeyUpQtyOpex(s, e) {//OnChange
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(s.GetText()).toFixed(2);
    var cost = parseFloat(accounting.unformat(CostOPEX.GetText()));
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalCostOPEX.SetText(parseFloat(total).toFixed(2));
            TotalCostOPEX.SetIsValid(true);
        }
    } else {
        TotalCostOPEX.SetText("");
        TotalCostOPEX.SetIsValid(false);
    }
}
function OnKeyUpQtyMan(s, e) {//OnChange
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(s.GetText()).toFixed(2);
    var cost = parseFloat(accounting.unformat(CostMAN.GetText()));
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalCostMAN.SetText(parseFloat(total).toFixed(2));
            TotalCostMAN.SetIsValid(true);
        }
    } else {
        TotalCostMAN.SetText("");
        TotalCostMAN.SetIsValid(false);
    }
}
function OnKeyUpQtyCapex(s, e) {//OnChange
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(s.GetText()).toFixed(2);
    var cost = parseFloat(accounting.unformat(CostCAPEX.GetText()));
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalCostCAPEX.SetText(parseFloat(total).toFixed(2));
            TotalCostCAPEX.SetIsValid(true);
        }
    } else {
        TotalCostCAPEX.SetText("");
        TotalCostCAPEX.SetIsValid(false);
    }
}

function OnKeyUpQtyRev(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(s.GetText()).toFixed(2);
    var cost = parseFloat(accounting.unformat(PrizeRev.GetText()));
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            TotalPrizeRev.SetText(parseFloat(total).toFixed(2));
            TotalPrizeRev.SetIsValid(true);
        }
    } else {
        TotalPrizeRev.SetText("");
        TotalPrizeRev.SetIsValid(false);
    }
}

//MRP Items
function ActivityCodeIndexChange(s, e) {
    //var selectedString = s.GetSelectedItem().text;
    //var indexDash = selectedString.indexOf('-');
    //var codeString = selectedString.substring(0, indexDash + 1);
    //var temp = selectedString.substring(indexDash + 1, selectedString.length);
    //var secondString = temp.substring(0, temp.indexOf('-'));

    //var selValue = s.GetSelectedItem().value;
    //var selText = s.GetSelectedItem().text;
    //ActivityCodeDirect.SetText((codeString + secondString).toString());
}

var isItemBeginCallback = 3;
function OnBeginCallback(s, e) {
    if (e.command == "STARTEDIT") {
        var visibleIndex = OPEXGrid.GetFocusedRowIndex();
        OPEXGrid.GetRowValues(visibleIndex, "isItem", OnGetGridViewValues);
    }
}

function OnGetGridViewValues(values) {
    if (values[0] >= 0) isItemBeginCallback = values[0];
}

function pageinit(s, e) {
    if (isItemBeginCallback == 0) {
        document.getElementById("div1").style.display = "none";
        document.getElementById("div2").style.display = "none";
        ItemCodeOPEX.SetText("");
    }
}

var isItem = 1;
function ExpenseCodeIndexChangeOPEX(s, e) {
    //document.getElementById("itemTD").style.display = "none";
    //document.getElementById("itemTD2").style.display = "none";
    //ItemCodeOPEX.SetVisible(false);
    isItem = parseInt(s.GetSelectedItem().GetColumnText('isItem'));
    switch (isItem) {
        case 0://Non PO
            document.getElementById("div1").style.display = "none";
            document.getElementById("div2").style.display = "none";
            DescriptionOPEX.SetText("");
            DescriptionOPEX.GetInputElement().readOnly = false;
            ItemCodeOPEX.SetText("");
            break;
        case 1://PO
            document.getElementById("div1").style.display = "block";
            document.getElementById("div2").style.display = "block";
            DescriptionOPEX.SetText("");
            DescriptionOPEX.GetInputElement().readOnly = true;
            ItemCodeOPEX.SetText("");
            break;
    }
}

function Hide() {
    document.getElementById("div1").style.display = "none";
    document.getElementById("div2").style.display = "none";
    DescriptionOPEX.SetText("");
    DescriptionOPEX.GetInputElement().readOnly = false;
    ItemCodeOPEX.SetText("");
}

function ItemCodeOPEX_KeyPress(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    //KEY (ENTER) keycode: 13
    if (key == 13) {
        ASPxClientUtils.PreventEvent(e.htmlEvent);
        listboxOPEX.SetVisible(true);
        listboxOPEX.PerformCallback(ItemCodeOPEX.GetInputElement().value);
    }
}

function listbox_selectedOPEX(s, e) {
    var selValue = s.GetSelectedItem().value;
    var selText = s.GetSelectedItem().text;
    ItemCodeOPEX.SetText(selValue);
    DescriptionOPEX.SetText(selText);
    DescriptionOPEX.SetIsValid(true);
    listboxOPEX.SetVisible(false);
}

function ActivityCodeIndexChangeMAN(s, e) {
    var selectedString = s.GetSelectedItem().text;
    var indexDash = selectedString.indexOf('-');
    var codeString = selectedString.substring(0, indexDash + 1);
    var temp = selectedString.substring(indexDash + 1, selectedString.length);
    var secondString = temp.substring(0, temp.indexOf('-'));
    ActivityCodeMAN.SetText((codeString + secondString).toString());
}

//Entity to BU Callback
var postponedCallbackEntitytoBU = false;
function EntityCodeIndexChange(s, e) {
    SetComboBoxEntityID(s);
    var selectedString = s.GetSelectedItem().text;
    EntityCodeDirect.SetText(selectedString.toString());

    console.log("pass callback");

    if (BUCodeCallbackPanelDirect.InCallback()) {
        postponedCallbackEntitytoBU = true;
    }
    else {
        BUCodeCallbackPanelDirect.PerformCallback();
    }
}

//END CALL BU
function BU_EndCallBack(s, e) {
    if (postponedCallbackEntitytoBU) {
        BUCodeCallbackPanelDirect.PerformCallback();
        postponedCallbackEntitytoBU = false;
    }
}

function BUCodeIndexChange(s, e) {
    SetComboBoxBUID(s);
    var selectedString = s.GetSelectedItem().text;
    BUCodeDirect.SetText(selectedString.toString());
}

function UserLevelDescIndexChange(s, e) {
    SetComboBoxUserLevelID(s);
    var selectedString = s.GetSelectedItem().text;
    EmployeeLevelDirect.SetText(selectedString.toString());
}

function UserStatuscIndexChange(s, e) {
    SetComboBoxUserStatusID(s);
    var selectedString = s.GetSelectedItem().text;
    UserStatusDirect.SetText(selectedString.toString());
}

function SetComboBoxEntityID(s) {
    var EntityID = s.GetValue();
    EntityValueClient.SetText(EntityID);
}

function SetComboBoxBUID(s) {
    var BUID = s.GetValue();
    BUValueClient.SetText(BUID);
}

function SetComboBoxUserLevelID(s) {
    var UserLevelID = s.GetValue();
    UserLevelValueClient.SetText(UserLevelID);
}

function SetComboBoxUserStatusID(s) {
    var UserStatusID = s.GetValue();
    UserStatusValueClient.SetText(UserStatusID);
}

function updateUserList(s, e) {
    var endCode = EntityValueClient.GetText();
    var buCode = BUValueClient.GetText();

    if (endCode.length > 0 && buCode.length > 0) {
        UserListGrid.UpdateEdit();
    }
}


//Direct Materials ITem Code list box
function listbox_selected(s, e) {
    var selValue = s.GetSelectedItem().value;
    var selText = s.GetSelectedItem().text;
    ItemCodeDirect.SetText(selValue);
    ItemDescriptionDirect.SetText(selText);
    ItemDescriptionDirect.SetIsValid(true);
    listbox.SetVisible(false);
}

function ItemCodeDirect_KeyPress(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    //KEY (ENTER) keycode: 13
    if (key == 13) {
        ASPxClientUtils.PreventEvent(e.htmlEvent);
        listbox.SetVisible(true);
        listbox.PerformCallback(ItemCodeDirect.GetInputElement().value);
    }
}

//FOR SIDEBAR
var postponedCallbackRequired = false;
var params = "";
function focused(s, e, type) {
    var pk = s.GetRowKey(e.visibleIndex);;
    params = type + "-" + pk;

    if (FloatCallbackPanel.InCallback())
        postponedCallbackRequired = true;
    else
        FloatCallbackPanel.PerformCallback(params);
}



//Floating SideBar
function FloatEndCallback(s, e) {
    if (postponedCallbackRequired) {
        FloatCallbackPanel.PerformCallback(params);
        postponedCallbackRequired = false;
    }
}

//Main Splitter Resize
function OnMainSplitterPaneResized(s, e) {
    //var ele = MainSplitterClient.GetPaneByName('containForm').GetElement();
    ////ele.SetHeight(e.pane.GetClientHeight());
    //ele.SetWidth(e.pane.GetClientHeight());
    var name = e.pane.name;
    ResizeContentSplitter(e.pane);

}

function ResizeContentSplitter(control) {
    var ele = MainSplitterClient.GetPaneByName('containForm').GetElement();
    ele.SetWidth(control.GetClientHeight());
    console.log("Panel Height: ");
}

//FOR WORFLOW
function updateWorkflowMaster(s, e) {
    var effectDate = EffectDateDirect.GetText();
    var entCode = EntCodeDirect.GetText();
    var buCode = BUCodeDirect.GetText();
    var buHead = BUHeadDirect.GetText();

    if (effectDate.length > 0 && entCode.length > 0 && buCode.length > 0 && buHead.length > 0) {
        grdWorkflowMasterDirect.UpdateEdit();
    }
}

var postponedCallbackRequiredWorkflow = false;
var paramsWorkflow = "";
function focusedWorkflowMaster(s, e, type) {
    var pk = s.GetRowKey(e.visibleIndex);;
    paramsWorkflow = type + "-" + pk;

    //if (FloatCallbackPanel.InCallback())
    //    postponedCallbackRequiredWorkflow = true;
    //else
    //    postponedCallbackRequiredWorkflow.PerformCallback(paramsWorkflow);
}


var creatorkey = -1;
var hidcreatorkey = -1;
// POCreation
function POCustomButtonClick(s, e) {
    var button = e.buttonID;
    if (button == "Delete") {
        var result = confirm("Delete this row?");
        if (result)
            e.processOnServer = true;
    } else if (button == "Edit") {
        e.processOnServer = true;
    } else if (button == "Preview") {
        e.processOnServer = true;
    }

}

function POEndCallback(s, e) {
    var hidden_val = HiddenVal.Get('hidden_value');
    if (hidden_val == "AlreadyPO") {
        NotificationMessage.SetText("Already PO");
        Notify.SetHeaderText("Alert");
        Notify.Show();
        HiddenVal.Set('hidden_value', ' ');
    } else if (hidden_val == "InvalidCreator") {
        NotificationMessage.SetText("You are not authorized to access this item");
        Notify.SetHeaderText("Alert");
        Notify.Show();
        HiddenVal.Set('hidden_value', ' ');
    }
}

function POgrid_selectionChanged(s, e) {
    //selList.PerformCallback();
    //s.GetSelectedFieldValues('MRPCategory;Item', GetSelectedFieldValuesCallback);
    s.GetSelectedFieldValues('MRPCategory;Item;UOM;Cost;POQty', GetSelectedFieldValuesCallback);
}

function GetSelectedFieldValuesCallback(values) {
    selList.BeginUpdate();
    //try {
    selList.ClearItems();
    console.log(values.length);
    for (var i = 0; i < values.length; i++) {
        s = "";
        selList.AddItem(values[i], i);
        //for (j = 0; j < values[i].length; j++) {
        //    //s = s + values[i][j] + " ";
        //    //selList.InsertItem(i, values[i][j]);

        //}

        //selList.AddItem(s); 
    }
    //} finally {
    //    selList.EndUpdate();
    //}

    selList.EndUpdate();
    document.getElementById("selCount").innerHTML = POAddEditGrid.GetSelectedRowCount();
}

//FOR PO ADD/EDIT PROD CATEGORY DROPDOWN
function procategory_indexchange(s, e) {
    POAddEditGrid.PerformCallback();
}



//FOR WAREHOUSE IN PO ADD/EDIT
var postponedCallbackRequiredWarehouse = false;
function site_indexchanged(s, e) {
    if (WarehouseCallback.InCallback()) {
        postponedCallbackRequiredWarehouse = true;
        postponedCallbackRequiredLocation = true;
    }
    else {
        WarehouseCallback.PerformCallback();
        LocationCallback.PerformCallback();
    }
}

function warehouse_endcallback(s, e) {
    if (postponedCallbackRequiredWarehouse) {
        WarehouseCallback.PerformCallback();
        postponedCallbackRequiredWarehouse = false;
    }
}

var postponedCallbackRequiredLocation = false;
function warehouse_indexchanged(s, e) {
    Location.SetText("");
    if (LocationCallback.InCallback())
        postponedCallbackRequiredLocation = true;
    else
        LocationCallback.PerformCallback();
}

function location_endcallback(s, e) {
    if (postponedCallbackRequiredLocation) {
        LocationCallback.PerformCallback();
        postponedCallbackRequiredLocation = false;
    }
}

//FOR VENDOR
var postponedCallbackRequiredCurrency = false;
var postponedCallbackRequiredTerms = false;
function vendor_indexchanged(s, e) {
    if (CurrencyCallback.InCallback()) {
        postponedCallbackRequiredCurrency = true;
        postponedCallbackRequiredTerms = true;
    }
    else {
        CurrencyCallback.PerformCallback();
        TermsCallback.PerformCallback();
    }
}

//END CALL BACK CURRENCY
function currency_endcallback(s, e) {
    if (postponedCallbackRequiredCurrency) {
        CurrencyCallback.PerformCallback();
        postponedCallbackRequiredCurrency = false;
    }
}

//END CALL BACK TERMS
function terms_endcallback(s, e) {
    if (postponedCallbackRequiredTerms) {
        TermsCallback.PerformCallback();
        postponedCallbackRequiredTerms = false;
    }
}

//Gridview PO
function OnKeyUpQtyPO(s, e) {//OnChange
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(s.GetText()).toFixed(2);
    var cost = parseFloat(accounting.unformat(POCost.GetText()));
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            POTotalCost.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        POTotalCost.SetText("");
    }
}

function OnKeyUpCostPO(s, e) {//OnChange
    //var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(POQty.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            POTotalCost.SetText(accounting.formatMoney(total));
        }
    } else {
        POTotalCost.SetText("");
    }
}


//FOR USERLIST
var postponedCallbackuserBU = false;
function UserEntity_IndexChanged(s, e) {
    if (BUCallBackPanelDirect.InCallback()) {
        postponedCallbackuserBU = true;
    }
    else {
        BUCallBackPanelDirect.PerformCallback();
    }
}

function UserBU_EndCallback(s, e) {
    if (postponedCallbackuserBU) {
        BUCallBackPanelDirect.PerformCallback();
        postponedCallbackuserBU = false;
    }
}

function updateUserListNew(s, e) {
    var endCode = EntityCodeDirect.GetText();
    var levelCode = UserLevelDirect.GetText();
    var statusCode = UserStatusDirect.GetText();

    if (endCode.length > 0 && levelCode.length > 0 && statusCode.length > 0) {
        UserListGrid.UpdateEdit();
    }
}

//FOR BU / Dept Head
var postponedCallbackHeadBU = false;
function HeadEntity_IndexChanged(s, e) {
    if (BUCallBackPanelHeadDirect.InCallback()) {
        postponedCallbackHeadBU = true;
    }
    else {
        BUCallBackPanelHeadDirect.PerformCallback();
    }
}

function HeadBU_EndCallback(s, e) {
    if (postponedCallbackHeadBU) {
        BUCallBackPanelHeadDirect.PerformCallback();
        postponedCallbackHeadBU = false;
    }
}

function updateBUDeptHeadList(s, e) {
    var endCode = EntityCodeHeadDirect.GetText();
    var headCode = BUHeadDirect.GetText();
    var effectDate = EffectDateHeadDirect.GetText();

    if (endCode.length > 0 && headCode.length > 0 && effectDate.length > 0) {
        BUDeptListGridDirect.UpdateEdit();
    }
}


//FOR SCM
var postponedCallbackSCMProcCat = false;
function OnGridFocusedRowChangedSCMProcOff(s, e) {
    grdSCMProcurementOffDetailsDirect.PerformCallback();
}

function OnGridFocusedRowChangedSCMProcOff_EndCallback(s, e) {
    grdSCMProcurementOffDetailsDirect.Refresh();
}
//END OF ADD FORM SCRIPT HERE.....

//FOR Finance
var postponedCallbackFinBudBU = false;
function FinBudEntity_IndexChanged(s, e) {
    if (FinBUCallbackPanelDirect.InCallback()) {
        postponedCallbackFinBudBU = true;
    }
    else {
        FinBUCallbackPanelDirect.PerformCallback();
    }
}

function FinBudBU_EndCallback(s, e) {
    if (postponedCallbackFinBudBU) {
        FinBUCallbackPanelDirect.PerformCallback();
        postponedCallbackFinBudBU = false;
    }
}

function OnGridFocusedRowChangedFinBud(s, e) {
    grdFinanceBudgetDetDirect.PerformCallback();
}

function OnGridFocusedRowChangedFinBud_EndCallback(s, e) {
    grdFinanceBudgetDetDirect.Refresh();

}


//mrp_inventanalyst
function MRPanalystfocused(s, e, type) {
    //var pk = s.GetRowKey(e.visibleIndex);;
    //params = type + "-" + pk;

    //if (FloatCallbackPanel.InCallback())
    //    postponedCallbackRequired = true;
    //else
    //    FloatCallbackPanel.PerformCallback(params);
}

function OnKeyUpQtytInvDirect(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(accounting.unformat(s.GetText()));
    var cost = parseFloat(InvEdittedCost.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            InvEdittiedTotalCost.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        InvEdittiedTotalCost.SetText("");
    }
}

function OnKeyUpCosttInvDirect(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(InvEdittedQty.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            InvEdittiedTotalCost.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        InvEdittiedTotalCost.SetText("");
    }
}


function OnKeyUpQtytInvOpex(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(accounting.unformat(s.GetText()));
    var cost = parseFloat(InvEdittedCostOp.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            InvEdittiedTotalCostOp.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        InvEdittiedTotalCostOp.SetText("");
    }
}

function OnKeyUpCosttInvOpex(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(InvEdittedQtyOp.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            InvEdittiedTotalCostOp.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        InvEdittiedTotalCostOp.SetText("");
    }
}


function OnKeyUpQtytInvManPower(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(accounting.unformat(s.GetText()));
    var cost = parseFloat(InvEdittedCostManPo.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            InvEdittiedTotalCostManPo.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        InvEdittiedTotalCostManPo.SetText("");
    }
}

function OnKeyUpCosttInvManPower(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(InvEdittedQtyManPo.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            InvEdittiedTotalCostManPo.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        InvEdittiedTotalCostManPo.SetText("");
    }
}

function OnKeyUpQtytInvCapex(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(accounting.unformat(s.GetText()));
    var cost = parseFloat(InvEdittedCostCapex.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            InvEdittiedTotalCostCapex.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        InvEdittiedTotalCostCapex.SetText("");
    }
}

function OnKeyUpCosttInvCapex(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(InvEdittedQtyCapex.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            InvEdittiedTotalCostCapex.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        InvEdittiedTotalCostCapex.SetText("");
    }
}

//mrp_forapproval.aspx
//for Approval Qty Direct Materials
function OnKeyUpApprovedQtyDirect(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(accounting.unformat(s.GetText()));
    var cost = parseFloat(ApprovedCostDM.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            ApprovedTotalCostDM.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        ApprovedTotalCostDM.SetText("");
    }
}

//for Approval Cost Direct Materials
function OnKeyUpApprovedCostDirect(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(ApprovedQtyDM.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            ApprovedTotalCostDM.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        ApprovedTotalCostDM.SetText("");
    }
}

//for Approval Qty Opex
function OnKeyUpQtyApprovedQtyOpex(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(accounting.unformat(s.GetText()));
    var cost = parseFloat(ApprovedCostOpex.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            ApprovedTotalCostOpex.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        ApprovedTotalCostOpex.SetText("");
    }
}


//for Approval Cost Opex
function OnKeyUpCostApprovedCostOpex(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(ApprovedQtyOpex.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            ApprovedTotalCostOpex.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        ApprovedTotalCostOpex.SetText("");
    }
}


//for Approval Qty Manpower
function OnKeyUpApprovedQtyManPower(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(accounting.unformat(s.GetText()));
    var cost = parseFloat(ApprovedCostManPower.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            ApprovedTotalCostManPower.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        ApprovedTotalCostManPower.SetText("");
    }
}

//for Approval Cost Manpower
function OnKeyUpApprovedCostManPower(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(ApprovedQtyManPower.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            ApprovedTotalCostManPower.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        ApprovedTotalCostManPower.SetText("");
    }
}

//for Approval Qty Capex
function OnKeyUpApprovedQtyCapex(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var qty = parseFloat(accounting.unformat(s.GetText()));
    var cost = parseFloat(ApprovedCostCapex.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            ApprovedTotalCostCapex.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        ApprovedTotalCostCapex.SetText("");
    }
}


//for Approval Cost Capex
function OnKeyUpApprovedCostCapex(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    var cost = parseFloat(accounting.unformat(s.GetText()));
    var qty = parseFloat(ApprovedQtyCapex.GetText()).toFixed(2);
    var total = 0;
    if (qty > 0) {
        if (cost > 0) {
            total = cost * qty;
            ApprovedTotalCostCapex.SetText(parseFloat(total).toFixed(2));
        }
    } else {
        ApprovedTotalCostCapex.SetText("");
    }
}

//function (s, e) {
//    var entity = lblEntity.getvalue();
//    MRPNotificationMessage.SetText('You are not authorized to access this item');
//    MRPNotify.SetHeaderText('Alert');
//    MRPNotify.Show();
//    MRPHiddenVal.Set('hidden_value', ' ');
//}


function AddMOPCheckEntity(s, e) {
    console.log("pass script");
    var entity = ASPxLabelEntDirect.GetText();
    if (entity === "") {
        MRPNotificationMessage.SetText("No assigned entity!");
        MRPNotify.SetHeaderText("Alert");
        MRPNotify.Show();
    }
}



