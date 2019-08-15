﻿function updateCAPEX(s, e) {
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

    //var boolProdCat = true;
    //var isprod = getCookie("isProdCat");
    //switch (isprod) {
    //    case "1":
    //        if (ProdCatCAPEX.GetEnabled()) {
    //            if (ProdCatCAPEX.GetText().length > 0) {
    //                boolProdCat = true;
    //            } else {
    //                boolProdCat = false;
    //            }
    //        }
    //        break;
    //}

    var prodcat = ProdCatCAPEX.GetText();
    var itemDesc = DescriptionCAPEX.GetText();
    var uom = UOMCAPEX.GetText();
    var cost = CostCAPEX.GetText();
    var qty = QtyCAPEX.GetText();
    var totalcost = TotalCostCAPEX.GetText();

    if (prodcat.length > 0 && itemDesc.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool) {
        CAPEXGrid.UpdateEdit();
    }

    //var boolItemCode = true;
    //var isItem = getCookie("opisItem");
    //switch (isItem) {
    //    case "1":
    //        if (ItemCodeOPEX.GetEnabled()) {
    //            if (ItemCodeOPEX.GetText().length > 0)
    //                boolItemCode = true;
    //            else
    //                boolItemCode = false;
    //        }
    //        break;
    //}

    //var expense = ExpenseCodeOPEX.GetText();
    //var itemCode = ItemCodeOPEX.GetText();
    //var itemDesc = DescriptionOPEX.GetText();
    //var uom = UOMOPEX.GetText();
    //var cost = CostOPEX.GetText();
    //var qty = QtyOPEX.GetText();
    //var totalcost = TotalCostOPEX.GetText();

    //if (expense.length > 0 && itemDesc.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool && boolProdCat && boolItemCode) {
    //    OPEXGrid.UpdateEdit();
    //}

}

function OnKeyUpCostCapex(s, e) {//OnChange
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

function ActivityCodeIndexChange(s, e) {
    //var now = getNow();
    //document.cookie = 'dmvalue=' + s.GetValue() + '; expires=' + now.toUTCString() + '; path=/';
    //document.cookie = 'dmtext=' + s.GetText() + '; expires=' + now.toUTCString() + '; path=/';

    document.cookie = 'dmvalue=' + s.GetValue();
    document.cookie = 'dmtext=' + s.GetText();
}

function ActivityCodeIndexChangeMAN(s, e) {
    //var now = getNow();
    document.cookie = 'manvalue=' + s.GetValue();
    document.cookie = 'mantext=' + s.GetText();
}

function getNow() {
    var now = new Date();
    var time = now.getTime();
    time += 3600 * 1000;
    now.setTime(time);
    return now;
}

//this function for debugging
function getCookie(name) {
    var re = new RegExp(name + "=([^;]+)");
    var value = re.exec(document.cookie);
    return (value != null) ? unescape(value[1]) : null;
}

//Operating Expenditure
var isItem = 1;
var isProdCat = 1;
var postponedCallbackOPEXProCat = false;
function ExpenseCodeIndexChangeOPEX(s, e) {
    isItem = parseInt(s.GetSelectedItem().GetColumnText('isItem'));
    isProdCat = parseInt(s.GetSelectedItem().GetColumnText('isProdCategory'));
    console.log(isItem + " .... " + isProdCat);

    //Save cookies
    document.cookie = 'opvalue=' + s.GetValue();
    document.cookie = 'optext=' + s.GetText();
    document.cookie = 'opisItem=' + isItem;
    document.cookie = 'opisProdCat=' + isProdCat;

    //product category will always follow the expense
    //empty all cookies under product category in opex
    document.cookie = 'opproductvalue=' + "";
    document.cookie = 'opproducttext=' + "";

    switch (isItem) {
        case 0://Non PO
            document.getElementsByClassName("div1Class")[0].style.display = "none";
            document.getElementsByClassName("div2Class")[0].style.display = "none";
            DescriptionOPEX.SetText("");
            //old function change June 4, 2019 (See Documentation)
            //DescriptionOPEX.GetInputElement().readOnly = false;
            ItemCodeOPEX.SetText("");
            break;
        case 1://PO
            document.getElementsByClassName("div1Class")[0].style.display = "block";
            document.getElementsByClassName("div2Class")[0].style.display = "block";
            ItemCodeChkbxClient.SetChecked(true);
            DescriptionOPEX.SetText("");
            //old function change June 4, 2019 (See Documentation)
            //DescriptionOPEX.GetInputElement().readOnly = true;
            ItemCodeOPEX.SetText("");
            break;
    }

    switch (isProdCat) {
        case 0://hide product category combobox
            document.getElementsByClassName("CA_prodcombo_divClass")[0].style.display = "none";
            document.getElementsByClassName("CA_prodlabel_divClass")[0].style.display = "none";
            break;
        case 1://show product category combobox
            document.getElementsByClassName("CA_prodcombo_divClass")[0].style.display = "block";
            document.getElementsByClassName("CA_prodlabel_divClass")[0].style.display = "block";
            ProdCatChkbxClient.SetChecked(true);
            break;
    }

    //var entCode = EntityCodeAddEditDirect.GetText();
    //if (entCode == "0303") {
    //    document.getElementsByClassName("div1Class")[0].style.display = "none";
    //    document.getElementsByClassName("div2Class")[0].style.display = "none";
    //    DescriptionOPEX.SetText("");
    //    DescriptionOPEX.GetInputElement().readOnly = false;
    //    ItemCodeOPEX.SetText("");
    //}

    if (ProcCatOPEXCallbackClient.InCallback()) {
        postponedCallbackOPEXProCat = true;
    }
    else {
        ProcCatOPEXCallbackClient.PerformCallback();
    }
}

function ProcCatOPEX_SelectedIndexChanged(s, e) {
    document.cookie = 'opproductvalue=' + s.GetValue();
    document.cookie = 'opproducttext=' + s.GetText();
}

function ProdCat_SelectedIndexChanged(s, e) {
    document.cookie = 'caproductvalue=' + s.GetValue();
    document.cookie = 'caproducttext=' + s.GetText();
}

//Operating Unit in TRAIN Entity
function OperatingUnitDM_SelectedIndexChanged(s, e) {
    document.cookie = 'dm_operating_value=' + s.GetValue();
    document.cookie = 'dm_operating_text=' + s.GetText();

    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

//Direct Material 
function ActivityCodeChkbx_CheckedChanged(s, e) {
    if (s.GetChecked()) {
        ActivityCodeDirect.SetEnabled(true);
        ActivityCodeDirect.SetIsValid(true);
    } else {
        ActivityCodeDirect.SetText("");
        ActivityCodeDirect.SetValue("");
        ActivityCodeDirect.SetEnabled(false);
        ActivityCodeDirect.SetIsValid(false);

    }
}

//Direct Material 
function ExpenseChkbx_CheckedChanged(s, e) {
    if (s.GetChecked()) {
        ExpenseCodeDM.SetEnabled(true);
        ExpenseCodeDM.SetIsValid(true);
    } else {
        ExpenseCodeDM.SetText("");
        ExpenseCodeDM.SetValue("");
        ExpenseCodeDM.SetEnabled(false);
        ExpenseCodeDM.SetIsValid(false);

    }
}

//Direct Material
function ExpenseCode_SelectedIndexChanged(s, e) {
    document.cookie = 'dm_exp_value=' + s.GetValue();
    document.cookie = 'dm_exp_text=' + s.GetText();

    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function OperatingUnitOP_SelectedIndexChanged(s, e) {
    document.cookie = 'op_operating_value=' + s.GetValue();
    document.cookie = 'op_operating_text=' + s.GetText();

    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function OperatingUnitMAN_SelectedIndexChanged(s, e) {
    document.cookie = 'man_operating_value=' + s.GetValue();
    document.cookie = 'man_operating_text=' + s.GetText();

    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function OperatingUnitCA_SelectedIndexChanged(s, e) {
    document.cookie = 'ca_operating_value=' + s.GetValue();
    document.cookie = 'ca_operating_text=' + s.GetText();

    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function OperatingUnitREV_SelectedIndexChanged(s, e) {
    document.cookie = 'rev_operating_value=' + s.GetValue();
    document.cookie = 'rev_operating_text=' + s.GetText();

    var text = s.GetSelectedItem().text;
    if (text.length == 0)
        s.SetIsValid(false);
    else
        s.SetIsValid(true);
}

function ItemCodeOPEX_KeyPress(s, e) {
    var key = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    //KEY (ENTER) keycode: 13
    if (key == 13) {
        ASPxClientUtils.PreventEvent(e.htmlEvent);
        listboxOPEX.SetVisible(true);
        
        listboxOPEX.PerformCallback(DescriptionOPEX.GetInputElement().value);
        //listboxOPEX.PerformCallback(ItemCodeOPEX.GetInputElement().value);
    }
}

function listboxOPEX_EndCallback(s, e) {
    //alert(listboxOPEX.GetItemCount());
    if (listboxOPEX.GetItemCount() == 0)
        listboxOPEX.SetVisible(false);
}

function listbox_EndCallback(s, e) {
    if (listbox.GetItemCount() == 0)
        listbox.SetVisible(false);
}

function ProdCatChkbx_CheckedChanged(s, e) {
    if (s.GetChecked()) {
        ProcCatOPEX.SetEnabled(true);
        ProcCatOPEX.SetIsValid(true);
    } else {
        ProcCatOPEX.SetText("");
        ProcCatOPEX.SetValue("");
        ProcCatOPEX.SetEnabled(false);
        ProcCatOPEX.SetIsValid(false);
    }
}

function ItemCodeChkbx_CheckedChanged(s, e) {
    if (s.GetChecked()) {
        ItemCodeOPEX.SetEnabled(true);
        ItemCodeOPEX.SetIsValid(true);
        //old function change June 4, 2019 (See Documentation)
        //DescriptionOPEX.GetInputElement().readOnly = true;
    } else {
        ItemCodeOPEX.SetValue("");
        ItemCodeOPEX.SetText("");
        ItemCodeOPEX.SetEnabled(false);
        ItemCodeOPEX.SetIsValid(false);
        DescriptionOPEX.SetValue("");
        DescriptionOPEX.SetText("");
        //old function change June 4, 2019 (See Documentation)
        //DescriptionOPEX.GetInputElement().readOnly = false;
    }
}

// Direct Material UPDATE/SAVE Button
function updateDirectMat(s, e) {
    var entityval = entityhidden.Get('hidden_value');
    var bool = true;
    if (entityval == "display") {
        if (OperatingUnit.GetText().length == 0) {
            OperatingUnit.SetIsValid(false);
            bool = false;
        } else {
            OperatingUnit.SetIsValid(true);
            bool = true;
        }
    }

    var boolAct = true;
    if (ActivityCodeDirect.GetEnabled()) {
        if (ActivityCodeDirect.GetText().length > 0) {
            boolAct = true;
        } else {
            boolAct = false;
        }
    }

    var boolExp = true;
    if (ExpenseCodeDM.GetEnabled()) {
        if (ExpenseCodeDM.GetText().length > 0) {
            boolExp = true;
        } else {
            boolExp = false;
        }
    }

    var itemCode = ItemCodeDirect.GetText();
    var itemDesc = ItemDescriptionDirect.GetText();
    var uom = UOMDirect.GetText();
    var cost = CostDirect.GetText();
    var qty = QtyDirect.GetText();
    var totalcost = TotalCostDirect.GetText();

    if (itemCode.length > 0 && itemDesc.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool && boolAct && boolExp) {
        DirectMaterialsGrid.UpdateEdit();
    }
}

// opex UPDATE/SAVE Button
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

    var boolProdCat = true;
    var isprod = getCookie("isProdCat");
    switch (isprod) {
        case "1":
            if (ProcCatOPEX.GetEnabled()) {
                if (ProcCatOPEX.GetText().length > 0) {
                    boolProdCat = true;
                } else {
                    boolProdCat = false;
                }
            }
            break;
    }

    var boolItemCode = true;
    var isItem = getCookie("opisItem");
    switch (isItem) {
        case "1":
            if (ItemCodeOPEX.GetEnabled()) {
                if (ItemCodeOPEX.GetText().length > 0)
                    boolItemCode = true;
                else
                    boolItemCode = false;
            }
            break;
    }

    var expense = ExpenseCodeOPEX.GetText();
    var itemCode = ItemCodeOPEX.GetText();
    var itemDesc = DescriptionOPEX.GetText();
    var uom = UOMOPEX.GetText();
    var cost = CostOPEX.GetText();
    var qty = QtyOPEX.GetText();
    var totalcost = TotalCostOPEX.GetText();

    if (expense.length > 0 && itemDesc.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool && boolProdCat && boolItemCode) {
        OPEXGrid.UpdateEdit();
    }
}


//Manpower UPDATE/SAVE Button
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
    //var itemDesc = DescriptionMAN.GetText();
    var uom = UOMMAN.GetText();
    var cost = CostMAN.GetText();
    var qty = QtyMAN.GetText();
    var totalcost = TotalCostMAN.GetText();

    if (activity.length > 0 && type.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool) {
        ManPowerGrid.UpdateEdit();
    }

    //if (type.length > 0 && uom.length > 0 && cost.length > 0 && qty.length > 0 && totalcost.length > 0 && bool) {
    //    ManPowerGrid.UpdateEdit();
    //}
}